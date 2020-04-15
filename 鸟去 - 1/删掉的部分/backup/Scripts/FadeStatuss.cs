using UnityEngine;
using UnityEngine.UI;

public enum FadeStatus
{
    FadeIn,
    FadeOut
}
public class FadeStatuss : MonoBehaviour {
    public GameObject player;
    //设置的图片
    public Image m_Sprite;
    //透明值
    public float m_Alpha;
    //淡入淡出状态
    private FadeStatus m_Statuss;
    //效果更新的速度
    public float m_UpdateTime;

   

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //默认设置为淡入效果
        m_Statuss = FadeStatus.FadeIn;

    }
    

    // Update is called once per frame
    void Update()
    {
        //控制透明值变化
        if (player.GetComponent<PlayerEvents>().m_life <= 0)
        {
            if (m_Statuss == FadeStatus.FadeIn)
            {
                m_Alpha += m_UpdateTime * Time.deltaTime;
            }
            else if (m_Statuss == FadeStatus.FadeOut)
            {
                m_Alpha -= m_UpdateTime * Time.deltaTime;
            }

            UpdateColorAlpha();

        }
    }

    void UpdateColorAlpha()
    {
        //获取到图片的透明值
        Color ss = m_Sprite.color;
        ss.a = m_Alpha;
        //将更改过透明值的颜色赋值给图片
        m_Sprite.color = ss;
        //透明值等于的1的时候 转换成淡出效果
        if (m_Alpha > 1f)
        {
            m_Alpha = 1f;
            m_Statuss = FadeStatus.FadeOut;
        }
        
    }
}

  

