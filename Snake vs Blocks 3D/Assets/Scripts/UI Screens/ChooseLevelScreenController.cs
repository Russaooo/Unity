using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseLevelScreenController : MonoBehaviour
{
    public GameCore gameCore;

    
    
    public GameObject buttonTemplate;
    public RectTransform buttonsArea;

    public GridLayoutController GLC;
    public List<LevelButtonController> levelButtons = new();

    private float lastHeight;

    public bool levelButtonsCreated = false;
    
    public void CreateLevelButtons() {
        if (!levelButtonsCreated) {
            foreach (PredefinedLevel PL in gameCore.levelList) {
                GameObject newLevelButton = Instantiate(buttonTemplate, buttonsArea.transform);
                LevelButtonController NLBC = newLevelButton.GetComponent<LevelButtonController>();
                NLBC.levelNumberValue = PL.levelName;
                NLBC.levelBestScoreValue = PL.bestScore.ToString();
                NLBC.linkedLevel = PL;
                NLBC.RedrawValues();
                NLBC.gameObject.SetActive(true);
                levelButtons.Add(NLBC);

            }
            levelButtonsCreated = true;
        }
    }
    public void Update() {
        if (gameObject.activeSelf) {
            gameCore.UpdateButtonsDetailsOnChooseLevelScreen();
            if (lastHeight != GLC.GetCalculatedHeight()) {
                lastHeight = GLC.SetRectTransform(GLC.GetCalculatedHeight());
            };
        }
        
    }
}
