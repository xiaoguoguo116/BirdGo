using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressHightlight : MonoBehaviour {

    Material mat;
    bool isOn = true;
    float lightRate = 0f;
    private void Start()
    {
        mat = this.GetComponent<Image>().material;
        mat.SetFloat("_InnerGlowLerpRate", 0f);
        //this.GetComponent<Image>().material = null;

    }
    public void Turn()
    {
        if (isOn)
        {
            InvokeRepeating("LightOn", 0f, 0.01f);
            //this.GetComponent<Image>().material = mat;    
            isOn = false;
        }
        else
        {
            InvokeRepeating("LightOff", 0f, 0.01f);
            //this.GetComponent<Image>().material = null;
            isOn = true;
        }
    }

    void LightOn()
    {
        lightRate += 0.005f;
        mat.SetFloat("_InnerGlowLerpRate", lightRate);
        if (lightRate >= 0.7f)
        {
            CancelInvoke();
        }
    }

    void LightOff()
    {
        lightRate -= 0.005f;
        mat.SetFloat("_InnerGlowLerpRate", lightRate);
        if (lightRate <= 0f)
        {
            CancelInvoke();
        }
    }
}
