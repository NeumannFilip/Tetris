using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Tetromino Prefabs")]
    [SerializeField] public GameObject[] tetrominoPrefabs;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Scoring")]
    [SerializeField] private int pointsPerThreePieces = 300;

    private Dictionary<int, List<GameObject>> playerLandedPieces = new Dictionary<int, List<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerLandedPieces[1] = new List<GameObject>();
            playerLandedPieces[2] = new List<GameObject>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SpawnNewTetromino(int playerId)
    {
        int spawnIndex = playerId - 1;

        if (IsPositionBlocked(spawnPoints[spawnIndex].position))
        {
            Debug.Log("Game Over for Player " + playerId);
            return;
        }

        int prefabIndex = Random.Range(0, tetrominoPrefabs.Length);
        GameObject newPiece = Instantiate(
            tetrominoPrefabs[prefabIndex],
            spawnPoints[spawnIndex].position,
            Quaternion.identity
        );

        newPiece.GetComponent<TetrominoController>().PlayerId = playerId;
    }

    public void PieceLanded(GameObject landedPiece, int playerId)
    {
        playerLandedPieces[playerId].Add(landedPiece);

        if (playerLandedPieces[playerId].Count >= 3)
        {
            foreach (var piece in playerLandedPieces[playerId])
            {
                Destroy(piece);
            }
            playerLandedPieces[playerId].Clear();
            GameEvents.TriggerScoreUpdate(playerId, pointsPerThreePieces);
        }

        SpawnNewTetromino(playerId);
    }

    private void Start()
    {
        SpawnNewTetromino(1);
        SpawnNewTetromino(2);
    }

    private bool IsPositionBlocked(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Block")) return true;
        }
        return false;
    }
}