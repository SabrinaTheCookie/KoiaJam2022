using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeNetworkManager : MonoBehaviour
{
    public static List<Node> AllNodes { get; private set; }
    public static NodeHighlighter NodeHighlighter { get; private set;}
    private static bool _startTicking;

    private void OnEnable()
    {
        NodeSpawner.NewNode += AddNode;
        GameController.StartGame += StartLogic;
        GameController.StopGame += GameControllerOnStopGame;
    }

    private void GameControllerOnStopGame()
    {
        _startTicking = false;
    }

    private static void ChangeNodeType(NodeType type, Node node)
    {
        if (!node) return;

        if (type == NodeType.Neutral)
        {
            Destroy(node.Source);
            node.Source = null;
        }
        else
        {
            MakeNodeSource(node,
                !node.influenceSource || !node.influenceSource.Source
                    ? GameController.Instance.GameVariables.defaultReliablePower
                    : node.influenceSource.Source.informationPower);
        }
    }

    private void OnDisable()
    {
        NodeSpawner.NewNode -= AddNode;
        GameController.StartGame -= StartLogic;
        Node.NodeTypeChanged -= ChangeNodeType;
        GameController.StopGame -= GameControllerOnStopGame;
    }

    private void Awake()
    {
        AllNodes = new List<Node>();
        NodeHighlighter = GetComponent<NodeHighlighter>();
    }

    private static void AddNode(Node node)
    {
        AllNodes.Add(node);
    }

    public static int NumberNodes()
    {
        return AllNodes.Count;
    }

    private void StartLogic(GameVars gameVars)
    {
        // First choose the source nodes and set them
        // At this point, all nodes should be valid so we should connect them all
        foreach (Node node in AllNodes)
        {
            node.FindConnections(gameVars.maxDefaultNodeConnections);
        }

        SetupInformationSources(gameVars);

        _startTicking = true;

        // We don't need to be registered for this event until set up is complete
        Node.NodeTypeChanged += ChangeNodeType;
    }

    private static void SetupInformationSources(GameVars gameVars)
    {
        // Sort AllNodes to be in order of left to right on player screen
        AllNodes.Sort(new ComparisonX());

        //Generate Reliable Sources
        for (var i = 0; i < gameVars.numReliableSources; i++)
        {
            AllNodes[i].influence = -10;
            AllNodes[i].CheckPower();
            MakeNodeSource(AllNodes[i], gameVars.defaultReliablePower);
        }

        //Generate Misinformed Sources
        for (var i = NumberNodes() - 1; i >= NumberNodes() - gameVars.numBadSources; i--)
        {
            AllNodes[i].influence = 10;
            AllNodes[i].CheckPower();
            MakeNodeSource(AllNodes[i], gameVars.defaultMisinformationPower);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis - not called frequently 
    private static void MakeNodeSource(Node node, int power)
    {
        InformationSource infoSource = node.gameObject.AddComponent<InformationSource>();
        infoSource.Attach(node, power);
    }

    public static List<Node> FindNodes(ValidNode validNode)
    {
        List<Node> resultNodes = new List<Node>();
        
        foreach (Node node in AllNodes)
        {
            //Filter out unwanted node types
            bool typeMatch = false;
            foreach (NodeType nodeTypeFilter in validNode.nodeTypeFilters)
            {
                if (nodeTypeFilter is not NodeType.None)
                {
                    if(node.influenceSource)
                        if (node.influenceSource.type == nodeTypeFilter)
                            typeMatch = true;
                }
                    
            }

            //No type match? Look at next node
            if (!typeMatch) continue;

            //Filter out nodes without a neutral node connection
            if (validNode.isConnectedToNeutralNode)
            {
                bool connected = false;
            
                //Check for neutral connections
                foreach (Node connectedNode in node.connectedNodes)
                {
                    if (connectedNode.type is NodeType.Neutral) connected = true;
                }

                //Not connected to neutral node? Look at next node
                if (!connected) continue;
            }

            //Filter for influence ranges
            if (node.influence < validNode.minInfluence) continue;
            if (node.influence > validNode.maxInfluence) continue;

            //Add results
            resultNodes.Add(node);
            
        }

        return resultNodes;
    }

    private void Update()
    {
        // GAME JAM LOGIC COMMENTS AT BOTTOM
        if (!_startTicking) return;
        var tick = Time.deltaTime;

        foreach (Node node in AllNodes)
        {
            switch (node.type)
            {
                case NodeType.Reliable:
                case NodeType.Misinformed:
                    if (!node.Source) break;
                    node.Source.AddTime(tick);
                    break;
            }
        }

        foreach (Node node in AllNodes)
        {
            if (!_startTicking) break;
            node.CheckPower();
        }
    }

    public bool CheckAllNodesOfType(NodeType type)
    {
        return AllNodes.All(node => node.type == type);
    }
    
}