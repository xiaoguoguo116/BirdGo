using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MYSetWhaleToBG : MonoBehaviour {

    [SerializeField]
    GameObject[] whales;
    //List<GameObject> whales = new List<GameObject>();
	// Use this for initialization
	void Start () {
        GameObject sea = GameObject.Find("Environment/BackGround/bg3/sea (2)");
        // 从MY结点之下移动到远景结点之下【有些鲸不放在远景，需要手动筛选】
        /*
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Animal")
            {
                child.parent = sea.transform;
                whales.Add(child.gameObject);
            }
        }
        //*/
        foreach(GameObject ob in whales)
        {
            ob.transform.parent = sea.transform;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        foreach(GameObject ob in whales)
        {
            Destroy(ob);
        }
    }
}
