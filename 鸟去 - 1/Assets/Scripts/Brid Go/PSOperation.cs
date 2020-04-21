using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSOperation : MonoBehaviour {

    public bool CanPS;   //此时是否可以PS
    private GameObject Ratation;
    private GameObject Movement;
    private bool Rotate;
    private bool Move;
    //public GameObject RatateObject;//需要被PS的物体
    public Vector2 LastPosition;
    Vector2 ThisPosition;
    //public Vector2 KeepPosition;//用来预留lastposition，方便碰撞切位
    Vector2 LastVec;
    Vector2 ThisVec;
    Vector2 CenterPosition;
    public Vector3 a;
    float angel;
    private bool canRa;
    //GameObject PSTool;
    GameObject UIPause;
    GameObject PSid;
    int NeedBack;
    public bool PS;
    
    void Awake()
    {
        //NeedBack = GetComponent<Ghosting>().stayUp;
        //canRa = RatateObject.GetComponent<FingerTest2>().CanPS; //这个是在判断是否暂停了视频，如果暂停才可以旋转,在Update里还没加上
    }
    private void Start()
    {
        Ratation = GameObject.Find("CanRotate");
        Movement = GameObject.Find("CanMove");
        //PSTool = GameObject.Find("PSTool");
        UIPause = GameObject.Find("UI Root");
        PSid = GameObject.Find("CanRotate");
    }
    void OnMouseDown()
    {


        //if (NeedBack == 1)
        //  GetComponent<Ghosting>().backPosition();
        PSid.transform.localScale =  new Vector3(1, 1, 1);
        PSid.GetComponent<IsTouchRotate>().PSOBJ = null;
        if (!UIPause.GetComponent<UIManager>().Pause)//限制暂停后才能PS
            return;
        GlobalNN.PSTool.transform.position = new Vector3(0, 17.9f, -95.4f);
        PSid.GetComponent<IsTouchRotate>().PSOBJ = gameObject;
        //设置为游戏暂停后，CanPS设置为ture,可PS，然后当点击版面内高光可PS的物体，
        //就会将PSPane改变大小后，再移到当前物体上，
        float size = GetComponent<Collider2D>().bounds.size.x * transform.localScale.x;
        float PSsize = PSid.GetComponent<CircleCollider2D>().bounds.size.x * PSid.transform.localScale.x;

        PSid.transform.localScale = (size / PSsize) * new Vector3(PSid.transform.localScale.x, PSid.transform.localScale.y,1);
        //PSTool.GetComponent<Collider2D>().bounds.size =(size / PSsize) * new Vector3(PSTool.GetComponent<Collider2D>().bounds.size.x, PSTool.GetComponent<Collider2D>().bounds.size.y, 0));
        GlobalNN.PSTool.transform.position = transform.position;//将PS框移动到当前选中的可PS物体上
                                                               

        //GlobalNN.PSTool.SetActive(true);
        CanPS = true;
        transform.GetComponent<Collider2D>().isTrigger = true;
      //  PS = true;
        
    }

    void OnMouseUp()
    {
        GlobalNN.PSTool.transform.position = new Vector3(GlobalNN.PSTool.transform.position.x, GlobalNN.PSTool.transform.position.y, 0);
    }
    void Update()
    {

        
        //Debug.Log("44");
        if (CanPS)
        {
            if (PSid.GetComponent<IsTouchRotate>().PSOBJ.name != gameObject.name)
                return;
            Rotate = Ratation.GetComponent<IsTouchRotate>().IsCanRotate;
            Move = Movement.GetComponent<IsTouchMove>().IsCanMove;
            if (Rotate || Move) {
                CenterPosition = transform.position;//获取物体中心点
                ThisPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //将鼠标位置转换为世界坐标;
                if (Ratation.GetComponent<IsTouchRotate>().PSRotate)
                {
                    //Debug.Log("1");
                    LastPosition = ThisPosition;
                    Ratation.GetComponent<IsTouchRotate>().PSRotate = false;
                }
                if (Movement.GetComponent<IsTouchMove>().PSMove)
                {
                    //Debug.Log("2");
                    LastPosition = ThisPosition;
                    Movement.GetComponent<IsTouchMove>().PSMove = false;
                }
            }
            if (Rotate)
            {
                //NeedBack = GetComponent<Ghosting>().stayUp;
                LastVec = LastPosition - CenterPosition;
                ThisVec = ThisPosition - CenterPosition;             //起点为物体中心点，终点为此时鼠标位置的向量
                transform.Rotate(0, 0, Vector2.SignedAngle(ThisVec, LastVec));  // 返回当前坐标与目标坐标的角度 
                GlobalNN.PSTool.transform.Rotate(0, 0, -Vector2.SignedAngle(ThisVec, LastVec));
                LastPosition = ThisPosition;
                
            }
            if (Move)
            {

                a = new Vector3( ThisPosition.x - LastPosition.x,ThisPosition.y - LastPosition.y,0);
                transform.Translate(a , Space.World);
                GlobalNN.PSTool.transform.Translate(a, Space.World);
                //KeepPosition = LastPosition;
                LastPosition = ThisPosition;
                
                
                
            }
        }

    }
}
