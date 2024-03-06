using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSolver : MonoBehaviour
{

    public static CollisionSolver instance;

    private void Awake()
    { if (instance == null) { instance = this; }
    }

    public Vector2 VelocityElement(Vector3 pos, Vector3 otherPos, float xVel, float yVel, float xVelOther, float yVelOther)
    {
        float theta = Mathf.Atan2(otherPos.y - pos.y, otherPos.x - pos.x);
        float tVel = Rotation(xVel, yVel, theta).y;
        float thetaOther = theta;
        float nVelOther = Rotation(xVelOther, yVelOther, thetaOther).x;
        float nVel = nVelOther;
        //Debug.Log("theta =" + theta + ", tVel = " + tVel + ", nVel = " + nVel);

        return Rotation(nVel, tVel, -theta);
    }

    Vector2 Rotation(float xVel, float yVel, float theta)
    {
        return new Vector2(Mathf.Cos(theta) * xVel + Mathf.Sin(theta) * yVel,
                            -Mathf.Sin(theta) * xVel + Mathf.Cos(theta) * yVel);
    }
}
