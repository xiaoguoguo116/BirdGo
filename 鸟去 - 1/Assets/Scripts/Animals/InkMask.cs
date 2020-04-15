using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class InkMask : MonoBehaviour {

    [SerializeField]
    Text text;
    [SerializeField]
    GameObject[] Inks;
    Vector3[] oriScales = new Vector3[3];
    GameObject coral = null;
    bool toxic = false;

    // Use this for initialization
    void Awake () {
        // 全都透明
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        for(int i = 0; i < Inks.Length; i++)
        {
            var ink = Inks[i];
            Color color = ink.GetComponent<Image>().color;
            ink.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
            oriScales[i] = ink.GetComponent<RectTransform>().transform.localScale;
        }
    }

    public void CoralInfo(GameObject coral, bool toxic)
    {
        this.coral = coral;
        this.toxic = toxic;
    }

    private void OnEnable()
    {
        // 随机调整墨迹位置
        for(int i = 0; i < Inks.Length; i++)
        {
            //Inks[i].transform.position = new Vector2
        }

        StartCoroutine("ShowInks");
        if(SceneManager.GetActiveScene().name != "Game Network")
            StartCoroutine("BlackScreen");
    }
    // 文字和墨迹渐显渐隐，墨迹渐放大
    IEnumerator ShowInks()
    {
        text.DOFade(1, 1f);
        for(int i = 0; i < Inks.Length; i++)
        {
            var ink = Inks[i];
            ink.GetComponent<Image>().DOFade(1, 1f);
            ink.GetComponent<RectTransform>().transform.localScale = oriScales[i];
            ink.GetComponent<RectTransform>().transform.DOScale(1.5f * oriScales[i], 6f);
        }
        yield return new WaitForSeconds(4f);
        text.DOFade(0, 1f);
        foreach (var ink in Inks)        
            ink.GetComponent<Image>().DOFade(0, 2f);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);    // 必须在 BlackScreen 协程结束后执行，不然对于toxic无法重启场景
    }

    // 黑屏渐隐渐现；toxic重启场景
    IEnumerator BlackScreen()
    {
        Global.Input.SetActive(false);
        yield return new WaitForSeconds(1f);

        GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(3);
        GetComponent<Image>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);

        if(coral)
            coral.GetComponent<Coral>().Restore();
        Global.Input.SetActive(true);

        if (toxic)
        {
            string SceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(SceneName);
        }
    }



    // Update is called once per frame
    void Update () {
		
	}
}
