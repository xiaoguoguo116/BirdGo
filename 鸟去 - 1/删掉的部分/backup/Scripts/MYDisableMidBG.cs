using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MYDisableMidBG : MonoBehaviour {

    [SerializeField]
    GameObject sea, sea1;

    // Use this for initialization
    void Start () {
        //SpriteRenderer[] bg2 = Bg2.GetComponents<SpriteRenderer>();
        //foreach (SpriteRenderer img in bg2)
        //    img.DOFade(0, 1);
        sea.GetComponent<SpriteRenderer>().DOFade(0, 2);
        sea1.GetComponent<SpriteRenderer>().DOFade(0, 2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
