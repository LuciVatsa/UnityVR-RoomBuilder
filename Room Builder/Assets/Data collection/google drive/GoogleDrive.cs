using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleDrive : MonoBehaviour
{

    string item_name;
    float grab_time;
    float release_time;
    Vector3 start_location;
    Vector3 end_location;
    
    public void Start()
    {
        item_name = gameObject.name;
        GetComponent<Valve.VR.InteractionSystem.Throwable>().onPickUp.AddListener(Grab);
        GetComponent<Valve.VR.InteractionSystem.Throwable>().onDetachFromHand.AddListener(Submit);
    }
    public void Grab()
    {
        grab_time = Time.time;
        start_location = gameObject.transform.position;
        Debug.Log("grab");
    }

    public void Submit()
    {
        release_time = Time.time;
        end_location = gameObject.transform.position;
        StartCoroutine(Post(item_name, grab_time, release_time, start_location, end_location));
        Debug.Log("submit");
    }
    IEnumerator Post(string name, float grab_time, float release_time, Vector3 start_location, Vector3 end_location)
    {

        using (StreamWriter sw = new StreamWriter("Assets/Patient_Interaction_Info.txt", append:true))
        {
            sw.Write("Object Name: "); sw.WriteLine(name);
            sw.Write("Object Start Location: "); sw.WriteLine(start_location);
            sw.Write("Object End Location: "); sw.WriteLine(end_location);
            sw.Write("Distance Travelled: "); sw.WriteLine(end_location - start_location);
            sw.Write("Grab Time: "); sw.WriteLine(release_time - grab_time);
            sw.WriteLine("----------------------------------------------------------------");
        }
        yield return null;
    }
}
