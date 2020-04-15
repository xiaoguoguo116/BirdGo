using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipCoralAlive : MonoBehaviour {

    private Text text;
    float a = 0;

    [SerializeField]
    GameObject coral;
    [SerializeField]
    GameObject destination;
    [SerializeField]
    GameObject rush;

    [SerializeField]
    float a_speed = 0.2f;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        text.color = new Color(255, 255, 255, a);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(Global.Player.tag))
        {
            rush.SetActive(false);
            float deadLine = 2;
            if (coral.transform.position.x < Camera.main.ViewportToWorldPoint(Vector2.zero).x - deadLine)
            {
                Global.Input.SetActive(false);
                Invoke("DelayToDie", 2);
                InvokeRepeating("TipAppear", 0f, a_speed);
            }
            else
            {
                //coral.transform.SetParent(Global.MYNetwork);    // 防止随关卡一起被删除
                coral.transform.DOMove(destination.transform.position, 10f).SetEase(Ease.InSine).OnComplete(DestroyCoral);
                Global.SceneEvent.GetCurMystery().GetComponent<MYBackInTime>().removeFromBackInTime(coral);
                Global.SceneEvent.GetCurMystery().GetComponent<MYBackInTime>().removeFromBackInTime(Global.Player);
            }
        }
        

    }

    void DestroyCoral()
    {
        //Global.SceneEvent.GetCurMystery().GetComponent<MYBackInTime>().removeFromBackInTime(coral);
        //Destroy(coral);
        coral.SetActive(false);
    }

    void DelayToDie()
    {
        StartCoroutine(Global.Player.GetComponent<PlayerEvents>().Dead());
    }

    void TipAppear()
    {  
        a += 0.1f;
        text.color = new Color(255, 255, 255, a);
        if (a >= 1)
        {
            CancelInvoke("TipAppear");
        }
    }


}
