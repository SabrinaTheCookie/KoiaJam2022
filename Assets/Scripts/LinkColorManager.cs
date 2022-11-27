using UnityEngine;

public class LinkColorManager : MonoBehaviour
{
    [SerializeField] private NodeSpriteManager nodeSpriteManager;

    public void UpdateGradient(Link link, LinkColorGradient lcg)
    {
        lcg.SetTopColor(nodeSpriteManager.GetColor(link.endNode.type));
        lcg.SetBottomColor(nodeSpriteManager.GetColor(link.startNode.type));

        if (link.startNode.type == link.endNode.type) return;

        if (link.startNode.type == NodeType.Neutral)
        {
            lcg.SetOrigin(1 - link.startNode.influence / link.endNode.influence);
        }

        else if (link.endNode.type == NodeType.Neutral)
        {
            lcg.SetOrigin(link.endNode.influence / link.startNode.influence);
        }

        // Else, battle nodes, so the colours should reflect it properly
        else
        {
            if (link.startNode.type == NodeType.Reliable)
            {
                if (Mathf.Abs(link.startNode.influence) > Mathf.Abs(link.endNode.influence))
                {
                    lcg.SetOrigin(1 - (0.5f + (link.startNode.influence + link.endNode.influence) / 10f));
                }
                else
                {
                    lcg.SetOrigin(0.5f + (link.startNode.influence + link.endNode.influence) / 10f);
                }
            }
            else
            {
                if (Mathf.Abs(link.startNode.influence) > Mathf.Abs(link.endNode.influence))
                {
                    lcg.SetOrigin(0.5f + (link.startNode.influence + link.endNode.influence) / -10f);
                }
                else
                {
                    lcg.SetOrigin(1 - (0.5f + (link.startNode.influence + link.endNode.influence) / -10f));
                }
            }
        }
    }
}