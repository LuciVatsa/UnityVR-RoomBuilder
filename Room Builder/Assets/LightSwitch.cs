using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject Light;
    private bool LightState = true;
    private void OnCollisionEnter(Collision collision)
    {
        if(LightState)
        {
            Light.SetActive(false);
            LightState = false;
        }
        else
        {
            Light.SetActive(true);
            LightState = true;
        }
        
    }
}
               


