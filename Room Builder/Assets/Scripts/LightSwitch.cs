using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject[] Lights;
    private bool LightState = true;
    private GameObject Light;
    public void TurnLight()
    {
        Debug.Log("I was here");
        if(LightState)
        {
            foreach(GameObject light in Lights)
            {
                Light = light.GetComponent<FadeLight>();
                if(Light == null) {  light.SetActive(false); }
                else { light.GetComponent<FadeLight>().SetLightState(); }
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
               


