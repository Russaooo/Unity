
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public float xOffsetOriginal=0;
    public float xOffset;
    public void SetXOffset() {
        xOffset = -target.position.x+transform.position.x;
        if (xOffsetOriginal == 0) {
            xOffsetOriginal = -target.position.x + transform.position.x;
            xOffset = xOffsetOriginal;
        }
        else {
            xOffset = xOffsetOriginal;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            Vector3 transformPosition = transform.position;
            transformPosition.x = target.position.x+xOffset;
            transform.position = transformPosition;
        }
        
    }
}
