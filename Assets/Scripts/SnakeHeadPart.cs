using UnityEngine;

// Leaf im Kompositum: Der Kopf ist ein einzelnes Schlangenteil.
public class SnakeHeadPart : ISnakePart
{
    // Transform des Snake-GameObjects, also der Kopf der Schlange.
    private readonly Transform _transform;

    public SnakeHeadPart(Transform transform)
    {
        // Der Kopf bekommt seinen Transform von Snake.cs uebergeben.
        _transform = transform;
    }

    public Transform Transform
    {
        // Ueber das Interface kann das Kompositum auf die Position zugreifen.
        get { return _transform; }
    }

    public void MoveTo(Vector3 position)
    {
        // Der Kopf wird direkt an die neue Position gesetzt.
        _transform.position = position;
    }

    public void Remove()
    {
        // Der Kopf gehoert zum Snake-GameObject und wird nicht zerstoert.
    }
}
