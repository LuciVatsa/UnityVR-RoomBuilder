using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject[] Lights;
    private bool LightState = true;
    public void TurnLight()
    {
        if(LightState)
        {
            foreach(GameObject light in Lights)
            {
                light.GetComponent<Light>().intensity = 0;
            }
            LightState = false;
        }
        else
        {
            foreach (GameObject light in Lights)
            {
                light.GetComponent<Light>().intensity = 1;
            }
            LightState = true;
        }
    }
}
