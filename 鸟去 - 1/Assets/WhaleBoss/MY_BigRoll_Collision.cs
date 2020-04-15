using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MY_BigRoll_Collision : MonoBehaviour {

    [SerializeField]
    GameObject Exit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (Time.timeScale > 0) // 非倒流时
        {
            if (collision.gameObject.tag == "Squid")    
            {
                //collision.gameObject.SetActive(false);
                Global.SceneEvent.GetCurMystery().GetComponent<MYBackInTime>().removeFromBackInTime(collision.gameObject);
                //Destroy(collision.gameObject);
                collision.gameObject.SetActive(false);

                // 若乌贼还未到达出口，则直接消失，一篇漆黑，玩家只能等死
                // 若乌贼已经到达出口，就不能再死，再死就输了
                if (Exit.GetComponent<MY_BigRoll_Exit>().bExit)
                {
                    Global.Player.GetComponent<PlayerEvents>().m_life = 0; 
                    Invoke("Retry", 3);
                }

            }

            if (collision.gameObject.tag == "Player")
            {
                if (!Exit.GetComponent<MY_BigRoll_Exit>().bExit)    // 若乌贼已逃出出口，则只需“假死”
                {
                    collision.gameObject.GetComponent<PlayerEvents>().m_life = 0;
//                    Invoke("Retry", 3);
                }
                else
                    collision.gameObject.SetActive(false);
            }
        }
    }
    void Retry()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneName);
    }
}
