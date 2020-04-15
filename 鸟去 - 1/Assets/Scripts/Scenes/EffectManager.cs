using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

    [SerializeField]
    private ParticleSystem jellyFishes;
    [SerializeField]
    private ParticleSystem pops;
    [SerializeField]
    private ParticleSystem[] flows;
    [SerializeField]
    private int jellyCount;
    [SerializeField]
    private int popsCount;

    List<ParticleSystem> exist = new List<ParticleSystem>();
    // Use this for initialization
    void Start () {
        InvokeRepeating("CreateEffects", 0f, 30f);
	}

    private void Update()
    {
        foreach (ParticleSystem item in flows)
        {
            item.transform.position = Global.Player.transform.position;
        }
      
    }

    void CreateEffects()
    {
        if (exist.Count > 0)
        {
            for (int i = exist.Count - 1; i >= 0; i--)
            {
                if (exist[i].isStopped)
                {
                    exist[i].gameObject.SetActive(false);
                    exist.Remove(exist[i]);
                }                            
            }
            //foreach (ParticleSystem item in exist)
            //{
            //    Destroy(item.gameObject);
            //    exist.Remove(item);
            //}
        }
        RandomInit(pops, popsCount);
        RandomInit(jellyFishes, jellyCount);
    }

    void RandomInit(ParticleSystem particle, int count)
    {
        for(int i = 0; i < count; i++)
        {
            ParticleSystem ob = Instantiate(particle, new Vector2(Global.Player.transform.position.x + Random.Range(10f, 100f), Global.Player.transform.position.y + Random.Range(-6f, 8f)), Quaternion.identity);
            exist.Add(ob);
        }
    }
}
