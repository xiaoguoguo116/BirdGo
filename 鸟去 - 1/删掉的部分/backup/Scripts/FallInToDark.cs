using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FallInToDark : MonoBehaviour {

    [SerializeField]
    GameObject[] volcanic;
	// Use this for initialization
    public float delayTime = 0f;
	void Start () {
        foreach(GameObject v in volcanic)
        {
            v.SetActive(false);
        }

        SpriteRenderer[] bgSprites = GameObject.Find("B&W/Environment/BackGround").GetComponentsInChildren<SpriteRenderer>();
        //Transform[] transforms = GameObject.Find("B&W/Environment/BackGround").GetComponentsInChildren<Transform>();
        foreach (var t in bgSprites)
        {
            //if(delayTime = 0)
            iTween.FadeTo(t.gameObject, 0, delayTime);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
