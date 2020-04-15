using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class StoneClocknetwork : NetworkBehaviour {

    [SerializeField]
    [Tooltip("外圈跳一格的秒数")]
    float Timer = 2f;
    [SerializeField]
    GameObject Inside;
    [SerializeField]
    GameObject Outside;
    [SerializeField]
    GameObject Text;
    [SerializeField]
    GameObject BackText;
    Text text;
   // [SyncVar]
    public float year = 120;
    [SerializeField]
    AnimationCurve curve;
    ChangeColorToBlackAndWhite BlackAndWhite;
    GameObject circleProcess;
    [HideInInspector]
    public bool reverse = false;    // 正转or反转
    // Use this for initialization
    void Start () {
        try
        {
            BlackAndWhite = GameObject.Find("B&W").GetComponent<ChangeColorToBlackAndWhite>();
        }
        catch
        {
            Debug.Log("B&W is null");
        }
        text = Text.GetComponent<Text>();
        string sceneName = SceneManager.GetActiveScene().name;
        //for(int i = 0; i < Global.SceneStrArray.Length; ++i)
        //    if (Global.SceneStrArray[i] == SceneManager.GetActiveScene().name)
        //    {
        //        year = 2019 + 10 *i;
        //    }
        switch (SceneManager.GetActiveScene().name)
        {
            case "Game1":
                year = 2019;
                break;
            case "Game2":
                year = 2030;
                break;
            case "Game3":
                year = 2040;
                break;
            case "Game5":
                year = 1050;
                break;
        }
        text.text = ((int)(year)).ToString();
        StartCoroutine("InsideRoll");
        StartCoroutine("OutsideRoll");
        StartCoroutine("ShowYear");
        StartCoroutine("GameTimer");
        circleProcess = transform.GetChild(0).gameObject;

    }
    IEnumerator InsideRoll()
    {
        while (true)
        {
            if (reverse)
            {
                Inside.transform.Rotate(0, 0, -5);
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                Inside.transform.Rotate(0, 0, 5);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
    IEnumerator OutsideRoll()
    {
        while (true)
        {
            if (reverse)
            {
                yield return new WaitForSeconds(Timer/10);

                Hashtable args = new Hashtable();
                args.Add("easeType", iTween.EaseType.easeOutElastic);
                args.Add("time", 0.5f/10);
                args.Add("z", 30);
                iTween.RotateAdd(Outside, args);
            }
            else
            {
                yield return new WaitForSeconds(Timer);

                Hashtable args = new Hashtable();
                args.Add("easeType", iTween.EaseType.easeOutElastic);
                args.Add("time", 0.5f);
                args.Add("z", -30);
                iTween.RotateAdd(Outside, args);
            }
            
        }
    }
    IEnumerator GameTimer()
    {
        while (true)
        {
            if (reverse)
            {
                year += Time.deltaTime / (Timer * 3)*10;
                text.text = ((int)(year)).ToString();
                yield return null;
            }
            else
            {
                year -= Time.deltaTime / (Timer * 3);
                text.text = ((int)(year)).ToString();
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update () {

    }
  
    float timer = 0;
    IEnumerator ShowYear()
    {
        while (true)
        {
            while (timer < Timer * 3)
            {
                timer += Time.deltaTime;
                text.color = new Color(1, 1, 1, curve.Evaluate(timer));
                yield return null;
            }
            timer -= Timer * 3;
        }
    }

    public void Chaos(float time)
    {
        StopCoroutine("ShowYear");      // 暂停正常计时
        StopCoroutine("GameTimer"); 
        StartCoroutine(OutsideChaos(time));
        StartCoroutine(InsideChaos(time));


    }
    IEnumerator OutsideChaos(float time)
    {
        // 开黑白特效
        BlackAndWhite.Change();
        float dt = 0.2f;
        while((time -= dt) > 0)
        {
            Hashtable args = new Hashtable();
            args.Add("easeType", iTween.EaseType.easeOutElastic);
            args.Add("time", 0.5f);
            args.Add("z", Random.Range(-180f, 180f));
            iTween.RotateAdd(Outside, args);
            text.text = Random.Range(1900, 2100).ToString();
            text.color = new Color(1, 1, 1, 1);

            yield return new WaitForSeconds(dt);
        }
        StartCoroutine("ShowYear");     // 恢复正常计时
        StartCoroutine("GameTimer");
        // 关黑白特效
        BlackAndWhite.Change();
    }
    IEnumerator InsideChaos(float time)
    {
        float dt = 0.1f;
        while ((time -= dt) > 0)
        {
            Inside.transform.Rotate(0, 0, Random.Range(1f, 90f));
            yield return new WaitForSeconds(dt);
        }
    }

    /// <summary>
    /// 石钟倒流（自动恢复正常计时）
    /// </summary>
    /// <param name="length">30 -> 3s -> 270°</param>
    public void OnBackInTime(int length, int Length)
    {
        StopCoroutine("ShowYear");      // 暂停正常计时
        StopCoroutine("GameTimer");
        Outside.transform.SetSiblingIndex(1);   // 在Hierarchy视图中的同级的顺序，0是最上面
        Outside.transform.rotation = Quaternion.Euler(0, 0, - (float)length / Length * 360 + 78.64f);  // 78.64f是初始角度
        //circleProcess.GetComponent<CircleProcess>().process = 1;
        circleProcess.SetActive(true);
        Text.SetActive(false);
        BackText.SetActive(true);
        StartCoroutine(BackInTime(length, Length));

    }
    IEnumerator BackInTime(int length, int Length)
    {
        //Debug.Log(length);
        while (length-- > 0)
        {
            Outside.transform.Rotate(0, 0, 360f / Length);
            circleProcess.GetComponent<CircleProcess>().process -= 1f / Length;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        circleProcess.GetComponent<CircleProcess>().process = 0f;
        //circleProcess.SetActive(false);
        StartCoroutine("ShowYear");     // 恢复正常计时
        StartCoroutine("GameTimer");
        Outside.transform.SetSiblingIndex(0);
        Text.SetActive(true);
        BackText.SetActive(false);
    }

}
