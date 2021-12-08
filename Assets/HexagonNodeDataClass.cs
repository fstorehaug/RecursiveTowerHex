using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonNodeDataClass
{
    private Vector4 adress;

    public static float scaleFactor = 2.8f; // the amount we scale with each w step up.
    public static float rotationFactor = 20f; //The amount we rotate the world with each step up to preserve symetry;
    private static float centerToCenterDistance = 1.1f;

    //Theese vector reprecent the kartesian cordinates directions of the x, y and z directions in our hexagonal coordinate system
    private static Vector2 xdirection = new Vector2(Mathf.Sqrt(3), -1f).normalized * centerToCenterDistance;
    private static Vector2 ydirection = new Vector2(0f, 1f).normalized * centerToCenterDistance;
    private static Vector2 zdirection = new Vector2(-Mathf.Sqrt(3), -1f).normalized * centerToCenterDistance;


    public HexagonNodeDataClass(Vector4 adress)
    {
        this.adress = adress;
    }

    public Vector4 getAdress()
    {
        return adress;
    }

    public Vector3 getPosition()
    {
        Vector2 baseScaleXYPosition = (adress.x * xdirection + adress.y * ydirection + adress.z * zdirection);
        if (adress.w != 0)
            baseScaleXYPosition *= Mathf.Pow(scaleFactor, adress.w);

        return Quaternion.AngleAxis(adress.w * rotationFactor, Vector3.forward) * baseScaleXYPosition; //bit iffy on this but whatever
    }
    public float getSize()
    {
        return adress.w == 0 ? 1 : Mathf.Pow(scaleFactor, adress.w);
    }
    public float getRotation()
    {
        return adress.w * rotationFactor;
    }

    public Vector4 GetNeightborAdress(Vector3 direction)
    {
        return new Vector4(adress.x + direction.x, adress.y + direction.y, adress.z + direction.z, adress.w);
    }

    public Vector4 GetSuperNodeAdress()
    {
        return new Vector4(Mathf.Round(adress.x / 3), Mathf.Round(adress.y / 3), Mathf.Round(adress.z / 3), adress.w + 1);
    }

    public Vector4 GetCenterLeafNodeAdress()
    {
        return new Vector4(adress.x * 3, adress.y * 3, adress.z * 3, adress.w - 1);
    }
}

