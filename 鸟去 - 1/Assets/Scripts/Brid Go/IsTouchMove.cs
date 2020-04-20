using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTouchMove : MonoBehaviour {
    public bool IsCanMove = false;
    public bool PSMove; 
    // Use this for initialization
    public GameObject move; 
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    void OnMouseDown()
    {
        PSMove = true;
        IsCanMove = true;
        
        
    }
    void OnMouseUp()
    {
        IsCanMove = false;
    
    }
}
