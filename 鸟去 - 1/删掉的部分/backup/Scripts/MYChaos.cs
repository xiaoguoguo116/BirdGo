using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MYChaos : MonoBehaviour {

    StoneClock clock;
    
    // Use this for initialization
    void Start () {
        
        clock = GameObject.Find("StoneClock").GetComponent<StoneClock>();
        StartCoroutine("Chaos");
	}
    IEnumerator Chaos()
    {
        for (int i = 0; i < 3; i++)
        {
            float t = Random.Range(2, 3);
            clock.Chaos(t); // 石钟混乱t秒
            yield return new WaitForSeconds(t * 2);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
