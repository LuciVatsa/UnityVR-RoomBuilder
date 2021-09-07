using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class PlayerTracker : MonoBehaviour
{
    bool startRecord = false;
    private List<string[]> rowData = new List<string[]>();
    float preX, preY;
    double distance = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Save();
    }
    void Save()
    {
        string[] rowDataTemp = new string[6];
        rowDataTemp[0] = "Object Name";
        rowDataTemp[1] = "Time";
        rowDataTemp[2] = "PosX";
        rowDataTemp[3] = "PosY";
        rowDataTemp[4] = "PosZ";
        rowDataTemp[5] = "Distance";
        rowData.Add(rowDataTemp);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            if (!startRecord)
            {
                startRecord = true;
                preX = gameObject.transform.position.x;
                preY = gameObject.transform.position.y;
                distance = 0.0f;
                StartCoroutine(RecordPlayerData());
            }
            else
            {
                StopCoroutine(RecordPlayerData());
                WriteToFile();
                startRecord = false;
            }
        }

    }

    IEnumerator RecordPlayerData()
    {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        distance += Mathf.Sqrt(Mathf.Pow((x - preX), 2) + Mathf.Pow((y - preY), 2));
        string[] rowDataTemp = new string[8];
        rowDataTemp[0] = name;
        rowDataTemp[1] = Time.time.ToString();
        rowDataTemp[2] = x.ToString();
        rowDataTemp[3] = y.ToString();
        rowDataTemp[4] = gameObject.transform.position.z.ToString();
        rowDataTemp[5] = distance.ToString();
        preX = x;
        preY = y;
        rowData.Add(rowDataTemp);
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
#if UNITY_EDITOR
        return Application.dataPath + "/CSV" + "ObjectData" + name + ".csv";
#else
      return Application.dataPath + "/"+"CurrentInfo.csv";
#endif
    }
}
