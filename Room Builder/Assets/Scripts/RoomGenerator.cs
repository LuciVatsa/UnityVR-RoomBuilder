using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RoomGenerator : MonoBehaviour
{
    // a reference to the action
    public SteamVR_Action_Boolean _GenerateRoom;

    // a reference to the hand
    public SteamVR_Input_Sources handType;

    //Reference to Bed
    public GameObject Bed;

    //Reference to Toilet
    public GameObject Toilet;


    public Transform[] SpawnGrid;
    public Transform[] SpawnGridBed;
    public int[] toiletIndex;
    // Start is called before the first frame update
    void Start()
    {
        _GenerateRoom.AddOnStateDownListener(GenerateRoom, handType);

    }

    public void GenerateRoom(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        // first randomly choose a valid position for the bed from the given points 
        int bedPos = Random.Range(0, SpawnGridBed.Length);
        GameObject newBed  = (GameObject)Instantiate(Bed, SpawnGridBed[Random.Range(0, SpawnGridBed.Length)]);
        int rotationfactor = Random.Range(0, 5);
        newBed.transform.localRotation.eulerAngles.Set(0.0f, 90.0f * rotationfactor, 0.0f);
        int bedIndex = int.Parse(SpawnGridBed[bedPos].ToString()) - 1;

        Debug.Log("Here");

        int index = 0;
        //using above information to determine the optimal position for the toilet
        for (int i = 0; i < SpawnGrid.Length; i++)
        {
            //creating deadzone where toilet cant be spawned
            if (((bedIndex - i) % 8 == 0 || (bedIndex - i) % 8 == 1 || (bedIndex - i) % 8 == 2 || (bedIndex - i) % 8 == 6 || (bedIndex - i) % 8 == 7)
                &&( i != bedIndex || i != bedIndex - 1 || i != bedIndex + 1 || i != bedIndex + 8 || i != bedIndex - 8 || i != bedIndex + 7 || i != bedIndex - 7 || i != bedIndex + 9 || i != bedIndex - 9))
            {
                toiletIndex.SetValue(int.Parse(SpawnGrid[i].ToString()), index);
                index++;
            }
        }

        GameObject newToilet = (GameObject)Instantiate(Toilet, SpawnGrid[toiletIndex[Random.Range(0, toiletIndex.Length)]]);
        newToilet.transform.localRotation.eulerAngles.Set(0.0f, 90.0f * rotationfactor, 0.0f);
    }
}
