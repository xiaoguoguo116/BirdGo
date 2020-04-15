using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Net : MonoBehaviour {

    // Use this for initialization

    private GameObject Weapon;
    private bool bSet;
    public GameObject Tide;
	void Start () {
        Weapon = transform.GetComponentInParent<Weapon>().gameObject;
        bSet = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(-12 < Weapon.transform.localPosition.x && Weapon.transform.localPosition.x < -5 && bSet && Tide.activeInHierarchy)
        {
            GoAway();
            bSet = false;
        }
	}

    void GoAway()
    {
        transform.DORotate(new Vector3(0, 0, 300f), 1f);
        transform.DOMove(new Vector3(Weapon.transform.position.x, Weapon.transform.position.y + 50f, 0f), 3f);
    }
}
