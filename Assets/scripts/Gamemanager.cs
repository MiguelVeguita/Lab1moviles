using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Gamemanager : MonoBehaviour
{
    [Header("Configuración de Objetos")]
    public GameObject ObjetoCreado;
    public Color[] colors;
    public Sprite[] shapes;

    private Color selectedColor = Color.white;
    private Sprite selectedShape;

    public Canvas canvas;

   [Header("configuracion del deleteador")]
    public GameObject deletionAreaPrefab;
    public float deletionAreaDuration = 0.5f;

    private float doubleTapTimeThreshold = 0.3f;
    private float lastTapTime;
    private GameObject draggedObject; 
    private bool isDragging = false; 
    private Vector2 offset; 

   
    private Vector2 swipeStartPosition; 
    private bool isSwiping = false; 
    public float swipeThreshold = 50f;

    [Header("Configuración de TrailRender")]
    public GameObject trailRendererPrefab;
    private GameObject currentTrail;

    [Header("Lista de Objetos Creados")]
    private List<GameObject> objetosCreados = new List<GameObject>();

    public void SetColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < colors.Length)
        {
            selectedColor = colors[colorIndex];
            Debug.Log("Color seleccionado: " + selectedColor);
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

    void Start()
    {

    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (!IsTouchInRestrictedArea(touch.position))
                    {
                        if (Time.time - lastTapTime < doubleTapTimeThreshold)
                        {
                            CreateDeletionArea(touch.position);
                        }
                        else
                        {
                            CheckForObjectToDrag(touch.position);
                        }

                        lastTapTime = Time.time;

                        if (!isDragging)
                        {
                            swipeStartPosition = touch.position;
                            isSwiping = true;

                            CrearTrail(touch.position);
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging && draggedObject != null)
                    {
                        MoveObjectWithTouch(touch.position);
                    }

                    if (isSwiping && !isDragging && currentTrail != null)
                    {
                        UpdateTrail(touch.position);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    draggedObject = null;

                    if (isSwiping && !isDragging)
                    {
                        CheckForSwipe(touch.position);
                    }
                    isSwiping = false;

                    if (currentTrail != null)
                    {
                        Destroy(currentTrail, currentTrail.GetComponent<TrailRenderer>().time);
                        currentTrail = null;
                    }
                    break;
            }
        }
    }

    private bool IsTouchInRestrictedArea(Vector2 touchPosition)
    {
        float restrictedAreaHeight = 450f; 
        return touchPosition.y > Screen.height - restrictedAreaHeight;
    }

    private void CheckForObjectToDrag(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = screenPosition;

        List<RaycastResult> results = new List<RaycastResult>();

        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerData, results);

        if (results.Count > 0)
        {
            draggedObject = results[0].gameObject;
            isDragging = true;

            RectTransform rectTransform = draggedObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    screenPosition,
                    canvas.worldCamera,
                    out localPoint
                );
                offset = rectTransform.anchoredPosition - localPoint;
            }
        }
        else
        {
            InstantiateObject(screenPosition);
        }
    }

    private void MoveObjectWithTouch(Vector2 screenPosition)
    {
        if (draggedObject != null && canvas != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPosition,
                canvas.worldCamera,
                out localPoint
            );

            RectTransform rectTransform = draggedObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = localPoint + offset;
            }
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

            objetosCreados.Add(newObject);
        }
        else
        {
            Debug.Log("Prefab, forma o Canvas no asignados.");
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
      
    }

    private void CrearTrail(Vector2 screenPosition)
    {
        if (trailRendererPrefab != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));

            currentTrail = Instantiate(trailRendererPrefab, worldPosition, Quaternion.identity);

            TrailRenderer trailRenderer = currentTrail.GetComponent<TrailRenderer>();
            if (trailRenderer != null)
            {
                trailRenderer.startColor = selectedColor;
                trailRenderer.endColor = selectedColor;
            }
        }
    }

    private void UpdateTrail(Vector2 screenPosition)
    {
        if (currentTrail != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));

            currentTrail.transform.position = worldPosition;
        }
    }

    private void CheckForSwipe(Vector2 endPosition)
    {
        float swipeDistance = Vector2.Distance(swipeStartPosition, endPosition);

        if (swipeDistance >= swipeThreshold && !isDragging)
        {
            DeleteAllObjects();
        }
    }

    private void DeleteAllObjects()
    {
        for (int i = objetosCreados.Count - 1; i >= 0; i--)
        {
            if (objetosCreados[i] != null)
            {
                Destroy(objetosCreados[i]);
            }
        }

        objetosCreados.Clear();

        Debug.Log("Todos los objetos han sido eliminados.");
    }
}