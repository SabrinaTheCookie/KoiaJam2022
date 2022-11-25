using System;
using System.Collections;
using System.Collections.Generic;
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
    

    public List<Node> connectedNodes;
    public NodeType type;
    public int influence;
    public float connectionRadius;
    public Node influenceSource;

    //Node changed type
    public void ChangeNodeType(NodeType newType)
    {
        //Whenever a node changes
        NodeTypeChanged?.Invoke(NodeType.Misinformed);
    }
    
    public void Connect(Node adjacentNode)
    {
        
    }

    public void ConnectPair(Node pairedNode)
    {
        
    }

    //audio example class...
    public void OnEnable()
    {
        //Subscribe to actions
        NodeTypeChanged += PlayNodeChangeAudio;
    }

    public void OnDisable()
    {
        //Unsubscribe from actions
        NodeTypeChanged -= PlayNodeChangeAudio;
    }

    private void PlayNodeChangeAudio(NodeType nodeType)
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //All nodes start as Neutral
        ChangeNodeType(NodeType.Neutral);
        
        //Check for nearby nodes
        FindConnections();
    }

    private void FindConnections()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
