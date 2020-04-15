using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 动物的点击触发技能接口
/// 玩家若要触发一个技能，方法有两个：
/// 1、点击卡槽，则寻找屏幕里最左边的该类型动物（UseCard），触发其技能；
/// 2、点击该类型动物，则先检查是否有该卡牌（FingerTestNetwork.Tap），若有则触发。
/// 以上两种方法若能触发，则移除该卡牌（Clear）
/// </summary>
/// 

public interface ITouchable
{
    /// <summary>
    /// 可点击触发技能
    /// </summary>
    bool TouchEvent();
}

public abstract class TouchableBehaviour: MonoBehaviour, ITouchable
{
    // 子类需实现
    /// <summary>
    /// 
    /// </summary>
    /// <returns>true 表示点击有效</returns>
    public abstract bool TouchEvent();  // 点击触发的技能
}

public interface ITouchableNetwork: ITouchable {

    /// <summary>
    /// 客户端可点击触发技能
    /// 需要找到其在数组中的id，Cmd到服务器上执行其TouchEvent()
    /// </summary>
    void TouchEventNetwork();
}

/// <summary>
/// 可划屏移动的物体
/// </summary>
public interface IDrawable
{
    void DrawForce(Vector2 force);
}

public abstract class DrawableNetwork: NetworkBehaviour, IDrawable
{
    public void DrawForce(Vector2 force)
    {
        int id = GameManager.ControllableObjects.IndexOf(gameObject);
        if (id >= 0)
            Global.Player.GetComponent<PlayerEventsNetwork>().CmdDrawForce(id, force);
    }
}

public abstract class TouchableDrawableNetwork: TouchableNetwork, IDrawable
{
    public void DrawForce(Vector2 force)
    {
        int id = GameManager.ControllableObjects.IndexOf(gameObject);
        if (id >= 0)
            Global.Player.GetComponent<PlayerEventsNetwork>().CmdDrawForce(id, force);
    }
}

public abstract class TouchableNetwork: NetworkBehaviour, ITouchableNetwork
{
    // 子类需实现，里面可以调用 RpcEffect();
    public abstract bool TouchEvent();  // 点击触发技能中的物理和位置（只修改Rigidbody、Transform）

    // 子类需实现
    protected abstract void Effect();    // 技能带来的状态变化（是否点灯等）和特效（声音、动画、粒子等，与Rigidbody、Transform无关）

    // 子类若重写，需调用 base.Start();
    protected void Start()
    {
        //联机动态生成
        if(transform.parent == null)
            transform.SetParent(Global.MYNetwork);  // UNet Spawn的物体不会同步parent和scale
        GameManager.ControllableObjects.Add(gameObject);
    }

    //--------------------【以下开发者不用管】
    public virtual void TouchEventNetwork()
    {
        int id = GameManager.ControllableObjects.IndexOf(gameObject);
        //if (id >= 0)
            Global.Player.GetComponent<PlayerEventsNetwork>().CmdTouchEvent(id);
    }

    /// <summary>
    /// 在其他客户端运行特效
    /// </summary>
    [ClientRpc]
    protected void RpcEffect()
    {
        //if (!isLocalPlayer)
        {
            Effect();
        }
    }

    //called on client when the Network destroy that object (it was destroyed on server)
    public override void OnNetworkDestroy()
    {
        GameManager.ControllableObjects.Remove(gameObject);
        base.OnNetworkDestroy();
    }
}
