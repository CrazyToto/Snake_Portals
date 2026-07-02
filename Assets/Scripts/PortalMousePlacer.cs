using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PortalMousePlacer : MonoBehaviour
{
    // Dieses Portal wird mit der linken Maustaste versetzt.
    public Transform leftClickPortal;

    // Dieses Portal wird mit der rechten Maustaste versetzt.
    public Transform rightClickPortal;

    // Optionaler Bereich, in dem Portale platziert werden duerfen.
    public BoxCollider2D placementArea;

    // Wenn true, werden Portale am Rand festgehalten, statt Klicks ausserhalb zu ignorieren.
    public bool clampToPlacementArea = true;

    private Camera _camera;

    private void Awake()
    {
        // Die Hauptkamera wird gebraucht, um Mauspositionen in Weltpositionen umzuwandeln.
        _camera = Camera.main;
    }

    private void Update()
    {
        if (PauseMenuController.IsPaused)
        {
            return;
        }

        Mouse mouse = Mouse.current;

        // Ohne Maus, Kamera oder bei UI-Klicks soll kein Portal bewegt werden.
        if (mouse == null || _camera == null || IsPointerOverUi())
        {
            return;
        }

        if (mouse.leftButton.wasPressedThisFrame)
        {
            // Linksklick setzt das erste Portal.
            MovePortalToMousePosition(leftClickPortal, mouse);
        }
        else if (mouse.rightButton.wasPressedThisFrame)
        {
            // Rechtsklick setzt das zweite Portal.
            MovePortalToMousePosition(rightClickPortal, mouse);
        }
    }

    private void MovePortalToMousePosition(Transform portal, Mouse mouse)
    {
        // Wenn im Inspector kein Portal eingetragen ist, passiert nichts.
        if (portal == null)
        {
            return;
        }

        Portal portalComponent = portal.GetComponent<Portal>();

        // Ein Portal, das gerade von der Schlange benutzt wird, darf nicht versetzt werden.
        if (portalComponent != null && portalComponent.isBlocked)
        {
            return;
        }

        Vector2 screenPosition = mouse.position.ReadValue();

        // Bildschirmposition der Maus wird in eine Position in der Spielwelt umgerechnet.
        Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -_camera.transform.position.z));

        // Portale werden auf ganze Rasterfelder gesetzt.
        Vector3 gridPosition = new Vector3(
            Mathf.Round(worldPosition.x),
            Mathf.Round(worldPosition.y),
            portal.position.z
        );

        if (placementArea != null)
        {
            Bounds bounds = placementArea.bounds;

            // Wenn Clamp aus ist, werden Klicks ausserhalb des Spielfelds ignoriert.
            if (!bounds.Contains(gridPosition) && !clampToPlacementArea)
            {
                return;
            }

            // Wenn Clamp an ist, bleibt das Portal innerhalb des Spielfelds.
            gridPosition.x = Mathf.Clamp(gridPosition.x, Mathf.Ceil(bounds.min.x), Mathf.Floor(bounds.max.x));
            gridPosition.y = Mathf.Clamp(gridPosition.y, Mathf.Ceil(bounds.min.y), Mathf.Floor(bounds.max.y));
        }

        portal.position = gridPosition;
    }

    private bool IsPointerOverUi()
    {
        // UI-Klicks, zum Beispiel auf Slider oder Buttons, sollen keine Portale setzen.
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
