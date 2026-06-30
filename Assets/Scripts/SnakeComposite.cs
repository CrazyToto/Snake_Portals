using System.Collections.Generic;
using UnityEngine;

// Composite im Entwurfsmuster Kompositum:
// Die ganze Schlange besteht aus Kopf und Segmenten.
public class SnakeComposite : ISnakePart
{
    // Alle Teile der Schlange: zuerst der Kopf, danach die Koerpersegmente.
    private readonly List<ISnakePart> _parts = new List<ISnakePart>();

    public Transform Transform
    {
        // Die Position der ganzen Schlange ist die Position des Kopfes.
        get { return _parts[0].Transform; }
    }

    public int Count
    {
        // Anzahl aller Schlangenteile.
        get { return _parts.Count; }
    }

    public void AddPart(ISnakePart part)
    {
        // Neues Teil wird hinten an die Schlange angehaengt.
        _parts.Add(part);
    }

    public ISnakePart GetLastPart()
    {
        // Das letzte Teil wird gebraucht, wenn die Schlange wachsen soll.
        return _parts[_parts.Count - 1];
    }

    public void MoveTo(Vector3 position)
    {
        // Offset beschreibt, wie weit der Kopf verschoben wird.
        Vector3 offset = position - Transform.position;

        // Alle Teile werden um denselben Offset verschoben.
        for (int i = 0; i < _parts.Count; i++)
        {
            _parts[i].MoveTo(_parts[i].Transform.position + offset);
        }
    }

    public void MoveForward(Vector2 direction)
    {
        // Jedes Segment geht auf die vorherige Position seines Vorgaengers.
        for (int i = _parts.Count - 1; i > 0; i--)
        {
            _parts[i].MoveTo(_parts[i - 1].Transform.position);
        }

        Vector3 headPosition = Transform.position;

        // Der Kopf geht ein Rasterfeld in die aktuelle Richtung.
        Vector3 nextPosition = new Vector3(
            Mathf.Round(headPosition.x) + direction.x,
            Mathf.Round(headPosition.y) + direction.y,
            0.0f
        );

        _parts[0].MoveTo(nextPosition);
    }

    public void RemoveBodySegments()
    {
        // Nur die Koerpersegmente werden entfernt, der Kopf bleibt erhalten.
        for (int i = 1; i < _parts.Count; i++)
        {
            _parts[i].Remove();
        }

        // Danach wird die Liste wieder nur mit dem Kopf aufgebaut.
        ISnakePart head = _parts[0];
        _parts.Clear();
        _parts.Add(head);
    }

    public void Remove()
    {
        // Entfernt alle Teile der Schlange.
        for (int i = 0; i < _parts.Count; i++)
        {
            _parts[i].Remove();
        }

        _parts.Clear();
    }
}
