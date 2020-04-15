using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Click : MonoBehaviour
{

    [SerializeField]
    RippleEffect effect;
    [SerializeField]
    GameObject bg;

    Animation bgAnima;

    AsyncOperation async;

    [SerializeField]
    GameObject Comic, Text;
    [SerializeField]
    Text Version;
    void Start()
    {
        bgAnima = bg.GetComponent<Animation>();
        StartCoroutine(LoadGame());

        Version.text = Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPostion = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            effect.Emit(clickPostion);
            if (Comic.activeInHierarchy == false)
            {
                Invoke("ShowComic", 1f);
                Invoke("StartGame", (float)Comic.GetComponent<VideoPlayer>().clip.length + 1f);
            }

            //Invoke("Enter", 10f);
        }
    }

    IEnumerator LoadGame()
    {
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            //Debug.Log(async.progress);
            float progress = async.progress;
            if (progress >= 0.9f)
            {
                if (!bgAnima.isPlaying)
                {
                    bg.GetComponentInChildren<Text>().text = "点击屏幕任意处开始游戏";
                }
            }
            yield return null;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void Enter()
    {
        if (!bgAnima.isPlaying)
        {
            async.allowSceneActivation = true;
        }
    }

    public void ShowComic()
    {
        GetComponent<AudioSource>().enabled = false;
        Comic.SetActive(true);
        Text.SetActive(true);
    }
}
