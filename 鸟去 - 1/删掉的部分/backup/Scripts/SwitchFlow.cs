using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchFlow : Switch
{
    GameObject Flow1, Flow2;

    public override void SwitchUpdate(bool turnOn)
    {
        Flow1 = base.Reference[0];
        Flow2 = base.Reference[1];
        Flow1.SetActive(!turnOn);
        Flow2.SetActive(turnOn);
    }
}
