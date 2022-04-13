using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MenuManager : Editor
{
    //private static Dictionary<string, MonoScript> AllScripts = new Dictionary<string, MonoScript>();
    //List<Component> _scriptsList = new List<Component>();


    [MenuItem("VR/Data/Enable Data Scripts")]
    static void EnableScripts()
    {
        GameObject[] gameobjects;
        gameobjects = GameObject.FindGameObjectsWithTag("DataCollect");
        var head = GameObject.FindGameObjectWithTag("MainCamera");

        foreach(var go in gameobjects)
        {
            if (go.GetComponent<PlayerTracker>() != null)
                go.GetComponent<PlayerTracker>().enabled = true;

            if (go.GetComponent<ObjectPosition> () != null)
                go.GetComponent<ObjectPosition>().enabled = true;

            if (go.GetComponent<BodyCollideTracker>() != null)
                go.GetComponent<BodyCollideTracker>().enabled = true;
        }

        if (head.GetComponent<PlayerTracker>() != null)
            head.GetComponent<PlayerTracker>().enabled = true;

    }

    [MenuItem("VR/Data/Disable Data Scripts")]
    static void DisableScripts()
    {
        GameObject[] gameobjects;
        gameobjects = GameObject.FindGameObjectsWithTag("DataCollect");
        //var obj = GameObject.FindGameObjectWithTag("DataCollect");
        var head = GameObject.FindGameObjectWithTag("MainCamera");

        foreach (var go in gameobjects)
        {
            if (go.GetComponent<PlayerTracker>() != null)
                go.GetComponent<PlayerTracker>().enabled = false;

            if (go.GetComponent<ObjectPosition>() != null)
                go.GetComponent<ObjectPosition>().enabled = false;

            if (go.GetComponent<BodyCollideTracker>() != null)
                go.GetComponent<BodyCollideTracker>().enabled = false;
        }

        if (head.GetComponent<PlayerTracker>() != null)
            head.GetComponent<PlayerTracker>().enabled = false;

    }

    [MenuItem("VR/Room/Headwall Slide Door")]
    static void HeadwallSlideDoor()
    {
        RoomManager myObject = FindObjectOfType<RoomManager>();

        myObject.HeadwallSlideDoor.SetActive(true);
        myObject.HeadwallSlideDoorPoints.SetActive(true);
        myObject.HeadwallSwingDoor.SetActive(false);
        myObject.HeadwallSwingDoorPoints.SetActive(false); 
        myObject.Headwall2SlideDoor.SetActive(false);
        myObject.Headwall2SlideDoorPoints.SetActive(false);
        myObject.Headwall2SwingDoor.SetActive(false);
        myObject.Headwall2SwingDoorPoints.SetActive(false);

        myObject.FootwallSlideDoor.SetActive(false);
        myObject.FootwallSlideDoorPoints.SetActive(false);
        myObject.FootwallSwingDoor.SetActive(false);
        myObject.FootwallSwingDoorPoints.SetActive(false);
        myObject.Footwall2SlideDoor.SetActive(false);
        myObject.Footwall2SlideDoorPoints.SetActive(false);
        myObject.Footwall2SwingDoor.SetActive(false);
        myObject.Footwall2SwingDoorPoints.SetActive(false);

        myObject.Player.transform.SetParent(myObject.HeadwallSlideDoorPoints.transform);
        myObject.Player.transform.localPosition = myObject.PlayerLocalPosition;

    }

    [MenuItem("VR/Room/Headwall Swing Door")]
    static void HeadwallSwingDoor()
    {
        RoomManager myObject = FindObjectOfType<RoomManager>();

        myObject.HeadwallSlideDoor.SetActive(false);
        myObject.HeadwallSlideDoorPoints.SetActive(false);
        myObject.HeadwallSwingDoor.SetActive(true);
        myObject.HeadwallSwingDoorPoints.SetActive(true);
        myObject.Headwall2SlideDoor.SetActive(false);
        myObject.Headwall2SlideDoorPoints.SetActive(false);
        myObject.Headwall2SwingDoor.SetActive(false);
        myObject.Headwall2SwingDoorPoints.SetActive(false);

        myObject.FootwallSlideDoor.SetActive(false);
        myObject.FootwallSlideDoorPoints.SetActive(false);
        myObject.FootwallSwingDoor.SetActive(false);
        myObject.FootwallSwingDoorPoints.SetActive(false);
        myObject.Footwall2SlideDoor.SetActive(false);
        myObject.Footwall2SlideDoorPoints.SetActive(false);
        myObject.Footwall2SwingDoor.SetActive(false);
        myObject.Footwall2SwingDoorPoints.SetActive(false);

        myObject.Player.transform.SetParent(myObject.HeadwallSwingDoorPoints.transform);
        myObject.Player.transform.localPosition = myObject.PlayerLocalPosition;
    }

    [MenuItem("VR/Room/Headwall-2 Slide Door")]
    static void Headwall2SlideDoor()
    {
        RoomManager myObject = FindObjectOfType<RoomManager>();

        myObject.HeadwallSlideDoor.SetActive(false);
        myObject.HeadwallSlideDoorPoints.SetActive(false);
        myObject.HeadwallSwingDoor.SetActive(false);
        myObject.HeadwallSwingDoorPoints.SetActive(false);
        myObject.Headwall2SlideDoor.SetActive(true);
        myObject.Headwall2SlideDoorPoints.SetActive(true);
        myObject.Headwall2SwingDoor.SetActive(false);
        myObject.Headwall2SwingDoorPoints.SetActive(false);

        myObject.FootwallSlideDoor.SetActive(false);
        myObject.FootwallSlideDoorPoints.SetActive(false);
        myObject.FootwallSwingDoor.SetActive(false);
        myObject.FootwallSwingDoorPoints.SetActive(false);
        myObject.Footwall2SlideDoor.SetActive(false);
        myObject.Footwall2SlideDoorPoints.SetActive(false);
        myObject.Footwall2SwingDoor.SetActive(false);
        myObject.Footwall2SwingDoorPoints.SetActive(false);

        myObject.Player.transform.SetParent(myObject.Headwall2SlideDoorPoints.transform);
        myObject.Player.transform.localPosition = myObject.PlayerLocalPosition;
    }

    [MenuItem("VR/Room/Headwall-2 Swing Door")]
    static void Headwall2SwingDoor()
    {
        RoomManager myObject = FindObjectOfType<RoomManager>();

        myObject.HeadwallSlideDoor.SetActive(false);
        myObject.HeadwallSlideDoorPoints.SetActive(false);
        myObject.HeadwallSwingDoor.SetActive(false);
        myObject.HeadwallSwingDoorPoints.SetActive(false);
        myObject.Headwall2SlideDoor.SetActive(false);
        myObject.Headwall2SlideDoorPoints.SetActive(false);
        myObject.Headwall2SwingDoor.SetActive(true);
        myObject.Headwall2SwingDoorPoints.SetActive(true);

        myObject.FootwallSlideDoor.SetActive(false);
        myObject.FootwallSlideDoorPoints.SetActive(false);
        myObject.FootwallSwingDoor.SetActive(false);
        myObject.FootwallSwingDoorPoints.SetActive(false);
        myObject.Footwall2SlideDoor.SetActive(false);
        myObject.Footwall2SlideDoorPoints.SetActive(false);
        myObject.Footwall2SwingDoor.SetActive(false);
        myObject.Footwall2SwingDoorPoints.SetActive(false);

        myObject.Player.transform.SetParent(myObject.Headwall2SwingDoorPoints.transform);
        myObject.Player.transform.localPosition = myObject.PlayerLocalPosition;
    }

    [MenuItem("VR/Room/Footwall Slide Door")]
    static void FootwallSlideDoor()
    {
        RoomManager myObject = FindObjectOfType<RoomManager>();

        myObject.HeadwallSlideDoor.SetActive(false);
        myObject.HeadwallSlideDoorPoints.SetActive(false);
        myObject.HeadwallSwingDoor.SetActive(false);
        myObject.HeadwallSwingDoorPoints.SetActive(false);
        myObject.Headwall2SlideDoor.SetActive(false);
        myObject.Headwall2SlideDoorPoints.SetActive(false);
        myObject.Headwall2SwingDoor.SetActive(false);
        myObject.Headwall2SwingDoorPoints.SetActive(false);

        myObject.FootwallSlideDoor.SetActive(true);
        myObject.FootwallSlideDoorPoints.SetActive(true);
        myObject.FootwallSwingDoor.SetActive(false);
        myObject.FootwallSwingDoorPoints.SetActive(false);
        myObject.Footwall2SlideDoor.SetActive(false);
        myObject.Footwall2SlideDoorPoints.SetActive(false);
        myObject.Footwall2SwingDoor.SetActive(false);
        myObject.Footwall2SwingDoorPoints.SetActive(false);

        myObject.Player.transform.SetParent(myObject.FootwallSlideDoorPoints.transform);
        myObject.Player.transform.localPosition = myObject.PlayerLocalPosition;
    }

    [MenuItem("VR/Room/Footwall Swing Door")]
    static void FootwallSwingDoor()
    {
        RoomManager myObject = FindObjectOfType<RoomManager>();

        myObject.HeadwallSlideDoor.SetActive(false);
        myObject.HeadwallSlideDoorPoints.SetActive(false);
        myObject.HeadwallSwingDoor.SetActive(false);
        myObject.HeadwallSwingDoorPoints.SetActive(false);
        myObject.Headwall2SlideDoor.SetActive(false);
        myObject.Headwall2SlideDoorPoints.SetActive(false);
        myObject.Headwall2SwingDoor.SetActive(false);
        myObject.Headwall2SwingDoorPoints.SetActive(false);

        myObject.FootwallSlideDoor.SetActive(false);
        myObject.FootwallSlideDoorPoints.SetActive(false);
        myObject.FootwallSwingDoor.SetActive(true);
        myObject.FootwallSwingDoorPoints.SetActive(true);
        myObject.Footwall2SlideDoor.SetActive(false);
        myObject.Footwall2SlideDoorPoints.SetActive(false);
        myObject.Footwall2SwingDoor.SetActive(false);
        myObject.Footwall2SwingDoorPoints.SetActive(false);

        myObject.Player.transform.SetParent(myObject.FootwallSwingDoorPoints.transform);
        myObject.Player.transform.localPosition = myObject.PlayerLocalPosition;
    }

    [MenuItem("VR/Room/Footwall-2 Slide Door")]
    static void Footwall2SlideDoor()
    {
        RoomManager myObject = FindObjectOfType<RoomManager>();

        myObject.HeadwallSlideDoor.SetActive(false);
        myObject.HeadwallSlideDoorPoints.SetActive(false);
        myObject.HeadwallSwingDoor.SetActive(false);
        myObject.HeadwallSwingDoorPoints.SetActive(false);
        myObject.Headwall2SlideDoor.SetActive(false);
        myObject.Headwall2SlideDoorPoints.SetActive(false);
        myObject.Headwall2SwingDoor.SetActive(false);
        myObject.Headwall2SwingDoorPoints.SetActive(false);

        myObject.FootwallSlideDoor.SetActive(false);
        myObject.FootwallSlideDoorPoints.SetActive(false);
        myObject.FootwallSwingDoor.SetActive(false);
        myObject.FootwallSwingDoorPoints.SetActive(false);
        myObject.Footwall2SlideDoor.SetActive(true);
        myObject.Footwall2SlideDoorPoints.SetActive(true);
        myObject.Footwall2SwingDoor.SetActive(false);
        myObject.Footwall2SwingDoorPoints.SetActive(false);

        myObject.Player.transform.SetParent(myObject.Footwall2SlideDoorPoints.transform);
        myObject.Player.transform.localPosition = myObject.PlayerLocalPosition;
    }

    [MenuItem("VR/Room/Footwall-2 Swing Door")]
    static void Footwall2SwingDoor()
    {
        RoomManager myObject = FindObjectOfType<RoomManager>();

        myObject.HeadwallSlideDoor.SetActive(false);
        myObject.HeadwallSlideDoorPoints.SetActive(false);
        myObject.HeadwallSwingDoor.SetActive(false);
        myObject.HeadwallSwingDoorPoints.SetActive(false);
        myObject.Headwall2SlideDoor.SetActive(false);
        myObject.Headwall2SlideDoorPoints.SetActive(false);
        myObject.Headwall2SwingDoor.SetActive(false);
        myObject.Headwall2SwingDoorPoints.SetActive(false);

        myObject.FootwallSlideDoor.SetActive(false);
        myObject.FootwallSlideDoorPoints.SetActive(false);
        myObject.FootwallSwingDoor.SetActive(false);
        myObject.FootwallSwingDoorPoints.SetActive(false);
        myObject.Footwall2SlideDoor.SetActive(false);
        myObject.Footwall2SlideDoorPoints.SetActive(false);
        myObject.Footwall2SwingDoor.SetActive(true);
        myObject.Footwall2SwingDoorPoints.SetActive(true);

        myObject.Player.transform.SetParent(myObject.Footwall2SwingDoorPoints.transform);
        myObject.Player.transform.localPosition = myObject.PlayerLocalPosition;
    }

    [MenuItem("VR/Room/Enable All")]
    static void EnableAll()
    {
        RoomManager myObject = FindObjectOfType<RoomManager>();

        myObject.HeadwallSlideDoor.SetActive(true);
        myObject.HeadwallSlideDoorPoints.SetActive(true);
        myObject.HeadwallSwingDoor.SetActive(true);
        myObject.HeadwallSwingDoorPoints.SetActive(true);
        myObject.Headwall2SlideDoor.SetActive(true);
        myObject.Headwall2SlideDoorPoints.SetActive(true);
        myObject.Headwall2SwingDoor.SetActive(true);
        myObject.Headwall2SwingDoorPoints.SetActive(true);

        myObject.FootwallSlideDoor.SetActive(true);
        myObject.FootwallSlideDoorPoints.SetActive(true);
        myObject.FootwallSwingDoor.SetActive(true);
        myObject.FootwallSwingDoorPoints.SetActive(true);
        myObject.Footwall2SlideDoor.SetActive(true);
        myObject.Footwall2SlideDoorPoints.SetActive(true);
        myObject.Footwall2SwingDoor.SetActive(true);
        myObject.Footwall2SwingDoorPoints.SetActive(true);

    }

    [MenuItem("VR/Room/Disable All")]
    static void DisableAll()
    {
        RoomManager myObject = FindObjectOfType<RoomManager>();

        myObject.HeadwallSlideDoor.SetActive(false);
        myObject.HeadwallSlideDoorPoints.SetActive(false);
        myObject.HeadwallSwingDoor.SetActive(false);
        myObject.HeadwallSwingDoorPoints.SetActive(false);
        myObject.Headwall2SlideDoor.SetActive(false);
        myObject.Headwall2SlideDoorPoints.SetActive(false);
        myObject.Headwall2SwingDoor.SetActive(false);
        myObject.Headwall2SwingDoorPoints.SetActive(false);

        myObject.FootwallSlideDoor.SetActive(false);
        myObject.FootwallSlideDoorPoints.SetActive(false);
        myObject.FootwallSwingDoor.SetActive(false);
        myObject.FootwallSwingDoorPoints.SetActive(false);
        myObject.Footwall2SlideDoor.SetActive(false);
        myObject.Footwall2SlideDoorPoints.SetActive(false);
        myObject.Footwall2SwingDoor.SetActive(false);
        myObject.Footwall2SwingDoorPoints.SetActive(false);

    }
}
