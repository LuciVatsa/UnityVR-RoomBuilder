using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
public class ObjectPosition : MonoBehaviour
{

    private List<string[]> rowData = new List<string[]>();
    // Start is called before the first frame update
    void Start()
    {
        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = "Object Name";
        rowDataTemp[1] = "PosX";
        rowDataTemp[2] = "PosY";
        rowDataTemp[3] = "PosZ";
        rowData.Add(rowDataTemp);

        rowDataTemp[0] = name;
        rowDataTemp[1] = gameObject.transform.position.x.ToString();
        rowDataTemp[2] = gameObject.transform.position.y.ToString();
        rowDataTemp[3] = gameObject.transform.position.z.ToString();
        rowData.Add(rowDataTemp);

        WriteToFile();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        return Application.dataPath + "/CSV" + "ObjectPosotion-" + name + ".csv";
#else
      return Application.dataPath + "/"+"CurrentInfo.csv";
#endif
    }
}
