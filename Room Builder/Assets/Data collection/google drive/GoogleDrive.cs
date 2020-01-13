using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoogleDrive : MonoBehaviour
{

    [Header("Basic stats")]
    public string item_name;
    public string item_name_ID;
    public string grab_time;
    public string grab_time_ID;
    public string release_time;
    public string release_time_ID;
    public string start_location;
    public string start_location_ID;
    public string end_location;
    public string end_location_ID;

    public void Start()
    {
        item_name = gameObject.name;
        GetComponent<Valve.VR.InteractionSystem.Throwable>().onPickUp.AddListener(grab);
        GetComponent<Valve.VR.InteractionSystem.Throwable>().onDetachFromHand.AddListener(submit);
    }
    public void grab()
    {
        grab_time = Time.deltaTime.ToString();
        start_location = gameObject.transform.position.ToString();
        Debug.Log("grab");
    }

    public void submit()
    {
        release_time = Time.deltaTime.ToString();
        end_location = gameObject.transform.position.ToString();
        StartCoroutine(Post(item_name, grab_time, release_time));
        Debug.Log("submit");
    }

    [SerializeField]
    public string BASE_URL;
    IEnumerator Post(string name, string grab, string release)
    {
        WWWForm form = new WWWForm();

        form.AddField(item_name_ID, name);
        form.AddField(grab_time_ID, grab);
        form.AddField(release_time_ID, release);

        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL + "/formResponse", rawData);
        yield return www;
    }
}
