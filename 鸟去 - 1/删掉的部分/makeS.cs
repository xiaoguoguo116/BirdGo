using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//为了能让脚本在连上局域网的同时还能分别控制物体，所以要继承 NetworkBehaviour
public class makeS : NetworkBehaviour
{
    // public float traSpeed = 3;  //移动的速度
    //  public float rotSpeed = 120;  //一秒旋转的角度
    public GameObject bulletPre;   //子弹的prefab
    public Transform bulletTrans;  //生成子弹的位置

    public NetworkHash128 assetId { get; set; }
    // public bool brun = true;

    [Command]    //在客户端调用，但是在服务端运行，这是方法必须以 Cmd 开头
    void CmdFire()
    {

        GameObject bullet = Instantiate(bulletPre, bulletTrans.position, Quaternion.identity) as GameObject;

        // bullet.GetComponent<Rigidbody>().velocity = transform.forward * 10;
        //Destroy(bullet, 2);   //2秒后销毁子弹
        //NetworkManager.singleton.spawnPrefabs.Add(bullet);
        //  bullet.GetComponent<MeshRenderer>().material.color = Color.blue;
        NetworkServer.Spawn(bullet);    //在所有客户端都生成一个物体
                                        //StartCoroutine(WaitDeleteButtlet(bullet, 2f));

    }
    void Start()
    {
        assetId = bulletPre.GetComponent<NetworkIdentity>().assetId;
        ClientScene.RegisterSpawnHandler(assetId, SpawnBullet, UnSpawnBullet);
        if(isServer)
        CmdFire();
    }

    public GameObject SpawnBullet(Vector3 position, NetworkHash128 assetId)
    {
        return (GameObject)Instantiate(bulletPre, position, Quaternion.identity);
    }
    public void UnSpawnBullet(GameObject spawned)
    {
        Destroy(spawned);
    }
    // Update is called once per frame
    void Update() { 

    }
    
}
