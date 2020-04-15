using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CircleProcess : MonoBehaviour {

    // Use this for initialization
    [HideInInspector]
    public float process;
    private Image image;
	void Start () {
        image = transform.GetChild(0).GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        image.fillAmount = process;
	}
}
