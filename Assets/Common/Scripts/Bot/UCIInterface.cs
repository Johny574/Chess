using System;
using System.Collections;
using System.Collections.Generic;

public interface UCIInterface
{
    public abstract IEnumerator GetMove(Action<string> onMoveFound, List<string> moves);
    public abstract void Initilize();
    public abstract void StartNewGame();
}
