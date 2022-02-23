using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MenuManager : Editor
{
    private static Dictionary<string, MonoScript> AllScripts = new Dictionary<string, MonoScript>();
    List<Component> _scriptsList = new List<Component>();

    

    [MenuItem("VR/Enable Data Scripts")]
    static void EnableScripts()
    {
        GameObject[] gameobjects;
        gameobjects = GameObject.FindGameObjectsWithTag("DataCollect");
        //var obj = GameObject.FindGameObjectWithTag("DataCollect");
        var head = GameObject.FindGameObjectWithTag("MainCamera");

        foreach(var go in gameobjects)
        {
            foreach (MonoBehaviour script in go.GetComponents<MonoBehaviour>())
            {
                if (script.name.Contains("Tracker"))
                    script.enabled = true;
            }
        }

        string[] assetPaths = AssetDatabase.GetAllAssetPaths();
        foreach (string assetPath in assetPaths)
        {
            if (assetPath.Contains(".cs")) // or .js if you want
            {
                Debug.Log(assetPath);
            }
        }

        AllScripts.Clear();
        UnityEngine.Object[] scripts = Resources.LoadAll("Scripts");

        foreach (UnityEngine.Object script in scripts)
        {
            if (scripts.ToString().Contains("Tracker"))
            {
                
            }
        }

        Debug.Log(AllScripts.Count.ToString());
    }
}
