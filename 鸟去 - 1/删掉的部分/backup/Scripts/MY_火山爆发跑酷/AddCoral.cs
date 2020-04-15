using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddCoral : MonoBehaviour {

    // Use this for initialization
    public GameObject Coral;
    [SerializeField]
    private bool Test;
	void Start () {
        if(!Test)
        {
            //try
            //{
            //    string lastDeadScene = Global.lastDeadScene;
            //}
            //catch
            //{

            //}
            //string nowScene = SceneManager.GetActiveScene().name;
            //int lastDeadMy = Global.lastDeadMy;
            //int nowMy = GameObject.Find("Game Manager").GetComponent<SceneEventManager>().GetCurrentMyIndex();
            try
            {
                if (Global.lastDeadScene == SceneManager.GetActiveScene().name && Global.lastDeadMy == GameObject.Find("Game Manager").GetComponent<SceneEventManager>().GetCurrentMyIndex())//上次死亡是在当前场景的当前谜题就加个乌贼
                {
                    Coral.SetActive(true);
                }
            }
            catch
            {
                Debug.Log("上次没死");
            }
        }
        else
        {
            Coral.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
