using UnityEngine;

public class Apple : MonoBehaviour
{
    // Bereich, in dem der Apfel zufaellig erscheinen darf.
    public BoxCollider2D gridArea;

    // Punkte dieses Apfels; der eigentliche Gesamt-Score liegt im ScoreManager.
    public int score;

    // Radius, mit dem geprueft wird, ob an einer Spawn-Position schon etwas liegt.
    public float collisionCheckRadius = 0.25f;

    // Auf diesen Objekten darf der Apfel nicht erscheinen.
    private readonly string[] _blockedSpawnTags = { "Wall", "Obstacle", "Portal", "Player" };

    private void Start()
    {
        // Beim Start wird der Apfel direkt auf ein freies Feld gesetzt.
        RandomizePosition();
        score = 0;
    }

    private void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;

        // Es werden mehrere zufaellige Positionen probiert, bis ein freies Feld gefunden wird.
        for (int i = 0; i < 100; i++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            // Die Position wird gerundet, damit der Apfel genau auf dem Snake-Raster liegt.
            Vector3 spawnPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);

            if (!IsBlockedSpawnPosition(spawnPosition))
            {
                // Nur freie Positionen werden als neue Apfelposition uebernommen.
                transform.position = spawnPosition;
                return;
            }
        }
    }

    private bool IsBlockedSpawnPosition(Vector3 position)
    {
        // Alle Collider in der Naehe der moeglichen Spawn-Position werden gesucht.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, collisionCheckRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            // Der eigene Collider des Apfels zaehlt nicht als Hindernis.
            if (colliders[i].gameObject == gameObject)
            {
                continue;
            }

            for (int j = 0; j < _blockedSpawnTags.Length; j++)
            {
                // Wenn dort Wand, Portal oder Schlange liegt, ist das Feld blockiert.
                if (colliders[i].CompareTag(_blockedSpawnTags[j]))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Wenn die Schlange den Apfel frisst, wird ein neuer Apfel gespawnt.
            RandomizePosition();
            score = score + 1;
        }
    }
}
