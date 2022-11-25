using UnityEngine;

/// <summary>
/// A component defining the node that this source is attached to as being an information source,
/// either misinformation or reliable
/// </summary>
public class InformationSource : MonoBehaviour
{
    public NodeType sourceType;
    private Node _nodeAttachedTo;
    public int informationPower;

    [Header("Tick and Spread")] private float _timeSinceLastSpread;
    private float _spreadTime;

    public void Attach(Node node, int power)
    {
        _nodeAttachedTo = node;
        node.Source = this;
        sourceType = _nodeAttachedTo.type;
        informationPower = power;

        CalculateSpreadTime();
    }

    private void CalculateSpreadTime()
    {
        var nodesConnectedTo = _nodeAttachedTo.connectedNodes.Count;
        // Spread time is the power * numb of connected nodes
        _spreadTime = nodesConnectedTo * informationPower;
    }

    public void AddTime(float tick)
    {
        _timeSinceLastSpread += tick;

        if (_timeSinceLastSpread > _spreadTime)
        {
            _timeSinceLastSpread = 0f;
            SpreadPower();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void SpreadPower()
    {
        Debug.Log("node at position " + _nodeAttachedTo.transform.position + " spread power to " +
                  _nodeAttachedTo.connectedNodes.Count + " nodes.");

        // For every connected node that is not the parent node, if it is neutral, spread the power to it.
        foreach (Node node in _nodeAttachedTo.connectedNodes)
        {
            if (node.type == sourceType) continue;
            if (node == node.influenceSource) continue;

            node.influence += sourceType == NodeType.Reliable ? informationPower * -1 : informationPower;
            if (!node.influenceSource)
            {
                node.influenceSource = _nodeAttachedTo;
            }
        }
    }
}