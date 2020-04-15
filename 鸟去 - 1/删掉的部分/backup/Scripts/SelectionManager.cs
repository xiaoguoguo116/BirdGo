using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Video;

public class SelectionManager : MonoBehaviour
{

    [SerializeField]
    RippleEffect effect;
    [SerializeField]
    GameObject mask;
    [SerializeField]
    GameObject[] Comics;
    bool isContinue = false;

    //RectTransform[] selections;
    List<RectTransform> selections = new List<RectTransform>();

    AsyncOperation async;

    private void Start()
    {
        GameObject[] selects = GameObject.FindGameObjectsWithTag("Selections");
        //selections = new RectTransform[selects.Length];
        for (int i = 0; i < selects.Length; i++)
        {
            selections.Add(selects[i].GetComponent<RectTransform>());
        }
        mask.SetActive(false);
        //PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("Scene"))
            GameObject.Find("Continue").SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPostion = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            effect.Emit(clickPostion);
        }
    }

    GameObject currentButton;
    public void OnClickToLoad()
    {
        currentButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        RectTransform current = currentButton.GetComponent<RectTransform>();
        //Debug.Log(currentButton.name);
        selections.Remove(current);

        foreach (RectTransform item in selections)
        {
            item.DOScale(new Vector2(0f, 0f), 0.5f);
        }

        current.DOLocalMove(new Vector2(0f, 0f), 0.5f);
        current.DOScale(new Vector2(1.5f, 1.5f), 0.5f);
        Invoke("TurnDark", 1f);
    }

    public void OnClickToCotinue()
    {
        isContinue = true;
        Invoke("TurnDark", 0f);
    }

    void TurnDark()
    {
        mask.SetActive(true);
        mask.GetComponentInChildren<Text>().color = new Color(255, 255, 255, 0);
        mask.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0);
        mask.GetComponentsInChildren<Image>()[2].color = new Color(255, 255, 255, 0);
        mask.GetComponent<Image>().DOFade(1, 1f);
        Invoke("StartLoad", 1f);   
    }

    void StartLoad()
    {
        if (isContinue)
        {
            mask.GetComponentInChildren<Text>().color = new Color(255, 255, 255, 1);
            mask.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 1);
            mask.GetComponentsInChildren<Image>()[2].color = new Color(255, 255, 255, 1);
            if (PlayerPrefs.HasKey("Scene"))
            {
                StartCoroutine(LoadGame(PlayerPrefs.GetString("Scene")));
            }
            else
            {
                StartCoroutine(Global.SceneStrArray[0]);
            }
            
        }
        else
        {
            mask.GetComponentInChildren<Text>().color = new Color(255, 255, 255, 1);

            PlayerPrefs.SetInt("position", 0);
            PlayerPrefs.Save();

            Global.LevelIndex = int.Parse(currentButton.name) - 1;  // 按钮名字“1”~“3”，必须分别在Build的2~4的位置上【不健壮】
            Global.LevelVideoIndex = 0;
            SceneManager.LoadScene("Comic");
  
            //Comics[int.Parse(currentButton.name)-1].SetActive(true);//播放动画
            //try
            //    {
            //    Invoke("StartGame", (float)Comics[int.Parse(currentButton.name) - 1].GetComponent<VideoPlayer>().clip.length);//动画结束自动加载场景
            //}
            //catch
            //{
            //    StartCoroutine(LoadGame(int.Parse(currentButton.name) + 1));
            //}
        }   
    }

    //public void StartGame()
    //{
    //    SceneManager.LoadScene(int.Parse(currentButton.name) + 1);
    //    PlayerPrefs.SetInt("position", 0);
    //    PlayerPrefs.Save();
    //}

    //IEnumerator LoadGame(int index)
    //{     
    //    async = SceneManager.LoadSceneAsync(index);
    //    async.allowSceneActivation = false;

    //    while (!async.isDone)
    //    {
    //        float progress = async.progress;
    //        if (progress >= 0.9f)
    //        {
    //            async.allowSceneActivation = true;
    //            PlayerPrefs.SetInt("position", 0);
    //            PlayerPrefs.Save();
    //        }
    //        yield return null;
    //    }       
    //}

    IEnumerator LoadGame(string name)
    {
        async = SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            float progress = async.progress;
            if (progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}
