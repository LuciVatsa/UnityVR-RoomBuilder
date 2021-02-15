using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour
{
    public Transform Player;
    public Transform RoomLocation;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Room Check");
        Player.position = RoomLocation.position;
    }
}
