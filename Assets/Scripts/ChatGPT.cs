using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using RoslynCSharp.Example;
using System.Diagnostics;
using System.Collections;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] public InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;
        [SerializeField] public Compiler compiler;

        private float height;
        private OpenAIApi openai = new OpenAIApi();
        [SerializeField] private AudioClip readyToExecute;
        private AudioSource audioSource;

        public TextAsset instructions;

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt;
            

        private void Start()
        {
            button.onClick.AddListener(SendReply);
            audioSource = gameObject.AddComponent<AudioSource>();
            prompt = instructions.text;
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            compiler.input = message.Content; // Pass the message to the compiler
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        public async void SendReply()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text; 
            
            messages.Add(newMessage);
            
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                //# Region GPT-3.5
                //Model = "gpt-3.5-turbo-1106",
                //Model = "gpt-3.5-turbo-0613",
                //Model = "gpt-3.5-turbo-16k-0613",
                //Model = "gpt-3.5-turbo-16k",
                //Model = "gpt-3.5-turbo-0125",

                //# Region GPT-4
                //Model = "gpt-4-0125-preview",
                //Model = "gpt-4-turbo-preview",
                //Model = "gpt-4",
                //Model = "gpt-4-vision-preview",

                //# Region GPT-4o
                Model = "gpt-4o",

                Temperature = 0.1f,
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                messages.Add(message);
                AppendMessage(message);
                PlaySound(readyToExecute);
                stopwatch.Stop();
                UnityEngine.Debug.Log($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
            }
            else
            {
                UnityEngine.Debug.LogWarning("No text was generated from this prompt.");
                stopwatch.Stop();
                UnityEngine.Debug.Log($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
            }

            button.enabled = true;
            inputField.enabled = true;
        }
        private void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
