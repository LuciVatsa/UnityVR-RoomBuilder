using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject[] Lights;
    private bool LightState = true;
    private Component Light;
    public void TurnLight()
    {
        if(LightState)
        {
            foreach(GameObject light in Lights)
            {
                Light = light.GetComponent<FadeLight>();
                light.GetComponent<FadeLight>().SetFadeState(true); 
            }
            LightState = false;
        }
        else
        {
            foreach (GameObject light in Lights)
            {
                Light = light.GetComponent<FadeLight>();
                light.GetComponent<FadeLight>().SetFadeState(false);
            }
            LightState = true;
        }
        
    }
}
