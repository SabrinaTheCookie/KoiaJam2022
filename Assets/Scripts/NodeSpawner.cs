using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class NodeSpawner : MonoBehaviour
{
    public static event Action<Node> NewNode;

    [SerializeField] private GameObject nodePrefab;
    public float nodeSizeMult = 1f;
    private float _nodeRadius;
    public float boundaryBufferX;
    public float boundaryBufferY;

    [Tooltip("Value as a percentage of node size")]
    public float minDistanceBetweenNodes = 0.5f;

    private void Awake()
    {
        _nodeRadius = nodePrefab.GetComponent<Renderer>().bounds.size.x / 2 * nodeSizeMult;
        SetUpNodes();
        GameController.StopGame += KillSelf;
    }

    private void KillSelf(string s)
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        GameController.StopGame -= KillSelf;
    }

    private void SetUpNodes()
    {
        // Set up the nodes in the game world
        var nodesToPlace = Random.Range(
            GameController.Instance.GameVariables.minTotalNodes,
            GameController.Instance.GameVariables.maxTotalNodes);

        Limits boundaries = Limits.GetLimits(_nodeRadius * boundaryBufferX, _nodeRadius * boundaryBufferY);

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
                    Debug.Log("No space for this many nodes. Total nodes placed is " +
                              NodeNetworkManager.NumberNodes());
                    noMoreNodes = true;
                    break;
                }

                count += 1;
                xSpawn = Random.Range(boundaries.Left, boundaries.Right);
                ySpawn = Random.Range(boundaries.Top, boundaries.Bottom);
            }

            if (noMoreNodes) break;

            PlaceNode(xSpawn, ySpawn, i);
        }
    }

    private void PlaceNode(float xSpawn, float ySpawn, int i)
    {
        GameObject node = Instantiate(nodePrefab, new Vector3(xSpawn, ySpawn, 0), Quaternion.identity);
        node.transform.localScale = new Vector3(nodeSizeMult, nodeSizeMult, 1);
        node.name = "Node" + i;

        NewNode?.Invoke(node.GetComponent<Node>());
    }

    private bool OverlappingAnotherNode(Vector2 toCheck)
    {
        // Check every other node already created and make sure that no nodes are overlapping or too close
        return NodeNetworkManager.AllNodes.Select(node => Vector2.Distance(node.transform.position, toCheck))
            .Any(distanceBetweenNodes => distanceBetweenNodes < 2 * _nodeRadius * minDistanceBetweenNodes);
    }
}