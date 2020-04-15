using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingCard : MonoBehaviour {

    GameObject Buff;
    // Use this for initialization
    public void StartRoll (int i, GameObject BuffPref) {
        float left = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f)).x;
        float[] SkillPostations = { 1.91f, 0.6f, -0.95f };
        Buff = GameObject.Instantiate(BuffPref, this.transform.position, this.transform.rotation);

        iTween.MoveTo(gameObject, iTween.Hash
        (
            "x", left,   //1.91 0.6 -0.95
            "y", SkillPostations[i],
            "speed", 18f,
            "time", 120f,
            "easeType", iTween.EaseType.easeOutCubic,
            "oncomplete", "MoveEnd"
        )
    );
    }

    void MoveEnd()
    {
        //Global.Net.CmdDestroyCard(gameObject);
        Destroy(gameObject);
        Destroy(Buff);
    }

}
