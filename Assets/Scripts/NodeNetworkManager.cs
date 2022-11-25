using System.Collections.Generic;
using UnityEngine;

public class NodeNetworkManager : MonoBehaviour
{
    public static List<Node> AllNodes { get; private set; }
    private static bool _startTicking;

    private void OnEnable()
    {
        NodeSpawner.NewNode += AddNode;
        GameController.StartGame += StartLogic;
    }

    private void OnDisable()
    {
        NodeSpawner.NewNode -= AddNode;
        GameController.StartGame -= StartLogic;
    }

    private void Awake()
    {
        AllNodes = new List<Node>();
    }

    private static void AddNode(Node node)
    {
        AllNodes.Add(node);
    }

    public static int NumberNodes()
    {
        return AllNodes.Count;
    }

    private static void StartLogic(GameVars gameVars)
    {
        // First choose the source nodes and set them
        // At this point, all nodes should be valid so we should connect them all
        foreach (Node node in AllNodes)
        {
            node.FindConnections(gameVars.maxDefaultNodeConnections);
        }

        // Sort AllNodes to be in order of left to right on player screen
        AllNodes.Sort(new ComparisonX());

        for (var i = 0; i < gameVars.numReliableSources; i++)
        {
            AllNodes[i].ChangeNodeType(NodeType.Reliable);
        }

        for (var i = NumberNodes() - 1; i >= NumberNodes() - gameVars.numBadSources; i--)
        {
            AllNodes[i].ChangeNodeType(NodeType.Misinformed);
        }

        _startTicking = true;
    }

    private void Update()
    {
        if (!_startTicking) return;
    }
}