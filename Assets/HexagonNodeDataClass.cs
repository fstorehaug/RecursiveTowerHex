using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonNodeDataClass
{
    private Vector4 adress;

    public static double scaleFactor = Mathf.Sqrt(7); // the amount we scale with each w step up. srt((1/2)^2 + ((3/2)*sqrt(3))^2)
    public static float rotationFactor = 20f; //The amount we rotate the world in degrees with each step up to preserve symetry;
    private static float centerToCenterDistance = Mathf.Sqrt(3);

    //Theese vector reprecent the kartesian cordinates directions of the x, y and z directions in our hexagonal coordinate system
    private static Vector2 xdirection = new Vector2(0f,1f).normalized * centerToCenterDistance;
    private static Vector2 ydirection = new Vector2(Mathf.Sqrt(3), 1f).normalized * centerToCenterDistance;
    private static Vector2 zdirection = new Vector2(Mathf.Sqrt(3), -1f).normalized * centerToCenterDistance;

    public static Vector4[] directionalHexVectors =
{
        new Vector4(1f, 0f, 0f, 0f),
        new Vector4(-1f, 0f, 0f, 0f),
        new Vector4(0f, 1f, 0f, 0f),
        new Vector4(0f, -1f, 0f, 0f),
        new Vector4(0f, 0f, 1f, 0f),
        new Vector4(0f, 0f, -1f, 0f)
    };

    private Matrix4x4 heightspaceTransformation = new Matrix4x4(new Vector4(2f, 0f, -1f, 0f), new Vector4(1f, 2f, 0f, 0f), new Vector4(0f, 1f, 2f, 0f), new Vector4(0, 0, 0, 1));
    private Matrix4x4 InverseHeightTransformation { get { return heightspaceTransformation.inverse; } }

    public static Vector3[] DirectionalVectortransform
    {
        get
        {
            Vector3[] dirv = new Vector3[6];
            for (int i = 0; i <dirv.Length; i++)
            {
                dirv[i] = HexToPosCoordinates(directionalHexVectors[i]);
            }
            return dirv;
        }
    }

    public HexagonNodeDataClass(Vector4 adress)
    {
        this.adress = adress;
    }

    public static Vector3 HexToPosCoordinates(Vector4 adress)
    {
        Vector2 xypos = ((adress.x * xdirection) + (adress.y * ydirection) + (adress.z * zdirection));
        Vector3 xyhpos = new Vector3(xypos.x, xypos.y, adress.w);
        return xyhpos;
    }

    public Vector4 gethexAdress()
    {
        return adress;
    }

    public Vector3 getCartesianAdress()
    {
        return HexToPosCoordinates(adress);
    }

    public Vector3 getPosition()
    {
        Vector2 baseScaleXYPosition = ((adress.x * xdirection) + (adress.y * ydirection) + (adress.z * zdirection));
        baseScaleXYPosition *= (float)doublePower(scaleFactor, (int)adress.w);
        
        return Quaternion.AngleAxis(adress.w * rotationFactor, Vector3.forward) * baseScaleXYPosition; //bit iffy on this but whatever
    }
    public float getSize()
    {
        return (float)doublePower(scaleFactor, (int)adress.w);
    }

    private double doublePower(double number, int power)
    {
        if (power == 0)
        {
            return 1;
        }

        for(int i = 0; i < power-1; i++)
        {
            number *= number;
        }

        return number;
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
        return adress.x * InverseHeightTransformation.GetColumn(0) + adress.y * InverseHeightTransformation.GetColumn(1) + adress.z * InverseHeightTransformation.GetColumn(2) + new Vector4(0, 0, 0, adress.w + 1);
    }

    public Vector4 GetCenterLeafNodeAdress()
    {
        return adress.x * heightspaceTransformation.GetColumn(0) + adress.y * heightspaceTransformation.GetColumn(1) + adress.z * heightspaceTransformation.GetColumn(2) + new Vector4(0, 0, 0, adress.w - 1);

        //return (adress.x * new Vector4(2f, 0f, -1f, 0f)) + (adress.y * new Vector4(1f, 2f, 0f, 0f)) + (adress.z * new Vector4(0f, 1f, 2f, 0f) + new Vector4(0f, 0f, 0f, adress.w - 1f));

    }
}

