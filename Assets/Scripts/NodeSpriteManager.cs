using UnityEngine;

public class NodeSpriteManager : MonoBehaviour
{
    [SerializeField] private Sprite neutralSprite;
    [SerializeField] private Sprite reliableSprite;
    [SerializeField] private Sprite misinformedSprite;

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
                break;
            case NodeType.Neutral:
                sr.sprite = neutralSprite;
                break;
            case NodeType.Misinformed:
                sr.sprite = misinformedSprite;
                break;
            default:
                break;
        }
    }
}