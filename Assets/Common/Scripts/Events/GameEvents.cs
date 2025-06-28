



using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public Action StartNewGame;   
    public Action<Square, Square> PeaceMoved;   
}