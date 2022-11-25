using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NodeSpawner : MonoBehaviour
{
    // Instance var for singleton access
    public static NodeSpawner Instance { get; private set; }

    [SerializeField] private GameObject nodePrefab;
    public float nodeSizeMult = 1f;
    private float _nodeRadius;
    public int maxNodeConnections = 3;
    public int minTotalNodes;
    public int maxTotalNodes;
    private List<Node> _allNodes;

    private void Awake()
    {
        Instance = this;

        _nodeRadius = nodePrefab.GetComponent<Renderer>().bounds.size.x / 2 * nodeSizeMult;
    }

    private void OnEnable()
    {
        GameController.StartGame += SetUpNodes;
    }

    private void Start()
    {
        SetUpNodes();
    }

    private void SetUpNodes()
    {
        // Set up the nodes in the game world
        var nodesToPlace = Random.Range(minTotalNodes, maxTotalNodes);

        Limits boundaries = Limits.GetLimits(_nodeRadius);
        
        Debug.Log(boundaries);

        // Randomly generate locations for all the nodes. Ensure none overlap
        for (var i = 0; i < nodesToPlace; i++)
        {
        }
    }
}