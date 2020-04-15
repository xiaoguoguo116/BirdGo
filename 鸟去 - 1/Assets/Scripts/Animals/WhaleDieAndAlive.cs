using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WhaleDieAndAlive : MonoBehaviour {

    public GameObject animalStone;

    public float changeTime;
    // Use this for initialization
	void Start () {
        ////键值对儿的形式保存iTween所用到的参数  
        //Hashtable args = new Hashtable();

        ////最终透明度
        //args.Add("alpha", destAlpha);
        ////最终透明度,alpha和amount都是最终透明度,amount优先于alpha.内部实现是仅改变Color.a的值,再调用对应的Color方法.
        ////args.Add("amount", 0);
        ////是否包括子对象
        //args.Add("includechildren", true);
        ////当效果是应用在renderer(渲染器)组件上时,此参数确定具体应用到那个以命名颜色值上
        //args.Add("namedcolorvalue", iTween.NamedValueColor._Color);

        ////动画的时间  
        //args.Add("time", 2f);
        ////延迟执行时间  
        //args.Add("delay", 0f);

        ////这里是设置类型，iTween的类型又很多种，在源码中的枚举EaseType中  
        //args.Add("easeType", iTween.EaseType.easeInOutExpo);
        //args.Add("loopType", iTween.LoopType.pingPong);
        ////args.Add("complete", "ChangeStone");
        //iTween.FadeTo(gameObject, args);

        this.GetComponent<SpriteRenderer>().material.DOFade(0, changeTime).SetLoops(-1, LoopType.Yoyo);
        Invoke("ChangeStone", 2f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeStone()
    {
        animalStone.GetComponent<SpriteRenderer>().material.DOFade(0, changeTime).SetLoops(-1, LoopType.Yoyo);
    }
}
