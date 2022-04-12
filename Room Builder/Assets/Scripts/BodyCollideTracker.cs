using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class BodyCollideTracker : MonoBehaviour
{

    float startTime, endTime;

    private void Start()
    {
        string filePath = getPath();
        if (File.Exists(filePath))
            System.IO.File.WriteAllText(filePath, string.Empty);

        string output = "Player Contact,Contact Object Name,Start Time,End Time";
        StartCoroutine(WriteToFile(output));
    }



    void OnCollisionEnter(Collision collision)
    {
        startTime = Time.time;
    }

   
    void OnCollisionExit(Collision collision)
    {
        endTime = Time.time;

        string output = this.name + "," + collision.ToString() + "," + startTime.ToString() + "," + endTime.ToString();
        StartCoroutine(WriteToFile(output));
    }

    private string getPath()
    {
#if UNITY_EDITOR
        ObjectPosition g = FindObjectOfType<ObjectPosition>();
        string s = g.name;
        int found = s.IndexOf(" obj");
        string roomname = s.Substring(0, found);
        return Application.dataPath + "/CSV files/" + roomname + "/Contact Data/" + name + ".csv";
#else
        return Application.dataPath + "/"+"CurrentInfo.csv";
#endif
    }


    IEnumerator WriteToFile(string output)
    {
        string filePath = getPath();
        if (!File.Exists(filePath))
        {
            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(output);
            outStream.Close();
        }
        else
        {
            // Open the stream and write to it.
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(output);
            }
        }
        yield return null;
    }

    
}
