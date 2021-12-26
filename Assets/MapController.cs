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
    private Dictionary<Vector3, HexNode> PositionToHexagon = new Dictionary<Vector3, HexNode>();

    private Dictionary<int, GameObject> layers = new Dictionary<int, GameObject>(); //TODO: create a gameobject for each layer and child each hex to toggle on/off

    HexNode curentCenter;
    HexNode selectedNode;

    private void Start()
    {
        ZoomIn.Enable();
        ZoomOut.Enable();
        ZoomIn.performed += ZoomIn_performed;
        ZoomOut.performed += ZoomOut_performed;

        curentCenter = CreateNodeAtHexPosition(new Vector4(0f, 0f, 0f, 0f));
        selectedNode = curentCenter;
        AddeNeighbors(curentCenter);
    }

    private void ZoomOut_performed(InputAction.CallbackContext obj)
    {
        Vector3 targetPosition = HexagonNodeDataClass.GetPosition(curentCenter.node.GethexAdress());
        targetPosition.z = curentCenter.node.GethexAdress().w + 1;
        if (PositionToHexagon.ContainsKey(targetPosition))
        {
            curentCenter = PositionToHexagon[targetPosition];
        }
        else
        {
            curentCenter = CreateNodeAtHexPosition(curentCenter.node.GetSuperNodeAdress());
        }

        foreach (Vector4 directionalVector in HexagonNodeDataClass.directionalHexVectors)
        {
            if (!PositionToHexagon.ContainsKey(HexagonNodeDataClass.GetPosition(curentCenter.node.GethexAdress() + directionalVector)))
            {
                CreateNodeAtHexPosition(curentCenter.node.GethexAdress() + directionalVector);
            }
        }

        selectedNode = curentCenter;
        camera.OnZoomOut(targetPosition);

    }

    private void ZoomIn_performed(InputAction.CallbackContext obj)
    {
        if (selectedNode.node.GethexAdress().w == 0)
            return;

        Vector4 newCenterLeafnodeAdress = selectedNode.node.GetCenterLeafNodeAdress();
        if (hexagonDict.ContainsKey(newCenterLeafnodeAdress))
        {
            curentCenter = hexagonDict[newCenterLeafnodeAdress];
        } else
        {
            curentCenter = CreateNodeAtHexPosition(newCenterLeafnodeAdress);
            AddeNeighbors(curentCenter);
        }

        selectedNode = curentCenter;
        camera.OnZoomIn(curentCenter.node.GethexAdress());
    }

    private void AddeNeighbors(HexNode node)
    {
        Vector4 adress = node.node.GethexAdress();

         foreach (Vector4 direction in HexagonNodeDataClass.directionalHexVectors)
        {
            CreateNodeAtHexPosition(adress + direction);
        }
    }

    private HexNode CreateNodeAtHexPosition(Vector4 position)
    {
        GameObject HexInstance = Instantiate(HexagonPrefab);
        HexInstance.name = "HexNode: " + position.ToString();
        HexNode node = HexInstance.AddComponent<HexNode>();
        node.SetUpNode(this, new HexagonNodeDataClass(position));

        AddNodeToLayer(node);

        return node;
    }

    private void AddNodeToLayer(HexNode node)
    {
        int layernumber = (int)node.node.GethexAdress().w;
        if (!layers.ContainsKey(layernumber))
        {
            GameObject layer = new GameObject("Layer_" + layernumber);
            node.gameObject.transform.parent = layer.transform;
            layers.Add(layernumber, layer);
        } else
        {
            node.gameObject.transform.parent = layers[layernumber].transform;
        }
    }

    public void registerNodeAtAdress(HexNode node, Vector4 hexAdress, Vector3 positionalAdress)
    {
        if (!hexagonDict.ContainsKey(hexAdress))
        {
            hexagonDict.Add(hexAdress, node);
        }
        else
        {
            if (hexagonDict[hexAdress] != null)
                Destroy(hexagonDict[hexAdress]); //TODO pool this

            hexagonDict[hexAdress] = node;
        }

        if (!PositionToHexagon.ContainsKey(positionalAdress))
        {
            PositionToHexagon.Add(positionalAdress, node);
        } 
        else
        {
            if (PositionToHexagon[positionalAdress] != null)
                Destroy(PositionToHexagon[positionalAdress]);

            PositionToHexagon[positionalAdress] = node;
        }
    
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
