using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inkFade : MonoBehaviour {


    private float ColorAlpha = 0;
    public Image ink1;
    public Image ink2;
    public Image ink3;
    public Image ink4;
    public Image ink5;
    // Use this for initialization
    void Start () {
        //Debug.Log("pp");
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("qq");
    }

    private void OnCollisionEnter(Collision collider)
    {
       // Debug.Log("kaishi");
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("kk");
            //StartCoroutine(Fade());
        }
        
    }

    IEnumerator Fade()
    {
        if (ColorAlpha >= 0)
        {
            Debug.Log(ColorAlpha);
            yield return new WaitForSeconds(1f);
            ColorAlpha += Time.deltaTime / 2;
            ink1.GetComponent<Image>().color = new Color(255, 255, 255, ColorAlpha);
            ink2.GetComponent<Image>().color = new Color(255, 255, 255, ColorAlpha);
            ink3.GetComponent<Image>().color = new Color(255, 255, 255, ColorAlpha);
            ink4.GetComponent<Image>().color = new Color(255, 255, 255, ColorAlpha);
            ink5.GetComponent<Image>().color = new Color(255, 255, 255, ColorAlpha);
        }
    }
}
