using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class NodeSpawner : MonoBehaviour
{
    public static NodeSpawner Instance { get; private set; }

    [SerializeField] private GameObject nodePrefab;
    public float nodeSizeMult = 1f;
    private float _nodeRadius;
    public int maxNodeConnections = 3;
    public int minTotalNodes;
    public int maxTotalNodes;

    public List<Node> AllNodes { get; private set; }

    public float boundaryMultiplierBuffer;

    [Tooltip("Value as a percentage of node size")]
    public float minDistanceBetweenNodes = 0.5f;

    private void Awake()
    {
        Instance = this;

        _nodeRadius = nodePrefab.GetComponent<Renderer>().bounds.size.x / 2 * nodeSizeMult;
        AllNodes = new List<Node>();
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

        Limits boundaries = Limits.GetLimits(_nodeRadius * boundaryMultiplierBuffer);

        Debug.Log(boundaries);

        // Randomly generate locations for all the nodes. Ensure none overlap
        for (var i = 0; i < nodesToPlace; i++)
        {
            var noMoreNodes = false;

            // This does guarantee a node will always be at the center, which is fine for now
            var xSpawn = 0f;
            var ySpawn = 0f;
            
            var count = 0;
            while (OverlappingAnotherNode(new Vector2(xSpawn, ySpawn)))
            {
                if (count > 5000)
                {
                    Debug.Log("No space for this many nodes. Total nodes placed is " + AllNodes.Count);
                    noMoreNodes = true;
                    break;
                }

                count += 1;
                xSpawn = Random.Range(boundaries.Left, boundaries.Right);
                ySpawn = Random.Range(boundaries.Top, boundaries.Bottom);
            }

            if (noMoreNodes) break;

            PlaceNode(xSpawn, ySpawn);
        }
    }

    private void PlaceNode(float xSpawn, float ySpawn)
    {
        GameObject node = Instantiate(nodePrefab, new Vector3(xSpawn, ySpawn, 0), Quaternion.identity);
        node.transform.localScale = new Vector3(nodeSizeMult, nodeSizeMult, 1);
        AllNodes.Add(node.GetComponent<Node>());
    }

    private bool OverlappingAnotherNode(Vector2 toCheck)
    {
        // Check every other node already created and make sure that no nodes are overlapping or too close
        return AllNodes.Select(node => Vector2.Distance(node.transform.position, toCheck))
            .Any(distanceBetweenNodes => distanceBetweenNodes < 2 * _nodeRadius * minDistanceBetweenNodes);
    }
}