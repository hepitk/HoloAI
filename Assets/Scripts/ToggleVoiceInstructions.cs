using Samples.Whisper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleVoiceInstructions : MonoBehaviour
{
    public InputActionReference toggleVoiceInstructionsAction;
    private Whisper whisper;
    private AudioSource audioSource;
    [SerializeField] private AudioClip startRecordingClip;
    [SerializeField] private AudioClip stopRecordingClip;
    private bool recording = false;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        toggleVoiceInstructionsAction.action.started += Toggle;
        whisper = GetComponent<Whisper>();
    }

    private void OnDestroy() 
    {
        toggleVoiceInstructionsAction.action.started -= Toggle;    
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        if (recording)
        {
            recording = false;
            audioSource.PlayOneShot(stopRecordingClip);
            whisper.EndRecording(); 
        }
        else
        {
            recording = true;
            audioSource.PlayOneShot(startRecordingClip);
            whisper.StartRecording();        
        }
 
    }
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
