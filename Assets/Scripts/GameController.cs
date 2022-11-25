using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action StartGame;

    // Instance var for singleton access
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnStartGame()
    {
        StartGame?.Invoke();
    }
}