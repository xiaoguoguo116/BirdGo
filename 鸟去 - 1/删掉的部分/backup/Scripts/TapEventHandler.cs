using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TapEventHandler
{
    static TapEventHandler handler;

    public static TapEventHandler instance
    {
        get
        {
            if (handler == null)
            {
                handler = new TapEventHandler();
            }
            return handler;

        }

    }

    public TapEventHandler() { }

    public void EventHandle(GameObject ob)
    {
        switch (ob.tag)
        {
            case "Whale":
                WhaleEvent(ob);
                break;
            case "Weapon":
                WeaponEvent(ob);
                break;
            case "Squid":
                CoralEvent(ob);
                break;
            case "Player":
                TurtleEvent(ob);
                break;
            default:
                break;
        }
    }

    public static void WeaponEvent(GameObject ob)
    {
        // ob.GetComponent<Weapon>().Event();
    }
    private static void WhaleEvent(GameObject ob)
    {
        // ob.GetComponent<Whale>().startWater();
    }
    public static void CoralEvent(GameObject ob)
    {
        //ob.GetComponent<Coral>().Switch();
    }
    static void TurtleEvent(GameObject ob)
    {

    }
}