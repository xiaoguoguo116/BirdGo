using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// 需读取 关卡序号Global.LevelIndex 和 关卡前/后标志Global.LevelVideoIndex
/// 并修改 Global.LevelIndex 和跳转关卡
/// 【注意】如果视频播放与场景异步载入（不论是否开协程）同时进行，视频和声音会卡
/// </summary>
public class ComicPlay : MonoBehaviour
{
    [SerializeField]
    RippleEffect effect;
    [SerializeField]
    GameObject Comic, Text;
    [SerializeField]
    VideoClip[] OpenVideo, EndVideo;
    AsyncOperation async;
    void Start()
    {
        //Global.LevelIndex = 2;
        //Global.LevelVideoIndex = 1;

        ShowComic();
        try
        {
            Invoke("StartGame", (float)Comic.GetComponent<VideoPlayer>().clip.length + 1f);
        }
        catch
        {
            StartGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPostion = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            effect.Emit(clickPostion);
        }
    }

    void StartGame()
    {
        if (Global.LevelVideoIndex == 1) // 关卡片尾视频
        {
            if (Global.LevelIndex < Global.SceneStrArray.Length - 1)
            {
                Global.LevelIndex++;
                Global.LevelVideoIndex = 0;
                SceneManager.LoadScene("Comic");
            }
            else
            {
                Global.LevelIndex = 0;
                Global.LevelVideoIndex = 0;
                SceneManager.LoadScene("start");
            }
        }
        else
            SceneManager.LoadScene(Global.SceneStrArray[Global.LevelIndex]);
    }

    public void ShowComic()
    {
        VideoPlayer vp = Comic.GetComponent<VideoPlayer>();
        if (Global.LevelVideoIndex == 0)
            vp.clip = OpenVideo[Global.LevelIndex];
        else
            vp.clip = EndVideo[Global.LevelIndex];
        if (vp.clip)
        {
            vp.SetTargetAudioSource(0, Comic.GetComponent<AudioSource>());
            Comic.SetActive(true);
            Text.SetActive(true);
        }
    }


}
