using OpenAI;
using Samples.Whisper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace RoslynCSharp.Example 
{ 
public class ToggleExecuteInstructions : MonoBehaviour
{
    public InputActionReference toggleExecuteInstructionsAction;
    private Compiler compiler;

        private void Awake()
    {
        toggleExecuteInstructionsAction.action.started += Toggle;
        compiler = GetComponent<Compiler>();
    }

    private void OnDestroy()
    {
        toggleExecuteInstructionsAction.action.started -= Toggle;
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        compiler.RunCode();
    }
}
}