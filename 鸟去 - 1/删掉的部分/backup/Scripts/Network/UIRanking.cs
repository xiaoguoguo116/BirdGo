using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIRanking : MonoBehaviour {

    private struct RankData
    {
        public string playerName;
        public float playerTime;
    }
    List<RankData> rankData; //排行榜数据
    public Vector2 firstPosition;//排行榜第一个开始位置
    public float distanceVertical;//排场榜单每两个玩家的数值间隔
    public GameObject rankContent;//排行榜容器
    public GameObject rankOneItem;//单个排行榜预制体
    public Sprite[] headImages;//头像

    public int rankLength;//排行榜个数

    // Use this for initialization
    void Start () {
        GameObject rank = transform.Find("Panel/Rank").gameObject;
        rank.SetActive(true);
        RectTransform rank_rect = rank.GetComponent<RectTransform>();
        rank_rect.DOLocalMoveX(0f, 1f);
        showData();

        StartCoroutine("ToNextScene");
    }

    IEnumerator ToNextScene()
    {
        float delayTime = 0;
        yield return new WaitForSeconds(5f);
        GameObject Fade = Global.UI.transform.Find("Fade").gameObject;
        Fade.SetActive(true);
        Fade.GetComponent<Image>().DOFade(1, 1f);
        //LoadGame();
        GameManager.GameOver();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void showData()//button(下一步)调用
    {
        for (int i = 0; i < GameManager.AllPlayers.Count; ++i)
        {
            GameObject tempItem = Instantiate(rankOneItem, new Vector3(), new Quaternion(), this.rankContent.transform);
            RectTransform tempItemRect = tempItem.GetComponent<RectTransform>();
            tempItemRect.anchoredPosition = new Vector2(this.firstPosition.x, this.firstPosition.y - i * this.distanceVertical);
            Text[] showTexts = tempItem.GetComponentsInChildren<Text>();
            showTexts[0].text = (i + 1).ToString();
            showTexts[1].text = GameManager.AllPlayers[i].GetComponent<PlayerEventsNetwork>().m_PlayerName;//rankData[i].playerName;
            showTexts[2].text = "\t\t" + GameManager.AllPlayers[i].GetComponent<PlayerEventsNetwork>().DeathTimes.ToString();
            tempItem.GetComponentsInChildren<Image>()[1].sprite = headImages[i % headImages.Length];
        }

    }


}
