


#if UNITY_WEBGL
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLUCI : UCIInterface
{
    [DllImport("__Internal")] private static extern void InitStockfish();
    [DllImport("__Internal")] private static extern void SendCommandToStockfish(string command);
    [DllImport("__Internal")] private static extern string GetOutputFromStockfish();

    public void Initilize() => InitStockfish();
    public void StartNewGame()
    {
        SendCommandToStockfish("uci");
        SendCommandToStockfish("ucinewgame");
        SendCommandToStockfish("isready");
    }

    public IEnumerator GetMove(Action<string> onMoveFound, List<string> moves)
    {
        SendCommandToStockfish($"position startpos moves {string.Join(" ", moves)}");
        SendCommandToStockfish("go movetime 1000");

        string bestMove = "";
        float timeout = 3f;
        float timer = 0f;

        while (timer < timeout)
        {
            string output = GetOutputFromStockfish();

            foreach (var line in output.Split('\n'))
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("bestmove"))
                {
                    bestMove = trimmed.Split(' ')[1];
                    onMoveFound?.Invoke(bestMove);
                    yield break;
                }
            }

            timer += Time.deltaTime;
            yield return null; // wait for next frame
        }

        Debug.LogWarning("Stockfish timed out without bestmove.");
        onMoveFound?.Invoke(null);
    }
}
#endif