using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
public class ObjectPosition : MonoBehaviour
{

    private List<string[]> rowData = new List<string[]>();

    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/d/e/1FAIpQLSfn_gFBb0aNqiTKOfoIBcHEExZG8y7Eb1wicrRQzM1N1ZlbnQ/formResponse";


    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(Post("my name", "my time", "1", "2", "3", "4", "5", "6"));

        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = "Object Name";
        rowDataTemp[1] = "PosX";
        rowDataTemp[2] = "PosZ";
        rowData.Add(rowDataTemp);

        int children = transform.childCount;
        for (int i = 0; i < children; ++i) {

            int children_c = transform.GetChild(i).childCount;
            Save(transform.GetChild(i).name, transform.GetChild(i).transform.localPosition.x.ToString(), transform.GetChild(i).transform.localPosition.z.ToString());
            /*
            for (int j = 0; i < children_c; ++i)
            {
                Save(transform.GetChild(i).GetChild(j).name);
            
            }*/
        }

        WriteToFile();
    }

    void Save(string name, string x, string y)
    {


        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = name;
        rowDataTemp[1] = x;
        rowDataTemp[2] = y;
        rowData.Add(rowDataTemp);

       
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
        return Application.dataPath + "/CSV" + "ObjectPosition.csv";
#else
      return Application.dataPath + "/"+"CurrentInfo.csv";
#endif
    }

    IEnumerator Post(string name, string time, string px, string py, string pz, string rx, string ry, string rz)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.632009876", name);
        form.AddField("entry.2030563574", time);
        form.AddField("entry.927362915", px);
        form.AddField("entry.443342963", py);
        form.AddField("entry.2144466818", pz);
        form.AddField("entry.960360919", rx);
        form.AddField("entry.929895134", ry);
        form.AddField("entry.1102053453", rz);

        byte[] rawDataGoogle = form.data;
        WWW www = new WWW(BASE_URL, rawDataGoogle);
        yield return www;
    }
}
