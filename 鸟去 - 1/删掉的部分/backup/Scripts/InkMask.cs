using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InkMask : MonoBehaviour {

    [SerializeField]
    Text Tab;
    [SerializeField]
    GameObject[] Inks;
	// Use this for initialization
	void Start () {
        // 全都透明
        Tab.color = new Color(255, 255, 255, 0);
        foreach (var ink in Inks)
            ink.GetComponent<Image>().color = new Color(255, 255, 255, 0);

	}

    private void OnEnable()
    {
        // 随机调整墨迹位置
        for(int i = 0; i < Inks.Length; i++)
        {
            //Inks[i].transform.position = new Vector2
        }

        StartCoroutine("ShowInks");
    }
    // 渐显渐隐
    IEnumerator ShowInks()
    {
        Tab.DOFade(1, 1f);
        foreach(var ink in Inks)
        {
            ink.GetComponent<Image>().DOFade(1, 1f);
        }
        yield return new WaitForSeconds(2f);
        Tab.DOFade(0, 1f);
        foreach (var ink in Inks)        
            ink.GetComponent<Image>().DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
