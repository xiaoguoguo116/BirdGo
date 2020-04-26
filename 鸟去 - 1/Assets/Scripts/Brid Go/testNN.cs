using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testNN : MonoBehaviour {

     bool canPs;
    void Start()
    {
        canPs = this.GetComponent<CanGhosting>().canPs;  //获取此时物体是否在可移动状态

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
