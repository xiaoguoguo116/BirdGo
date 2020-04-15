using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startPhysics : MonoBehaviour {

    // Use this for initialization
    //控制本关卡的物理物体在规定时候开启
    public GameObject[] gameObjects;
    public float delayTime;
	void Start () {
        StartCoroutine("openPhysics");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerable openPhysics()//开启物理物体
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.AddComponent<Rigidbody2D>();
            //obj.GetComponent<Rigidbody2D>().freezeRotation = false;
        }
        yield return new WaitForSeconds(0.5f);
    }
}
