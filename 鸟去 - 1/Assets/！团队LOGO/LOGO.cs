using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System;

public class LOGO : MonoBehaviour {

    [SerializeField]
    GameObject[] poster;

    [SerializeField]
    String nextScene;

    int id = 0;
    // Use this for initialization
    void Start () {
        foreach(var ob in poster)
        {
            ob.GetComponent<RawImage>().enabled = false;
        }
        StartCoroutine(ShowPaint());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //这里采用alpha渐变方法
    IEnumerator ShowPaint()
    {
        RawImage image = poster[id].GetComponent<RawImage>();
        image.enabled = true;
        image.CrossFadeAlpha(0, 0f, true);//淡出执行参数，0=透明 0.5f=时间 true=
        yield return new WaitForSeconds(0.7f);
        image.CrossFadeAlpha(1, 1f, true);//淡入执行参数，1=显示 0.5f=时间 true=
        yield return new WaitForSeconds(2f);
        image.CrossFadeAlpha(0, 1f, true);//淡出执行参数，0=透明 0.5f=时间 true=
        yield return new WaitForSeconds(0.7f);
        if (++id < poster.Length)
            StartCoroutine(ShowPaint());
        else
            SceneManager.LoadSceneAsync(nextScene);
    }
}
