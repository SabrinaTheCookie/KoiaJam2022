using System;
using UnityEngine;

[Serializable]
public struct GameVars
{
    [Range(1, 10)] public int numReliableSources;
    [Range(1, 10)] public int numBadSources;
    [Range(1, 200)] public int minTotalNodes;
    [Range(1, 200)] public int maxTotalNodes;
    [Range(1, 10)] public int maxDefaultNodeConnections;
}