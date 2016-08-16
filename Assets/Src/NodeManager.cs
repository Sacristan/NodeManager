using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour
{
    [SerializeField]
    private List<Node> nodes = new List<Node>();

    [SerializeField]
    private GameObject buttonTemplate;

    public void AddNew()
    {
        nodes.Add(CreateNode());
    }

    void Remove(int index)
    {
        nodes.RemoveAt(index);
    }

    private Node CreateNode()
    {
        GameObject button = Instantiate(buttonTemplate, GetPositionForNode(), Quaternion.identity) as GameObject;
        button.transform.SetParent(transform);
        return button.GetComponent<Node>();
    }

    private Vector3 GetPositionForNode()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        return pos;
    }

}
