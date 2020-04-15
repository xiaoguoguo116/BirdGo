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
    [SerializeField]
    Sprite cardImg;

    [SerializeField]
    GameObject BuffPre;
    GameObject Buff;

    float[] SkillPostations = { 1.91f, 0.6f, -0.95f };


    void Start()
    {

    }

    private void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == Global.Player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (GameManager.SkillButton[i].tag == "Skill(none)")
                {
                    GameManager.SkillButton[i].tag = this.tag;
                    
                    float left = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f)).x;
                    iTween.MoveTo(gameObject, iTween.Hash
                                (
                                    "x", left,   //1.91 0.6 -0.95
                                    "y", SkillPostations[i],
                                    "speed", 18f,
                                    "time", 120f,
                                    "easeType", iTween.EaseType.easeOutCubic,
                                    "oncomplete", "MoveEnd"
                                )
                            );
                    Buff = GameObject.Instantiate(BuffPre, this.transform.position, this.transform.rotation);
                    GameManager.SkillButton[i].GetComponent<Image>().sprite = cardImg;
                    break;
                }

            }
        }
    }

    void MoveEnd()
    {
        Global.Player.GetComponent<PlayerEventsNetwork>().CmdDestroyCard(gameObject);
        Destroy(Buff);
    }
}
