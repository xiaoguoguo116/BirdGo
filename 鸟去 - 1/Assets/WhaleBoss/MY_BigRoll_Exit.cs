using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MY_BigRoll_Exit : MonoBehaviour
{

    [SerializeField]
    GameObject Coral;
    [HideInInspector]
    public bool bExit = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bExit)
        {
            Vector3 dst = new Vector3 (Coral.transform.position.x, Coral.transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, dst, Time.deltaTime);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Coral)  // 若乌贼到达出口，镜头改为跟随乌贼
        {
            bExit = true;
            //Coral.GetComponent<CircleCollider2D>().enabled = false;
            Camera.main.GetComponent<CameraFollow>().enabled = false;
            //Invoke("Win", 5);
        }
    }

    //void Win()
    //{
    //    Global.UI.GetComponent<UIManager>().victoryPanel.SetActive(true);
    //    Global.Input.SetActive(false);
    //    PlayerPrefs.SetInt("position", 0);
    //    Camera.main.GetComponent<CameraFollow>().enabled = false;
    //}
}
