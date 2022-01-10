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
    public GameObject playerParent;
    float parentX, parentZ;

    private float nextActionTime = 0.0f;
    private float period = 0.5f;

    //[SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScV0zZv-NrzbAZZDE9ldUbeDckJzTrhgxTMRGZg5usuf5EtGg/formResponse";

    // Start is called before the first frame update
    void Start()
    {
        Save();
        parentX = playerParent.transform.localPosition.x;
        parentZ = playerParent.transform.localPosition.z;
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
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            float x = gameObject.transform.localPosition.x + parentX;
            float z = gameObject.transform.localPosition.z + parentZ;
            StartCoroutine(Post(name, Time.time.ToString(), x.ToString(), z.ToString(), distance.ToString()));
            //Debug.Log("POS: (" + x.ToString() + " , " + z.ToString() + " )");
            // execute block of code here
        }

        
        /*
        if (Input.GetKeyDown("p"))
        {
            if (!startRecord)
            {
                startRecord = true;
                preX = gameObject.transform.localPosition.x;
                preZ = gameObject.transform.localPosition.z;
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
        }*/

    }

    IEnumerator RecordPlayerData()
    {
        while (true)
        {
            //Debug.Log("0");
            float x = gameObject.transform.localPosition.x;
            float z = gameObject.transform.localPosition.z;
            distance += Mathf.Sqrt(Mathf.Pow((x - preX), 2) + Mathf.Pow((z - preZ), 2));
            /*
            string[] rowDataTemp = new string[5];
            rowDataTemp[0] = name;
            rowDataTemp[1] = Time.time.ToString();
            rowDataTemp[2] = x.ToString();
            rowDataTemp[3] = z.ToString();
            rowDataTemp[4] = distance.ToString();*/
            preX = x;
            preZ = z;
            //rowData.Add(rowDataTemp);

            StartCoroutine(Post(name, Time.time.ToString(), x.ToString(), z.ToString(), distance.ToString()));

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
        form.AddField("entry.494714337", name);
        form.AddField("entry.1661933848", time);
        form.AddField("entry.262891016", px);
        form.AddField("entry.254378415", pz);
        form.AddField("entry.1716180825", distance);

        byte[] rawDataGoogle = form.data;
        WWW www = new WWW(BASE_URL, rawDataGoogle);
        yield return www;
    }
}
