using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCore : MonoBehaviour {
    // ссылки на контроллеры основных объектов
    public PlayerController player;
    public LevelController level;
    public GameObject predefinedStages;
    // ссылка на контроллер UI
    public UIActionsListener UI;
    // состояния игры
    public enum GameState {
        BeforeStartGame,
        ChooseLevel,
        PrepareToPlay,
        StartGame,
        Play,
        Win,
        Loose
    }
    private bool gamePaused = false;
    // время для исчезновения сегмента при встрече с коробкой
    public float timeToRecalculateDefault = 0.15f;
    // Карта уровней
    public List<PredefinedLevel> levelList = new();

    public PredefinedLevel levelLoadRequested;
    public GameObject currentLevelLoaded;

    public List<AudioClip> audioClips = new();
    public AudioSource audioSource;
    public AudioSource backgroundAudioSource;

    public void PlayAudioOnLoosingLife() {
        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[0]);
    }
    public void PlayAudioOnReceivingLife() {
        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[1]);
    }
    public void PlayAudioOnLoose() {
        audioSource.PlayOneShot(audioClips[2]);
    }
    public void PlayAudioOnWin() {
        audioSource.PlayOneShot(audioClips[3]);
    }

    private void UpdatePredefinedLevels() {
        PredefinedLevel PLcomponent;
        Transform PL = GameObject.Find("Predefined Levels").transform;
        Debug.Log("Levels amount: "+PL.childCount);
        if (PL.childCount > 0) {
            levelList.Clear();
            for (int i = 0; i < PL.childCount; i++) {
                Transform PLC = PL.GetChild(i);
                if (PLC.TryGetComponent<PredefinedLevel>(out PLcomponent)) {
                    Debug.Log(PLcomponent.name);
                    PLcomponent.levelID = i;
                    levelList.Add(PLcomponent);
                }
            }
        }
    }

    public GameObject previousUILayer;

    public GameState currentGameState;
    private void Awake() {
        player.SavePlayerOriginalPosition();
        ChangeStateToBeforeStartGame();
        UpdatePredefinedLevels();
        LoadSavedScores();
        UI.ChooseLevelScreenController.CreateLevelButtons();
        //UpdateButtonsDetailsOnChooseLevelScreen();
    }
    public void ChangeStateToBeforeStartGame() {
        currentGameState = GameState.BeforeStartGame;
    }
    public void ChangeStateToChooseLevel() {
        currentGameState = GameState.ChooseLevel;
    }
    public void ChangeStateToPrepareToPlay() {
        currentGameState = GameState.PrepareToPlay;
        Debug.Log("Change state to \"Prepare to play\"");
    }
    public void ChangeStateToStartGame() {
        currentGameState = GameState.StartGame;
    }
    public void ChangeStateToPlay() {
        currentGameState = GameState.Play;
    }
    public void ChangeStateToWin() {
        currentGameState = GameState.Win;
        PlayAudioOnWin();
    }
    public void ChangeStateToLoose() {
        currentGameState = GameState.Loose;
        PlayAudioOnLoose();
    }
    public void StartMove() {
        player.StartMove();
        ChangeStateToPlay();
    }
    public void HidePreviousUI() {
        if (previousUILayer != null) {
            if (previousUILayer.activeSelf) {
                previousUILayer.GetComponent<UIScreenDefaultFunctions>().Hide();
            }
        }
    }
    public void GoNextLevel() {
        Debug.Log("GoNextLevel()");
        PredefinedLevel currentLevelLoadedController = currentLevelLoaded.GetComponent<PredefinedLevel>();
        int levelID = currentLevelLoadedController.levelID;
        Debug.Log("Current levelID was :" + levelID.ToString());
        if (levelID < levelList.Count - 1) {
            levelID++;
        }
        else {
            levelID = 0;
        }
        Debug.Log("Next levelID is :" + levelID.ToString());
        levelLoadRequested = levelList[levelID];
        //ToggleBackgroundAudioVolume();
    }
    public void UpdateButtonsDetailsOnChooseLevelScreen() {
        
        for(int i=0;i<levelList.Count-1;i++) {
            UI.ChooseLevelScreenController.levelButtons[i].levelBestScoreValue = levelList[i].bestScore.ToString();
            UI.ChooseLevelScreenController.levelButtons[i].RedrawValues();
        }
    }
    public void GoRestartLevel() {
        Debug.Log("GoRestartLevel()");
        PredefinedLevel currentLevelLoadedController = currentLevelLoaded.GetComponent<PredefinedLevel>();
        int levelID = currentLevelLoadedController.levelID;
        Debug.Log("Current levelID was :" + levelID.ToString());
        Debug.Log("Next levelID is :" + levelID.ToString());
        levelLoadRequested = levelList[levelID];
        //ToggleBackgroundAudioVolume();
    }
    public void TogglePause() {
        if (UI.duringPlayScreenController.lockPause) { return; }
        if (gamePaused) {
            gamePaused = false;
            player.StartMove();
            UI.duringPlayScreenController.ToggleGamePausedLabel();
        }
        else {
            gamePaused = true;
            player.StopMove();
            UI.duringPlayScreenController.ToggleGamePausedLabel();
        }
    }
    private void UpdateSavedScores() {
        string saveData = "";
        foreach(PredefinedLevel PL in levelList) {
            saveData += PL.levelID.ToString();
            saveData += ":";
            saveData += PL.bestScore.ToString();
            if (!PL.Equals(levelList.Last())) {
                saveData += ";";
            }
        }
        PlayerPrefs.SetString("Save", saveData);
        PlayerPrefs.Save();
    }
    private void LoadSavedScores() {
        string saveData = "";
        string[] saveDataArr;
        saveData = PlayerPrefs.GetString("Save");
        Debug.Log(saveData.Length);
        if (saveData.Length > 0) {
            saveDataArr = saveData.Split(";");
            List<int> saveDataIntArr = new List<int>();
            Debug.Log(saveDataArr.Count());
            foreach (string k in saveDataArr) {
                Debug.Log("[" + k + "]");
                string[] kArr = k.Split(":");
                saveDataIntArr.Add(int.Parse(kArr[1]));
            }
            Debug.Log("saveDataIntArr: " + saveDataIntArr.Count);
            Debug.Log("levelList: " + levelList.Count);
            for (int i = 0; i < saveDataIntArr.Count; i++) {
                if (levelList[i] != null) {
                    levelList[i].bestScore = saveDataIntArr[i];
                }
            }
        }
    }
    private void Update() {
        
        switch (currentGameState) {
            case GameState.BeforeStartGame: // загрузка стартового экрана
                if (!UI.startScreenObject.activeSelf) {
                    UI.startScreenObject.GetComponent<UIScreenDefaultFunctions>().Show();
                }
                break;
            case GameState.ChooseLevel:
                
                if (!UI.ChooseLevelScreen.activeSelf) {
                    UpdateButtonsDetailsOnChooseLevelScreen();
                    HidePreviousUI();
                    UI.ChooseLevelScreen.GetComponent<UIScreenDefaultFunctions>().Show();
                    previousUILayer = UI.ChooseLevelScreen;
                }
                break;
            case GameState.PrepareToPlay:
                Debug.Log("Prepare to Play script activated");
                if (levelLoadRequested != null) {
                    if (currentLevelLoaded != null) {
                        if (currentLevelLoaded.TryGetComponent<PredefinedLevel>(out PredefinedLevel currentLevelLoadedController)) {
                            currentLevelLoadedController.Unload();
                            player.Drop();
                        }
                    }
                    levelLoadRequested.Load();
                    levelLoadRequested = null;
                    HidePreviousUI();
                    if (!UI.beforeMovementScreen.activeSelf) {
                        UI.beforeMovementScreen.GetComponent<UIScreenDefaultFunctions>().Show();
                    }
                    if (!UI.duringPlayScreen.activeSelf) {
                        
                        UI.duringPlayScreen.GetComponent<UIScreenDefaultFunctions>().Show();
                    }
                    previousUILayer = UI.beforeMovementScreen;
                    currentLevelLoaded.GetComponentInChildren<FinishTriggerController>().FillWaypointsInfo(true);
                    player.Init();
                    player.Show();
                    player.RestorePlayerOriginalPosition();
                    UI.duringPlayScreenController.RedrawCurrentLevel();
                }
                break;

            case GameState.Play:
                if (UI.beforeMovementScreen.activeSelf) {
                    HidePreviousUI();
                }
                previousUILayer = UI.duringPlayScreen;
                break;

            case GameState.Win:
                if (!UI.levelWinScreen.activeSelf) {
                    UI.levelWinScreen.GetComponent<UIScreenDefaultFunctions>().Show();
                    HidePreviousUI();
                    previousUILayer = UI.levelWinScreen;
                    PredefinedLevel currentLevelLoadedController = currentLevelLoaded.GetComponent<PredefinedLevel>();
                    int previousBestScore = currentLevelLoadedController.bestScore;
                    int currentScore = player.playerHealth;
                    
                    if (currentScore > previousBestScore) {
                        int levelID = currentLevelLoadedController.levelID;
                        levelList[levelID].bestScore = currentScore;
                        UI.levelWinScreenController.ShowBestScore(currentScore);
                        UpdateButtonsDetailsOnChooseLevelScreen();
                    }
                    else {
                        UI.levelWinScreenController.ShowCurrentScore(currentScore);
                    }
                    UpdateSavedScores();
                    UpdateButtonsDetailsOnChooseLevelScreen();
                }
                break;
            case GameState.Loose:
                if (!UI.levelLooseScreen.activeSelf) {
                    UI.levelLooseScreen.GetComponent<UIScreenDefaultFunctions>().Show();
                    HidePreviousUI();
                    previousUILayer = UI.levelLooseScreen;
                }
                break;
        }
        if (levelLoadRequested != null) {
            Debug.Log("level load request found!");
            ChangeStateToPrepareToPlay();
        }

    }
}
