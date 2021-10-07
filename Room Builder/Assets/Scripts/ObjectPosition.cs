using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
public class ObjectPosition : MonoBehaviour
{

    private List<string[]> rowData = new List<string[]>();

    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSecfRJrXAHlYo7pNjUOmGreT-AfRZ-Qg9KMJxb87yCoTUIHow/formResponse";


    // Start is called before the first frame update
    void Start()
    {

        
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
        StartCoroutine(Post(rowData[0], rowData[1], rowData[2]));



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

    IEnumerator Post(string name, string px, string pz)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1972476854", name);
        form.AddField("entry.14134886", px);
        form.AddField("entry.1995358365", pz);

        byte[] rawDataGoogle = form.data;
        WWW www = new WWW(BASE_URL, rawDataGoogle);
        yield return www;
    }
}
