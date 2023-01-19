using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BoxFrontalCollisionController : MonoBehaviour
{
    public GameCore gameCore;
    public BoxController BC;
    public PlayerController PC;
    public float timeToRecalculateDefault;
    private float timeToRecalculate;
    private bool looseFlag = false;
    private void OnCollisionEnter(Collision collision) {
        if(collision.transform.name== "Player Segment - Head") {
            Debug.Log("����������� ��������");
            PC.DisableControl();
            PC.StopMove();
            PC.boxCollision = true;
            collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

        }
    }
    private void OnCollisionStay(Collision collision) {
        if (collision.transform.name == "Player Segment - Head") {
            if (timeToRecalculate <= 0) {
                if (PC.playerHealth > 0 & BC.value > 0) {
                    PC.RemoveSegment();
                    BC.DecreaseValue();
                    gameCore.PlayAudioOnLoosingLife();
                }
                if (BC.value == 0) {
                    Debug.Log("��������� ���������");
                    ExitCollision(collision);
                }
                if (PC.playerHealth == 0 && looseFlag==false) {
                    Debug.Log("��������");
                    looseFlag = true;
                    gameCore.ChangeStateToLoose();
                }
                timeToRecalculate = timeToRecalculateDefault;
            }
            else {
                timeToRecalculate -= Time.deltaTime;
            }
        }
        
    }
    private void ExitCollision(Collision collision) {
        if (collision.transform.name == "Player Segment - Head") {
            Debug.Log("����� �� ��������");
            PC.boxCollision = false;
            PC.StartMove();
            BC.Disable();
            //collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnValidate() {
        BC = transform.parent.GetComponent<BoxController>();
        PC = GameObject.Find("Player").GetComponent<PlayerController>();
        // ��������� ���������� ������� �� ��������� �� ����
        if (gameCore != null) {
            if (timeToRecalculateDefault != 0) {
                timeToRecalculateDefault = gameCore.timeToRecalculateDefault;
            }
        }
        
    }
}
