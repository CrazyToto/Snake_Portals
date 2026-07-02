using UnityEngine;

// Leaf im Kompositum: Ein einzelnes Koerpersegment der Schlange.
public class SnakeSegmentPart : ISnakePart
{
    // Transform des einzelnen Koerpersegments.
    private readonly Transform _transform;

    public SnakeSegmentPart(Transform transform)
    {
        // Das Segment bekommt seinen Transform beim Erstellen uebergeben.
        _transform = transform;
    }

    public Transform Transform
    {
        // Ueber das Interface kann das Kompositum auf die Position zugreifen.
        get { return _transform; }
    }

    public void MoveTo(Vector3 position)
    {
        // Das Segment wird direkt an die neue Position gesetzt.
        _transform.position = position;
    }

    public void Remove()
    {
        // Koerpersegmente werden beim Reset aus der Szene geloescht.
        Object.Destroy(_transform.gameObject);
    }
}
