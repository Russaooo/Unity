using System.CodeDom.Compiler;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPointController : MonoBehaviour
{
    public LevelSegmentController levelSegmentController; // ссылка на контроллер родительского сегмента
    public int positionZ; // порядковая позиция по оси Z
    public int positionX; // порядковая позиция по оси X

    // Регулировка содержимого точки спавна
    [Range(0, 2)]
    public int boxFoodOrNone=0;
    public bool enableBox = false;
    public bool enableFood = false;
    public bool enableLeftBorder = false;
    public bool enableRightBorder = false;
    // Галочка для генерации содержимого точки спавна
    public bool generateContent = false;
    // Галочка для обновления значения value у дочерних объектов Box или Food
    public bool updateValue = false;
    [Range(1, 50)]
    public int boxOrFoodValue = 1; // стоимость прохода через ящик или калорийность еды
    private int boxOrFoodValueLimit = 50;

    private Vector3 foodPositionOffet = new Vector3(2f,0.8f,2f);
    private Vector3 leftBorderPositionOffet = new Vector3(0f, 0.4f, 3.9f);
    private Vector3 rightBorderPositionOffet = new Vector3(0f,0.4f,-0.1f);

    private void SetVariables() {
        //transform.parent.TryGetComponent<LevelSegmentController>(out levelSegmentController);
        //levelSegmentController = transform.parent.GetComponent<LevelSegmentController>();
        // В точке спавна можно создавать либо Box, либо Food и/или Левый/Правый Border
        if (boxFoodOrNone == 0) {
            enableBox = true;
            enableFood = false;
            enableLeftBorder = false;
            enableRightBorder = false;
        }
        if (boxFoodOrNone == 1) {
            enableBox = false;
            enableFood = true;
        }
        if (boxFoodOrNone == 2) {
            enableBox = false;
            enableFood = false;
        }
    }
    void UpdateShaderVariables(Transform transform) {
        var tempMaterial = new Material(transform.Find("Box").GetComponent<MeshRenderer>().sharedMaterial);
        tempMaterial.SetFloat("_ColorMixValue", (float)((float)boxOrFoodValue / (float)boxOrFoodValueLimit));
        transform.Find("Box").GetComponent<MeshRenderer>().sharedMaterial = tempMaterial;
    }
    private void UpdateValues() {
        if (updateValue) {
            if (enableBox) {
                BoxController boxController = transform.Find("Box").GetComponent<BoxController>();
                boxController.value = boxOrFoodValue;
                boxController.RedrawValues();
                UpdateShaderVariables(transform);
            }
            if (enableFood) {
                FoodController foodController = transform.Find("Food").GetComponent<FoodController>();
                foodController.value = boxOrFoodValue;
                foodController.RedrawValues(); 
            }
            updateValue = false;
        }

        
    }
    private void GenerateContent() {
        if (generateContent) {
            if (enableBox) {
                GameObject templateBox = levelSegmentController.spawnPointTemplate.transform.Find("Box").gameObject;
                GameObject newBox = Instantiate(templateBox, transform.position, Quaternion.identity, transform);
                
                newBox.name = "Box";

                BoxController newBoxController = newBox.GetComponent<BoxController>();
                newBoxController.value = boxOrFoodValue;
                
                newBoxController.Init();
                newBoxController.RedrawValues();

                UpdateShaderVariables(transform);
            }
            if (enableFood) {
                GameObject templateFood = levelSegmentController.spawnPointTemplate.transform.Find("Food Spawn").transform.Find("Food 1 - "+positionZ).gameObject;
                GameObject newFood = Instantiate(templateFood, transform.position+foodPositionOffet, Quaternion.identity, transform);
                newFood.name = "Food";

                FoodController newFoodController = newFood.GetComponent<FoodController>();
                newFoodController.value = boxOrFoodValue;
                newFoodController.Init();
                newFoodController.RedrawValues();
            }
            if (enableLeftBorder) {
                GameObject templateLeftBorder = levelSegmentController.spawnPointTemplate.transform.Find("LB").transform.Find("LB l").gameObject;
                GameObject newLeftBorder = Instantiate(templateLeftBorder, transform.position + leftBorderPositionOffet, Quaternion.identity, transform);
                newLeftBorder.name = "Left Border";
            }
            if (enableRightBorder) {
                GameObject templateRightBorder = levelSegmentController.spawnPointTemplate.transform.Find("LB").transform.Find("LB r").gameObject;
                GameObject newRightBorder = Instantiate(templateRightBorder, transform.position + rightBorderPositionOffet, Quaternion.identity, transform);
                newRightBorder.name = "Right Border";
            }
            generateContent = false;
        }
    }
    void OnValidate() {
        SetVariables();
        GenerateContent();
        UpdateValues();
    }
}
