using System;
using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    // Instance var for singleton access
    public static NodeSpawner Instance { get; private set; }

    [SerializeField] public GameObject nodePrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameController.StartGame += SetUpNodes;
    }

    private void SetUpNodes()
    {
        // Set up the nodes in the game world
    }
}