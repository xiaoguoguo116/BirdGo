using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickWave : MonoBehaviour {

    RippleEffect effect;
	// Use this for initialization
	void Start () {
        effect = this.GetComponent<RippleEffect>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPostion = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            effect.Emit(clickPostion);
        }
    }
}
