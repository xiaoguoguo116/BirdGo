using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipCoralAlive1 : MonoBehaviour {

    private Text text;
    float a = 0;

    [SerializeField]
    GameObject coral;

    [SerializeField]
    float a_speed = 0.2f;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        text.color = new Color(255, 255, 255, a);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Squid")
        {
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;  // 乌贼不会再被火轮碰死了
            Global.UI.GetComponent<UIManager>().victoryPanel.SetActive(true);
            Global.Input.SetActive(false);
            PlayerPrefs.SetInt("position", 0);
        }
        else if (collision.gameObject.tag == "Player" && ! coral.activeSelf)
        {
            Global.Input.SetActive(false);
            Invoke("DelayToDie", 2);
            InvokeRepeating("TipAppear", 0f, a_speed);
        }

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
