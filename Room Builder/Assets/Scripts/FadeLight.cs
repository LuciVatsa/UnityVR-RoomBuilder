using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeLight : MonoBehaviour
{
    private bool bStartFade = false;
    public float FadeRate = 0.05f;
    void Update()
    {
        if(bStartFade)
        {
            StartCoroutine("Fade");
        } 
        else
        {
            StartCoroutine("UnFade");
        }
    }

    public void SetFadeState(bool FadeState)
    {
        bStartFade = FadeState;
    }

    IEnumerator Fade()
    {
        for (float ft = 1f; ft >= 0; ft -= FadeRate) 
        {
            gameObject.GetComponent<Light>().intensity = ft;
            yield return null;
        }
    }
    IEnumerator UnFade()
    {
        for (float ft = 0f; ft <= 1.4f; ft += FadeRate) 
        {
            gameObject.GetComponent<Light>().intensity = ft;
            yield return null;
        }
    }
}
