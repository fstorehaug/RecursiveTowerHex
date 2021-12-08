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
        
        transform.position = node.getPosition();
        transform.Rotate(Vector3.forward, node.getRotation());

        mapController.registerNodeAtAdress(this, node.getAdress());
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
        AddAndNotifyNode(mapController.GetNodeAtAdress(node.getAdress() + new Vector4(1f, 0f, 0f, 0f)));
        AddAndNotifyNode(mapController.GetNodeAtAdress(node.getAdress() + new Vector4(-1f, 0f, 0f, 0f)));
        AddAndNotifyNode(mapController.GetNodeAtAdress(node.getAdress() + new Vector4(0f, 1f, 0f, 0f)));
        AddAndNotifyNode(mapController.GetNodeAtAdress(node.getAdress() + new Vector4(0f, -1f, 0f, 0f)));
        AddAndNotifyNode(mapController.GetNodeAtAdress(node.getAdress() + new Vector4(0f, 0f, 1f, 0f)));
        AddAndNotifyNode(mapController.GetNodeAtAdress(node.getAdress() + new Vector4(0f, 0f, -1f, 0f)));
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
