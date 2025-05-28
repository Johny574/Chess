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
                Onclick();
            }
        }
    }

    public void Onclick()
    {

        GameManager gm = GameManager.Instance;
        Player currentPlayer = gm.CurrentPlayer();
        if (Occupant == null)
        {
            if (currentPlayer.Selected != null)
            {
                if (currentPlayer.Selected.LegalMoves.Contains(this))
                {
                    new MoveCommand(currentPlayer.Selected, currentPlayer.Selected.Square, this, currentPlayer).Excecute();
                }
            }
        }
        else
        {
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