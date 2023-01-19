using TMPro;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public int value = 0;
    private TextMeshPro valueOnTop,valueOnFront;
    public void RedrawValues() {
        valueOnTop.text = value.ToString();
        valueOnFront.text = value.ToString();
    }
    public void Init() {
        valueOnTop = transform.Find("Value Top").GetComponent<TextMeshPro>();
        valueOnFront = transform.Find("Value Front").GetComponent<TextMeshPro>();
    }
    public void DecreaseValue() {
        if (value > 0) {
            value--;
        } 
    }
    public void Disable() {
        gameObject.SetActive(false);
    }
    public void OnEnable() {
        gameObject.SetActive(true);
    }
    private void Update() {
        if(valueOnTop == null || valueOnFront == null) {
            Init();
        }
        else {
            RedrawValues();
        }
        
    }
    void OnValidate() {
        Init();
        RedrawValues();
    }
}
