//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Rank : MonoBehaviour
//{
//    public string[] Competitor; //玩家姓名
//    public int[] deadCount; //死亡次数

//    Text RankText;
//    private float Timer = 0; //刷新时间
//    private bool flag = false;  //变换标志位
//    private int flag2 = 0;
//    private int flag3 = 2;
//    private string Temp1;
//    private int Temp2;
////    private bool Rankde = true;
////    GameObject[] Players;

//    private void Start()
//    {
//        RankText = GetComponent<Text>();
//        //Rank2();
//    }
//    private void Update()
//    {
//         flag3 = flag2;
//        //Players = GameObject.FindGameObjectsWithTag("Player");
//        // flag2 = Players.Length;
//        flag2 = Global.AllPlayers.Count;
//        if (flag2 == flag3 )
//        {
//            Rank2();
////            Rankde = false;
//        }   
//    }

//    public void Rank2() {

//        //Players = GameObject.FindGameObjectsWithTag("Player");
//        //Global.AllPlayers.Clear();
//        //foreach (GameObject player in Players) {
//        //    Global.AllPlayers.Add(player);
//        //}
//        Competitor = new string[Global.AllPlayers.Count];
//        deadCount = new int[Global.AllPlayers.Count];
//        for (int i = 0; i < Global.AllPlayers.Count; i++)
//        {
//            Competitor[i] = Global.AllPlayers[i].GetComponent<PlayerEventsNetwork>().m_playerName;
//            deadCount[i] = Global.AllPlayers[i].GetComponent<PlayerEventsNetwork>().PlayerLoseLife;
//            //Debug.Log(Global.AllPlayers[i].GetComponent<PlayerEventsNetwork>().m_playerName);
            
//        }
//        if (flag2 != 0)
//        Global.Player.GetComponent<PlayerEventsNetwork>().CmdRank2(Competitor, deadCount);
//        //sort();
        
//    }
//    public void sort() //排序
//    {
//        for (int i = 1; i < Competitor.Length; i++)
//        {
//            for (int j = 0; j < Competitor.Length - i; j++)
//            {
//                if (deadCount[j + 1] < deadCount[j])
//                {
//                    Temp1 = Competitor[j];
//                    Competitor[j] = Competitor[j + 1];
//                    Competitor[j + 1] = Temp1;

//                    Temp2 = deadCount[j];
//                    deadCount[j] = deadCount[j + 1];
//                    deadCount[j + 1] = Temp2;
//                    flag = true;
//                }
//            }
//        }
//        SetText();
//    }

//    void SetText()  //文本编辑
//    {

//        RankText.text = "玩家      死亡次数\n";
//        for (int i = 0; i < Competitor.Length; i++)
//        {
//            RankText.text += "                    ";
//            RankText.text += Competitor[i].ToString();
//            RankText.text += "                    ";
//            RankText.text += deadCount[i].ToString();
//        }
//    }
//}
