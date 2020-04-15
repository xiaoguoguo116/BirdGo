using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 生成后延时释放刚体，提高稳定性
/// </summary>
public class MY_DelayedRelease : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().freezeRotation = true;
        Invoke("Release", 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Release()
    {
        GetComponent<Rigidbody2D>().freezeRotation = false;
    }

}
