using UnityEngine;
using System.Collections;

public class Cruise : MonoBehaviour
{
    // 巡航结束时，为防止触发划屏OnSwipe()
    // 开始巡航时设置成true；
    // 结束巡航（手指离开屏幕）时，如果期间手指有移动，则会触发OnSwipe
    // 如果手指没有移动，则不会触发OnSwipe，所以要视情况而定
    [HideInInspector]
    public bool isCancelOnSwipe = false;
    FingerTest fingerTest;

    Vector3 p0, p1;                          // 划屏的屏幕坐标
    //public Vector3 firstPosition, secondPosition;   // 世界坐标

    bool isClicked = false;
    float timer;

    // Use this for initialization
    void Start()
    {
        fingerTest = transform.GetComponent<FingerTest>();

    }

    // Update is called once per frame
    void Update()
    {
        bool isMouseDown = Input.GetMouseButton(0);
        if (isMouseDown && !isClicked)  // 点击屏幕
        {
            p0 = Input.mousePosition;
            //firstPosition = Camera.main.ScreenToWorldPoint(new Vector3(p0.x, p0.y, 1));

            isClicked = true;
            timer = 0;
        }
        else if (isMouseDown)   // 划动屏幕
        {
            p1 = Input.mousePosition;
            //p1 = Vector3.Lerp(p0, p1, fingerTest.maxDist / Vector3.Distance(p0, p1));   // p1不能超过额定的位置
            //secondPosition = Camera.main.ScreenToWorldPoint(new Vector3(p1.x, p1.y, 1));

            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                isCancelOnSwipe = true;
                StartCoroutine("cruise");
                timer = float.MinValue;
                
            }
        }

        if (Input.GetMouseButtonUp(0))  // 离开屏幕
        {
            isClicked = false;
            timer = 0;
            if (p0 == p1)   // 结束巡航（手指离开屏幕）时，如果手指没有移动，则不会触发OnSwipe
                isCancelOnSwipe = false;
            StopCoroutine("cruise");
            //print("cruise end");
        }
    }

    IEnumerator cruise()
    {
        while (true)
        {
            fingerTest.Swipe(p1 - Camera.main.WorldToScreenPoint(Global.Player.transform.position));

            yield return new WaitForSeconds(0.9f);  // 要小于龟的自动恢复等待时间
        }
    }
}

