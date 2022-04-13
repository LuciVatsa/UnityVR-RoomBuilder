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

        string output = this.name + "," + collision.collider.name + "," + startTime.ToString() + "," + endTime.ToString();
        StartCoroutine(WriteToFile(output));
    }

    private string getPath()
    {
#if UNITY_EDITOR
        RoomManager rm = FindObjectOfType<RoomManager>();
        System.Tuple<string, string> path = rm.GetPath();
        string m_path = path.Item1 + "/Contact Data/";
        Directory.CreateDirectory(m_path);
        return m_path + path.Item2 + name + ".csv";
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
