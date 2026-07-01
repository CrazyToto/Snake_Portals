using UnityEngine;
using UnityEngine.UI;

public class OhnePortal : MonoBehaviour
{
    private Portal[] _portals;
    public GameObject[] middleWalls;
    public GameObject[] centralWalls;
    private PortalMousePlacer _portalMousePlacer;
    public BoxCollider2D portalBlocker;

    private void Start()
    {
        CachePortalObjects();

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(DisablePortals);
    }

    private void CachePortalObjects()
    {
        _portals = FindObjectsByType<Portal>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        _portalMousePlacer = FindFirstObjectByType<PortalMousePlacer>(FindObjectsInactive.Include);
    }

    private void DisablePortals()
    {
        if (_portals == null || _portals.Length == 0)
        {
            CachePortalObjects();
        }

        foreach (Portal portal in _portals)
        {
            if (portal != null)
            {
                portal.gameObject.SetActive(false);
            }
        }

        if (_portalMousePlacer != null)
        {
            _portalMousePlacer.gameObject.SetActive(false);
        }

        foreach (GameObject wall in middleWalls)
        {
            if (wall != null)
            {
                wall.SetActive(false);
            }
        }

        foreach (GameObject wall in centralWalls)
        {
            if (wall != null)
            {
                wall.SetActive(true);
            }
        }

        if (portalBlocker != null)
        {
            portalBlocker.enabled = false;
        }
    }
}
