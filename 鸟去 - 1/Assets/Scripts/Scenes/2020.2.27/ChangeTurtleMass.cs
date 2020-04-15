/*
通过改变质量实现有些能推动，有些推不动（能避免大量的关卡漏洞！）
能推动的石头染成黄色
仅第四关：某些谜题如果有不希望龟推动的物体（比如绳子等），则在这个谜题加一个脚本，进入时龟质量改为0.1，退出时恢复。如果该谜题中还有龟需要推动的物体，
则给物体加脚本：接触龟时龟的质量为1，脱离后恢复。鲸喷水、洋流的力量需要根据龟的质量改变（自适应）。
（由于铰链关节的强度是固定的，绳子的质量不能过大，所以若要龟推不动绳子，必须减小龟的质量）
（如果龟质量是1，碰到不可推动的物体包括绳子再改为0.1，这个方法是不妥的，因为绳子出现的地方比较随意，龟的质量变轻的地方也比较随意；
而且给绳子加脚本比较麻烦）
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂在龟能推动的物体上，龟触碰后改变龟的质量（默认是1），离开后恢复为本谜题设置的质量
/// </summary>
public class ChangeTurtleMass : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == Global.Player)
        {
            Global.Player.GetComponent<Rigidbody2D>().mass = Global.SceneEvent.GetCurMystery().GetComponent<MYTurtleMass>().originalMass;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == Global.Player)
        {
            Global.Player.GetComponent<Rigidbody2D>().mass = Global.SceneEvent.GetCurMystery().GetComponent<MYTurtleMass>().turtleMass;
        }
    }
}
