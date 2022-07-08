using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class DoorTracker : MonoBehaviour
{
    bool startRecord = false;
    private List<string[]> rowData = new List<string[]>();
    float preX, preZ;
    double distance = 0.0f;

    private Transform origin;

    float originX, originY, originZ;

    private float nextActionTime = 0.0f;
    private float period = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        origin = FindObjectOfType<ObjectPosition>().origin;
        
        originX = origin.position.x;
        originY = origin.position.y;
        originZ = origin.position.z;

        string filePath = getPath();
        if (File.Exists(filePath))
            System.IO.File.WriteAllText(filePath, string.Empty);

        string header = "Object Name,Time,PosX,PosY,PosZ,RotX,RotY,RotZ";
        StartCoroutine(WriteToFile(header));
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            float x;
            float y;
            float z;
        
            x = gameObject.transform.position.x - originX;
            y = gameObject.transform.position.y - originY;
            z = gameObject.transform.position.z - originZ;

            string rx = gameObject.transform.rotation.eulerAngles.x.ToString();
            string ry = gameObject.transform.rotation.eulerAngles.y.ToString();
            string rz = gameObject.transform.rotation.eulerAngles.z.ToString();

            string output = name + "," + Time.time.ToString() + "," + x.ToString() + "," + y.ToString() + "," + z.ToString() + "," + rx + "," + ry + "," + rz;
            StartCoroutine(WriteToFile(output));
        }

    }

    IEnumerator WriteToFile(string output)
    {
        string filePath;
        
            filePath = getPath();

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

    void WriteToFile()
    {
        Debug.Log("Writing to file Now");
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }
        string filePath = getPath();
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        Debug.Log("Finished Writing to File");
    }

    private string getPath()
    {
        //#if UNITY_EDITOR
        RoomManager rm = FindObjectOfType<RoomManager>();
        Tuple<string, string> path = rm.GetPath();
        string m_path = path.Item1 + "/Door Data/";
        Directory.CreateDirectory(m_path);
        return m_path + path.Item2 + transform.parent.name + ".csv";

        //#else
        //      return Application.dataPath + "/"+"CurrentInfo.csv";
        //#endif
    }

    
}
