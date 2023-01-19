using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActionsListener : MonoBehaviour
{
    public GameCore gameCore;

    public GameObject startScreenObject;

    public GameObject ChooseLevelScreen;
    public ChooseLevelScreenController ChooseLevelScreenController;
    
    public GameObject beforeMovementScreen;
    
    public GameObject duringPlayScreen;
    public DuringPlayScreenController duringPlayScreenController;

    public GameObject levelWinScreen;
    public LevelWinScreenController levelWinScreenController;

    public GameObject levelLooseScreen;

    public void OnUIStartScreenButtonClick() {
        startScreenObject.GetComponent<UIScreenDefaultFunctions>().Hide();
        gameCore.ChangeStateToChooseLevel();
        //gameCore.GameStart();
    }
    private void Update() {
        if (beforeMovementScreen.activeSelf) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                //beforeMovementScreen.GetComponent<UIScreenDefaultFunctions>().Hide();
                gameCore.StartMove();
            }
        }
        if (duringPlayScreen.activeSelf) {
            duringPlayScreenController.UpdateHealthIndicator();
        }
    }
}
