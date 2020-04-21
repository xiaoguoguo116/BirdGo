using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    public bool Pause = false;
    GameObject pausePanel;
    GameObject pauseButton;
    GameObject restartButton;
    GameObject Fade;
    //public bool BackTimeKey = false;

    [HideInInspector]
    public GameObject knowledge;
    [HideInInspector]
    public GameObject getKnow;
    [HideInInspector]
    public GameObject deadPanel;
    [HideInInspector]
    public GameObject victoryPanel;
    public Scene scene;

    private List<RankData> rankData; //排行榜数据
    public Vector2 firstPosition;//排行榜第一个开始位置
    public float distanceVertical;//排场榜单每两个玩家的数值间隔
    public GameObject rankContent;//排行榜容器
    public GameObject rankOneItem;//单个排行榜预制体
    public Text CurrentText;//当前玩家名字输入框
    public Sprite[] headImages;//头像

    public int rankLength;//排行榜个数
    public GameObject endTip;//结束时碰撞的提示框【专用于Game3关底】
    public GameObject comic;//结束漫画节点【专用于Game5关底】
    GameObject Slider;
    private struct RankData
    {
        public string playerName;
        public float playerTime;
        
    }

    private void Start()
    {
        Slider = GameObject.Find("Slider");
        //pausePanel = GameObject.Find("PausePanel");
        //pauseButton = GameObject.Find("PauseButton");
        //knowledge = GameObject.Find("Knowledge");
        //getKnow = GameObject.Find("getKnow");
        //deadPanel = GameObject.Find("deadPanel");
        //victoryPanel = GameObject.Find("Victory");
        //restartButton = GameObject.Find("Restart");
        //Fade = GameObject.Find("Fade");

        // transform.Find 可以找到隐藏的对象
        pauseButton = transform.Find("PauseButton").gameObject;
        getKnow = transform.Find("PauseButton/getKnow").gameObject;
        //restartButton = transform.Find("PausePanel/MovingPanel/Restart").gameObject;

        pausePanel = transform.Find("PausePanel").gameObject;   
        knowledge = transform.Find("Knowledge").gameObject;

        victoryPanel = transform.Find("Victory").gameObject;

        Fade = transform.Find("Fade").gameObject;

        pausePanel.SetActive(false);
//        knowledge.SetActive(false);
        getKnow.SetActive(false);

        victoryPanel.SetActive(false);
        Fade.SetActive(true);
        pauseButton.SetActive(true);

        Transform rankingTrans = transform.Find("RankingPanel");
        if (rankingTrans)
            rankingTrans.gameObject.SetActive(false);

        Fade.GetComponent<Image>().DOFade(0, 5f);
        Invoke("DestroyFade", 5.1f);

        scene = SceneManager.GetActiveScene();
        Time.timeScale = 1;

        loadData();
    }

    void DestroyFade()
    {
        Fade.SetActive(false);
    }
    public void ClickToPause()
    {

        GameObject.Find("GameManagerNN").GetComponent<GameManagerNN>().IsStop = true;
        GameObject.Find("MYBackTime").GetComponent<MYBackInTime>().StopGame();
        
        GameObject.Find("MYBackTime").GetComponent<StopGame>().IsStopGame();
        Slider.transform.position = new Vector3(Slider.transform.position.x, Slider.transform.position.y, 0);
        Pause = true; 
        pauseButton.SetActive(false);
        pausePanel.SetActive(true);
        
        //pausePanel.transform.Find("MovingPanel").transform.DOBlendableMoveBy(new Vector2(0, -230) , /*0.5f*/0).SetEase(Ease.OutCubic).SetUpdate(true);
        //pausePanel.transform.Find("MovingPanel/getKnow").gameObject.SetActive(getKnow.activeSelf);

        /*if (Global.InputNetwork)    // 如果是联机场景
            Global.InputNetwork.SetActive(false);
        else
        {
            //Global.Input.SetActive(false);
            Time.timeScale = 0;
        }*/
    }
    public void ClickToBack()
    {
        GameObject.Find("MYBackTime").GetComponent<MYBackInTime>().ReGame();
        StartCoroutine(GameObject.Find("MYBackTime").GetComponent<StopGame>().BackToGame());
        GameObject.Find("GameManagerNN").GetComponent<GameManagerNN>().IsStop = false;
        Debug.Log(GameObject.Find("GameManagerNN").GetComponent<GameManagerNN>().IsStop);
        GameObject.Find("GameManagerNN").GetComponent<GameManagerNN>().AddTime();
        Debug.Log("ack");
        GameObject.Find("PSTool").transform.position = new Vector3(-64, 64, 0);
        
        if (GlobalNN.GameManagerNN.GetComponent<GameManagerNN>().ContrlTimes > 0)
        {
            GlobalNN.GameManagerNN.GetComponent<GameManagerNN>().ContrlTimes = 0;
            GlobalNN.GameManagerNN.GetComponent<GameManagerNN>().BackTimes--;
            GlobalNN.GameManagerNN.GetComponent<GameManagerNN>().BeforeTime = GlobalNN.GameManagerNN.GetComponent<GameManagerNN>().Time;
        }

        Slider.transform.position = new Vector3(Slider.transform.position.x, Slider.transform.position.y, -100);
        Pause = false;
        //pausePanel.transform.Find("MovingPanel").transform.DOBlendableMoveBy(new Vector2(0, 230), 0).SetUpdate(true);
        //pausePanel.SetActive(false);
        Invoke("PausePanelClose", 0.1f);    // 延迟关闭，不然FingerGestures执行时没有UI阻挡，会产出穿透UI的感觉
        pauseButton.SetActive(true);


        if (Global.InputNetwork)    // 如果是联机场景
            Global.InputNetwork.SetActive(true);
        else
            //Global.Input.SetActive(true);

            Time.timeScale = 1;
    }
    void PausePanelClose()
    {
        pausePanel.SetActive(false);
    }

    public void ClickToRestart()
    {
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1;
    }

    public void CliccToExit()
    {
        SceneManager.LoadScene("Select");
        Time.timeScale = 1;
    }
    public void CliccToExitNetwork()
    {
        //SceneManager.LoadScene("UNet");
        GameManager.GameOverNetwork();
        Time.timeScale = 1;
    }

    public void CliccToRank()
    {
        GameObject rank = victoryPanel.transform.Find("Panel/Rank").gameObject;
        GameObject inputPlace = victoryPanel.transform.Find("Panel/InputPlace").gameObject;
        rank.SetActive(true);
        RectTransform inputPlace_rect = inputPlace.GetComponent<RectTransform>();
        RectTransform rank_rect = rank.GetComponent<RectTransform>();
        inputPlace_rect.DOLocalMoveX(-3000f, 1f);
        rank_rect.DOLocalMoveX(0f, 1f);
        StartCoroutine("ToNextScene");
    }

    IEnumerator ToNextScene()
    {
        float delayTime = 0;

        yield return new WaitForSeconds(3f);
        //if (comic)//如果有关底动画就打开
        //{
        //    //iTween.FadeTo(victoryPanel.transform.Find("Panel/InputPlace").gameObject, 0, 1);
        //    //iTween.FadeTo(victoryPanel.transform.Find("Panel/Rank").gameObject, 0, 1);
        //    comic.SetActive(true);
        //    victoryPanel.transform.Find("Panel/Rank").gameObject.SetActive(false);
        //    victoryPanel.transform.Find("Panel/InputPlace").gameObject.SetActive(false);
        //    victoryPanel.transform.Find("Panel").gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        //    Invoke("LoadGame", (float)comic.GetComponent<VideoPlayer>().clip.length);
        //    //delayTime = 3f;
        //}
        ////yield return new WaitForSeconds(2f + delayTime);
        //else
        //{
            Fade.SetActive(true);
            Fade.GetComponent<Image>().DOFade(1, 1f);
            LoadGame();
        //}
        
        //int sceneIndex = GameObject.Find("GameManager").GetComponent<SceneEventManager>().GetCurrentSceneIndex(SceneManager.GetActiveScene().name);
        //if (sceneIndex < Global.SceneStrArray.Length - 1)
        //    SceneManager.LoadScene(Global.SceneStrArray[sceneIndex + 1]);
        //else
        //    SceneManager.LoadScene("start");
        //Time.timeScale = 1;
    }

    public void LoadGame()
    {
        //int sceneIndex = GameObject.Find("GameManager").GetComponent<SceneEventManager>().GetCurrentSceneIndex(SceneManager.GetActiveScene().name);
        //if (sceneIndex < Global.SceneStrArray.Length - 1)
        //    SceneManager.LoadScene(Global.SceneStrArray[sceneIndex + 1]);
        //else
        //    SceneManager.LoadScene("start");

        //if (Global.LevelIndex < Global.SceneStrArray.Length - 1)
        //{
        //    Global.LevelIndex++;
        //    SceneManager.LoadScene("Comic");
        //}
        //else
        //{
        //    Global.LevelIndex = 0;
        //    SceneManager.LoadScene("start");
        //}

        
        Global.LevelVideoIndex = 1;
        SceneManager.LoadScene("Comic");
    }

    public void ClickToKnow()
    {
        knowledge.SetActive(true);
        getKnow.SetActive(false);

        if (Global.InputNetwork)    // 如果是联机场景
            Global.InputNetwork.SetActive(false);
        else
        {
            Global.Input.SetActive(false);
            Time.timeScale = 0;
        }
    }

    public void ClickToCloseKnow()
    {
        knowledge.SetActive(false);

        if (Global.InputNetwork)    // 如果是联机场景
            Global.InputNetwork.SetActive(true);
        else
            Global.Input.SetActive(true);

        Time.timeScale = 1;
    }

    IEnumerator delayShowVectory(float time)
    {

        yield return new WaitForSeconds(time);
        victoryPanel.SetActive(true);
        GameObject.Find("Input").SetActive(false);
    }

    public void ShowVictory(float time = 0)
    {
        StartCoroutine(delayShowVectory(time));
    }

    public void loadData()//载入本地玩家数据到游戏中
    {
        int i = 0;
        this.rankData = new List<RankData>();
        while (PlayerPrefs.HasKey("playerName_" + SceneManager.GetActiveScene().name + "_" + i.ToString()) && i < this.rankLength)
        {
            RankData tempRank;
            tempRank.playerName = PlayerPrefs.GetString("playerName_" + SceneManager.GetActiveScene().name + "_" + i.ToString());
            tempRank.playerTime = PlayerPrefs.GetFloat("playerTime_" + SceneManager.GetActiveScene().name + "_" + i.ToString());
            rankData.Add(tempRank);
            i++;
        }
    }

    public void saveData()//点击下一步button保存数据
    {
        int i = 0;
        for (i = 0; i < rankLength && i < rankData.Count; ++i)
        {
            PlayerPrefs.SetString("playerName_" + SceneManager.GetActiveScene().name + "_" + i.ToString(), rankData[i].playerName);
            PlayerPrefs.SetFloat("playerTime_" + SceneManager.GetActiveScene().name + "_" + i.ToString(), rankData[i].playerTime);
        }
    }

    public void showData()//button(下一步)调用
    {
        RankData currentData;
        
        

        for (int i = 0; i < rankData.Count && i < this.rankLength; ++i)
        {
            GameObject tempItem = Instantiate(rankOneItem, new Vector3(), new Quaternion(), this.rankContent.transform);
            RectTransform tempItemRect = tempItem.GetComponent<RectTransform>();
            tempItemRect.anchoredPosition = new Vector2(this.firstPosition.x, this.firstPosition.y - i * this.distanceVertical);
            Text[] showTexts = tempItem.GetComponentsInChildren<Text>();
            showTexts[0].text = (i + 1).ToString();
            showTexts[1].text = rankData[i].playerName;
            int year = (int)rankData[i].playerTime;
            int month = (int)((rankData[i].playerTime - year) * 12);
            int days = (int)((rankData[i].playerTime - year) * 360 - 30 * month);
            showTexts[2].text = year.ToString() + "年" + month.ToString() + "月" + days.ToString() + "日";
            tempItem.GetComponentsInChildren<Image>()[1].sprite = headImages[i];
        }
        saveData();
    }
}
