///
// 技能卡上的脚本
// 本地玩家碰到技能卡，则飞入卡槽，存入GameManager.SkillButton，
// 玩家收集的卡牌只在本地玩家上保存
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GetCard : NetworkBehaviour
{
    //[SerializeField]
    //Sprite cardImg;

    [SerializeField]
    GameObject BuffPref;
    GameObject Buff;

    GameObject rolling;

    float[] SkillPostations = { 1.91f, 0.6f, -0.95f };


    void Start()
    {

    }

    private void Update()
    {

    }
    //private void OnTriggerEnter2D(Collider2D collider)
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject == Global.Player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (GameManager.SkillButton[i].tag == "Skill(none)")
                {
                    GameManager.SkillButton[i].tag = this.tag;
                    
                    float left = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f)).x;

                    //GameObject rolling = GameObject.Instantiate(rollingPref, Global.UI.transform);
                    rolling = new GameObject("rollingCard");
                    rolling.transform.position = transform.position;
                    rolling.transform.localScale = transform.localScale;
                    rolling.AddComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                    rolling.AddComponent<RollingCard>().StartRoll(i, BuffPref);
                   
                    GameManager.SkillButton[i].GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;//cardImg;

                    Global.Net.CmdDestroyCard(gameObject);
              
                    break;
                }

            }
        }
    }


}
