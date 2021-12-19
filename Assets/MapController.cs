using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private GameObject HexagonPrefab;

    [SerializeField]
    private CameraController camera;

    public InputAction ZoomOut;
    public InputAction ZoomIn;

    private Dictionary<Vector4, HexNode> hexagonDict = new Dictionary<Vector4, HexNode>();

    private Dictionary<int, GameObject> layers; //TODO: create a gameobject for each layer and child each hex to toggle on/off

    HexNode curentCenter;
    HexNode selectedNode;

    private void Start()
    {
        ZoomIn.Enable();
        ZoomOut.Enable();
        ZoomIn.performed += ZoomIn_performed;
        ZoomOut.performed += ZoomOut_performed;

        curentCenter = CreateNodeAtPosition(new Vector4(0f, 0f, 0f, 0f));
        AddeNeighbors(curentCenter);
    }

    private void ZoomOut_performed(InputAction.CallbackContext obj)
    {
        Vector4 targetPosition = curentCenter.node.GetSuperNodeAdress();
        if (hexagonDict.ContainsKey(targetPosition))
        {
            curentCenter = hexagonDict[targetPosition];
        }
        else
        {
            curentCenter = CreateNodeAtPosition(targetPosition);
        }

        foreach (Vector4 directionalVector in HexagonNodeDataClass.directionalHexVectors)
        {
            if (!hexagonDict.ContainsKey(targetPosition + directionalVector))
            {
                CreateNodeAtPosition(targetPosition + directionalVector);
            }
        }

        selectedNode = curentCenter;
        camera.OnZoomOut(targetPosition);

    }

    private void ZoomIn_performed(InputAction.CallbackContext obj)
    {
        Vector4 newCenterLeafnodeAdress = selectedNode.node.GetCenterLeafNodeAdress();
        if (hexagonDict.ContainsKey(newCenterLeafnodeAdress))
        {
            curentCenter = hexagonDict[newCenterLeafnodeAdress];
        } else
        {
            curentCenter = CreateNodeAtPosition(newCenterLeafnodeAdress);
            AddeNeighbors(curentCenter);
        }

        selectedNode = curentCenter;
        camera.OnZoomIn(curentCenter.node.gethexAdress());
    }

    private void AddeNeighbors(HexNode node)
    {
        Vector4 adress = node.node.gethexAdress();

         foreach (Vector4 direction in HexagonNodeDataClass.directionalHexVectors)
        {
            CreateNodeAtPosition(adress + direction);
        }
    }

    private HexNode CreateNodeAtPosition(Vector4 position)
    {
        GameObject HexInstance = Instantiate(HexagonPrefab);
        HexInstance.name = "HexNode: " + position.ToString();
        HexNode node = HexInstance.AddComponent<HexNode>();
        node.SetUpNode(this, new HexagonNodeDataClass(position));

        return node;
    }

    public void registerNodeAtAdress(HexNode node, Vector4 adress)
    {
        if (!hexagonDict.ContainsKey(adress))
        {
            hexagonDict.Add(adress, node);
            return;
        }
         
        if (hexagonDict[adress] != null)
            Destroy(hexagonDict[adress]); //TODO pool this
        
        hexagonDict[adress] = node;
    }

    public HexNode GetNodeAtAdress(Vector4 key)
    {
        if (!hexagonDict.ContainsKey(key))
        {
            return null;
        }
        return hexagonDict[key];
    }

    public void OnNodeClicked(HexNode node)
    {
        if (selectedNode != null)
            selectedNode.OnDeSelected();
        
        selectedNode = node;
        selectedNode.OnSelected();
    }

    private void OnDestroy()
    {
        ZoomIn.performed -= ZoomIn_performed;
        ZoomOut.performed -= ZoomOut_performed;
    }

}
