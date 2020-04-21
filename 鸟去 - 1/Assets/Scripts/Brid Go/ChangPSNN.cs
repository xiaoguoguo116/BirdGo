using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangPSNN : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //GetComponent<PolygonCollider2D>().isTrigger = true;
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.name == "CanRotate" || collision.name == "CanMove"|| collision.name == name)
            return;
        if (!GameObject.Find("CanRotate").GetComponent<IsTouchRotate>().PSOBJ)
            return;
        if (GameObject.Find("CanRotate").GetComponent<IsTouchRotate>().PSOBJ.GetComponent<PSOperation>().CanPS)//改变canps和pstool状态
        {
            //GlobalNN.PSTool.transform.position = new Vector3(GlobalNN.PSTool.transform.position.x, GlobalNN.PSTool.transform.position.y, 1);
            GameObject.Find("CanRotate").GetComponent<IsTouchRotate>().PSOBJ.GetComponent<PSOperation>().CanPS = false;
            //这里需要关闭ismove并且将正在ps的物体重置position到上一时刻位子，避免bug穿透
            //GameObject.Find("CanMove").GetComponent<IsTouchMove>().PSMove = false;//关闭ps位置
            //Vector3 a = GlobalNN.PSTool.GetComponent<PSOperation>().a
            transform.Translate(-this.GetComponent<PSOperation>().a, Space.World);
           // GlobalNN.PSTool.transform.Translate(-this.GetComponent<PSOperation>().a, Space.World);
            GlobalNN.PSTool.transform.position = new Vector3(-64, 64, 0);
        }
    }
    // Update is called once per frame
    void Update () {

    }
}
