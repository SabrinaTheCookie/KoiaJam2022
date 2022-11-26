using UnityEngine;

public class LinkColorManager : MonoBehaviour
{
    [SerializeField] private NodeSpriteManager nodeSpriteManager;

    public void UpdateGradient(Link link, LinkColorGradient lcg)
    {
        lcg.SetTopColor(nodeSpriteManager.GetColor(link.endNode.type));
        lcg.SetBottomColor(nodeSpriteManager.GetColor(link.startNode.type));

        if (link.startNode.type == link.endNode.type) return;

        if (Mathf.Abs(link.startNode.influence - link.endNode.influence) > 10f)
        {
            lcg.SetOrigin(0);
            return;
        }
        
    }
}