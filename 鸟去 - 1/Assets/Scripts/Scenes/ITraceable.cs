using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///// <summary>
///// 可回溯对象
///// 负责在回溯机制里保存每一个时间片下的对象各自的状态量
///// 绳子断连；开关；锯鳐变色；乌贼亮灭
///// </summary>
//public class Traceable : MonoBehaviour
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="count">时间片总数</param>
//    public virtual void Init(int count) { }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="tail">当前时间片</param>
//    public virtual void Save(int tail) { }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="tail">当前时间片</param>
//    public virtual void Load(int tail) { }
//}

/// <summary>
/// 可回溯对象
/// 负责在回溯机制里保存每一个时间片下的对象各自的状态量
/// 绳子断连；开关；锯鳐变色；乌贼亮灭
/// </summary>
public interface ITraceable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="count">时间片总数</param>
    void Init(int count);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tail">当前时间片</param>
    void Save(int tail);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tail">当前时间片</param>
    void Load(int tail);
}


