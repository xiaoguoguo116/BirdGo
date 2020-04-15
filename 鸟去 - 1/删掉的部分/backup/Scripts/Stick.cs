using UnityEngine;
using System.Collections;

public class Stick : MonoBehaviour
{
    FingerTest fingerTest;

    public Vector3 p0, p1;                          // 划屏的屏幕坐标
    //public Vector3 firstPosition, secondPosition;   // 世界坐标

    private bool isClicked = false;
    float timer;



    // Use this for initialization
    void Start()
    {
        fingerTest = GameObject.Find("Input").GetComponent<FingerTest>();

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
            p1 = Vector3.Lerp(p0, p1, fingerTest.maxDist / Vector3.Distance(p0, p1));   // p1不能超过额定的位置
            //secondPosition = Camera.main.ScreenToWorldPoint(new Vector3(p1.x, p1.y, 1));

            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                StartCoroutine("cruise");
                timer = float.MinValue;
            }
        }

        if (Input.GetMouseButtonUp(0))  // 离开屏幕
        {
            isClicked = false;
            timer = 0;
            StopCoroutine("cruise");
            print("cruise end");
        }
    }

    IEnumerator cruise()
    {
        while (true)
        {
            fingerTest.Swipe(p1 - p0);
            print(p1 - p0);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
