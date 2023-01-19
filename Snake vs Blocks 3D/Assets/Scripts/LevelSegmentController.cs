using UnityEditor.SceneManagement;
using UnityEngine;

public class LevelSegmentController : MonoBehaviour
{
    public GameCore gameCore;
    public GameObject spawnPointTemplate; // объект с содержимым точки спавна игровых объектов
    public GameObject spawnPointPlaceholder; // заместитель точки спавна
    public Transform predefinedStages;

    public bool generateSpawnPoints = false; // регулирует, нужно ли создать на сцене точки спавна или нет
    public bool addToPredefinedStages = false; // добавляет копию стадиии в коллекцию уровней
    public bool hidePlaceholders = false; // скрывает полупрозрачные розовые кубы с зелёными крестами
    public bool displayPlaceholders = false; // отображает полупрозрачные розовые кубы с зелёными крестами
    private void SetSpawnPointsVariables() {
        if (transform.Find("Spawn Point Template") != null && transform.Find("Spawn Point - Placeholder") != null) {
            spawnPointTemplate = transform.Find("Spawn Point Template").gameObject;
            spawnPointPlaceholder = transform.Find("Spawn Point - Placeholder").gameObject;
        }
    }
    private void GenerateSpawnPoints() {
        int positionZ,positionX;
        if (generateSpawnPoints) {
            positionZ = 1;
            for (float i = 2f; i <= 18f; i = i + 4f) {
                positionX = 1;
                for (float j = 0f; j <= 36f; j = j + 4f) {
                    GameObject newGeneratedSpawnPoint = Instantiate(spawnPointPlaceholder, new Vector3(j, 0f, i), Quaternion.identity, transform);
                    newGeneratedSpawnPoint.name = "Spawn Point";
                    newGeneratedSpawnPoint.SetActive(true);
                    SpawnPointController newGeneratedSpawnPointCointroller = newGeneratedSpawnPoint.GetComponent<SpawnPointController>();
                    newGeneratedSpawnPointCointroller.positionX = positionX;
                    newGeneratedSpawnPointCointroller.positionZ = positionZ;
                    positionX++;
                }
                positionZ++;
            }
            generateSpawnPoints = false;
        }
    }
    private void AddToPredefinedStages() {
        if (addToPredefinedStages) {
            GameObject newStage = Instantiate(gameObject, predefinedStages);
            newStage.name = "Stage";
            newStage.SetActive(false);
            newStage.GetComponent<LevelSegmentController>().addToPredefinedStages = false;
            addToPredefinedStages = false;
        }
    }
    private void HidePlaceholders() {
        if (hidePlaceholders) {
            SpawnPointController[] spawnPointController = transform.GetComponentsInChildren<SpawnPointController>();
            foreach (SpawnPointController SPC in spawnPointController) {
                SPC.transform.Find("Placeholder").gameObject.SetActive(false);
            }
            hidePlaceholders = false;
        }
    }
    private void DisplayPlaceholders() {
        if (displayPlaceholders) {
            SpawnPointController[] spawnPointController = transform.GetComponentsInChildren<SpawnPointController>();
            foreach (SpawnPointController SPC in spawnPointController) {
                SPC.transform.Find("Placeholder").gameObject.SetActive(true);
            }
            displayPlaceholders = false;
        }
    }
    private void OnValidate() {
        SetSpawnPointsVariables();
        GenerateSpawnPoints();
        AddToPredefinedStages();
        HidePlaceholders();
        DisplayPlaceholders();
    }
}
