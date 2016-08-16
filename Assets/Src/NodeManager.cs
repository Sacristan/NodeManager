using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NodeManager : MonoBehaviour
{
    private static NodeManager instance;

    [SerializeField]
    private Node[] nodes;

    [SerializeField]
    private GameObject buttonTemplate;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void BuildObject()
    {
        List<Node> list = nodes.ToList();

        Node node = CreateNode();
        list.Add(node);

        nodes = list.ToArray<Node>();
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
