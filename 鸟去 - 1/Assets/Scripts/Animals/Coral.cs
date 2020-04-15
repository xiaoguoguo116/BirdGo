using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coral : TouchableBehaviour, ITraceable
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
    [Tooltip("是否有毒")]
    public bool toxic = false;
    [SerializeField]
    [Header("失明时长（秒），一般不超过3秒")]
    public float BlindTime = 3f;
    [Tooltip("相对龟的移速比例")]
    [SerializeField]
    float speedScale = 0.5f;  
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
        // lt = ligh.GetComponent<Light>();

        player = Global.Player;
        //Materials是MeshRenderer的一个属性
        m_material = gameObject.GetComponent<SpriteRenderer>().material;
        //Debug.Log(m_material.name);
        //修改材质的颜色
        m_material.color = Color.black;
        //设置主贴图
        //        m_material.mainTexture = newTex;
        //设置游戏
        //  lt.intensity = 30;
        m_material.SetColor("_Color", Color.white);

        //Ink = transform.Find("Ink").gameObject;
        // 喷墨
        Inkjet = transform.Find("Ink").GetComponent<ParticleSystem>();
        Inkjet.Stop();
        StartCoroutine("AutoInkjet");

        Trigger = transform.Find("Trigger");
        LightWithEdge = transform.Find("LightWithEdge");
        Spotlight = LightWithEdge.transform.Find("Spotlight");

        RandTime = Random.Range(0f, 1f);

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

    public void Restore()
    {
        StartCoroutine("AutoInkjet");
        StopCoroutine("LookAtPlayer");
        transform.rotation = Quaternion.identity;
    }

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
    public void RotateTo()
    {
        StartCoroutine("LookAtPlayer");
    }
    IEnumerator LookAtPlayer()
    {
        while (true)
        {
            Vector3 targetdir = player.transform.position - transform.position;
            Flip(targetdir);
            targetdir.z = 0;
            Quaternion rotation;
            if (targetdir.x >= 0)
                rotation = Quaternion.FromToRotation(Vector3.right, targetdir);
            else
                rotation = Quaternion.FromToRotation(Vector3.left, targetdir);
            transform.rotation = rotation;
            //iTween.RotateTo(gameObject, rotation.eulerAngles, 5f);

            yield return null;
        }
    }

    void Update()
    {
        //Trigger.localScale = Vector2.one * (1 + curve.Evaluate(Time.time / 2 + RandTime));
        //LightWithEdge.localScale = Vector2.one * (1 + curve.Evaluate(Time.time / 2));
        Spotlight.GetComponent<Light>().spotAngle = LightRange * 30 * (1 + curve.Evaluate(Time.time / 2 + RandTime));

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
            bLight = !bLight;
            ShowLight();
            return true;
        }
        else
            return false;
    }

    public void AddForce(Vector2 force)
    {
        GetComponent<Rigidbody2D>().AddForce(force * speedScale);
    }
}

