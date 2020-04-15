using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarNumberPanel : MonoBehaviour {

    public GameObject StarPosition1;
    public GameObject star;
    int number;
    void Awake()
    {
        number = GameObject.Find("GameManager").GetComponent<Start>().Num;
       // if (number > 1)
            Instantiate(star, Vector2.zero, Quaternion.identity);
    }

}
