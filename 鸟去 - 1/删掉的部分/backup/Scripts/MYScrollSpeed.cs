using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MYScrollSpeed : MonoBehaviour {

    [SerializeField]
    [Tooltip("卷屏速度系数（>0，默认1，不会自动恢复）")]
    float ScrollSpeed = 1.0f;
	// Use this for initialization
	void Start () {
        // 【在联机模式里，此处无效】
        if(Camera.main.GetComponent<CameraFollow>() != null)
             Camera.main.GetComponent<CameraFollow>().cameraMoveSpeed *= ScrollSpeed;
        if (Camera.main.GetComponent<CameraFollowNetwork>() != null)
            Camera.main.GetComponent<CameraFollowNetwork>().cameraMoveSpeed *= ScrollSpeed;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void OnDestroy()
    //{
    //    Camera.main.GetComponent<CameraFollow>().cameraMoveSpeed /= 0.2f;
    //}
}
