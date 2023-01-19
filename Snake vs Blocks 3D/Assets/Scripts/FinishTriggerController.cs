using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTriggerController : MonoBehaviour
{
    public PlayerController PC;

    private Vector3 targetPosition;
    public int currentWP=0;
    public List<Vector3> waypoints = new List<Vector3>();
    public List<GameObject> waypointsObject = new List<GameObject> ();

    public UIActionsListener UI;

    public bool fillWaypointsInfo = false;
    public bool showWaypoints = false;
    public bool hideWaypoints = false;

    private bool levelPassed = false;
    private bool triggeredChangeState = false;
    private void OnTriggerEnter(Collider other) {
        if(other.name == "Player Segment - Head") {
            UI.duringPlayScreenController.lockPause = true;
            Debug.Log("WP - "+currentWP+": "+waypoints[currentWP]);
            targetPosition = waypoints[currentWP];
            PC.moveToTarget = targetPosition;
            PC.finishTrigger = true;
            PC.DisableControl();
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.name == "Player Segment - Head") {
            if (!levelPassed) {
                if (currentWP != waypoints.Count) {
                    targetPosition = waypoints[currentWP];
                    PC.moveToTarget = targetPosition;
                    if (PC.snakeHead.transform.position == targetPosition) {
                        if (currentWP < waypoints.Count - 1) {
                            Debug.Log("WP - " + currentWP + ": " + waypoints[currentWP]);
                        }
                        else {
                            currentWP = waypoints.Count - 1;
                        }
                    }
                }
                else {
                    Debug.Log("Уровень пройден");
                    levelPassed = true;
                    UI.duringPlayScreenController.lockPause = false;
                }
            }

        }
    }
    public void FillWaypointsInfo(bool force=false) {
        if (fillWaypointsInfo || force) {
            Transform[] WP = gameObject.GetComponentsInChildren<Transform>();
            waypoints.Clear();
            waypointsObject.Clear();
            foreach (Transform wp in WP) {
                if (wp.parent.GetInstanceID() == transform.GetInstanceID()) {
                    waypoints.Add(wp.position);
                    waypointsObject.Add(wp.gameObject);
                    wp.GetComponent<WaypointEventListener>().triggerNumber = waypoints.Count-1;
                }
            }
            fillWaypointsInfo = false;
        }
    }
    private void HideWaypoints() {
        if (hideWaypoints) {
            MeshRenderer[] WP = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer wp in WP) {
                if (wp.transform.parent.GetInstanceID() == transform.GetInstanceID()) {
                    wp.enabled = false;
                }
            }
            hideWaypoints = false;
        }
    }
    private void ShowWaypoints() {
        if (showWaypoints) {
            MeshRenderer[] WP = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer wp in WP) {
                if (wp.transform.parent.GetInstanceID() == transform.GetInstanceID()) {
                    wp.enabled = true;
                }
            }
            showWaypoints = false;
        }
    }
    private void OnValidate() {
        FillWaypointsInfo();
        HideWaypoints();
        ShowWaypoints();
    }
    private void Awake() {
        fillWaypointsInfo = true;
        UI = GameObject.Find("UIActionsListener").GetComponent<UIActionsListener>();
    }
    private void Update() {
        if (levelPassed) {
            if (!triggeredChangeState) {
                triggeredChangeState = true;
                PC.gameCore.ChangeStateToWin();
            }
        }
    }
}
