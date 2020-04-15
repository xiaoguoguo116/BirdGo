using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 墙是3D相机拍摄，放到比主场景镜头更深处渲染（使得龟可以遮挡墙），要求主场景的背景图必须去掉（不然会完全遮挡住墙）
/// 大倒流是通过触发器里添加倒流脚本实现的
/// </summary>
public class MYWall : MonoBehaviour {

    [SerializeField]
    GameObject CommonEffects;
    [SerializeField]
    GameObject Environment;
    [SerializeField]
    GameObject Fade;
    [SerializeField]
    GameObject Wall;
    [SerializeField]
    GameObject BackWall;

    CameraFollow cameraFollow;
    // Use this for initialization
    void Start ()
    { 
        transform.Find("3DCamera").transform.position = Camera.main.transform.position;
        transform.Find("3DCamera").transform.parent = Camera.main.transform;
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        StartCoroutine("BlackBackground");

	}
    IEnumerator BlackBackground()
    {
        yield return new WaitForSeconds(5.2f);  // >5.1s    // 发布时删去此行（专门测试本谜题时才用，用来等待UIManager里淡进的时间）
        // 渐黑
        Fade.SetActive(true);
        Fade.GetComponent<Image>().DOFade(1, 1f);
        yield return new WaitForSeconds(1f);

        // 关闭特效等
        //CommonEffects.SetActive(false);
        Environment.SetActive(false);   // 主场景的背景图必须去掉
        Camera.main.GetComponent<DisdortEffect>().enabled = false;

        // 渐亮
        Fade.GetComponent<Image>().DOFade(0, 1f);
        Camera.main.cullingMask = (1<<14);
        //yield return new WaitForSeconds(1f);
    }
    IEnumerator Stop()
    {
        while (cameraFollow.cameraMoveSpeed > 0)
        {
            cameraFollow.cameraMoveSpeed = Mathf.Lerp(cameraFollow.cameraMoveSpeed, 0, Time.deltaTime);
            yield return null;
        }
    }

    // Update is called once per frame
    bool isStop = false;
    void Update() {
        if (!isStop && Camera.main.transform.position.x > Wall.transform.position.x - 8)
        {
            StartCoroutine("Stop");
            //BackWall.SetActive(true);
            //BackWall.transform.DOMoveX(Wall.transform.position.x - 15, 3);
            isStop = true;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Global.Player && GetComponent<MYBackInTime>() == null)
        {
            gameObject.AddComponent<MYBackInTime>();
        }

    }

    public void MoveCamera(float distance, float time)//移动相机
    {
        GameObject camera3d = GameObject.Find("3DCamera");
        iTween.MoveBy(camera3d, new Vector3(0, 0, distance), time);
    }

}
