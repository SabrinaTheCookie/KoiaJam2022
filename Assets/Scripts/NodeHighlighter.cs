using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ValidNode
{
    public CursorTools tool;
    public float minInfluence;
    public float maxInfluence;
    public NodeType[] nodeTypeFilters;
    public bool isConnectedToNeutralNode;
}
public class NodeHighlighter : MonoBehaviour
{
    public ValidNode validPromoteNode;
    public ValidNode validVerifyNode;

    private List<Node> highlightedNodes;
    public void HighlightValidNodes(CursorTools tool)
    {
        highlightedNodes = new List<Node>();

        highlightedNodes = tool switch
        {
            CursorTools.Promote => NodeNetworkManager.FindNodes(validPromoteNode),
            CursorTools.Verify => NodeNetworkManager.FindNodes(validVerifyNode),
            CursorTools.Unfollow => null,
        };

        if (highlightedNodes is null) return;
        foreach (Node node in highlightedNodes)
        {
            //Highlight node
            node.GetComponent<SpriteRenderer>().color = Color.green;
            Debug.Log("Highlighting node " + node.name);
        }

    }

    public void DeHighlightNodes()
    {
        if (highlightedNodes is null) return;
        
        foreach (Node node in highlightedNodes)
        {
            //DeHighlight node
            node.GetComponent<SpriteRenderer>().color = Color.white;
            Debug.Log("DeHighlighting node " + node.name);
        }
    }

}
