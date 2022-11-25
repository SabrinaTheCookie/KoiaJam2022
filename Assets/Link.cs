using UnityEngine;
using UnityEngine.UIElements;

public class Link : MonoBehaviour
{
    public Sprite sprite;
    public Collider2D coll2D;

    public LayerMask linkLayerMask;
    
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
        
        //Also hitting nodes...
        //Check overlap - if overlapping, destroy this link and undo the connection (That has been established)
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, transform.rotation.z, linkLayerMask);

        Debug.Log(hits.Length);
        foreach (Collider2D hit in hits)
        {
            Debug.Log(hit.tag);
            if (!hit)
            {
                Debug.Log("No overlaps found");
                return;
            }

            if (!hit.CompareTag("Link"))
            {
                Debug.Log("No overlap with links found");
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
        
        Debug.Log("Destroy me daddy");
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
        var angle = Vector2.Angle(new Vector2(0.0f, 1.0f), new Vector2(linkDirection.x, linkDirection.y));
        if (linkDirection.x > 0.0f) angle = 360.0f - angle;
        transform.Rotate(0,0, angle);
    }
}
