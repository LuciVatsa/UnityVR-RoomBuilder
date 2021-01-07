using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Valve.VR;
public class DistanceCalculator : MonoBehaviour
{
    private Transform prevTransform;
    private float totalDist;
    private float CurrentDist;
    private float TotalTime;
    private int bStart;
    private List<string[]> rowData = new List<string[]>();

    // a reference to the action
    public SteamVR_Action_Boolean _RecordData;

    // a reference to the hand
    public SteamVR_Input_Sources handType;

    // Start is called before the first frame update
    void Start()
    {
        Save();
        bStart = 0;
        prevTransform = gameObject.transform;
        _RecordData.AddOnStateDownListener(RecordData, handType);
        CurrentDist = 0;
    }

    public void RecordData(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        bStart += 1;

    }
    void Save()
    {
        string[] rowDataTemp = new string[3];
        rowDataTemp[0] = "Object Name";
        rowDataTemp[1] = "Time";
        rowDataTemp[2] = "Distance";
        rowData.Add(rowDataTemp);
    }

    // Update is called once per frame
    void Update()
    {
        if(bStart==1)
        { 
            TotalTime += Time.deltaTime;
            if (gameObject.transform.position.magnitude > prevTransform.position.magnitude)
            {
                CurrentDist += Vector3.Distance(gameObject.transform.position, prevTransform.position);
                prevTransform = gameObject.transform;
            }
            else if(prevTransform.position.magnitude > gameObject.transform.position.magnitude)
            {
                CurrentDist += Vector3.Distance(prevTransform.position, gameObject.transform.position);
            }
            totalDist += CurrentDist;
        }
        else if (Input.GetKeyDown("w"))
        {
            Debug.Log(totalDist.ToString() + "  " + TotalTime.ToString());
            bStart = 0;
            string[] rowdataTemp = new string[3];
            rowdataTemp[0] = name;
            rowdataTemp[1] = TotalTime.ToString();
            rowdataTemp[2] = totalDist.ToString();
            WriteToFile();
        }
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
        string filePath = GetPath();
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        Debug.Log("Finished Writing to File");
    }

    private string GetPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "ObjectData" + name + ".txt";
#else
      return Application.dataPath + "/"+"CurrentInfo.csv";
#endif
    }
}
