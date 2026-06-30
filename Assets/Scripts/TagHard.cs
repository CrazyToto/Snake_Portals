using UnityEngine;
using UnityEngine.UI;

public class TagHard : MonoBehaviour
{
    // Alle Objekte, die durch diesen Button zu Hindernissen werden.
    public GameObject[] objectsToChange;

    private void Start()
    {
        // Der tag-hard Button ruft beim Klicken ChangeTagsToObstacle auf.
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ChangeTagsToObstacle);
    }

    private void ChangeTagsToObstacle()
    {
        foreach (GameObject obj in objectsToChange)
        {
            // Diese Objekte setzen die Schlange beim Beruehren zurueck.
            obj.tag = "Obstacle";
        }
    }
}
