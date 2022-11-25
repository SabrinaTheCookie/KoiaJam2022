using System;
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

        _startTicking = true;
    }

    private void AddInformationSource(NodeType type)
    {
    }

    private void RemoveInformationSource(NodeType type)
    {
    }

    private void Update()
    {
        if (!_startTicking) return;
    }
}