# HoloAI

- Project uses Roslyn C# - Runtime Compiler that can be bought from the Unity asset store: https://assetstore.unity.com/packages/tools/integration/roslyn-c-runtime-compiler-142753, copy contents of that asset to Assets-folder
- Project also needs OpenAI API key. Put that to .openai in your home directory (e.g. C:User\UserName\ for Windows or ~\ for Linux or Mac) in file named auth.json

auth.json should look like this:
{
    "api_key": "sk-...W6yi",
    "organization": "org-...L7W"
}