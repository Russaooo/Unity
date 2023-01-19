using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonController : MonoBehaviour
{
    public UIActionsListener UI;
    public GameCore GameCore;
    public TMPro.TextMeshProUGUI levelNumberIndicator;
    public string levelNumberValue;
    public TMPro.TextMeshProUGUI levelBestScoreIndicator;
    public string levelBestScoreValue;

    public PredefinedLevel linkedLevel;
    // Start is called before the first frame update
    public void RedrawValues() {
        levelNumberIndicator.text = levelNumberValue;
        levelBestScoreIndicator.text = levelBestScoreValue;
    }
    public void SendMessageToGameCore() {
        GameCore.levelLoadRequested = linkedLevel;
        GameCore.previousUILayer = UI.ChooseLevelScreen;
    }
    
}
