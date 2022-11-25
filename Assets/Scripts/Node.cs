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
    public List<Node> connectedNodes;
    public NodeType type;
    public int influence;
    public Node influenceSource;

    public void Connect(Node adjacentNode)
    {
        
    }

    public void ConnectPair(Node pairedNode)
    {
        
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
