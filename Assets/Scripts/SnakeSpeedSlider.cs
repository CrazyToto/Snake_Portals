using UnityEngine;
using UnityEngine.UI;

public class SnakeSpeedSlider : MonoBehaviour
{
    // Die Schlange, deren Geschwindigkeit veraendert werden soll.
    public Snake snake;

    // Der UI-Slider, der die Geschwindigkeit steuert.
    public Slider slider;

    private void Awake()
    {
        // Wenn kein Slider eingetragen ist, wird der Slider auf demselben GameObject gesucht.
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    private void Start()
    {
        // Ohne Slider oder Schlange kann keine Verbindung aufgebaut werden.
        if (slider == null || snake == null)
        {
            return;
        }

        // Der Slider zeigt am Anfang die aktuelle Geschwindigkeit der Schlange.
        slider.value = snake.movesPerSecond;

        // Jede Slider-Aenderung ruft die Methode der Schlange auf.
        slider.onValueChanged.AddListener(snake.SetMovesPerSecond);
    }

    private void OnDestroy()
    {
        // Beim Loeschen wird der Listener wieder entfernt.
        if (slider != null && snake != null)
        {
            slider.onValueChanged.RemoveListener(snake.SetMovesPerSecond);
        }
    }
}
