using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class WhaleNetwork : TouchableNetwork
{
    [SyncVar]
    [Tooltip("是否反向")]
    [SerializeField]
    public bool Reverse = false;
    [SerializeField]
    public float MoveSpeed = 0;
    [SerializeField]
    ParticleSystem Particle;
    Animator m_ani;
    GameObject waterForce;
    [SerializeField]
    [Tooltip("自动间歇喷水")]
    bool AutoBump = false;
    const float Duration = 1f;  // 喷力持续时间（秒）

    //public void TouchEventNetwork()
    //{
    //    int id = GameManager.Touchables.IndexOf(gameObject);
    //    if (id >= 0)
    //        Global.Player.GetComponent<PlayerEventsNetwork>().CmdTouchEvent(id);
    //}

    /// <summary>
    /// 本函数只在服务器端被调用
    /// </summary>
    public override bool TouchEvent() {

        if (waterForce.activeInHierarchy == false)
        {
            waterForce.SetActive(true);
            Invoke("StopWaterForce", Duration);

            //Effect();
            RpcEffect();
            return true;
        }
        else
            return false;
        
    }
    void StopWaterForce()
    {
        waterForce.SetActive(false);
    }

    /// <summary>
    /// 特效（声音、动画、粒子）
    /// </summary>
    protected override void Effect()
    {
        gameObject.GetComponent<AudioSource>().Play();
        m_ani.SetTrigger("whale");
        StartCoroutine((WaterParticles(Duration)));
    }


    // Use this for initialization
    void Start () {

        base.Start();

        if (Reverse)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            MoveSpeed = -MoveSpeed;
        }

        waterForce = transform.Find("WaterForce").gameObject;
        m_ani = this.GetComponent<Animator>();
        if (GameObject.Find("GameManager").GetComponent<SceneEventManager>().GetCurrentSceneIndex(SceneManager.GetActiveScene().name) == 3)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.905f, 0.764f, 0.764f, 1f);
        }
    }

    private void OnEnable()
    {
        if (AutoBump)
        {
            InvokeRepeating("startWater", 0f, 4f);
        }
    }

    // Update is called once per frame
    void Update () {
        this.transform.Translate(new Vector3(Time.deltaTime * MoveSpeed, 0, 0));

    }

    /// <summary>
    /// 持续second秒产生水粒子
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    IEnumerator WaterParticles(float second)
    {
        while (second > 0)
        {
            StartCoroutine(OneWaterParticle());
            yield return null;
            second -= Time.deltaTime;
        }
    }
    IEnumerator OneWaterParticle()
    {
        ParticleSystem ob = Instantiate(Particle, transform);
        ob.transform.localPosition = waterForce.transform.localPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 5f), 0);       
        ob.transform.localRotation = Particle.transform.rotation;

        yield return new WaitForSeconds(2);
        Destroy(ob);
    }

}
