using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutController : MonoBehaviour
{
    public RectTransform RT;
    public GridLayoutGroup GLG;
    /*public bool FindRectTransform() {
        if(TryGetComponent<RectTransform>(out RT)) {
            return true;
        }
        else {
            return false;
        }
        
    }
    public bool FindGridLayoutGroup() {
        if (TryGetComponent<GridLayoutGroup>(out GLG)) {
            return true;
        }
        else {
            return false;
        }
    }*/
    public float GetCalculatedHeight() {
        return GLG.minHeight + 12f;
    }
    public float SetRectTransform(float height) {
        RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        //RT.sizeDelta = new Vector2(0, GLG.preferredHeight);//GLG.preferredHeight;
        return height;
    }
}
