using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject[] Lights;
    private bool LightState = true;
    private GameObject Fade;
    void Start()
    {
        Fade = GetComponent<TurnOffLight>();
    }
    public void TurnLight()
    {
        Debug.Log("I was here");

        if(LightState)
        {
            foreach(GameObject light in Lights)
            {
                light.SetActive(false);
            }
            LightState = false;
        }
        else
        {
            foreach (GameObject light in Lights)
            {
                light.SetActive(true);
            }
            LightState = true;
        }
        
    }
}
               


