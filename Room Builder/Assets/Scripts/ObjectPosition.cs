using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
public class ObjectPosition : MonoBehaviour
{

    private List<string[]> rowData = new List<string[]>();

    //[SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSckeScN9oBB87ntLwAq31_CRvH70n2IhGAFad11Yq1K9liJ-A/formResponse";

    string title;
    public Transform origin;

    // Start is called before the first frame update
    void Start()
    {
        title = this.ToString();
        
        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = "Object Name";
        rowDataTemp[1] = "PosX";
        rowDataTemp[2] = "PosY";
        rowDataTemp[3] = "PosZ";
        rowData.Add(rowDataTemp);

        Transform[] allchildren = transform.GetComponentsInChildren<Transform>();
        for(int i = 0; i < allchildren.Length; i++)
        {
            //Debug.Log(allchildren[i].name);
            Transform child = allchildren[i];
            if(child.tag == "ObjectPosition")
            {
                string x = (child.position.x - origin.position.x).ToString();
                string y = (child.position.y - origin.position.y).ToString();
                string z = (child.position.z - origin.position.z).ToString();
                //Debug.Log("[" + title + "] " + child.name + ": (" + x + ", " + y + ", " + z + ")");

                Save("[" + title + "] " + child.name, x, y, z);

            }
        }

        //int children = transform.childCount;
        //for (int i = 0; i < children; ++i) {

        //    int children_c = transform.GetChild(i).childCount;
        //    Save("[" + title + "] " + transform.GetChild(i).name, transform.GetChild(i).transform.localPosition.x.ToString(), transform.GetChild(i).transform.localPosition.z.ToString());

        //}

        string filePath = getPath();
        if (File.Exists(filePath))
            System.IO.File.WriteAllText(filePath, string.Empty);

        WriteToFile();
    }

    void Save(string name, string x, string y, string z)
    {


        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = name;
        rowDataTemp[1] = x;
        rowDataTemp[2] = y;
        rowDataTemp[3] = z;
        rowData.Add(rowDataTemp);
       // StartCoroutine(Post(name, x, y));



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

    IEnumerator WriteToFile(string output)
    {
        string filePath = getPath();
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

    private string getPath()
    {
#if UNITY_EDITOR
        //string s = this.gameObject.name;
        //int found = s.IndexOf(" obj");
        //string roomname = s.Substring(0, found);
        RoomManager rm = FindObjectOfType<RoomManager>();
        System.Tuple<string, string> path = rm.GetPath();
        string m_path = path.Item1 + "/Object Position/";
        Directory.CreateDirectory(m_path);
        return m_path + path.Item2 + "obj points.csv";

        //return Application.dataPath + "/CSV files/" + this.gameObject.name + "/Object Position /" + this.gameObject.name +" obj points.csv";
#else
      return Application.dataPath + "/"+"CurrentInfo.csv";
#endif
    }

    IEnumerator Post(string name, string px, string pz)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.329330472", name);
        form.AddField("entry.664238963", px);
        form.AddField("entry.658431416", pz);

        byte[] rawDataGoogle = form.data;
        WWW www = new WWW(BASE_URL, rawDataGoogle);
        yield return www;
    }
}
