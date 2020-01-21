using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeLight : MonoBehaviour
{
    private bool bStartFade = false;
    GameObject Light;
    private void Start() {
        Light = GetComponent<Light>();
    }
    // Update is called once per frame
    void Update()
    {
        if(bStartFade)
        {
            StartCoroutine("Fade");
        } 
    }

    void SetLightState()
    {
        bStartFade = true;
    }

    IEnumerator Fade()
    {
        if(Light == null)
        {
            Debug.Log("Light Object Not Assigned");
        }
        for (float ft = 1f; ft >= 0; ft -= 0.05f) 
        {
            Light.intensity = ft;
            yield return null;
        }
    }
}

