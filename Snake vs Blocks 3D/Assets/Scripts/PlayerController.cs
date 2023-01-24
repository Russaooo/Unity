using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public GameCore gameCore;
    public Transform mainCamera;
    public Camera _mainCamera;
    public Transform snakeHead;
    public Rigidbody snakeHeadRB;
    public List<GameObject> playerSegments = new List<GameObject>();
    public List<Vector3> segmentPositions = new List<Vector3>();
    public float DistanceOffset;
    public bool playerControlEnabled = false;
    public bool playerCreated = false;
    public bool movementStarted = false;
    public int playerHealth = 0;
    public float snakeSpeed = 2f;
    public float sensitivity = 10;
    private float sidewaysSpeed;
    public Vector3 playerOriginalPosition;
    
    private Vector2 lastClickPosition;
    public bool boxCollision = false;
    // для движения по вэйпоинтам финиш-триггера
    public bool finishTrigger = false;
    public Vector3 moveToTarget;
    public void SavePlayerOriginalPosition() { 
        playerOriginalPosition = transform.position;
    }
    public void RestorePlayerOriginalPosition() {
        transform.position = playerOriginalPosition;
        transform.Find("Player Segment").position = playerOriginalPosition;
    }
    public void Hide() {
        gameObject.SetActive(false);
    }
    public void Show() {
        gameObject.SetActive(true);
    }
    public void EnableControl() {
        playerControlEnabled = true;
    }
    public void DisableControl() {
        playerControlEnabled = false;
    }
    public void PushSegment(GameObject g) {
        playerSegments.Add(g);
    }
    public void PushPosition(GameObject t) {
        segmentPositions.Add(t.transform.position);
    }
    public void AddHead() {
        GameObject segment = transform.Find("Player Segment").gameObject;
        Vector3 nextPosition;
        GameObject newSegment;
        // создаём голову
        nextPosition = segment.transform.position;
        newSegment = Instantiate(segment, nextPosition, Quaternion.identity, transform);
        newSegment.name = "Player Segment - Head";
        newSegment.SetActive(true);
        PushPosition(newSegment);
        snakeHead = newSegment.transform;
        snakeHeadRB = newSegment.GetComponent<Rigidbody>();
    }
    public void AddSegment() {
        GameObject segment = transform.Find("Player Segment").gameObject;
        Vector3 nextPosition;
        GameObject newSegment;
        nextPosition = segmentPositions[^1];
        nextPosition.x -= (snakeHead.localScale.x + DistanceOffset);
        newSegment = Instantiate(segment, nextPosition, Quaternion.identity, transform);
        newSegment.name = "Player Segment - Body Element";
        newSegment.GetComponent<Rigidbody>().isKinematic = true;
        newSegment.SetActive(true);
        PushSegment(newSegment);
        PushPosition(newSegment);
        playerHealth++;
        
    }
    public void PopSegment() {
        Destroy(playerSegments[0]);
        playerSegments.RemoveAt(0);
    }
    public void PopPosition() {
        segmentPositions.RemoveAt(segmentPositions.Count-1);
    }
    public void RemoveSegment() {
        if (playerHealth > 0) {
            playerHealth--;
            PopSegment();
            PopPosition(); 
        }
    }
    private void AddMainCameraFollower() {
        mainCamera.GetComponent<CameraFollower>().target = snakeHead;
        mainCamera.GetComponent<CameraFollower>().SetXOffset();
    }
    public void Init() {
        AddHead(); // создали голову
        AddMainCameraFollower(); // добавили слежение камерой за головой
        AddSegment();
        AddSegment();
        AddSegment();
        AddSegment();
        AddSegment();
        AddSegment();
        playerCreated = true;
    }
    public void Drop() {
        Destroy(snakeHead.gameObject);
        foreach(GameObject PS in playerSegments) {
            Destroy(PS);
        }
        playerHealth = 0;
        playerSegments.Clear();
        segmentPositions.Clear();
        playerCreated = false;
        movementStarted = false;
        playerControlEnabled = false;
        boxCollision = false;
        finishTrigger = false;
        //moveToTarget = ;
    }
    public void StartMove() {
        if (!movementStarted) {
            movementStarted = true;
            EnableControl();
        }
    }
    public void StopMove() {
        if (movementStarted) {
            movementStarted = false;
            DisableControl();
        }
    }
    public void ReactOnControl() {
        if (movementStarted && playerControlEnabled) {
            if (Input.GetMouseButtonDown(0)) {
                //Debug.Log("LMB down");
                lastClickPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0)) {
                //Debug.Log("LMB up");
                sidewaysSpeed = 0;
            }
            else if (Input.GetMouseButton(0)) {
                Vector2 delta = (Vector2)_mainCamera.ScreenToViewportPoint(Input.mousePosition) - lastClickPosition;
                sidewaysSpeed += delta.x * sensitivity;
                lastClickPosition = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
            }
        }
    }
    public void ApplyMove() {
        if(movementStarted && playerControlEnabled) {
            if (Mathf.Abs(sidewaysSpeed) > 4) {
                sidewaysSpeed = 4 * Mathf.Sign(sidewaysSpeed);
            }
            snakeHeadRB.velocity = new Vector3(0,0, -sidewaysSpeed * 5);
            Vector3 newPosition = snakeHead.position;
            snakeHead.position = Vector3.MoveTowards(newPosition, new Vector3(newPosition.x + snakeSpeed,newPosition.y,newPosition.z), (snakeSpeed + gameCore.currentLevelLoaded.GetComponent<PredefinedLevel>().levelExtraSpeed) * Time.deltaTime);
            //sidewaysSpeed = 0;
        }
        else {
            if (boxCollision) {
                snakeHeadRB.velocity = new Vector3(snakeSpeed, 0, 0);
            }
            if (finishTrigger) {
                snakeHeadRB.velocity = Vector3.zero;
                MoveToTarget();
            }
        }
    }
    private void MoveToTarget() {
            snakeHead.transform.position = Vector3.MoveTowards(snakeHead.transform.position, moveToTarget, 10 * Time.deltaTime);
    }
    public void MoveSegments() {
        if (movementStarted | boxCollision) {
            float moveDistance = (snakeHead.position - segmentPositions[0]).magnitude;
            if (moveDistance >= snakeHead.localScale.x) {
                segmentPositions.Insert(0, snakeHead.position);
                segmentPositions.RemoveAt(segmentPositions.Count - 1);
                moveDistance -= (snakeHead.localScale.x);
            }
            for(int i = 0; i < playerSegments.Count; i++) {
                if (boxCollision) {
                    playerSegments[i].transform.position = Vector3.Lerp(playerSegments[i].transform.position, segmentPositions[i]+new Vector3(-snakeHead.localScale.x, 0,0), 1/gameCore.timeToRecalculateDefault*Time.deltaTime);
                }
                else {
                    playerSegments[i].transform.position = Vector3.Lerp(segmentPositions[i + 1], segmentPositions[i], moveDistance / snakeHead.localScale.x);
                }
            }
        }
    }
    void Update() {
        ReactOnControl();
        ApplyMove();
        MoveSegments();
    }
}
