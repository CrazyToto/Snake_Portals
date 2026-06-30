using UnityEngine;

public class Portal : MonoBehaviour
{
    // Das Portal, zu dem dieses Portal fuehrt.
    public Portal targetPortal;

    public Vector3 TeleportPosition(Vector2 exitDirection)
    {
        // Ziel ist nicht die Portalmitte, sondern ein Feld hinter dem Zielportal.
        // Dadurch wird die Schlange nicht sofort wieder zurueck teleportiert.
        Vector3 targetPosition = targetPortal.transform.position;
        return new Vector3(
            targetPosition.x + exitDirection.x,
            targetPosition.y + exitDirection.y,
            targetPosition.z
        );
    }
}
