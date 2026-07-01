using UnityEngine;
using UnityEngine.UI;

public class MitPortal : MonoBehaviour
{
    private Portal[] _portals;
    private PortalMousePlacer _portalMousePlacer;

    private void Start()
    {
        CachePortalObjects();

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(EnablePortals);
    }

    private void CachePortalObjects()
    {
        _portals = FindObjectsByType<Portal>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        _portalMousePlacer = FindFirstObjectByType<PortalMousePlacer>(FindObjectsInactive.Include);
    }

    private void EnablePortals()
    {
        if (_portals == null || _portals.Length == 0)
        {
            CachePortalObjects();
        }

        foreach (Portal portal in _portals)
        {
            if (portal != null)
            {
                portal.gameObject.SetActive(true);
            }
        }

        if (_portalMousePlacer != null)
        {
            _portalMousePlacer.gameObject.SetActive(true);
        }
    }
}
