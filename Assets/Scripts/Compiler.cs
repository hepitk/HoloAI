using RoslynCSharp.Compiler;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RoslynCSharp.Example
{
     public class Compiler : MonoBehaviour
    {
        private string activeCSharpSource = null;
        private ScriptProxy activeCompilerScript = null;
        private ScriptDomain domain = null;
        public string input;
        public TextAsset codeTemplate;
        public AssemblyReferenceAsset[] assemblyReferences;

        public GameObject sceneManager;

        public Button runButton;

        public void Awake()
        {
            runButton.onClick.AddListener(RunCode);
        }

        public void Start()
        {
            // Create the domain
            domain = ScriptDomain.CreateDomain("HolodeckCode", true);

            // Add assembly references
            foreach (AssemblyReferenceAsset reference in assemblyReferences)
                domain.RoslynCompilerService.ReferenceAssemblies.Add(reference);

            // Load the code template
            input = codeTemplate.text;      
        }

        /// <summary>
        /// Main run method.
        /// This causes any code to be compiled and executed on the Scene Manager.
        /// </summary>
        public void RunCode()
        {
            Debug.Log(input);
            // Get the C# code from the input field
            string cSharpSource = input;

            // Don't recompile the same code
            if (activeCSharpSource != cSharpSource || activeCompilerScript == null)
            {

                //try
                {
                    // Compile code
                    ScriptType type = domain.CompileAndLoadMainSource(cSharpSource, ScriptSecurityMode.UseSettings, assemblyReferences);

                    // Check for null
                    if (type == null)
                    {
                        if (domain.RoslynCompilerService.LastCompileResult.Success == false)
                            throw new Exception("Code contained errors. Please fix and try again");
                        else if (domain.SecurityResult.IsSecurityVerified == false)
                            throw new Exception("Code failed code security verification");
                        else
                            throw new Exception("Code does not define a class. You must include one class definition");
                    }                    

                    // Create an instance
                    activeCompilerScript = type.CreateInstance(sceneManager);
                    activeCSharpSource = cSharpSource;
                }

            }
            else
            {
                Debug.LogWarning("Code already compiled");
            }
        }
    }
}
