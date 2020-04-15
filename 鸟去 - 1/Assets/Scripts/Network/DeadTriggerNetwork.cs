using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadTriggerNetwork : MonoBehaviour {

    [HideInInspector]
    public float BlindTime = 3;
    GameObject player;
    GameObject Coral;
    GameObject Mask;

    // Use this for initialization
    void Start () {
        player = Global.Player;
        Coral = transform.parent.gameObject;
        //Mask = Global.UIRoot.transform.Find("UI/Blind").gameObject;
        Mask = Global.UI.transform.Find("Blind").gameObject;
    }

    //private void OnTriggerStay2D(Collider2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    //private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        //if (collision.gameObject == Global.Player)
        {
            Coral.GetComponent<CoralNetwork>().Restore();   // 先恢复，以免同时碰到多个龟后开启多个协程

            Coral.GetComponent<CoralNetwork>().RotateTo(collision.gameObject);
            Coral.GetComponent<CoralNetwork>().Letout();

            if(collision.gameObject == Global.Player)
                Mask.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            Coral.GetComponent<CoralNetwork>().Restore();
    }

    IEnumerator Dead()
    {
        Global.Input.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        player.GetComponent<PlayerEvents>().m_life -= 10;
    }

    // 方案之一：黑屏效果
    //IEnumerator Blind()
    //{
        //Mask.SetActive(true);
        //Mask.GetComponentInChildren<Text>().color = new Color(255, 255, 255, 0);
        //Mask.GetComponent<Image>().DOFade(1, 1f);
        //Invoke("StartLoad", 1f);
        //yield return new WaitForSeconds(1);
        // Mask.GetComponentInChildren<Text>().color = new Color(255, 255, 255, 1);
        //yield return new WaitForSeconds(3/*BlindTime*/); // 3

        //Mask.GetComponentInChildren<Text>().color = new Color(255, 255, 255, 0);
        //Mask.GetComponent<Image>().DOFade(0, 1f);
        //yield return new WaitForSeconds(1);
        //Global.Input.SetActive(true);

        /*if(BlindTime > 3)
        {
            string SceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(SceneName);
        }*/
    //}

    /* 方案之一：全屏粒子动画
    Text text;
    IEnumerator Blind()
    {
        Global.Input.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        Camera.main.transform.Find("BigInk").GetComponent<ParticleSystem>().Play();
        Global.UIRoot.transform.Find("UI/Blind").gameObject.SetActive(true);
        text = Global.UIRoot.transform.Find("UI/Blind/Text").GetComponent<Text>();
        StartCoroutine("BlindText");
        yield return new WaitForSeconds(3f);
        Camera.main.transform.Find("BigInk").GetComponent<ParticleSystem>().Stop();
        Global.UIRoot.transform.Find("UI/Blind").gameObject.SetActive(false);
        Global.Input.SetActive(true);
    }
    float timer = 0;
    IEnumerator BlindText()
    {
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            text.color = new Color(1, 1, 1, timer * 2);
            yield return null;
        }
        timer = 0;
        yield return new WaitForSeconds(2f);
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            text.color = new Color(1, 1, 1, 1 - timer * 2);
            yield return null;
        }
        timer = 0;
    }
    //*/
}
