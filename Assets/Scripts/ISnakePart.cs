using UnityEngine;

// Component im Entwurfsmuster Kompositum:
// Kopf, Segment und ganze Schlange koennen gleich behandelt werden.
public interface ISnakePart
{
    Transform Transform { get; }

    void MoveTo(Vector3 position);

    void Remove();
}
