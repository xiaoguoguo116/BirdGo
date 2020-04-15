using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [ExecuteInEditMode]
    public class BgRefresh : MonoBehaviour
    {
        BgCycle bgCycle;
        void Start()
        {
            bgCycle = GetComponent<BgCycle>();
            refresh();
        }
        void Update()
        {
            refresh();
        }
        void refresh()//刷新背景
        {
            if (!bgCycle.isRuntime()) bgCycle.bgRefresh();
        }
    }

