using UnityEngine;

public class LevelWinScreenController : MonoBehaviour
{
    public GameCore gameCore;
    public PlayerController playerController;

    public TMPro.TextMeshProUGUI bestScore;
    public TMPro.TextMeshProUGUI currentLevelScore;

    public void ShowBestScore(int score) {
        transform.Find("Best Score").gameObject.SetActive(true);
        transform.Find("Current Score").gameObject.SetActive(false);
        bestScore.text = score.ToString();
    }
    public void ShowCurrentScore(int score) {
        transform.Find("Current Score").gameObject.SetActive(true);
        transform.Find("Best Score").gameObject.SetActive(false);
        currentLevelScore.text = score.ToString();
    }
}
