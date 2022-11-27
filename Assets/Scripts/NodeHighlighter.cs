using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ValidNode
{
    public Color highlightColor;
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
    public Color dehighlightColor = Color.white;

    private List<Node> highlightedNodes;
    public void HighlightValidNodes(CursorTools tool)
    {
        highlightedNodes = new List<Node>();
        
        //Get all nodes that match are valid matches
        highlightedNodes = tool switch
        {
            CursorTools.Promote => NodeNetworkManager.FindNodes(validPromoteNode),
            CursorTools.Verify => NodeNetworkManager.FindNodes(validVerifyNode),
            CursorTools.Unfollow => null,
        };

        //Get corresponding highlight colour
        Color highlightColor = tool switch
        {
            CursorTools.Promote => validPromoteNode.highlightColor,
            CursorTools.Verify => validVerifyNode.highlightColor,
            CursorTools.Unfollow => Color.white,
        };

        if (highlightedNodes is null) return;
        foreach (Node node in highlightedNodes)
        {
            //Highlight node
            node.GetComponent<SpriteRenderer>().color = highlightColor;
            Debug.Log("Highlighting node " + node.name);
        }

    }

    public void DeHighlightNodes()
    {
        if (highlightedNodes is null) return;
        
        foreach (Node node in highlightedNodes)
        {
            //DeHighlight node
            node.GetComponent<SpriteRenderer>().color = dehighlightColor;
            Debug.Log("DeHighlighting node " + node.name);
        }
    }

}
