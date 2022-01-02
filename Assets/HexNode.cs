using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexNode : MonoBehaviour
{


    public HexagonNodeDataClass node;
    private MapController mapController;

    List<HexNode> neighbors;

    private void OnMouseDown()
    {
        mapController.OnNodeClicked(this);
    }

    public void SetUpNode(MapController mapController, HexagonNodeDataClass node)
    {
        neighbors = new List<HexNode>();

        this.node = node;
        this.mapController = mapController;
        
        transform.position = HexagonNodeDataClass.GetPosition(node.GethexAdress());
        transform.Rotate(Vector3.forward, node.getRotation());
        transform.localScale = Vector3.one * node.getSize();

        mapController.registerNodeAtAdress(this, node.GethexAdress());
    }

    public void AddNeighbor(HexNode node)
    {
        neighbors.Add(node);
    }

    public void RemoveNeighbor(HexNode node)
    {
        neighbors.Remove(node);
    }

    public void NotifyNeighbourOfExistance()
    {
        Vector3 adress = node.getCartesianAdress();
        foreach (Vector3 offsett in HexagonNodeDataClass.DirectionalVectortransform)
        {
            AddAndNotifyNode(mapController.GetNodeAtAdress(HexagonNodeDataClass.ReducedHexAdress( adress + offsett)));
        }
    }

    private void AddAndNotifyNode(HexNode node)
    {
        if (node == null)
        {
            return;
        }

        AddNeighbor(node);
        node.AddNeighbor(this);
    }

    public void OnSelected()
    {
        Debug.Log(this.gameObject.name + " has been selected");
    }   
    
    public void OnDeSelected()
    {
        Debug.Log(this.gameObject.name + " has been Deselected");
    }

    private void OnDestroy()
    {
        foreach(HexNode node in neighbors)
        {
            node.RemoveNeighbor(this);
        }
    }
}
