using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

public class CollideTracker : MonoBehaviour
{
    enum Finger { Sphere, Sphere1, Sphere2, Sphere3, thumb_distal, finger_index_2_r, finger_index_1_r, finger_index_0_r, finger_middle_2_r, finger_middle_1_r, finger_middle_0_r, finger_ring_2_r, finger_ring_1_r, finger_pinky_2_r, finger_pinky_1_r };

    //List<HandData> handDatasTmp = new List<HandData>();
    List<HandData> handDatasResults = new List<HandData>();

    HandData[] handDatasTmp = new HandData[15];

    float startTime, endTime;
    /*search "form action" in after right click and select "view page source"*/
    //[SerializeField]
    //private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSewEczoXrGsfBbS_5ByJ7T6bMgo6rKQAHRPjZl6FmAM7DEpOA/formResponse";
    private string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLScwb_zNFMZLWFpufV5x1mm41cfcYPQM4dHPo5JaBuvXuELcUw/formResponse";

    //Valve.VR.InteractionSystem.HandCollider hand;

    private void Start()
    {
        //hand = GetComponent<Valve.VR.InteractionSystem.HandCollider>();

    }

    /*
     * Trigger enter works, but player's hand need to add rigidbody and isTrigger need to be enable
     * Also, this scripts need to be attach to every object that can be collided
     */

    void OnCollisionEnter(Collision collision)
    {
        startTime = Time.time;
        //Debug.Log("Enter! time: " + startTime.ToString());
        Collider myCollider = collision.contacts[0].thisCollider;

        int indexNum;
        switch (myCollider.name)
        {
            case "Sphere":
                indexNum = 0;
                break;
            case "Sphere (1)":
                indexNum = 1;
                break;
            case "Sphere (2)":
                indexNum = 2;
                break;
            case "Sphere (3)":
                indexNum = 3;
                break;
            case "thumb_distal":
                indexNum = 4;
                break;
            case "finger_index_2_r":
                indexNum = 5;
                break;
            case "finger_index_1_r":
                indexNum = 6;
                break;
            case "finger_index_0_r":
                indexNum = 7;
                break;
            case "finger_middle_2_r":
                indexNum = 8;
                break;
            case "finger_middle_1_r":
                indexNum = 9;
                break;
            case "Spherefinger_middle_0_r":
                indexNum = 10;
                break;
            case "finger_ring_2_r":
                indexNum = 11;
                break;
            case "finger_ring_1_r":
                indexNum = 12;
                break;
            case "finger_pinky_2_r":
                indexNum = 13;
                break;
            case "finger_pinky_1_r":
                indexNum = 14;
                break;
            default:
                indexNum = 15;
                break;
        }

        if (indexNum == 15) return;

        if (handDatasTmp[indexNum].hasValue)
        {
            handDatasResults.Add(handDatasTmp[indexNum]);
            handDatasTmp[indexNum].hasValue = false;
        }

        handDatasTmp[indexNum].startTime = startTime;

        Debug.Log("Start Name of collider: " + myCollider.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    void OnCollisionExit(Collision collision)
    {
        endTime = Time.time;

        float totalTime = endTime - startTime;

        ContactPoint[] contacts = new ContactPoint[10];

        // Get the contact points for this collision
        int numContacts = collision.GetContacts(contacts);
        /*
        Collider myCollider = collision.contacts[0].thisCollider;
        Debug.Log("Name of collider: " + myCollider.name);
        */
        // Iterate through each contact point
        for (int i = 0; i < numContacts; i++)
        {
            Debug.Log("other: " + contacts[i].otherCollider.name + ", this: " + contacts[i].thisCollider.name);
        }

        //Debug.Log("other: " + collision.GetContact(0).otherCollider.name + ", this: " + collision.GetContact(0).thisCollider.name);
       
        Debug.Log("\nName: " + name + ", Start: " +  startTime.ToString() + ", End: " +  endTime.ToString() + ", Total: " +  totalTime.ToString()+ ", Collision Object: " + collision.gameObject);
        //StartCoroutine(Post(collision.gameObject.ToString(), name, startTime.ToString(), endTime.ToString(), totalTime.ToString()));
    }

    /*
    void OnTriggerEnter()
    {
        startTime = Time.time;
        Debug.Log("Trigger Enter! time: " + startTime.ToString());
    }

    void OnTriggerExit()
    {
        endTime = Time.time;

        float totalTime = endTime - startTime;
        Debug.Log("Trigger Exit! time: " + endTime.ToString());

        StartCoroutine(Post(name, startTime.ToString(), endTime.ToString(), totalTime.ToString());
    }
    */

    IEnumerator Post(string i_playerContactName, string i_name, string i_startTime, string i_endTime, string i_totalTime)
    {
        WWWForm form = new WWWForm();
        /*
        form.AddField("entry.1156870628", i_playerContactName);
        form.AddField("entry.1219406179", i_name);
        form.AddField("entry.1375789603", i_startTime);
        form.AddField("entry.1681171193", i_endTime);
        form.AddField("entry.1523515611", i_totalTime);*/

        form.AddField("entry.1700357637", i_playerContactName);
        form.AddField("entry.1172557804", i_name);
        form.AddField("entry.1173118163", i_startTime);
        form.AddField("entry.343679423", i_endTime);
        form.AddField("entry.1413142285", i_totalTime);

        byte[] rawDataGoogle = form.data;
        WWW www = new WWW(BASE_URL, rawDataGoogle);
        yield return www;
    }

    struct HandData
    {
        public bool hasValue;
        public Finger finger;
        public float startTime;
        public float totalTime;
    }
}

