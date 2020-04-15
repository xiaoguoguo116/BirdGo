using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleStoneClock : MonoBehaviour
{

    [SerializeField]
    [Tooltip("外圈跳一格的秒数")]
    float Timer = 2f;
    [SerializeField]
    GameObject Inside;
    [SerializeField]
    GameObject Outside;
    [HideInInspector]
    public bool reverse = false;    // 正转or反转
    // Use this for initialization
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
    }

    private void OnEnable()
    {
        StartCoroutine("InsideRoll");
        StartCoroutine("OutsideRoll");
    }

    IEnumerator InsideRoll()
    {
        while (true)
        {
            Inside.transform.Rotate(0, 0, 5);
            yield return new WaitForSeconds(0.02f);
        }
    }
    IEnumerator OutsideRoll()
    {
        while (true)
        {
            yield return new WaitForSeconds(Timer);
            Hashtable args = new Hashtable();
            args.Add("easeType", iTween.EaseType.easeOutElastic);
            args.Add("time", 0.5f);
            args.Add("z", -30);
            iTween.RotateAdd(Outside, args);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
