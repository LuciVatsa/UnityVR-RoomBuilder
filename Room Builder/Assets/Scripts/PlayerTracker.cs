using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    bool startRecord = false;
    bool isRecording = false;
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
        string[] rowDataTemp = new string[9];
        rowDataTemp[0] = "Object Name";
        rowDataTemp[1] = "Time";
        rowDataTemp[2] = "PosX";
        rowDataTemp[3] = "PosY";
        rowDataTemp[4] = "PosZ";
        rowDataTemp[5] = "RotX";
        rowDataTemp[6] = "RotY";
        rowDataTemp[7] = "RotZ";
        rowDataTemp[8] = "Distance";
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
            }
        }

    }

    IEnumerator RecordPlayerData()
    {
        startRecord = false;
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        distance += Mathf.Sqrt(Mathf.Pow((x - preX), 2) + Mathf.Pow((y - preY), 2));
        string[] rowDataTemp = new string[8];
        rowDataTemp[0] = name;
        rowDataTemp[1] = Time.time.ToString();
        rowDataTemp[2] = x.ToString();
        rowDataTemp[3] = y.ToString();
        rowDataTemp[4] = gameObject.transform.position.z.ToString();
        rowDataTemp[5] = gameObject.transform.rotation.eulerAngles.x.ToString();
        rowDataTemp[6] = gameObject.transform.rotation.eulerAngles.y.ToString();
        rowDataTemp[7] = gameObject.transform.rotation.eulerAngles.z.ToString();
        rowDataTemp[8] = gameObject.transform.rotation.eulerAngles.z.ToString();
        rowDataTemp[9] = distance.ToString();
        preX = x;
        preY = y;
        rowData.Add(rowDataTemp);
        yield return new WaitForSeconds(1.0f); ;
    }
}
