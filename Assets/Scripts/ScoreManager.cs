using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Textfeld fuer den aktuellen Score.
    public Text scoreText;

    // Aktueller Score, statisch damit andere Skripte direkt darauf zugreifen koennen.
    public static int scoreCount;

    // Textfeld fuer den Highscore.
    public Text highscoreText;

    // Gespeicherter Highscore.
    public static int highscoreCount;

    private void Start()
    {
        // Der Highscore wird aus den Unity PlayerPrefs geladen.
        highscoreCount = PlayerPrefs.GetInt("Highscore", 0);
    }

    private void Update()
    {
        // Wenn der aktuelle Score groesser ist, wird er als neuer Highscore gespeichert.
        if (scoreCount > highscoreCount)
        {
            highscoreCount = scoreCount;
            PlayerPrefs.SetInt("Highscore", highscoreCount);
        }

        // Die Textfelder werden jedes Frame aktualisiert.
        scoreText.text = "" + scoreCount;
        highscoreText.text = "" + highscoreCount;
    }
}
