using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal targetPortal; // Referenz auf das Zielportal

    public Vector3 TeleportPosition()
    {
        return targetPortal.transform.position; // Gibt die Position des Zielportals zurück
    }
}