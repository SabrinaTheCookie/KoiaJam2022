using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action<GameVars> StartGame;
    public static event Action<string> StopGame;
    
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
    [SerializeField] private GameObject gameEndUI;
    [SerializeField] private GameObject gameWon;
    [SerializeField] private GameObject gameLost;
    [SerializeField] private GameObject gameUI;

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
        gameUI.SetActive(true);
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
        // Check to see if the player has won or lost
        if (_nodeNetworkManager.CheckAllNodesOfType(NodeType.Reliable))
        {
            _state = GameState.Won;
            GameEnded(_state);
            StopGame?.Invoke("win");
        }
        else if (_nodeNetworkManager.CheckAllNodesOfType(NodeType.Misinformed))
        {
            _state = GameState.Lost;
            GameEnded(_state);
            StopGame?.Invoke("lose");
        }
    }

    private void GameEnded(GameState state)
    {
        Node.NodeTypeChanged -= NodeOnNodeTypeChanged;
        gameEndUI.SetActive(true);
        gameUI.SetActive(false);
        switch (state)
        {
            case GameState.MainMenu:
                break;
            case GameState.Playing:
                break;
            case GameState.Won:
                gameWon.SetActive(true);
                break;
            case GameState.Lost:
                gameLost.SetActive(true);
                break;
        }
    }

    private void OnDisable()
    {
        Node.NodeTypeChanged -= NodeOnNodeTypeChanged;
    }

    public void ResetGame()
    {
        Destroy(_nodeNetworkManager);
        gameLost.SetActive(false);
        gameWon.SetActive(false);
        gameEndUI.SetActive(false);
        OnStartGame();
    }
}