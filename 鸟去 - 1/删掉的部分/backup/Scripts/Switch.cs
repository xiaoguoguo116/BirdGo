using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Switch : MonoBehaviour, ITraceable
{

    [SerializeField]
    [Tooltip("初始状态")]
    bool TurnOn = false;

    [SerializeField]
    [Tooltip("可反复开关")]
    bool Repeatable = false;

    [SerializeField]
    public GameObject[] Reference;
    ////////////////
    bool[] record;
    bool bSwitchable = true;    // 可切换（防止开关抖动）

    /// <summary>
    /// 根据turnOn更新开关的功能
    /// </summary>
    public virtual void SwitchUpdate(bool turnOn) { }

    public void Init(int count)
    {
        record = new bool[count];
    }
    public void Save(int tail)
    {
        record[tail] = TurnOn;
    }

    public void Load(int tail)
    {
        TurnOn = record[tail];
        if (TurnOn)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(0.24f, 0.176f, 0.16f, 1f);
        }
        else
        {
            GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        }

        SwitchUpdate(TurnOn);

    }
    ////////////////
    // Use this for initialization
    void Start () {
        if (TurnOn)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(0.24f, 0.176f, 0.16f, 1f);
        }
        else
        {
            GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (bSwitchable && collision.gameObject.tag == "Player")
        {
            TurnOn = !TurnOn;
            if (TurnOn)
            {
                GetComponent<SpriteRenderer>().material.color = new Color(0.24f, 0.176f, 0.16f, 1f);
            }
            else
            {
                GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
            }

            SwitchUpdate(TurnOn);

            bSwitchable = false;
            if(Repeatable)
                Invoke("Continue", 0.8f);// 开关切换的最短延时（防止开关抖动）
        }
    }
    void Continue()
    {
        bSwitchable = true;
    }

}
