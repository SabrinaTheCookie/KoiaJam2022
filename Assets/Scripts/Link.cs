using System.Collections;
using UnityEngine;

public class Link : MonoBehaviour
{
    public Sprite sprite;
    public Collider2D coll2D;

    public LayerMask linkLayerMask;
    
    public Node startNode;
    public Node endNode;

    public float linkAngle;

    private LinkColorGradient _lcg;
    private LinkColorManager _lcmRef;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _lcg = GetComponent<LinkColorGradient>();
        _lcmRef = FindObjectOfType<LinkColorManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.StopGame += KillSelf;
    }

    private void KillSelf(string s)
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        GameController.StopGame -= KillSelf;
    }

    public void Update()
    {
        _lcmRef.UpdateGradient(this, _lcg);
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
        //TODO Stop overlaps - This still isn't working :(
        
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
        linkLength -= startNode.transform.localScale.magnitude * 1.3f / 2f;
        
        transform.localScale = new Vector3(.075f, linkLength, 1);
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

    public void LinkUnfollowed()
    {
        Debug.Log("Link Broken!");
        
        //VFX for broken link ENABLED
        _spriteRenderer.color = Color.grey;
        
        //Disable start and end nodes
        startNode.TemporaryDisconnect(endNode);
        endNode.TemporaryDisconnect(startNode);

        StartCoroutine(ERefollow());
        
    }

    private void LinkRefollowed()
    {
        Debug.Log("Link Re-enabled");

        //VFX for broken link DISABLED
        _spriteRenderer.color = Color.white;

        //Re-enable nodes
        startNode.ManuallyReconnect(endNode);
        endNode.ManuallyReconnect(startNode);
    }

    private IEnumerator ERefollow()
    {
        //Wait refollow duration
        yield return new WaitForSeconds(GameController.Instance.GameVariables.linkRefollowDuration);
        
        //Timer done! LET THE INFORMATION FLOW!
        LinkRefollowed();
        
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
