using UnityEngine;
using UnityEngine.UI;

public class OhnePortal : MonoBehaviour
{
    private Portal[] _portals;
    private PortalMousePlacer _portalMousePlacer;

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
    }
}
