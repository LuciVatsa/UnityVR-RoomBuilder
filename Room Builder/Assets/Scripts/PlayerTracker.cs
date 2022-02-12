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
    float parentX, parentY, parentZ;

    private float nextActionTime = 0.0f;
    private float period = 0.5f;

    /*search "form action" in after right click and select "view page source"*/
    private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScV0zZv-NrzbAZZDE9ldUbeDckJzTrhgxTMRGZg5usuf5EtGg/formResponse";

    // Start is called before the first frame update
    void Start()
    {
        //Save();
        parentX = playerParent.transform.localPosition.x;
        parentY = playerParent.transform.localPosition.y;
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
            float y = gameObject.transform.localPosition.y + parentY;
            float z = gameObject.transform.localPosition.z + parentZ;
            string rx = gameObject.transform.rotation.eulerAngles.x.ToString();
            string ry = gameObject.transform.rotation.eulerAngles.y.ToString();
            string rz = gameObject.transform.rotation.eulerAngles.z.ToString();
            
            string lrx = gameObject.transform.localRotation.eulerAngles.x.ToString();
            string lry = gameObject.transform.localRotation.eulerAngles.y.ToString();
            string lrz = gameObject.transform.localRotation.eulerAngles.z.ToString();

            //Debug.Log("rotation global: (" + rx + ", " + ry + ", " + rz + ") local: (" + lrx + ", " + lry + ", " + lrz + ")");
            
            StartCoroutine(Post(name, Time.time.ToString(), x.ToString(), y.ToString(), z.ToString(), rx, ry, rz));

            string[] rowDataTemp = new string[5];
            StartCoroutine(WriteToFile(rowDataTemp));
        }

        //Debug.Log("rotation x: " + gameObject.transform.rotation.eulerAngles.x.ToString() + "rotation y: " + gameObject.transform.rotation.eulerAngles.y.ToString() + "rotation z: " + gameObject.transform.rotation.eulerAngles.z.ToString());


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
    /*
    IEnumerator RecordPlayerData()
    {
        while (true)
        {
            //Debug.Log("0");
            float x = gameObject.transform.localPosition.x;
            float z = gameObject.transform.localPosition.z;
            distance += Mathf.Sqrt(Mathf.Pow((x - preX), 2) + Mathf.Pow((z - preZ), 2));
            
            string[] rowDataTemp = new string[5];
            rowDataTemp[0] = name;
            rowDataTemp[1] = Time.time.ToString();
            rowDataTemp[2] = x.ToString();
            rowDataTemp[3] = z.ToString();
            rowDataTemp[4] = distance.ToString();
            preX = x;
            preZ = z;
            //rowData.Add(rowDataTemp);

            StartCoroutine(Post(name, Time.time.ToString(), x.ToString(), y.ToString(), z.ToString()));

            yield return null;
        }
    }*/

    IEnumerator WriteToFile(string[] output)
    {
        Debug.Log("Writing to file Now");

        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        sb.AppendLine(string.Join(delimiter, output[0]));
        
        string filePath = getPath();
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        Debug.Log("Finished Writing to File");
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

    IEnumerator Post(string name, string time, string px, string py, string pz, string rx, string ry, string rz)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.494714337", name);
        form.AddField("entry.1661933848", time);
        form.AddField("entry.262891016", px);
        form.AddField("entry.254378415", py);
        form.AddField("entry.1716180825", pz);
        form.AddField("entry.840493581", rx);
        form.AddField("entry.14936832", ry);
        form.AddField("entry.638208784", rz);

        byte[] rawDataGoogle = form.data;
        WWW www = new WWW(BASE_URL, rawDataGoogle);
        yield return www;
    }
}
