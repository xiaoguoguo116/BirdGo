using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autodestroye : MonoBehaviour {

    public float m_timer = 2.0f;
    void Start()
    { Destroy(this.gameObject, m_timer); }
}
