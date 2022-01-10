using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

public class CollideTracker : MonoBehaviour
{
    public string handLorR;
    enum Finger { Sphere, Sphere1, Sphere2, Sphere3, thumb_distal, finger_index_2_r, finger_index_1_r, finger_index_0_r, finger_middle_2_r, finger_middle_1_r, finger_middle_0_r, finger_ring_2_r, finger_ring_1_r, finger_pinky_2_r, finger_pinky_1_r };
    float elapsed = 0f;

    //List<HandData> handDatasTmp = new List<HandData>();
    List<HandData> handDatasResults = new List<HandData>();

    HandData[] handDatasTmp = new HandData[]
    {
        new HandData { hasValue = false, finger = Finger.Sphere, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.Sphere1, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.Sphere2, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.Sphere3, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.thumb_distal, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.finger_index_2_r, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.finger_index_1_r, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.finger_index_0_r, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.finger_middle_2_r, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.finger_middle_1_r, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.finger_middle_0_r, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.finger_pinky_2_r, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.finger_pinky_1_r, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.finger_ring_2_r, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f},
        new HandData { hasValue = false, finger = Finger.finger_ring_1_r, name = "", startTime = 0.0f, collideObject = "", totalTime = 0.0f}
    };

    float startTime, endTime;
    /*search "form action" in after right click and select "view page source"*/
    //[SerializeField]
    //private string BASE_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSewEczoXrGsfBbS_5ByJ7T6bMgo6rKQAHRPjZl6FmAM7DEpOA/formResponse";
    private string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLScwb_zNFMZLWFpufV5x1mm41cfcYPQM4dHPo5JaBuvXuELcUw/formResponse";

    //Valve.VR.InteractionSystem.HandCollider hand;

    private void Start()
    {
        
    }

    /*
     * Trigger enter works, but player's hand need to add rigidbody and isTrigger need to be enable
     * Also, this scripts need to be attach to every object that can be collided
     */
    /*
    private void Update()
    {
        
        if (Input.GetKeyDown("s"))
        {
            Debug.Log("Press S : Writing to File");

            foreach(var handData in handDatasTmp)
            {
                if (handData.hasValue)
                {
                    handDatasResults.Add(handData);
                }
            }

            foreach(var handData in handDatasResults)
            {
                if(handData.totalTime != 0.0f)
                {
                    StartCoroutine(Post(handData.name, handData.collideObject, handData.startTime.ToString(), (handData.startTime + handData.totalTime).ToString(), handData.totalTime.ToString()));
                    Debug.Log("name:" + handData.name + ", collide object:" + handData.collideObject + ", start time:" + handData.startTime + ", total time: " + handData.totalTime);
                }
                    
            }
        }

        elapsed += Time.deltaTime;
        if (elapsed >= 5f)
        {
            elapsed = elapsed % 5f;
            UpdateData();
        }
    }
        */
    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        foreach (var handData in handDatasTmp)
        {
            if (handData.hasValue)
            {
                handDatasResults.Add(handData);
            }
        }

        foreach (var handData in handDatasResults)
        {
            if (handData.totalTime != 0.0f)
            {
                StartCoroutine(Post(handData.name, handData.collideObject, handData.startTime.ToString(), (handData.startTime + handData.totalTime).ToString(), handData.totalTime.ToString()));
                //Debug.Log("name:" + handData.name + ", collide object:" + handData.collideObject + ", start time:" + handData.startTime + ", total time: " + handData.totalTime);
                
            }

        }
    }

    void UpdateData()
    {
        foreach (var handData in handDatasResults)
        {
            if (handData.totalTime != 0.0f)
            {
                StartCoroutine(Post(handData.name, handData.collideObject, handData.startTime.ToString(), (handData.startTime + handData.totalTime).ToString(), handData.totalTime.ToString()));
                Debug.Log("name:" + handData.name + ", collide object:" + handData.collideObject + ", start time:" + handData.startTime + ", total time: " + handData.totalTime);
                handDatasResults.Remove(handData);
            }

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        startTime = Time.time;
        //Debug.Log("Enter! time: " + startTime.ToString());

        int indexNum = FindIndex(collision);
        //Debug.Log("index number: " + indexNum);
        if (indexNum == 15) return;

        HandData handData = handDatasTmp[indexNum];

        if (handData.hasValue)
        {
            //Debug.Log("contact name ------------------------ " + handData.collideObject);
            //Debug.Log("contain Plane? ------------------------ " + handData.collideObject.Contains("Plane"));

            if (!handData.collideObject.Contains("Collider") && !handData.collideObject.Contains("Plane"))
            {
                //handDatasResults.Add(handData);
                StartCoroutine(Post(handData.name, handData.collideObject, handData.startTime.ToString(), (handData.startTime + handData.totalTime).ToString(), handData.totalTime.ToString()));

            }
            
        }


        handData.hasValue = true;

        Finger f = (Finger)indexNum;

        handData.name = handLorR + ": " + f.ToString();
        handData.collideObject = collision.gameObject.ToString();
        handData.startTime = startTime;

        handDatasTmp[indexNum] = handData;

        //Debug.Log("Start Name of collider: " + myCollider.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        
        int indexNum = FindIndex(collision);
        //Debug.Log("Index num: " + indexNum +" Stay");
        if (indexNum == 15) return;

        float currentTime = Time.time;
        float startTime = handDatasTmp[indexNum].startTime;

        handDatasTmp[indexNum].totalTime = currentTime - startTime;
    }
    /*
    void OnCollisionExit(Collision collision)
    {

        return;

        endTime = Time.time;

        float totalTime = endTime - startTime;

        ContactPoint[] contacts = new ContactPoint[10];

        // Get the contact points for this collision
        int numContacts = collision.GetContacts(contacts);
        
        // Iterate through each contact point
        for (int i = 0; i < numContacts; i++)
        {
            Debug.Log("other: " + contacts[i].otherCollider.name + ", this: " + contacts[i].thisCollider.name);
        }

        //Debug.Log("other: " + collision.GetContact(0).otherCollider.name + ", this: " + collision.GetContact(0).thisCollider.name);
       
        Debug.Log("\nName: " + name + ", Start: " +  startTime.ToString() + ", End: " +  endTime.ToString() + ", Total: " +  totalTime.ToString()+ ", Collision Object: " + collision.gameObject);
        //StartCoroutine(Post(collision.gameObject.ToString(), name, startTime.ToString(), endTime.ToString(), totalTime.ToString()));
    }*/


    int FindIndex(Collision collision)
    {
        Collider myCollider = collision.contacts[0].thisCollider;

        int indexNum;

        //Debug.Log("myCollider name:" + myCollider.name);
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

        return indexNum;
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
        public string name;
        public string collideObject;
        public bool hasValue;
        public Finger finger;
        public float startTime;
        public float totalTime;
    }
}

