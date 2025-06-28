using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class WindowsUCI : UCIInterface
{
    Process stockfish;
    // string stockFishPath = Path.Combine(Application.streamingAssetsPath, "Stockfish", "stockfish-windows-x86-64-avx2.exe");
    string stockFishPath = Path.Combine(Application.dataPath, "Stockfish", "stockfish-windows-x86-64-avx2.exe");

    public void Initilize()
    {
        stockfish = new Process();
        stockfish.StartInfo.FileName = stockFishPath;
        stockfish.StartInfo.RedirectStandardInput = true;
        stockfish.StartInfo.RedirectStandardOutput = true;
        stockfish.StartInfo.UseShellExecute = false;
        stockfish.StartInfo.CreateNoWindow = true;
        stockfish.Start();
    }

    public void StartNewGame()
    {
        stockfish.StandardInput.WriteLine("uci");
        stockfish.StandardInput.WriteLine("ucinewgame");
        stockfish.StandardInput.Flush();
    }

    public void OnApplicationQuit()
    {
        stockfish.Kill();
    }

    public IEnumerator GetMove(Action<string> onMoveFound, List<string> moves)
    {
        stockfish.StandardInput.WriteLine($"position startpos moves {moves}");
        stockfish.StandardInput.WriteLine("go movetime 1000"); // Or "go depth 10"
        stockfish.StandardInput.Flush();

        yield return new WaitForSeconds(1);

        string bestMove;

        while (true)
        {
            string line = stockfish.StandardOutput.ReadLine();

            if (line.StartsWith("bestmove"))
            {
                bestMove = line.Split(' ')[1];
                break;
            }
        }

        onMoveFound.Invoke(bestMove);
    }
}