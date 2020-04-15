using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorToBlackAndWhite : MonoBehaviour
{

    // Use this for initialization
    GameObject root;
    private Material[] beforeChangeMaterial;
    private Shader[] beforeShader;
    private bool changed = false;
    private SpriteRenderer[] nowRender;

    void Start()
    {
        this.root = GameObject.Find("B&W");
    }

    [SerializeField]
    Material clockMaterial;
    void playBackAudio()
    {
        //Debug.Log("播放");
        AudioSource[] sources = GameObject.Find("GameManager").gameObject.GetComponents<AudioSource>();
        GameObject obs = GameObject.Find("UI Root").transform.Find("UI/StoneClock").gameObject;
        Image[] imgs = obs.GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
            img.material = clockMaterial;
        foreach (AudioSource item in sources)//开始回溯，环境音效静音
            item.volume = 0f;
        sources[sources.Length - 1].volume = 1f;    // “倒带声”必须放在检视器的最后一个声音上
    }

    void endBackAudio()
    {
        //Debug.Log("停止");
        AudioSource[] sources = GameObject.Find("GameManager").gameObject.GetComponents<AudioSource>();
        GameObject obs = GameObject.Find("UI Root").transform.Find("UI/StoneClock").gameObject;
        Image[] imgs = obs.GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
            img.material = null;
        foreach (AudioSource item in sources)
            item.volume = 1f;
        sources[sources.Length - 1].volume = 0f;//回溯音效静音
    }

    /// <summary>
    /// 黑白特效的开关，另外包括声音、游戏暂停的处理
    /// </summary>
    public void Change() //chenge all image render shader to grey
    {
        //SceneEventManager Manager = GameObject.Find("GameManager").gameObject.GetComponent<SceneEventManager>();
        
        if (this.changed)   // 关闭特效
        {
            Debug.Log("have been changed");
            endBackAudio(); // 声音
            //if(Global.SceneEvent.GetCurMystery().name != "MY8")
            if (Global.SceneEvent.GetCurrentMyIndex() != Global.SceneEvent.GetMysteryCount() - 1 &&     // 游戏时间暂停（5号和最后一个谜题不用暂停）
                Global.SceneEvent.GetCurrentMyIndex() != 5)
                Time.timeScale = 1;
#if true    
            for (int i = 0; i < this.nowRender.Length; ++i) // assign beforeChangeMaterial
            {
                try
                {
                    this.nowRender[i].material.shader = this.beforeShader[i];
                }
                catch
                {
                    Debug.Log("该对象已被清楚");
                }
            }
#endif
        }
        else   // 开启特效
        {
            Debug.Log("unchange");
            playBackAudio();    // 声音
            //if (Global.SceneEvent.GetCurMystery().name != "MY8")
            if (Global.SceneEvent.GetCurrentMyIndex() != Global.SceneEvent.GetMysteryCount() - 1 &&     // 游戏时间暂停（5号和最后一个谜题不用暂停）
                Global.SceneEvent.GetCurrentMyIndex() != 5)
                Time.timeScale = 0;
#if true    
            this.nowRender = this.root.GetComponentsInChildren<SpriteRenderer>();   // 使能掉的对象不会被遍历到
            this.beforeChangeMaterial = new Material[nowRender.Length];
            this.beforeShader = new Shader[nowRender.Length];
            for (int i = 0; i < this.nowRender.Length; ++i) // assign beforeChangeMaterial
            {
                //this.beforeChangeMaterial[i] = nowRender[i].material;
                this.beforeShader[i] = nowRender[i].material.shader;
                //nowRender[i].material = this.greyMat;

                //nowRender[i].material.shader = Shader.Find("Custom/ImageGreyShader");
                nowRender[i].material.shader = Shader.Find("Sprites/Default_B&W");
            }
#endif
        }
        this.changed = !this.changed;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
