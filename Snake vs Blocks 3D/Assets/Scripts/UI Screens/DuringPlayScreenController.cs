using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DuringPlayScreenController : MonoBehaviour
{
    public GameCore gameCore;
    public GameObject healthValue;
    public PlayerController playerController;
    public TMPro.TextMeshProUGUI currentLevelValue;
    public GameObject gamePausedLabel;
    public bool lockPause = false;
    public void UpdateHealthIndicator() {
        healthValue.GetComponent<TextMeshProUGUI>().text = playerController.playerHealth.ToString();
    }
    public void RedrawCurrentLevel() {
        currentLevelValue.text = gameCore.currentLevelLoaded.GetComponent<PredefinedLevel>().levelName;
    }
    public void ToggleGamePausedLabel() {  
        if (gamePausedLabel.activeSelf) {
            gamePausedLabel.SetActive(false);
        }
        else {
            gamePausedLabel.SetActive(true);
        }
    }
}
