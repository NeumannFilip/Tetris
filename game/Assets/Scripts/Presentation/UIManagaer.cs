using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    [Header("Player 1")]
    [SerializeField] private TMP_Text player1Name;
    [SerializeField] private TMP_Text player1Score;
    [SerializeField] private Transform player1NextPiece;

    [Header("Player 2")]
    [SerializeField] private TMP_Text player2Name;
    [SerializeField] private TMP_Text player2Score;
    [SerializeField] private Transform player2NextPiece;


    private int _player1Score;
    private int _player2Score;

    private void ClearPreview(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnScoreUpdated += UpdateScore;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreUpdated -= UpdateScore;
    }

    private void OnDestroy()
    {
        GameEvents.OnScoreUpdated -= UpdateScore;
    }



    private GameObject GetPreviewPrefab(TetrominoType type)
    {
     
        return GameManager.Instance.tetrominoPrefabs[(int)type];
    }


    public void UpdateScore(int playerId, int points)
    {
        if (playerId == 1)
        {
            // Fixed variable name to match serialized field
            player1Score.text = (int.Parse(player1Score.text) + points).ToString();
        }
        else
        {
            // Fixed variable name to match serialized field
            player2Score.text = (int.Parse(player2Score.text) + points).ToString();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameEvents.OnScoreUpdated += UpdateScore;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
