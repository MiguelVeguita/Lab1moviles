using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    public GameObject ObjetoCreado;
    public Color[] colors; 
    public Sprite[] shapes;

    private Color selectedColor = Color.white; 
    private Sprite selectedShape;

    public Canvas canvas;

    private float doubleTapTimeThreshold = 0.3f; 
    private float lastTapTime;
    public GameObject deletionAreaPrefab;
    public float deletionAreaDuration = 0.5f;

    public void SetColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < colors.Length)
        {
            selectedColor = colors[colorIndex];
            Debug.Log("color seleccionado: " + selectedColor);
        }

    }
    public void SetShape(int shapeIndex)
    {
        if (shapeIndex >= 0 && shapeIndex < shapes.Length)
        {
            selectedShape = shapes[shapeIndex];
            Debug.Log("Forma seleccionada: " + selectedShape.name);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Time.time - lastTapTime < doubleTapTimeThreshold)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                CreateDeletionArea(touchPosition);
            }
            else
            {
                Vector2 touchPosition = Input.GetTouch(0).position;
                InstantiateObject(touchPosition);
            }

            lastTapTime = Time.time;
        }
    }
    private void InstantiateObject(Vector2 screenPosition)
    {
        if (ObjetoCreado != null && selectedShape != null && canvas != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPosition,
                canvas.worldCamera,
                out Vector2 localPoint
            );

            GameObject newObject = Instantiate(ObjetoCreado, canvas.transform);
            RectTransform rectTransform = newObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = localPoint;
            }

            Image image = newObject.GetComponent<Image>();
            if (image != null)
            {
                image.color = selectedColor;
                image.sprite = selectedShape;
                Debug.Log("Objeto creado con color: " + selectedColor + " y forma: " + selectedShape.name);
            }
        }
        else
        {
            Debug.LogWarning("Prefab, forma o Canvas no asignados.");
        }
    }
    private void CreateDeletionArea(Vector2 screenPosition)
    {
        if (deletionAreaPrefab != null && canvas != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPosition,
                canvas.worldCamera,
                out Vector2 localPoint
            );

            GameObject deletionArea = Instantiate(deletionAreaPrefab, canvas.transform);
            RectTransform rectTransform = deletionArea.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = localPoint; 
            }

            Destroy(deletionArea, deletionAreaDuration);
        }
        else
        {
            Debug.LogWarning("Prefab del área de eliminación o Canvas no asignados.");
        }
    }


}
