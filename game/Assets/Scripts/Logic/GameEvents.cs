public static class GameEvents
{
    public static event System.Action<int, int> OnScoreUpdated;

    public static void TriggerScoreUpdate(int playerId, int points)
    {
        OnScoreUpdated?.Invoke(playerId, points);
    }
}
