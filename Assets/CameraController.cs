using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    public void OnZoomOut(Vector4 targetPosition)
    {
        Vector3 position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z) + new Vector3(0f, 0f , -10f);

        transform.position = position;
        transform.Rotate(Vector3.forward, HexagonNodeDataClass.rotationFactor);

        camera.orthographicSize *= HexagonNodeDataClass.scaleFactor;
    }

    public void OnZomeOut(Vector4 targetPosition)
    {
        Vector3 position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);

        transform.position = position;
        transform.Rotate(Vector3.up, -HexagonNodeDataClass.rotationFactor);

        camera.orthographicSize /= HexagonNodeDataClass.scaleFactor;
    }

}
