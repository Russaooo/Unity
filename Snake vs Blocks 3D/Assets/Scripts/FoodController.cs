using TMPro;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public int value = 0;
    public int tempValue = 0;
    public bool activated = false;
    public PlayerController PC;
    public float timeToRecalculateDefault;
    private float timeToRecalculate;
    private TextMeshPro valueIndicator;
    public void RedrawValues() {
        valueIndicator.text = value.ToString();
    }
    public void Init() {
        valueIndicator = transform.Find("Table").Find("Value").GetComponent<TextMeshPro>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.name == "Player Segment - Head") {
            if (activated == false) {
                activated = true;
                Debug.Log("Food trigger enter");
                tempValue = value;
                if (tempValue > 0) {
                    for (int i = tempValue; i >= 0; i--) {
                        PC.AddSegment();
                    }
                    PC.gameCore.PlayAudioOnReceivingLife();
                }
            }
            
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.name == "Player Segment - Head" && activated) {
            Debug.Log("Food trigger EXIT");
            Disable();
        }
    }
    public void Enable() {
        gameObject.SetActive(true);
    }
    public void Disable() {
        gameObject.SetActive(false);
    }
    private void OnValidate() {
        Init();
        RedrawValues();
    }
    private void OnAwake() {
        PC = GameObject.Find("Player").GetComponent<PlayerController>();
    }
}
