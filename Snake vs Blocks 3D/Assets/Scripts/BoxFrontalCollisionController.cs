using UnityEngine;

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
            Debug.Log("Фронтальная коллизия");
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
                    Debug.Log("Контейнер пропадает");
                    ExitCollision(collision);
                }
                if (PC.playerHealth == 0 && looseFlag==false) {
                    Debug.Log("Проигрыш");
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
            Debug.Log("Выход из коллизии");
            PC.boxCollision = false;
            PC.StartMove();
            BC.Disable();
            //collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnValidate() {
        BC = transform.parent.GetComponent<BoxController>();
        if (GameObject.Find("Player") != null) {
            GameObject.Find("Player").TryGetComponent<PlayerController>(out PC);
        }
        
        // установка локального таймера по настройке из ядра
        if (gameCore != null) {
            if (timeToRecalculateDefault != 0) {
                timeToRecalculateDefault = gameCore.timeToRecalculateDefault;
            }
        }
        
    }
}
