using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    private GameInput _gameInput;
    private Dictionary<int, TetrominoController> _activePieces = new();

    private void Awake()
    {
        Debug.Log("InputHandler initialized");

        _gameInput = new GameInput();
        _gameInput.Player1.Enable();
        _gameInput.Player2.Enable();

       
        _gameInput.Player1.MoveLeft.performed += ctx =>
        {
            Debug.Log("Player1 MoveLeft pressed (A key)");
            MoveTetromino(1, Direction.Left);
        };

        _gameInput.Player1.MoveRight.performed += ctx =>
        {
            Debug.Log("Player1 MoveRight pressed (D key)");
            MoveTetromino(1, Direction.Right);
        };

        _gameInput.Player1.FastDrop.performed += ctx =>
        {
            Debug.Log("Player1 FastDrop pressed (S key)");
            FastDrop(1);
        };

      
        _gameInput.Player2.MoveLeft.performed += ctx =>
        {
            Debug.Log("Player2 MoveLeft pressed (Left Arrow)");
            MoveTetromino(2, Direction.Left);
        };

        _gameInput.Player2.MoveRight.performed += ctx =>
        {
            Debug.Log("Player2 MoveRight pressed (Right Arrow)");
            MoveTetromino(2, Direction.Right);
        };

        _gameInput.Player2.FastDrop.performed += ctx =>
        {
            Debug.Log("Player2 FastDrop pressed (Down Arrow)");
            FastDrop(2);
        };
    }

    private void UpdateActivePieces()
    {
        var controllers = FindObjectsByType<TetrominoController>(FindObjectsSortMode.None);
        _activePieces.Clear();

        foreach (var controller in controllers)
        {
            Rigidbody2D rb = controller.GetComponent<Rigidbody2D>();

            if (rb.bodyType == RigidbodyType2D.Dynamic)
            {
                int playerId = controller.transform.position.x < 0 ? 1 : 2;
                _activePieces[playerId] = controller;
                Debug.Log($"Tracking Player {playerId} piece at {controller.transform.position}");
            }
        }
    }

    private TetrominoController GetPlayerController(int playerId)
    {
    
        var controllers = FindObjectsByType<TetrominoController>(FindObjectsSortMode.None);

        foreach (var controller in controllers)
        {
            Rigidbody2D rb = controller.GetComponent<Rigidbody2D>();

            bool isCorrectPlayer = (playerId == 1 && controller.transform.position.x < 0) ||
                                  (playerId == 2 && controller.transform.position.x > 0);

            if (rb.bodyType == RigidbodyType2D.Dynamic && isCorrectPlayer)
            {
                return controller;
            }
        }

        Debug.LogWarning($"No active piece found for Player {playerId}");
        return null;
    }

    private void MoveTetromino(int playerId, Direction direction)
    {
        Debug.Log($"Attempting to move Player {playerId} piece {direction}");
        var controller = GetPlayerController(playerId);

        if (controller == null)
        {
            Debug.LogWarning($"No active controller found for Player {playerId}");
            return;
        }

        Debug.Log($"Moving Player {playerId} piece with force");
        controller.Move(direction);
    }

    private void FastDrop(int playerId)
    {
        Debug.Log($"Fast dropping Player {playerId} piece");
        var controller = GetPlayerController(playerId);

        if (controller == null)
        {
            Debug.LogWarning($"No active controller found for Player {playerId}");
            return;
        }

        controller.FastDrop();
    }
    private void OnDestroy()
    {
        _gameInput?.Dispose();
    }

}