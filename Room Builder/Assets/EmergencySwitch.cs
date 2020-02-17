using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencySwitch : MonoBehaviour
{
    public AudioSource SosAudio;

    bool bIsSwitchOn = false;

    public void SOSTime()
    {
        SosAudio.loop = true;
        if (!bIsSwitchOn)
        {
            SosAudio.Play();
            bIsSwitchOn = true;
        }
        else if (bIsSwitchOn)
        {
            SosAudio.Stop();
            bIsSwitchOn = false;

        }
    }
}
