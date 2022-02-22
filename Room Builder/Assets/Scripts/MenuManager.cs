using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MenuManager : MonoBehaviour
{
    private static Dictionary<string, MonoScript> AllScripts = new Dictionary<string, MonoScript>();

    [MenuItem("VR/Enable Data Scripts")]
    static void EnableScripts()
    {
        AllScripts.Clear();
        UnityEngine.Object[] scripts = Resources.LoadAll("Scripts");

        foreach (UnityEngine.Object script in scripts)
        {
            if (scripts.ToString().Contains("Tracker"))
            {
                scripts.;
            }
        }

        Debug.Log(AllScripts.Count.ToString());
    }
}
