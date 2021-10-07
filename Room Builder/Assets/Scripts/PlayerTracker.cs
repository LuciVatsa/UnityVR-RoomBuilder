using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class PlayerTracker : MonoBehaviour
{
    bool startRecord = false;
    private List<string[]> rowData = new List<string[]>();
    float preX, preZ;
    double distance = 0.0f;

    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSdHGDTxBF0qNXewS4Q4BSRmcuJvSxzL5OJvBiyBSZAfg4TSow/formResponse";

    // Start is called before the first frame update
    void Start()
    {
        Save();
    }
    void Save()
    {
        string[] rowDataTemp = new string[5];
        rowDataTemp[0] = "Object Name";
        rowDataTemp[1] = "Time";
        rowDataTemp[2] = "PosX";
        rowDataTemp[3] = "PosZ";
        rowDataTemp[4] = "Distance";
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
                preZ = gameObject.transform.position.z;
                distance = 0.0f;
                StartCoroutine(RecordPlayerData());
            }
            else
            {
                Debug.Log("stop");
                StopCoroutine(RecordPlayerData());
                WriteToFile();
                startRecord = false;
            }
        }

    }

    IEnumerator RecordPlayerData()
    {
        while (true)
        {
            Debug.Log("0");
            float x = gameObject.transform.position.x;
            float z = gameObject.transform.position.z;
            distance += Mathf.Sqrt(Mathf.Pow((x - preX), 2) + Mathf.Pow((z - preZ), 2));
            string[] rowDataTemp = new string[5];
            rowDataTemp[0] = name;
            rowDataTemp[1] = Time.time.ToString();
            rowDataTemp[2] = x.ToString();
            rowDataTemp[3] = z.ToString();
            rowDataTemp[4] = distance.ToString();
            preX = x;
            preZ = z;
            rowData.Add(rowDataTemp);

            StartCoroutine(Post(rowData[0], rowData[1], rowData[2], rowData[3], rowData[4]));

            yield return null;
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

    IEnumerator Post(string name, string time, string px, string pz, string distance)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.801113195", name);
        form.AddField("entry.1584820276", time);
        form.AddField("entry.299970136", px);
        form.AddField("entry.2112585411", pz);
        form.AddField("entry.696155006", distance);

        byte[] rawDataGoogle = form.data;
        WWW www = new WWW(BASE_URL, rawDataGoogle);
        yield return www;
    }
}
