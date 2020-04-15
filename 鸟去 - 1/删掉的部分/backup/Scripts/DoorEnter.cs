using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorEnter : MonoBehaviour {

    [SerializeField]
    int count;

    [SerializeField]
    Light en_light;

    bool isLighting = false;

    GameObject currentCoral;
    GameObject player;

    Camera main;
	// Use this for initialization
	void Start () {
        player = Global.Player;
        main = Camera.main;
    }
	// Update is called once per frame
	void Update () {
        if (count >= 4 && !isLighting)
        {
            isLighting = true;
        }

        if (isLighting)
        {
            en_light.intensity += 0.005f;
            if (en_light.intensity >= 1f)
            {
                en_light.intensity = 1f;
            }

            //if (playerMove)
            //{
            //    Invoke("DestroyThis", 0f);
            //}
        }
	}

    Color currentColor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Squid"))
        {
            currentCoral = collision.gameObject;
            Debug.Log("乌贼进门了");
            currentColor = currentCoral.GetComponent<SpriteRenderer>().color;
            InvokeRepeating("BeDisappear", 0f, 0.01f);
        }

        if (collision.gameObject.tag.Equals("Player") && isLighting/*en_light.intensity >= 1f*/)
        {
            Debug.Log("玩家跳转");
            SceneEventManager sm = Global.SceneEvent;
            //player.transform.position = sm.points[sm.points.Length - 2].transform.position;
            player.transform.position = sm.GetNextPoint().position;
            //player.GetComponent<Rigidbody>().velocity = Vector2.zero; // ∵AddForce是持续过程，此句无效

            //main.orthographicSize = 8f;
            //Global.CamHight = main.orthographicSize;
            //Global.CamWidth = Global.CamHight * main.aspect;
        }
    }

    //void DestroyThis()
    //{
    //    Destroy(this);
    //}

    void BeDisappear()
    {
        float a = currentCoral.GetComponent<SpriteRenderer>().color.a - 0.01f;
        currentCoral.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, a);
        if (a < 0)
        {
            Destroy(currentCoral);
            currentCoral = null;
            CancelInvoke();
            count += 1;
        }
    }

    private void OnDestroy()
    {
        if(main)
            main.GetComponent<CameraFollow>().enabled = true;
    }
}
