using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTouchRotate : MonoBehaviour {
    public bool IsCanRotate = false;
    public GameObject PSOBJ ;
    public bool PSRotate;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
    void OnMouseDown()
    {
        PSRotate = true;
        IsCanRotate = true;
   
    }
    void OnMouseUp()
    {
        IsCanRotate = false;
      
    }
}
