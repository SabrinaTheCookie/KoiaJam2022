using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct GameVars
{
    [Range(1, 10)] public int numReliableSources;
    [Range(1, 10)] public int numBadSources;
    [Range(1, 200)] public int minTotalNodes;
    [Range(1, 200)] public int maxTotalNodes;
    [Range(1, 10)] public int maxDefaultNodeConnections;
    [Range(1, 10)] public int defaultReliablePower;
    [Range(1, 10)] public int defaultMisinformationPower;
    [Range(1, 60)] public int linkRefollowDuration;

}