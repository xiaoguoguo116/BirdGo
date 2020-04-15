///
// 某一个卡槽的脚本
// 玩家若要触发一个技能，方法有两个：
// 1、点击卡槽，则寻找屏幕里最左边的该类型动物（UseCard），触发其技能；
// 2、点击该类型动物，则先检查是否有该卡牌（FingerTestNetwork.Tap），若有则触发。
// 以上两种方法若能触发，则移除该卡牌（Clear）
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    Sprite bgImg;

    [HideInInspector]
    GameObject[] skillButton = new GameObject[3];

    private void Start()
    {
        bgImg = GetComponent<Image>().sprite;
    }

    /// <summary>
    /// 根据卡牌Tag寻找屏幕里最左边的该类型动物，触发其技能。
    /// 如果能够触发，则消除该卡牌
    /// </summary>
    /// <param name="skill">卡牌Tag</param>
    /// <returns></returns>
    public bool UseCard(string skill)
    {
        string targetTag = null;
        switch (skill)
        {
            case "Skill(whale)": targetTag = "Whale"; break;
            case "Skill(cuttle)": targetTag = "Squid"; break;
            case "Skill(saw)": targetTag = "Weapon"; break;
            case "Skill(Turtle)": Global.Player.GetComponent<PlayerEventsNetwork>().CmdTouchEvent(-1); return true; break;
        }
       
        //需要触发技能的对象，多于一个的话，取最左端那个
        int id = -1;
        float leftmost = float.MaxValue;

        // 找屏幕范围内最左边的该类型动物
        float left = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f)).x;
        float right = Camera.main.ViewportToWorldPoint(new Vector2(1f, 0f)).x;
        for(int i = 0; i < GameManager.ControllableObjects.Count; i++)
        {
            float x = GameManager.ControllableObjects[i].transform.position.x;
            if (x > left && x < right &&
                GameManager.ControllableObjects[i].tag == targetTag && x < leftmost)
            {
                id = i;
                leftmost = x;
            }
        }

        if (id >= 0)
        {
            Global.Player.GetComponent<PlayerEventsNetwork>().CmdTouchEvent(id);
            return true;
        }
        else
            return false;
    }

    public void OnSkillTrigger()
    {
        string skillTag = this.gameObject.tag;

        if (UseCard(skillTag))
        {
            Clear();
        }
    }

    public void Clear()
    {
        //*
        gameObject.tag = "Skill(none)";
        gameObject.GetComponent<Image>().sprite = bgImg;
        //*/
    }

}
