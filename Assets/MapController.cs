using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapController : MonoBehaviour
{ 
    [SerializeField]
    private GameObject HexagonPrefab;

    private Dictionary<Vector4, HexNode> hexagonDict = new Dictionary<Vector4, HexNode>();

    HexNode curentCenter;

    private void Start()
    {
        curentCenter = CreateNodeAtPosition(new Vector4(0f, 0f, 0f, 0f));
        AddeNeighbors(curentCenter);
    }

    private void AddeNeighbors(HexNode node)
    {
        Vector4 adress = node.node.getAdress();

     CreateNodeAtPosition(adress + new Vector4(1f, 0f, 0f, 0f));
     CreateNodeAtPosition(adress + new Vector4(-1f, 0f, 0f, 0f));
     CreateNodeAtPosition(adress + new Vector4(0f, 1f, 0f, 0f));
     CreateNodeAtPosition(adress + new Vector4(0f, -1f, 0f, 0f));
     CreateNodeAtPosition(adress + new Vector4(0f, 0f, 1f, 0f));
     CreateNodeAtPosition(adress + new Vector4(0f, 0f, -1f, 0f));
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
            hexagonDict.Add(adress, node);
        else
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


}
