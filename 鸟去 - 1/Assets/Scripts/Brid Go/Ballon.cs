using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : MonoBehaviour {
    public float speed = 0.7f;//气球上升速度
    GameObject UIPause;        //用于获取游戏是否暂停
	// Use this for initialization
	void Start () {
        UIPause = GameObject.Find("UI Root");
    }
    void Update () {
        if (UIPause.GetComponent<UIManager>().Pause)//游戏暂停时气球不上升
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);//气球速度设为零
            speed = 0;
        }
        else
        {
            speed = 0.7f;
        }
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, speed)); //气球施加上升的力
    }
    
}
