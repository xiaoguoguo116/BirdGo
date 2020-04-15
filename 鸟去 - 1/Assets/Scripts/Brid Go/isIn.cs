using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isIn : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
    }
}
