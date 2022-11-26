using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action<GameVars> StartGame;
    public static event Action StopGame;
    
    private enum GameState {
        MainMenu,
        Playing,
        Won,
        Lost
    }

    // Instance var for singleton access
    public static GameController Instance { get; private set; }

    public GameVars GameVariables => gameVariables;

    [SerializeField] private GameObject nodeSpawnerPrefab;
    [SerializeField] private GameVars gameVariables;
    [SerializeField] private GameObject nnmPrefab;

    private NodeNetworkManager _nodeNetworkManager;
    private GameState _state;

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
        _state = GameState.MainMenu;
    }

    /// <summary>
    /// Called when the game begins - sets up the game world and starts the gameplay through invoke
    /// </summary>
    public void OnStartGame()
    {
        // Set up game world
        GameObject obj = Instantiate(nnmPrefab, Vector3.zero, Quaternion.identity);
        _nodeNetworkManager = obj.GetComponent<NodeNetworkManager>();
        Instantiate(nodeSpawnerPrefab, Vector3.zero, Quaternion.identity);
        
        StartGame?.Invoke(GameVariables);
        
        Node.NodeTypeChanged += NodeOnNodeTypeChanged;
        _state = GameState.Playing;
    }

    private void NodeOnNodeTypeChanged(NodeType type, Node node)
    {
        StopGame?.Invoke();
        
        // Check to see if the player has won or lost
        if (_nodeNetworkManager.CheckAllNodesOfType(NodeType.Reliable))
        {
            _state = GameState.Won;
            GameEnded(_state);
        }
        else if (_nodeNetworkManager.CheckAllNodesOfType(NodeType.Misinformed))
        {
            _state = GameState.Lost;
            GameEnded(_state);
        }
    }

    private void GameEnded(GameState state)
    {
        switch (state)
        {
            case GameState.Won:
                // Show the win screen
                break;
            case GameState.Lost:
                // Show the loss screen
                break;
        }
    }

    private void OnDisable()
    {
        Node.NodeTypeChanged -= NodeOnNodeTypeChanged;
    }
}