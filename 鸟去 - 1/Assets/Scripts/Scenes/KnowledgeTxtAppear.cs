using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnowledgeTxtAppear : MonoBehaviour {

    [SerializeField]
    string[] messages;

    Text text;
    int index = 0;
    private void Start()
    {
       text = this.GetComponentInChildren<Text>();
    }
    private void OnEnable()
    {
        Debug.Log("出现");
        //InvokeRepeating("SetText", 0f, 2f);
    }
    
    void SetText()
    {
        if (index < messages.Length)
        {
            text.text = messages[index];
            index += 1;
        }
        else
        {
            CancelInvoke();
            this.gameObject.SetActive(false);
        }
    }
}
