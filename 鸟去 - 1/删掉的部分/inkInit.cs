using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class inkInit : MonoBehaviour
{

    // Use this for initialization
    private void Awake()
    {
        gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }

}
