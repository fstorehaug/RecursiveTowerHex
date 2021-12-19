using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycaster : MonoBehaviour
{
    [SerializeField]
    private InputAction LeftMouse;
    [SerializeField]
    private InputAction mousePos;
    [SerializeField]
    private MapController mapcontroller;

    private void Start()
    {
        LeftMouse.Enable();
        mousePos.Enable();

        LeftMouse.performed += LeftMouse_performed;
    }

    private void LeftMouse_performed(InputAction.CallbackContext obj)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.tag.Contains("hexnode"))
            {
                HexNode selected = hit.collider.gameObject.GetComponent<HexNode>();
                mapcontroller.OnNodeClicked(selected);   
            }
        }
    }
}
