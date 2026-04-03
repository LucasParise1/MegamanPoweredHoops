using UnityEngine;

[CreateAssetMenu(fileName = "GameplayData", menuName = "Scriptable Objects/GameplayData")]
public class GameplayData : ScriptableObject
{
    public float MatchTime;
    public int Player1Points;
    public int Player2Points;

    private UIManager _uiManager;

    public void ResetData()
    {
        Player1Points = 0;
        Player2Points = 0;
        _uiManager.UpdatePlayer1Score(Player1Points);
    }

    public void SetUIManager(UIManager manager) => _uiManager = manager;

    public void AddPlayer1Points(int points)
    {
        Player1Points += points;
        _uiManager.UpdatePlayer1Score(Player1Points);
    }

    public void AddPlayer2Points(int points) => Player2Points += points;
}