using UnityEngine;

public class NodeSpriteManager : MonoBehaviour
{
    [SerializeField] private Sprite neutralSprite;
    [SerializeField] private Sprite reliableSprite;
    [SerializeField] private Sprite misinformedSprite;
    [SerializeField] private Color neutralColorRing;
    [SerializeField] private Color reliableColorRing;
    [SerializeField] private Color misinformedColorRing;

    private void OnEnable()
    {
        Node.NodeTypeChanged += ChangeNodeSprite;
    }

    // ReSharper disable Unity.PerformanceAnalysis - this is called so infrequently that it won't matter
    private void ChangeNodeSprite(NodeType type, Node nodeRef)
    {
        SpriteRenderer sr = nodeRef.GetComponent<SpriteRenderer>();

        switch (type)
        {
            case NodeType.Reliable:
                sr.sprite = reliableSprite;
                nodeRef.circleSprite.color = reliableColorRing;
                break;
            case NodeType.Neutral:
                sr.sprite = neutralSprite;
                nodeRef.circleSprite.color = neutralColorRing;
                break;
            case NodeType.Misinformed:
                sr.sprite = misinformedSprite;
                nodeRef.circleSprite.color = misinformedColorRing;
                break;
            default:
                break;
        }
    }
}