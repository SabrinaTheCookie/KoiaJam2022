using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action<GameVars> StartGame;

    // Instance var for singleton access
    public static GameController Instance { get; private set; }

    public GameVars GameVariables => gameVariables;

    [SerializeField] private GameObject nodeSpawnerPrefab;
    [SerializeField] private GameVars gameVariables;
    [SerializeField] private GameObject nnmPrefab;

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
    }

    /// <summary>
    /// Called when the game begins - sets up the game world and starts the gameplay through invoke
    /// </summary>
    public void OnStartGame()
    {
        // Set up game world
        Instantiate(nnmPrefab, Vector3.zero, Quaternion.identity);
        Instantiate(nodeSpawnerPrefab, Vector3.zero, Quaternion.identity);
        StartGame?.Invoke(GameVariables);
    }
}