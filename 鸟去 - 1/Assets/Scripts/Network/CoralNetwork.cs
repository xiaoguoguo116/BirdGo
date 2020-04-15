using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Networking;

public class CoralNetwork : TouchableDrawableNetwork, ITraceable
{
    Material m_material;
    //[SerializeField]
    //Texture newTex; // 没用？

    GameObject player;
    [SerializeField]
    [Tooltip("可切换状态")]
    bool Switchable = true;
    bool bLight = true;  // 开关
    //GameObject Ink;
    ParticleSystem Inkjet;
    [SerializeField]
    [Header("失明时长，大于3表示需重来")]
    float BlindTime = 3f;
    [SerializeField]
    AnimationCurve curve;
    Transform Trigger;
    Transform LightWithEdge;
    Transform Spotlight;

    [SerializeField]
    private float LightRange = 1f;
    float RandTime; // 视野呼吸不要同步

    ////////////////
    bool[] Lights;
    public void Init(int count)
    {
        Lights = new bool[count];
    }
    public void Save(int tail)
    {
        Lights[tail] = bLight;
    }
    public void Load(int tail)
    {
        bLight = Lights[tail];
        ShowLight();
    }
    ////////////////

    //public Mozhi mozhi;
    // Use this for initialization
    protected void Start()
    {
        base.Start();

        Global.MYNetwork.GetComponent<MYBackInTimeNetwork>().addToBackInTime(gameObject);

        player = Global.Player;
        m_material = gameObject.GetComponent<SpriteRenderer>().material;
        //修改材质的颜色
        m_material.color = Color.black;

        m_material.SetColor("_Color", Color.white);
        // 喷墨
        Inkjet = transform.Find("Ink").GetComponent<ParticleSystem>();
        Inkjet.Stop();
        StartCoroutine("AutoInkjet");

        Trigger = transform.Find("Trigger");
        LightWithEdge = transform.Find("LightWithEdge");
        Spotlight = LightWithEdge.transform.Find("Spotlight");

        RandTime = UnityEngine.Random.Range(0f, 1f);
        transform.Find("Trigger").GetComponent<DeadTriggerNetwork>().BlindTime = BlindTime;

        // 出生时从小变大
        Vector3 origin = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(origin, 1);
    }
    IEnumerator AutoInkjet()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            Inkjet.Play();
            yield return new WaitForSeconds(0.5f);
            Inkjet.Stop();
        }
    }
    public void Letout()
    {
        StopCoroutine("AutoInkjet");
        Inkjet.Play();
    }

    //IEnumerator IEnumeratorLook = null;
    Coroutine CoroutineLook = null;
    public void Restore()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine("AutoInkjet");

            if (CoroutineLook != null)
                StopCoroutine(CoroutineLook);

            transform.rotation = Quaternion.identity;
        }
        else
            print("activeInHierarchy = false");


    }

    /// <summary>
    /// 翻转是通过localScale的x取相反数（或者修改SpriteRenderer -> Flip -> X）
    /// UNet的同步组件无法同步localScale，因此客户端与服务器端都要执行操作
    /// </summary>
    /// <param name="direction"></param>
    public void Flip(Vector3 direction)
    {
        if (direction.x >= 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            Inkjet.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            Inkjet.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    
    public void RotateTo(GameObject target)
    {
        //IEnumeratorLook = LookAtPlayer(target);
        //StartCoroutine(IEnumeratorLook);

        CoroutineLook = StartCoroutine(LookAtPlayer(target));
    }

    
    IEnumerator LookAtPlayer(GameObject target)
    {
        while (true)
        {
            if (target)
            {
                Vector3 targetdir = target.transform.position - transform.position;
                Flip(targetdir);
                targetdir.z = 0;
                Quaternion rotation;
                if (targetdir.x >= 0)
                    rotation = Quaternion.FromToRotation(Vector3.right, targetdir);
                else
                    rotation = Quaternion.FromToRotation(Vector3.left, targetdir);
                transform.rotation = rotation;
                //iTween.RotateTo(gameObject, rotation.eulerAngles, 5f);
            }
            yield return null;
        }
    }

    void Update()
    {
        //Trigger.localScale = Vector2.one * (1 + curve.Evaluate(Time.time / 2 + RandTime));
        //LightWithEdge.localScale = Vector2.one * (1 + curve.Evaluate(Time.time / 2));
        Spotlight.GetComponent<Light>().spotAngle = LightRange * 30 * (1 + curve.Evaluate(Time.time / 2 + RandTime));

        Flip(GetComponent<Rigidbody2D>().velocity); // Flip的处理不能通过UNet的同步组件完成，只能客户端本地执行
    }

    void ShowLight()
    {
        if (bLight)
        {
            m_material.SetColor("_Color", Color.white);
            for (int j = 0; j < this.transform.childCount - 1; j++)
            {
                this.transform.GetChild(j).gameObject.SetActive(true);
            }
        }
        else
        {
            m_material.SetColor("_Color", Color.black);
            for (int j = 0; j < this.transform.childCount - 1; j++)
            {
                this.transform.GetChild(j).gameObject.SetActive(false);
            }
        }
    }
    public override bool TouchEvent()
    {
        if (Switchable)
        {
            RpcEffect();
            return true;
        }
        else
            return false;
    }
    protected override void Effect()
    {
        bLight = !bLight;
        ShowLight();
    }

    public override void OnNetworkDestroy()
    {
        Global.MYNetwork.GetComponent<MYBackInTimeNetwork>().removeFromBackInTime(gameObject);
        base.OnNetworkDestroy();
    }


}

