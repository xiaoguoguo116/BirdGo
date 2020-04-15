using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNetwork : MonoBehaviour {
    [SerializeField]
    private float smoothingSpeed = 2f;
    //{{ 自动卷屏机制（不可后退，会被卷死）
    //自动卷屏速度（摄像机在不跟随玩家对象的情况下的移动速度）
    [SerializeField]
    public float cameraMoveSpeed = 1;
    // Use this for initialization
    void Start () {

    }

    void LateUpdate() {
        if (!Global.Player)
            return;
        Vector3 defaultOffset = new Vector3(transform.position.x + cameraMoveSpeed, Mathf.Clamp(Global.Player.transform.position.y, -Global.BGHight + Global.CamHight, Global.BGHight - Global.CamHight), -10f);
        transform.position = Vector3.Lerp(transform.position, defaultOffset, smoothingSpeed * Time.deltaTime);
    }
    // Update is called once per frame
}
