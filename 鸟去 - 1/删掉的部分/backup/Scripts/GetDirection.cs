using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDirection {

    static GetDirection instance;
    public static GetDirection getDiretion
    {
        get
        {
            if (instance == null)
            {
                instance = new GetDirection();
            }
            return instance;
        }
    }

    public Vector2 direction(SwipeGesture gesture)
    {
        switch (gesture.Direction)
        {
            case FingerGestures.SwipeDirection.Right:
                return Vector2.right;
                break;
            case FingerGestures.SwipeDirection.Left:
                return Vector2.left;
                break;
            case FingerGestures.SwipeDirection.Up:
                return Vector2.up;
                break;
            case FingerGestures.SwipeDirection.Down:
                return Vector2.down;
                break;
            case FingerGestures.SwipeDirection.UpperLeftDiagonal:
                Vector2 upAndLeft = Vector2.up + Vector2.left;
                return upAndLeft;
                break;
            case FingerGestures.SwipeDirection.UpperRightDiagonal:
                Vector2 upAndRight = Vector2.up + Vector2.right;
                return upAndRight;
                break;
            case FingerGestures.SwipeDirection.LowerRightDiagonal:
                Vector2 downAndRight = Vector2.down + Vector2.right;
                return downAndRight;
                break;
            case FingerGestures.SwipeDirection.LowerLeftDiagonal:
                Vector2 downAndLeft = Vector2.down + Vector2.left;
                return downAndLeft;
                break;
            //case FingerGestures.SwipeDirection.None:
            //    break;
            //case FingerGestures.SwipeDirection.Vertical:
            //    break;
            //case FingerGestures.SwipeDirection.Horizontal:
            //    break;
            //case FingerGestures.SwipeDirection.Cross:
            //    break;
            //case FingerGestures.SwipeDirection.UpperDiagonals:
            //    break;
            //case FingerGestures.SwipeDirection.LowerDiagonals:
            //    break;
            //case FingerGestures.SwipeDirection.Diagonals:
            //    break;
            //case FingerGestures.SwipeDirection.All:
            //    break;
            default:
                return Vector2.zero;
                break;
        }
    }
}
