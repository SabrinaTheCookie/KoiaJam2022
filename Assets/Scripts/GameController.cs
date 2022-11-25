using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action StartGame;

    private void OnStartGame()
    {
        StartGame?.Invoke();
    }
}