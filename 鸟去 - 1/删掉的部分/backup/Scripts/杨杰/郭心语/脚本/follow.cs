using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {
    public GameObject sphere;
    public GameObject effort;
    float x, y,z;

	// Use this for initialization
	void Start () {
        x = sphere.transform.position.x; //获取球的初始位置并赋值给粒子
        y = sphere.transform.position.y;
        z = sphere.transform.position.z;
        effort.transform.position = new Vector3(x, y, z);
    }
	
	// Update is called once per frame
	void Update () {
        x = sphere.transform.position.x; //实时获取球的位置并赋值给粒子
        y = sphere.transform.position.y;
        z = sphere.transform.position.z;
        effort.transform.position = new Vector3(x, y, z);
    }
}
