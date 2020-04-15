// 本工程弃用

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionFlow : MonoBehaviour {

    [SerializeField]
    int Direction;
    [SerializeField]
    FlowRippleEffect SwipEffect;

    float w, h;
    // Use this for initialization
    void Start () {
        w = transform.localScale.x * GetComponent<SpriteRenderer>().sprite.bounds.size.x/* / (Global.CamWidth * 2)*/;
        h = transform.localScale.y * GetComponent<SpriteRenderer>().sprite.bounds.size.y/* / (Global.CamHight * 2)*/;
        print("w,h:" + w + "," + h);
        if (SwipEffect)
            StartCoroutine("LoopFlow");
    }
    IEnumerator LoopFlow()
    {
        while (true)
        {
            switch(Direction)
            {
                case 0: // 向右
                    Vector2 start = Camera.main.WorldToViewportPoint(transform.position - new Vector3(w / 2, 0, 0));
                    Vector2 end = Camera.main.WorldToViewportPoint(transform.position + new Vector3(w / 2, 0, 0));
                    Vector2 top = Camera.main.WorldToViewportPoint(transform.position - new Vector3(h / 2, 0, 0));
                    Vector2 bottom = Camera.main.WorldToViewportPoint(transform.position + new Vector3(h / 2, 0, 0));
                    SwipEffect.Emit(start, Vector2.right, new Vector4(start.x, end.x, top.y, bottom.y));
                    break;
            }
            //Vector2 end = Camera.main.ScreenToViewportPoint(transform.position + new Vector3(w / 2, 0, 0));
            //Vector2 start = Camera.main.WorldToViewportPoint(transform.position - new Vector3(w / 2, 0, 0));
            
            
            yield return new WaitForSeconds(1f);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
