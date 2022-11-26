using System;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.UIElements;

public class Link : MonoBehaviour
{
    public Sprite sprite;
    public Collider2D coll2D;

    public LayerMask linkLayerMask;
    
    public Node startNode;
    public Node endNode;

    public float linkAngle;
    public void Start()
    {
        
    }

    public void SetupLink(Node newStartNode, Node newEndNode)
    {
        //Find Components
        if(!sprite)
            sprite = GetComponent<Sprite>();
        
        if(!coll2D)
            coll2D = GetComponent<Collider2D>();
        
        //Node setup
        
        startNode = newStartNode;
        endNode = newEndNode;

        Vector2 startNodePos = startNode.transform.position;
        Vector2 endNodePos = endNode.transform.position;
        
        //Find the center of the two nodes
        Vector3 centerOfNodes = new(
            (startNodePos.x + endNodePos.x) / 2,
            (startNodePos.y + endNodePos.y) / 2,
            5f);

        //Move to center of nodes
        transform.position = centerOfNodes;

        //Get direction vector between start and end of link
        Vector2 linkDirection = endNodePos - startNodePos;
        
        //Set Link Size/Length
        SetLinkSize(linkDirection);
        
        //Set Link Angle
        SetLinkAngle(linkDirection);
        
        //Check Link Collisions
        CheckLinkCollisions();

    }

    private void CheckLinkCollisions()
    {
        //TODO Stop overlaps - This still isnt working :(
        
        //Check overlap - if overlapping, destroy this link and undo the connection (That has been established)
        
        //Get all overlapping links
        //This angle seems to be incorrect some of the time? Maybe only on objects on the right side (x > 0)
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        
        foreach (Collider2D hit in hits)
        {

            if (!hit.CompareTag("Link"))
            {
                Debug.Log("No overlap with links found, but collided with a Node");
                return;
            }

            Debug.Log("Overlap found at - " + transform.position);
            
            BreakConnection();
        }
        
    }

    private void BreakConnection()
    {
        //Remove connection from start node
        startNode.BreakConnection(endNode);
        
        //Remove connection from end node
        endNode.BreakConnection(startNode);
        
        Debug.Log("Destroying link - Start Node: " + startNode + " | End Node: " + endNode);
        //Destroy this link
        Destroy(gameObject);
    }

    private void SetLinkSize(Vector2 linkDirection)
    {
        float linkLength = linkDirection.magnitude;
        
        //Remove the radius of nodes from the length of the link.
        linkLength -= startNode.transform.localScale.magnitude / 2;
        
        transform.localScale = new Vector3(.1f, linkLength, 1);
    }

    private void SetLinkAngle(Vector3 linkDirection)
    {
        //Get angle of link direction
        var angle = Vector2.Angle(new Vector2(0.0f, 1.0f), new Vector2(linkDirection.x, linkDirection.y));
        
        // if facing (Correct logic?) right, inverse the angle.
        if (linkDirection.x > 0.0f) angle = 360.0f - angle;
        
        //Save angle
        linkAngle = angle;
        
        //Rotate
        transform.Rotate(0,0, linkAngle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
