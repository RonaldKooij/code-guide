using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.Linq;
using System.Reflection;

public class CodeWindow : EditorWindow
{
    private string[] _assemblies;
    private int _assembleyIndex = 0;

    // Add menu named "Custom Window" to the Window menu
    [MenuItem("Window/Organisation/Code Overview", false, 3000)]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CodeWindow window = (CodeWindow)EditorWindow.GetWindow(typeof(CodeWindow));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        //Set the Icon and title of our editor window
        titleContent = EditorGUIUtility.IconContent("TreeEditor.Duplicate");
        titleContent.text = "Code Overview";

        //Update the list of assemblies, and create the dropdown menu
        _assemblies = CompilationPipeline.GetAssemblies().Select(x => x.name).ToArray();
        _assembleyIndex = EditorGUILayout.Popup(_assembleyIndex, _assemblies, EditorStyles.toolbarDropDown);

        if (GUILayout.Button("Generate overview", EditorStyles.toolbarButton))
        {
            GenerateOverview();
        }

        EditorGUILayout.EndHorizontal();
    }

    //Generate the Project(code) overview   (Move to seperate class later)
    private void GenerateOverview()
    {
        //Load the assembly thats been selected in the dropdown    
        System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(_assemblies[_assembleyIndex]);

        //Get the classes we are interested in
        foreach (var type in assembly.GetTypes())
        {
            //Might want to add interfaces later, or at least add the option to see them in the overview
            if (type.IsSealed || type.IsInterface)
                continue;

            //Can do class stuff here
            Debug.Log(type + " " + type.BaseType);

            //Get the methods we are interested in
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                //For now we ignore inherited methods, and only want the methods that are implemented here, otherwise we get a lot of Monobehaviour stuff
                if (method.DeclaringType != type)
                    continue;

                //Can do method stuff here
                //Debug.Log(type + " " + method);                
            }

            /*
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                if (field.DeclaringType != type)
                    continue;

                Debug.Log(type + "   " + field);
            }*/
        }
    }
}