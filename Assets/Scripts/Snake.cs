using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    // Aktuelle Bewegungsrichtung der Schlange.
    private Vector2 _direction = Vector2.right;

    // Die ganze Schlange wird als Kompositum verwaltet.
    private SnakeComposite _snake;

    // Zaehlt die Zeit seit dem letzten Schritt.
    private float _moveTimer;

    // Prefab fuer neue Koerpersegmente.
    public Transform segmentPrefab;

    // Startlaenge der Schlange inklusive Kopf.
    public int initialSize = 4;

    // Anzahl der Raster-Schritte pro Sekunde.
    public float movesPerSecond = 5.0f;

    private void Start()
    {
        // Beim Spielstart wird die Schlange aufgebaut.
        ResetState();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        // Falls keine Tastatur vorhanden ist, wird keine Eingabe verarbeitet.
        if (keyboard == null)
        {
            return;
        }

        // Die Schlange darf nicht direkt in die Gegenrichtung fahren.
        if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame)
        {
            if (_direction != Vector2.down)
            {
                _direction = Vector2.up;
            }
        }
        else if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)
        {
            if (_direction != Vector2.up)
            {
                _direction = Vector2.down;
            }
        }
        else if (keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame)
        {
            if (_direction != Vector2.left)
            {
                _direction = Vector2.right;
            }
        }
        else if (keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame)
        {
            if (_direction != Vector2.right)
            {
                _direction = Vector2.left;
            }
        }
    }

    private void FixedUpdate()
    {
        // Solange die Schlange noch nicht aufgebaut ist, kann sie sich nicht bewegen.
        if (_snake == null)
        {
            return;
        }

        // Die Bewegung laeuft zeitgesteuert, damit der Slider die Geschwindigkeit regeln kann.
        _moveTimer += Time.fixedDeltaTime;
        float moveInterval = GetMoveInterval();

        if (_moveTimer >= moveInterval)
        {
            // Das Kompositum bewegt Kopf und Segmente gemeinsam weiter.
            _snake.MoveForward(_direction);
            _moveTimer -= moveInterval;
        }
    }

    public void SetMovesPerSecond(float value)
    {
        // Mindestgeschwindigkeit ist 1, damit die Schlange nie komplett stehen bleibt.
        movesPerSecond = Mathf.Max(1.0f, value);
    }

    private float GetMoveInterval()
    {
        // Aus Schritten pro Sekunde wird die Wartezeit zwischen zwei Schritten berechnet.
        return 1.0f / Mathf.Max(1.0f, movesPerSecond);
    }

    private void Grow()
    {
        // Ein neues Segment entsteht an der Position des letzten Schlangenteils.
        Transform segment = Instantiate(segmentPrefab);
        segment.position = _snake.GetLastPart().Transform.position;

        // Das neue Segment wird als Leaf zum Kompositum hinzugefuegt.
        _snake.AddPart(new SnakeSegmentPart(segment));
    }

    private void ResetState()
    {
        if (_snake == null)
        {
            // Beim ersten Reset wird das Kompositum mit dem Kopf erstellt.
            _snake = new SnakeComposite();
            _snake.AddPart(new SnakeHeadPart(transform));
        }
        else
        {
            // Bei spaeteren Resets werden nur die Koerpersegmente entfernt.
            _snake.RemoveBodySegments();
        }

        // Startposition und Startrichtung der Schlange.
        _snake.MoveTo(new Vector3(-12, 0, 0));
        _direction = Vector2.right;

        // Nach einem Reset darf die Schlange sofort wieder loslaufen.
        _moveTimer = GetMoveInterval();

        // Die Startsegmente werden hinter dem Kopf aufgebaut.
        for (int i = 1; i < initialSize; i++)
        {
            Transform segment = Instantiate(segmentPrefab);
            segment.position = transform.position;
            _snake.AddPart(new SnakeSegmentPart(segment));
        }
    }

    private void ResetScore()
    {
        // Der Score wird bei Kollision mit einem Hindernis zurueckgesetzt.
        ScoreManager.scoreCount = 0;
    }

    private void Teleport()
    {
        // Bei einer Wand wird die Schlange auf die gegenueberliegende Seite gesetzt.
        if (_direction == Vector2.left || _direction == Vector2.right)
        {
            _snake.MoveTo(new Vector3(transform.position.x * -1, transform.position.y, 0.0f));
        }
        else
        {
            _snake.MoveTo(new Vector3(transform.position.x, transform.position.y * -1, 0.0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple"))
        {
            // Apfel eingesammelt: Schlange waechst und Score steigt.
            Grow();
            ScoreManager.scoreCount += 1;
        }
        else if (other.CompareTag("Obstacle"))
        {
            // Hindernis getroffen: Schlange und Score werden zurueckgesetzt.
            ResetState();
            ResetScore();
        }
        else if (other.CompareTag("Wall"))
        {
            // Wand bedeutet in diesem Spiel Teleport auf die andere Seite.
            Teleport();
        }
        else if (other.CompareTag("Portal"))
        {
            Portal portal = other.GetComponent<Portal>();

            if (portal != null)
            {
                // Beim Portal kommt die Schlange ein Feld hinter dem Zielportal heraus.
                _snake.MoveTo(portal.TeleportPosition(_direction));
            }
        }
    }
}
