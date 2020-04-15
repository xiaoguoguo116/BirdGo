using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneEventManager : MonoBehaviour
{
    //GameObject[] backgounds;
    [SerializeField]
    [Tooltip("调试用：-2表示正常；-1表示无谜题；0~n表示跳到第几个谜题处")]
    int PositionIndex = -2;

    [SerializeField]
    GameObject PointParent;
    [SerializeField]
    Transform MysteryParent;
    List<Transform> points = new List<Transform>();
    List<GameObject> Mysterys = new List<GameObject>();

    [SerializeField]
    [Tooltip("乌贼关后半程要调亮灯光")]
    Light Light;
    [SerializeField]
    [Tooltip("乌贼关从第几个谜题开始要调亮灯光")]
    int LightOnIndex = 4;

    [SerializeField]
    Sprite[] knowledgeImgs;

    [SerializeField]
    string[] knowledgeTitle;
    [SerializeField]
    [TextArea]
    string[] knowledgeMessage;
    [SerializeField]
    [TextArea]
    string[] knowledgeSkill;


    GameObject currentBg;
    GameObject previousBg;
    GameObject lastMy = null;

    Transform nextPoint;
    Transform k_current;

    UIManager uiManager;

    [HideInInspector]
    public int p_index = 0;    // 当前正在经历的谜题序号
    int k_index = 0;

    [HideInInspector]
    //public int deadTime;//死亡次数
    private void Awake()
    {
        //Global.SceneStrArray = new string[] { "Game1", "Game2", "Game3", "Game5"};
        Global.UI = GameObject.Find("UI Root").transform.Find("UI").gameObject;
        Global.UI.SetActive(true);
        uiManager = Global.UI.GetComponent<UIManager>();
        Global.Input = GameObject.Find("Input");
        Global.InputNetwork = GameObject.Find("InputNetwork");

        //        Global.Player = GameObject.FindGameObjectWithTag("Player"); 
        Global.Player = GameObject.Find("Player");

        Global.SceneEvent = this;
        PlayerPrefs.SetString("Scene", SceneManager.GetActiveScene().name);
        // 载入CheckPoint和谜题
        Transform[] ps = PointParent.GetComponentsInChildren<Transform>();  // 含孙子对象
        for (int i = 1; i < ps.Length; i++)
            points.Add(ps[i]);
        for (int i = 0; i < MysteryParent.childCount; i++)                  // 不含孙子对象
        {
            var child = MysteryParent.GetChild(i).gameObject;
            if (child.activeSelf)
                Mysterys.Add(child);
        }

        if (PositionIndex != -2)
            PlayerPrefs.SetInt("position", PositionIndex);  // 出生点序号（PlayerPrefs可以用来存盘）  

        //if (PlayerPrefs.HasKey("deadTime"))
        //{
        //    deadTime = PlayerPrefs.GetInt("deadTime");
        //}
        //else
        //{
        //    deadTime = 0;
        //}

        //player = Global.Player;
        if (PlayerPrefs.HasKey("position"))
        {
            p_index = PlayerPrefs.GetInt("position");
            Global.Player.transform.position = points[p_index].position;

            if (points[p_index] != null)
            {
                p_index = p_index - 1;
                nextPoint = points[p_index + 1];
                k_current = nextPoint;
            }
        }

        foreach (GameObject item in Mysterys)   // 必须放在Awake里，因为有些谜题的Start里会初始化一些东西
        {
            item.SetActive(false);
        }
        previousBg = null;

    }
    // Use this for initialization
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (Global.Player == null)
            return;
        //CreateBg();
        CreateKnowledge();
        CreateMystery();
        if (Global.Player.transform.position.x >= points[points.Count - 1].position.x)
        {
            //Debug.Log("你赢了");
            try
            {
                uiManager.victoryPanel.SetActive(true);
                Global.Input.SetActive(false);
                PlayerPrefs.SetInt("position", 0);
                Camera.main.GetComponent<CameraFollow>().enabled = false;
                //Time.timeScale = 0f;
            }
            catch (System.Exception)
            {
                return;
            }

        }
    }

    void CreateKnowledge()
    {
        try
        {
            if (Global.Player.transform.position.x > k_current.position.x && (k_index < points.Count))
            {
                if (knowledgeTitle[k_index] == "")
                {
                    k_index += 1;
                    k_current = points[k_index];
                    return;
                }
                uiManager.getKnow.SetActive(true);
                uiManager.knowledge.GetComponentsInChildren<Image>()[1].sprite = knowledgeImgs[k_index];
                uiManager.knowledge.GetComponentsInChildren<Text>()[0].text = knowledgeTitle[k_index];
                uiManager.knowledge.GetComponentsInChildren<Text>()[1].text = knowledgeMessage[k_index];
                uiManager.knowledge.GetComponentsInChildren<Text>()[2].text = knowledgeSkill[k_index];
                k_index += 1;
                k_current = points[k_index];
            }
        }
        catch (System.Exception e)
        {
            //Debug.Log(e.Message);
            //k_current.position = Global.Player.transform.position;
            return;
        }
    }

    void CreateMystery()
    {
        try
        {
            if (Global.Player.transform.position.x >= nextPoint.position.x && (p_index < points.Count))
            {
                if (p_index >= 0)
                {
                    if (lastMy)
                        Destroy(lastMy);
                    lastMy = Mysterys[p_index];
                }

                //if (PlayerPrefs.GetInt("isDeadLastTime") == 0 || !PlayerPrefs.HasKey("isDeadLastTime"))
                //    deadTime = 0;
                //else
                //    deadTime++;
                //PlayerPrefs.SetInt("isDeadLastTime", 0);
                //PlayerPrefs.SetInt("deadTime", deadTime);
                //Debug.Log("连续死亡次数" + deadTime.ToString());
                p_index++;
                Mysterys[p_index].SetActive(true);

                nextPoint = points[p_index + 1];
                PlayerPrefs.SetInt("position", p_index);
                //Debug.Log("当前存档：" + PlayerPrefs.GetString("Scene") + " ; " + PlayerPrefs.GetInt("position"));

                // 乌贼关后半程要调亮灯光
                if (SceneManager.GetActiveScene().name == "Game3" && p_index >= LightOnIndex)
                    Light.intensity = 1f;

            }
            //if (lastMy && Global.Player.transform.position.x >= lastMy.transform.position.x + 100f)
            if (lastMy && Global.Player.transform.position.x >= points[p_index].position.x + Global.CamWidth * 2)
            {
                Destroy(lastMy);
                lastMy = null;
            }

        }
        catch (System.Exception)
        {
            return;
        }
    }

    public int GetCurrentMyIndex()
    {
        return p_index;
    }

    public GameObject GetCurMystery()
    {
        return Mysterys[p_index];
    }

    public int GetMysteryCount()
    {
        return Mysterys.Count;
    }

    public Transform GetNextPoint()
    {
        return points[p_index + 1];
    }

    //[SerializeField]
    //Material clockMaterial;
    //Stack<AudioSource> stack = new Stack<AudioSource>();
    //[HideInInspector]
    //public bool isBigBack = false;
    //Image[] imgs;
    //public void TimingBackAudio()
    //{
    //    AudioSource[] sources = this.GetComponents<AudioSource>();
    //    GameObject obs = GameObject.Find("UI Root").transform.Find("UI/StoneClock").gameObject;
    //    imgs = obs.GetComponentsInChildren<Image>();
    //    foreach (Image img in imgs)
    //    {
    //        img.material = clockMaterial;
    //    }
    //    开始回溯，环境音效静音
    //    foreach (AudioSource item in sources)
    //    {
    //        item.volume = 0f;
    //        stack.Push(item);
    //    }
    //    回溯音效
    //    大回溯时间长，音效的持续时间也长
    //    if (isBigBack)
    //    {
    //        StartCoroutine(AudioBack(10f));
    //    }
    //    else
    //    {
    //        StartCoroutine(AudioBack(1f));
    //    }
    //}

    //public IEnumerator AudioBack(float time)
    //{
    //    AudioSource timingBack = stack.Pop();
    //    //回溯音效播放
    //    timingBack.volume = 1f;
    //    yield return new WaitForSeconds(time);
    //    foreach (Image img in imgs)
    //    {
    //        img.material = null;
    //    }
    //    //回溯音效静音
    //    timingBack.volume = 0f;
    //    //环境音效开启
    //    while (stack.Count > 0)
    //    {
    //        AudioSource erAudio = stack.Pop();
    //        erAudio.volume = 0.5f;
    //    }
    //}

    public int GetCurrentSceneIndex(string sceneStr)//tool函数，输入当前场景name字符串，返回当前场景下标
    {
        for(int i = 0; i < Global.SceneStrArray.Length; ++i)
        {
            if(sceneStr == Global.SceneStrArray[i])
            {
                return i;
            }
        }
        return -1;
    }
}

