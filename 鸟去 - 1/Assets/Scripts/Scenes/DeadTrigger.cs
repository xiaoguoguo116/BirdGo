using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadTrigger : MonoBehaviour {

    float BlindTime = 3;
    bool toxic = false;
    GameObject player;
    GameObject Coral;
    GameObject Mask;

    // Use this for initialization
    void Start () {
        player = Global.Player;
        Coral = transform.parent.gameObject;
        //Mask = Global.UIRoot.transform.Find("UI/Blind").gameObject;
        BlindTime = Coral.GetComponent<Coral>().BlindTime;
        toxic = Coral.GetComponent<Coral>().toxic;
        if (toxic)
            Mask = Global.UI.transform.Find("Blind Toxic").gameObject;
        else
            Mask = Global.UI.transform.Find("Blind").gameObject;
    }

    //private void OnTriggerStay2D(Collider2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    //private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Coral.GetComponent<Coral>().RotateTo();
            Coral.GetComponent<Coral>().Letout();

            // 从喷到龟到龟恢复行动的过程中，与龟有关的协程不能放在本脚本里，
            // 因为本乌贼若在这个过程中被销毁，则龟的协程无法继续完成（Game3的回家谜题）
            if (Global.Input.activeSelf == true) 
                                                 //StartCoroutine(Blind());
                                                 //StartCoroutine(Dead());
            {
                Mask.SetActive(true);
                Mask.GetComponent<InkMask>().CoralInfo(Coral, toxic);
            }

        }
    }

    //IEnumerator Dead()
    //{
    //    Global.Input.SetActive(false);
    //    yield return new WaitForSeconds(1.5f);
    //    player.GetComponent<PlayerEvents>().m_life -= 10;
    //}

    IEnumerator Blind()
    {
        Global.Input.SetActive(false);
        yield return new WaitForSeconds(1f);


        // 方案之一：黑屏效果
        Mask.SetActive(true);
        Mask.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(Mathf.Min(3, BlindTime));
        Mask.GetComponent<Image>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);

        Coral.GetComponent<Coral>().Restore();
        Global.Input.SetActive(true);


        //*
        if (toxic)
        {
            string SceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(SceneName);
        }
        //*/
    }

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
