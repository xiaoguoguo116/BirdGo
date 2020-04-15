using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    RectTransform[] tweens;
    [SerializeField]
    RectTransform back;
    [SerializeField]
    RectTransform[] panels;
    [SerializeField]
    RectTransform[] title;

    List<Tweener> tweenList;
    bool isUp = false;

    [SerializeField]
    float firstY = 100f;
    [SerializeField]
    float rate = 0.5f;

    RectTransform currentPanel;

    // Use this for initialization
    void Start()
    {
        back.GetComponent<Image>().DOColor(new Color(255, 255, 255, 0), 0f);
        back.GetComponent<Button>().enabled = false;

        InitPosition(tweens);
        InitPosition(panels);

        float newRate = rate;

        foreach (RectTransform item in tweens)
        {        
            item.DOLocalMoveY(firstY, newRate);
            newRate += 0.2f;
            firstY -= 75f;
        }

        Debug.Log("当前存档：" + PlayerPrefs.GetString("Scene") + " ; " + PlayerPrefs.GetInt("position"));
    }

    void InitPosition(RectTransform[] list)
    {
        foreach (RectTransform item in list)
        {
            Tweener tweener = item.DOLocalMove(new Vector2(0f, -1000f), 0f);
            tweener.SetAutoKill(false);
            //tweenList.Add(tweener);
            //tweener.Pause(); 
        }
    }
    public void ClickToMove()
    {
        GameObject currentButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        //Debug.Log(currentButton.name);

        back.GetComponent<Image>().DOColor(new Color(255, 255, 255, 255), rate);
        back.GetComponent<Button>().enabled = true;

        float newRate = rate;
        //back.DOScale(new Vector2(1f, 1f), 0.5f);
        for (int i = 0; i < tweens.Length; i++)
        {
            if (currentButton.name.Equals(tweens[i].gameObject.name))
            {
                currentPanel = panels[i];
                currentPanel.DOLocalMove(new Vector2(0f, -30f), rate);
            }
            tweens[i].DOLocalMoveX(1000f, newRate);
            newRate += 0.2f;
        }
            
    }

    public void ClickToBack()
    {
        back.GetComponent<Image>().DOColor(new Color(255, 255, 255, 0), rate);
        back.GetComponent<Button>().enabled = false;
        float newRate = rate;
        currentPanel.DOLocalMove(new Vector2(0f, -1000f), rate);
        //back.DOScale(new Vector2(0f, 0f), 0.5f);
        foreach (RectTransform item in tweens)
        {
            item.DOLocalMoveX(0f, newRate);
            newRate += 0.2f;
        }   
    }

    public void ClickToQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
