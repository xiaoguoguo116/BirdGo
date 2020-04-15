using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ComicBG : MonoBehaviour {

    // Use this for initialization
    public GameObject[] Comics;
    public float StartDelayTime;
	void Start () {
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        //Hashtable way = new Hashtable();
        //way.Add("time", StartDelayTime);
        //way.Add("alpha", 1f);
        ////way.Add("onComplate", "ShowComics");
        //iTween.FadeTo(gameObject, way);

        ////Hashtable args = new Hashtable();

        ////最终透明度
        ////args.Add("alpha", 0);
        ////最终透明度,alpha和amount都是最终透明度,amount优先于alpha.内部实现是仅改变Color.a的值,再调用对应的Color方法.
        ////args.Add("amount", 0);
        ////是否包括子对象
        //args.Add("includechildren", false);

        ////iTween.FadeTo(gameObject,255, 1);
        //foreach (GameObject i in Comics)
        //{
        //    i.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        //}
        //Invoke("ShowComics", StartDelayTime);


        Hashtable args = new Hashtable();

        //最终透明度
        args.Add("alpha", 0);
        //动画的时间  
        args.Add("time", 0.5f);
        args.Add("includechildren", false);
        iTween.FadeFrom(gameObject, args);
        ShowComics();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ShowComics()//按顺序出现漫画
    {
        for(int i = 0; i < Comics.Length; ++i)
        {
            iTween.FadeFrom(Comics[i], iTween.Hash("time", 0.5f,
                "delay", i + 1,
                "alpha", 0f));
            if(i < 3)
                StartCoroutine(Fade(Comics[i]));
        }
    }

    IEnumerator Fade(GameObject ob)
    {
        yield return new WaitForSeconds(4f);
        ob.SetActive(false);
    }

}
