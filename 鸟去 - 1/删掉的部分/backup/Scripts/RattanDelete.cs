//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RattanDelete : MonoBehaviour {

//    //[HideInInspector]
//    //public List<float> timePs;
//    //[HideInInspector]
//    //public Transform[] ps;
//    [HideInInspector]
//    public float Time;

//    // Use this for initialization
//    void Start () {
//        //ps = GetComponentsInChildren<Transform>();  // 含孙子对象
//        //for (int i = 1; i < ps.Length; i++)
//        //    timePs.Add(0);
//    }

//	// Update is called once per frame
//	void Update () {

//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RattanDelete : MonoBehaviour, ITraceable
{

    ////////////////
    bool[] record;
    public void Init(int count)
    {
        record = new bool[count];
    }
    public void Save(int tail)
    {
        record[tail] = GetComponent<HingeJoint2D>().enabled;
    }

    public void Load(int tail)
    {
        GetComponent<HingeJoint2D>().enabled = record[tail];
    }
    ////////////////

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

