using System;
using MoveCommands;
using UnityEngine;

[Serializable]
public class Square : MonoBehaviour
{
    public int[] Origin = new int[2] { 0, 0 };
    public Peace Occupant = null;
    public Camera _mainCamera;
    private SpriteRenderer _renderer;
    public Color DefaultColor;
    public char Column;
    public int Row;
    
    void Awake()
    {
        _mainCamera = Camera.main;
        _renderer = GetComponent<SpriteRenderer>();
        DefaultColor = _renderer.color;
    }

    void Update()
    {
        Vector2 mousepoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (_renderer.bounds.Contains(mousepoint))
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
    }

    public void OnClick()
    {

        GameManager gm = GameManager.Instance;
        Player currentPlayer = gm.CurrentPlayer();
        
        if (Occupant == null)
        {
            if (currentPlayer.Selected != null)
            {
                if (currentPlayer.Selected.GetType() == typeof(King))
                {
                    var castleCommands = ((King)currentPlayer.Selected).CastleCommands;
                    
                    for (int i = 0; i < castleCommands.Count; i++)
                    {
                        if (castleCommands[i] != null)
                        {
                            if (castleCommands[i][0].To == this)
                            {
                                for (int c = 0; c < castleCommands[i].Length; c++)
                                {
                                    castleCommands[i][c].Excecute();
                                }
                            }   
                        }

                    }
                }
                else if (currentPlayer.Selected.LegalMoves.Contains(this))
                {
                    new MoveCommand(currentPlayer.Selected, currentPlayer.Selected.Square, this, currentPlayer).Excecute();
                }
            }
        }
        else
        {
            // if not 2 player
            if (!GameManager.Instance.TwoPlayer)
            {
                if (GameManager.Instance.LocalPlayer() != currentPlayer)
                {
                    return;
                }
            }

            // select 
            if (currentPlayer.Peaces.Contains(Occupant))
            {
                if (currentPlayer.Selected != null)
                {
                    currentPlayer.Selected.Deselect();
                }

                currentPlayer.Selected = Occupant;
                currentPlayer.Selected.Select();
                return;
            }
        
            // take
            if (currentPlayer.Selected != null)
            {
                if (currentPlayer.Selected.LegalMoves.Contains(this))
                {
                    new TakeCommand(currentPlayer.Selected, currentPlayer.Selected.Square, this, currentPlayer).Excecute();
                }
            }
        }
    }
}