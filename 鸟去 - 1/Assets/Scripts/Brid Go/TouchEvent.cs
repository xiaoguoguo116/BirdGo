using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEvent : MonoBehaviour {
    [HideInInspector]
    public Start start;
    private void OnTriggerEnter2D(Collider2D collider)
    {
       Destroy(this.gameObject); //销毁星星
       Start start= GameObject.Find("GameManager").GetComponent<Start>();    
        start.AddStart();   //调用星星数+1的方法
    }
}
