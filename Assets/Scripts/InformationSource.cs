using System;
using Unity.Mathematics;
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
    private bool _isVerified;
    private float _timeToBeVerifiedFor;
    private float _timeVerified;

    [Header("Tick and Spread")]
    private float _timeSinceLastSpread;
    private float _spreadTime;

    public static event Action PowerSpread;

    public void Attach(Node node, int power)
    {
        _nodeAttachedTo = node;
        node.Source = this;
        sourceType = _nodeAttachedTo.type;
        informationPower = power;
        _timeToBeVerifiedFor = GameController.Instance.GameVariables.timeSourcesVerifiedFor;
        _spreadTime = 1f;
    }

    public void AddTime(float tick)
    {
        _timeSinceLastSpread += tick;

        if (_timeSinceLastSpread > _spreadTime)
        {
            _timeSinceLastSpread = 0f;
            SpreadPower();
        }

        if (_isVerified && Time.time > _timeVerified + _timeToBeVerifiedFor)
        {
            UnVerify();
        }
    }
    
    private void SpreadPower()
    {
        var powerPerNode = (float) informationPower / _nodeAttachedTo.connectedNodes.Count / 10f;

        // For every connected node that is not the parent node, if it is neutral, spread the power to it.
        foreach (Node node in _nodeAttachedTo.connectedNodes)
        {
            if (node.type == sourceType) continue;
            if (node == node.influenceSource) continue;

            node.influence += sourceType == NodeType.Reliable ? powerPerNode * -1 : powerPerNode;
            if (!node.influenceSource)
            {
                node.influenceSource = _nodeAttachedTo;
            }
        }

        Vector3 offset = Vector3.up * _nodeAttachedTo.transform.localScale.y / 2;
        //Create a like blip
        Instantiate(GameController.Instance.GameVariables.likePrefab, transform.position + offset, quaternion.identity);

    }

    public bool IsVerified()
    {
        return _isVerified;
    }

    public void VerifySource()
    {
        informationPower *= 2;
        _timeVerified = Time.time;
        _isVerified = true;
        _nodeAttachedTo.verifiedSprite.SetActive(true);
    }

    private void UnVerify()
    {
        _isVerified = false;
        informationPower /= 2;
        _nodeAttachedTo.verifiedSprite.SetActive(false);
    }
}