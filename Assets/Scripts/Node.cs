using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;


public enum NodeType
{
    Reliable,
    Neutral,
    Misinformed
}

public class Node : MonoBehaviour
{
    public static event Action CutNode;
    public static event Action<NodeType> NodeTypeChanged;

    //TODO move link prefab to manager
    public GameObject linkPrefab;

    public List<Node> connectedNodes;
    public NodeType type;
    public int influence;
    public float connectionRadius;
    public Node influenceSource;

    public bool searchForConnections;

    //Node changed type
    public void ChangeNodeType(NodeType newType)
    {
        //Whenever a node changes
        NodeTypeChanged?.Invoke(newType);
    }
    
    //Connects to a node. This is the first step in creating a node link.
    public void Connect(Node adjacentNode)
    {
        if (!adjacentNode) return;
        
        //Caution check for duplicate node connections.
        if (connectedNodes.Contains(adjacentNode)) return;
        
        //Register the new connection to this node
        connectedNodes.Add(adjacentNode);
        //Then Register on the connected node
        adjacentNode.ConnectPair(this);
        
        //Create a link between the two
        Link newLink = Instantiate(linkPrefab).GetComponent<Link>();
        newLink.SetupLink(this, adjacentNode);
        
    }

    //Connects to a node. This is the second (Final) step in creating a node link.
    public void ConnectPair(Node pairedNode)
    {
        //Acknowledge the connection between these nodes <3
        connectedNodes.Add(pairedNode);
    }

    // Start is called before the first frame update
    void Start()
    {
        //All nodes start as Neutral
        ChangeNodeType(NodeType.Neutral);
        
        //!!! TODO Move to manager/controller class
        //Check for nearby nodes
        if(searchForConnections)FindConnections(2);
    }

    private void FindConnections(int maxConnections)
    {

        //Get all nearby Nodes
        List<Node> nearbyNodes = new List<Node>();
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, connectionRadius);
        
        foreach (Collider2D collider2D in nearbyColliders)
        {
            nearbyNodes.Add(collider2D.GetComponent<Node>());
            
            //Stop connecting after maximum reached
            if (nearbyNodes.Count > maxConnections) break;
        }
        //Remove self from nearby nodes
        Node col = GetComponent<Node>();
        if (nearbyNodes.Contains(col))
        {
            nearbyNodes.Remove(col);
        }

        //Connect to each node
        foreach (Node node in nearbyNodes)
        {

            Connect(node);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
