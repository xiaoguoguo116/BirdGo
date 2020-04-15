using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeRuler : MonoBehaviour
{

    [SerializeField]
    GameObject RulerPerfab; // 将来改成Find
    [SerializeField]
    float speed = 100;

    const int CntRuler = 12;	//4;
    GameObject[] Rulers = new GameObject[CntRuler];
    float Width;
    Text text;
    [HideInInspector]
    public int year;
    [SerializeField]
    AnimationCurve curve;
    [SerializeField]
    Image timePlate1;
    [SerializeField]
    Image timePlate2;


    // Use this for initialization
    void Start()
    {
        text = transform.Find("Text").GetComponent<Text>();

        //for (int i = 0; i < Global.SceneStrArray.Length; ++i)
        //    if (Global.SceneStrArray[i] == SceneManager.GetActiveScene().name)
        //    {
        //        year = 2019 + 10 * i;
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

        Width = GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < CntRuler; i++)
        {
            Rulers[i] = MonoBehaviour.Instantiate(RulerPerfab) as GameObject;
            Rulers[i].transform.SetParent(transform);
            Rulers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(Width / CntRuler * i, -25);
            /*Rulers[i].transform.localScale = new Vector2(1f, 0.5f);
            if (i % 2 != 0)  // 季度刻度
            {
                Rulers[i].transform.localScale = new Vector2(0.5f, 0.25f);
            }*/
            Rulers[i].transform.localScale = new Vector3(0.4f, 1f);
            if (i % 2 != 0 || i % 4 != 0)  // 季度刻度
            {
                Rulers[i].transform.localScale = new Vector3(0.2f, 0.25f);
            }
        }
        //StartCoroutine("SpeedUp");
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < CntRuler; i++)
        {
            float x = Rulers[i].GetComponent<RectTransform>().anchoredPosition.x;
            x -= speed * Time.deltaTime;
            if (x < 0)
            {
                x += Width;
                //if (i % 2 == 0)
                if (i % 4 == 0)
                {
                  if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEvents>().m_life > 0)
                    {
                        text.text = (year++).ToString();
                        StartCoroutine("ShowYear");
                    }
                }
            }
            Rulers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -25);
        }
        //timePlate1.gameObject.transform.Rotate(new Vector3(0, 0, speed * -0.1f) * Time.deltaTime);
        //timePlate2.gameObject.transform.Rotate(new Vector3(0, 0, speed * -0.1f) * Time.deltaTime);
    }
    float timer = 0;
    IEnumerator ShowYear()
    {
        while (timer < 5f)
        {
            timer += Time.deltaTime;
            text.color = new Color(1, 1, 1, curve.Evaluate(timer));
            yield return null;
        }
        timer = 0;
    }

    IEnumerator SpeedUp()
    {
        //while(cameraFollow.cameraMoveSpeed < CameraMoveSpeed)
        while (true)
        {
            //cameraFollow.cameraMoveSpeed = Mathf.Lerp(cameraFollow.cameraMoveSpeed, CameraMoveSpeed, Time.deltaTime);
            //PlayerSpeed = Mathf.Lerp(PlayerSpeed, PlayerAutoSpeed, Time.deltaTime);
            //print(cameraFollow.cameraMoveSpeed + "," + CameraMoveSpeed);
            Debug.Log("SpeedUp");
            yield return null;
        }
    }

}
