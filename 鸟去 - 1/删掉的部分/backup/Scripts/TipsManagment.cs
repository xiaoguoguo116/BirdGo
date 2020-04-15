using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsManagment : MonoBehaviour {

    private Text text;
    float a = 0;

    [SerializeField]
    float a_speed = 0.2f;
    private bool isCollided = false;
    public float year;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        text.color = new Color(255, 255, 255, a);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(Global.Player.tag))
        {
            if(gameObject.tag == "endTip" && !this.isCollided)
            {
                isCollided = true;
                StartCoroutine("showVictory");
            }
            InvokeRepeating("TipAppear", 0f, a_speed);
        }

    }

    IEnumerator showVictory()
    {
        this.year = GameObject.Find("StoneClock").GetComponent<StoneClock>().year;
        GameObject victory = GameObject.Find("UI").transform.Find("Victory").gameObject;
        //Debug.Log(year);
        PlayerPrefs.SetInt("position", 0);
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        yield return new WaitForSeconds(6f);
        Global.Input.SetActive(false);
        victory.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(Global.Player.tag))
        {
            InvokeRepeating("TipDisappear", 0f, a_speed);
        }
    }
    void TipAppear()
    {  
        a += 0.1f;
        text.color = new Color(255, 255, 255, a);
        if (a >= 1)
        {
            CancelInvoke();
        }
    }

    void TipDisappear()
    {
        a -= 0.1f;
        text.color = new Color(255, 255, 255, a);
        if (a <= 0)
        {
            CancelInvoke();
            Destroy(this.gameObject);
        }
    }
}
