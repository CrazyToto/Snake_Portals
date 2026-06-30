using UnityEngine;
using UnityEngine.UI;

public class TagEasy : MonoBehaviour
{
    // Alle Objekte, die durch diesen Button zu Waenden werden.
    public GameObject[] objectsToChange;

    private void Start()
    {
        // Der tag-easy Button ruft beim Klicken ChangeTagsToWall auf.
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ChangeTagsToWall);
    }

    private void ChangeTagsToWall()
    {
        foreach (GameObject obj in objectsToChange)
        {
            // Diese Objekte verhalten sich danach wie Waende.
            obj.tag = "Wall";
        }
    }
}
