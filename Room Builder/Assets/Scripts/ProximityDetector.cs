using System.IO;
using System.Collections;
using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    float startTime;
    private void Start()
    {
        startTime = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        startTime = Time.time;
    }

    private void OnCollisionExit(Collision collision)
    {
        using (StreamWriter sw = new StreamWriter("Assets/Patient_Proximity_Info.txt", append: true))
        {
            sw.Write("Proximity Alert to object: "); sw.WriteLine(this.name);
            sw.Write("Body Part involved: "); sw.WriteLine(collision.gameObject.name);
            sw.Write("Distance between objects: "); sw.WriteLine(Mathf.Abs((this.transform.position - collision.gameObject.transform.position).magnitude));
            sw.Write("Collision at time: "); sw.WriteLine(Time.time.ToString());
            sw.Write("Proximity Duration: "); sw.WriteLine((Time.time - startTime).ToString());
            sw.WriteLine("--------------------------------------------------------------------------------");
        }
    }
}
