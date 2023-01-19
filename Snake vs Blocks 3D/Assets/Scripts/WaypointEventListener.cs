using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointEventListener : MonoBehaviour
{
    public FinishTriggerController finishTriggerController;
    public int triggerNumber;

    private void OnTriggerEnter(Collider other) {
        if(other.transform.name == "Player Segment - Head") {
            if (finishTriggerController.currentWP == triggerNumber) {
                finishTriggerController.currentWP++;
            }
        }
    }
}
