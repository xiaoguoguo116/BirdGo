using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star : MonoBehaviour {

    GameObject UIPause;
    void Start()
    {
        UIPause = GameObject.Find("UI Root");

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (UIPause.GetComponent<UIManager>().Pause)  //游戏在暂停状态时，星星碰撞后无效果
             return;
        else
            Destroy(this.gameObject);
    }
}
