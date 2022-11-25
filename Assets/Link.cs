using UnityEngine;

public class Link : MonoBehaviour
{
    public Sprite sprite;
    public Collider2D coll2D;
    
    public Node startNode;
    public Node endNode;
    
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
        //TODO Stop overlaps
        
        //Check overlap - if overlapping, destroy this link and undo the connection (That has been established)
        //Debug.Log(Physics2D.IsTouchingLayers(collider, 3));
    }

    private void SetLinkSize(Vector2 linkDirection)
    {
        transform.localScale = new Vector3(.1f, linkDirection.magnitude, 1);
    }

    private void SetLinkAngle(Vector3 linkDirection)
    {
        var angle = Vector2.Angle(new Vector2(0.0f, 1.0f), new Vector2(linkDirection.x, linkDirection.y));
        if (linkDirection.x > 0.0f) angle = 360.0f - angle;
        transform.Rotate(0,0, angle);
    }
}
