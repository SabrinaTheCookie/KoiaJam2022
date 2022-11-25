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

    private void Update()
    {
        // Every update cycle, we check how many nodes are we connected to to determine the power output
        var nodesConnectedTo = _nodeAttachedTo.connectedNodes.Count;
        
        // Tell each connected node what the current output is from this node
        
        // Sum all directly sourced strengths to each node and set the level snd type of the node
        
        // Spread the information outwards based on each nodes strength
    }
}
