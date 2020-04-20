using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGame : MonoBehaviour {
    GameObject UI;
    Transform position1;
    // Use this for initialization
    void Start () {
        UI = GameObject.Find("UI Root");
    }
    public void StopGameNN() {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
    public void returnGameNN()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
    // Update is called once per frame
    void Update () {
        if (UI.GetComponent<UIManager>().Pause)
            if(GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
               StopGameNN();
        if (!UI.GetComponent<UIManager>().Pause)
            if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Static)
                returnGameNN();
    }
}
