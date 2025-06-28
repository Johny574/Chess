using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public static Bot Instance;
    UCIInterface _uciInterface;
    List<string> _moves;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        _uciInterface = CreateInterface();
        _uciInterface.Initilize();
        StartNewGame();
    }

    UCIInterface CreateInterface()
    {
        #if UNITY_WEBGL
            return new WebGLUCI();
        #else
            return new WindowsUCI();
        #endif
    }

    void Start()
    {
        _moves = new();
        GameEvents.Instance.PeaceMoved += (from, to) => _moves.Add($"{char.ToLower((char)(from.Origin[1] + 65))}{from.Origin[0] + 1}{char.ToLower((char)(to.Origin[1] + 65))}{to.Origin[0] + 1}");
    }

    void OnDisable()
    {
        GameEvents.Instance.PeaceMoved -= (from, to) => _moves.Add($"{char.ToLower((char)(from.Origin[1] + 65))}{from.Origin[0] + 1}{char.ToLower((char)(to.Origin[1] + 65))}{to.Origin[0] + 1}");
    }

    public void StartNewGame()
    {
        _moves.Clear();
        _uciInterface.StartNewGame();
    }

    public IEnumerator GetMove(Action<string> onGetBestMove) => _uciInterface.GetMove(onGetBestMove, _moves);

    public void OnApplicationQuit()
    {
        #if !UNITY_WEBGL
            ((WindowsUCI)stockfish).OnApplicationQuit();
        #endif
    }
}