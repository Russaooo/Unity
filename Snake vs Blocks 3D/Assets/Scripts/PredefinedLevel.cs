using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredefinedLevel : MonoBehaviour
{
    public bool levelLoaded = false;
    public string levelName = "0";
    public float levelExtraSpeed = 0;
    public int bestScore = 0;
    public int levelID;

    public bool turnStagesOnALine = false;
    

    public void TurnStagesOnALine() {
        if (turnStagesOnALine) {
            LevelSegmentController[] stages = gameObject.GetComponentsInChildren<LevelSegmentController>();
            Vector3 firstPosition = Vector3.zero;
            Vector3 currentPosition = firstPosition;
            for(int i=0;i<stages.Length;i++){
                stages[i].transform.position = currentPosition;
                currentPosition.x += 40;
            }
            turnStagesOnALine=false;
        }
    }
    private void OnValidate() {
        //Debug.Log("PredefinedLevel.onValidate() happened.");
        TurnStagesOnALine();
    }
    public void Load() {
        //gameObject.SetActive(true);
        GameCore gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        Transform copyTo = GameObject.Find("Playable Level").transform;
        GameObject newLevel = Instantiate(gameObject, copyTo);
        gameCore.currentLevelLoaded = newLevel;
        gameCore.currentLevelLoaded.SetActive(true);
        gameCore.currentLevelLoaded.GetComponent<PredefinedLevel>().levelLoaded = true;
    }
    public void Unload() {
        if(transform.parent.name == "Playable Level") {
            Destroy(gameObject);
        }
    }
}
