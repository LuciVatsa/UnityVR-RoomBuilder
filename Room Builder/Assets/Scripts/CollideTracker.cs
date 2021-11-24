﻿using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

public class CollideTracker : MonoBehaviour
{
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
    }

    void OnCollisionExit(Collision collision)
    {
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
}
