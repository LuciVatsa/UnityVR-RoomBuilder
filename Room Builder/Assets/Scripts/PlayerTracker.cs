using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class PlayerTracker : MonoBehaviour
{
    bool startRecord = false;
    private List<string[]> rowData = new List<string[]>();
    float preX, preZ;
    double distance = 0.0f;
    
    [SerializeField]
    Transform origin;

    float originX, originY, originZ;

    private float nextActionTime = 0.0f;
    private float period = 0.5f;

    /*search "form action" in after right click and select "view page source"*/
    private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScV0zZv-NrzbAZZDE9ldUbeDckJzTrhgxTMRGZg5usuf5EtGg/formResponse";
    public bool IvPort;
    // Start is called before the first frame update
    void Start()
    {
        origin = FindObjectOfType<ObjectPosition>().origin;
        //Save();
        originX = origin.position.x;
        originY = origin.position.y;
        originZ = origin.position.z;

        string filePath = getPath();
        if (File.Exists(filePath))
            System.IO.File.WriteAllText(filePath, string.Empty);

        filePath = getTposePath();
        if (File.Exists(filePath))
            System.IO.File.WriteAllText(filePath, string.Empty);

        string header = "Object Name,Time,PosX,PosY,PosZ,RotX,RotY,RotZ";
        StartCoroutine(WriteToFile(header, false));
        StartCoroutine(WriteToFile(header, true));

        period = FindObjectOfType<RoomManager>().samplingRate;
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
        //if (Time.time > nextActionTime)
        //{
           // nextActionTime += period;
            float x;
            float y;
            float z;
            //if (IvPort)
            //{
            //    x = gameObject.transform.position.x - originX;
            //    y = gameObject.transform.position.y - originY;
            //    z = gameObject.transform.position.z - originZ;
            //}
            //else
            //{
            //    x = gameObject.transform.localPosition.x + parentX;
            //    y = gameObject.transform.localPosition.y + parentY;
            //    z = gameObject.transform.localPosition.z + parentZ;
            //}

            x = gameObject.transform.position.x - originX;
            y = gameObject.transform.position.y - originY;
            z = gameObject.transform.position.z - originZ;

            string rx = gameObject.transform.rotation.eulerAngles.x.ToString();
            string ry = gameObject.transform.rotation.eulerAngles.y.ToString();
            string rz = gameObject.transform.rotation.eulerAngles.z.ToString();
            
            //StartCoroutine(Post(name, Time.time.ToString(), x.ToString(), y.ToString(), z.ToString(), rx, ry, rz));

            string output = name + "," + Time.time.ToString() + "," + x.ToString() + "," + y.ToString() + "," + z.ToString() + "," + rx + "," + ry + "," + rz;
            StartCoroutine(WriteToFile(output, false));
        //}

        if (Input.GetKeyDown(KeyCode.T) && !IvPort)
        {
            Debug.Log("SAVE T POSE !!!");
            x = gameObject.transform.position.x - originX;
            y = gameObject.transform.position.y - originY;
            z = gameObject.transform.position.z - originZ;
            rx = gameObject.transform.rotation.eulerAngles.x.ToString();
            ry = gameObject.transform.rotation.eulerAngles.y.ToString();
            rz = gameObject.transform.rotation.eulerAngles.z.ToString();
            output = name + "," + Time.time.ToString() + "," + x.ToString() + "," + y.ToString() + "," + z.ToString() + "," + rx + "," + ry + "," + rz;
            StartCoroutine(WriteToFile(output, true));
        }

    }

    IEnumerator WriteToFile(string output, bool isTpose)
    {
        string filePath;
        if (isTpose)
            filePath = getTposePath();
        else
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
        string m_path = path.Item1 + "/Player Data/";
        Directory.CreateDirectory(m_path);
        return m_path + path.Item2 + name + ".csv";

//#else
//      return Application.dataPath + "/"+"CurrentInfo.csv";
//#endif
    }

    private string getTposePath()
    {
//#if UNITY_EDITOR
        
        RoomManager rm = FindObjectOfType<RoomManager>();
        Tuple<string, string> path = rm.GetPath();
        string m_path = path.Item1 + "/Player Data/";
        Directory.CreateDirectory(m_path);
        return m_path + path.Item2 + "Tpose.csv";

//#else
//      return Application.dataPath + "/"+"CurrentInfo.csv";
//#endif
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
