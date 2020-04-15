using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class deadPanel : MonoBehaviour {
    [HideInInspector]
    public string[] seaKnowledge;
    // Use this for initialization

    void Start () {
        seaKnowledge = new string[] {
            "鲸鲨，是海洋里最大的鱼",
            "格陵兰鲨鱼吃北极熊",
            "可能会不死的水母——灯塔水母",
            "月亮鱼是已知的唯一温血动物"
        };
        Debug.Log("seaKnowledgeLength:" + seaKnowledge.Length);
        showPanel();
    }

    public void jumpToPlay()//直接跳转到关卡
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void showPanel()
    {
        GameObject root = GameObject.Find("UI Root");
        SceneEventManager manager = GameObject.Find("GameManager").GetComponent<SceneEventManager>();
        MystoryManager mystoryManager = GameObject.Find("Mysterys").GetComponent<MystoryManager>();
        string[] mTips = mystoryManager.mTips;
        int p_index = manager.p_index;
        Debug.Log("连续死亡次数：" + Global.deadCount.ToString());
        if (Global.deadCount >= 3)//超过3次会有提示
        {
            if (mTips[p_index] != "")
            {

                this.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = mTips[p_index];
                this.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().fontSize = 70;
            }
            else
            {
                this.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = ((int)(root.GetComponentInChildren<StoneClock>().year)).ToString() + "年\n" + "最后一只玳瑁龟灭绝了";
            }

        }
        //else if (Global.deadCount > 3)//海洋小知识
        //{
        //    System.Random rd = new System.Random();
        //    Debug.Log("knowledgeLength" + seaKnowledge.Length.ToString());
        //    this.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = seaKnowledge[rd.Next(0, seaKnowledge.Length - 1)];
        //    this.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().fontSize = 70;//字号
        //}
        else
        {
            this.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = ((int)(root.GetComponentInChildren<StoneClock>().year)).ToString() + "年\n" + "最后一只玳瑁龟灭绝了";
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
