using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject HeadwallSlideDoor;
    public GameObject HeadwallSlideDoorPoints;
    public GameObject HeadwallSwingDoor;
    public GameObject HeadwallSwingDoorPoints;
    public GameObject Headwall2SlideDoor;
    public GameObject Headwall2SlideDoorPoints;
    public GameObject Headwall2SwingDoor;
    public GameObject Headwall2SwingDoorPoints;

    public GameObject FootwallSlideDoor;
    public GameObject FootwallSlideDoorPoints;
    public GameObject FootwallSwingDoor;
    public GameObject FootwallSwingDoorPoints;
    public GameObject Footwall2SlideDoor;
    public GameObject Footwall2SlideDoorPoints;
    public GameObject Footwall2SwingDoor;
    public GameObject Footwall2SwingDoorPoints;

    public GameObject Player;

    public Vector3 PlayerLocalPosition = new Vector3(3.787f, 0.0f, 1.797f);
    public int SubjectId;
    public int SetNumber;
    public enum IVenum { IV, NoIV };
    public IVenum IV;
    public enum SideEnum { Left, Right };
    public SideEnum side;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tuple<string, string> GetPath()
    {
        ObjectPosition g = FindObjectOfType<ObjectPosition>();
        string roomname = g.name;
        string folder = Application.dataPath + "/CSV files/" + "Sub" + SubjectId.ToString() + "/Set" + SetNumber.ToString() + "/" + roomname.ToString() + "/" + IV.ToString() + "/" + side.ToString();
        string hierarchyName = "Sub" + SubjectId.ToString() + "_Set" + SetNumber.ToString() + "_" + roomname.ToString() + "_" + IV.ToString() + "_" + side.ToString() + "_";

        return new Tuple<string, string>(folder, hierarchyName);
    }
}
