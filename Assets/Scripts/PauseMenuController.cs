using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    private static PauseMenuController _instance;

    private GameObject _pausePanel;
    private Text _statusText;
    private Button _pauseButton;
    private Button _resumeButton;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreatePauseMenu()
    {
        if (_instance != null)
        {
            return;
        }

        GameObject controllerObject = new GameObject("PauseMenuController");
        _instance = controllerObject.AddComponent<PauseMenuController>();
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        IsPaused = false;
        Time.timeScale = 1.0f;
        BuildPauseMenu();
    }

    private void BuildPauseMenu()
    {
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null)
        {
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        GameObject canvasObject = new GameObject("Pause Menu Canvas");
        canvasObject.transform.SetParent(transform, false);

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.matchWidthOrHeight = 0.5f;

        canvasObject.AddComponent<GraphicRaycaster>();
        EnsureEventSystem();

        Button openButton = CreateButton(canvasObject.transform, font, "Pause", new Vector2(150.0f, 54.0f));
        RectTransform openRect = openButton.GetComponent<RectTransform>();
        openRect.anchorMin = new Vector2(0.0f, 1.0f);
        openRect.anchorMax = new Vector2(0.0f, 1.0f);
        openRect.pivot = new Vector2(0.0f, 1.0f);
        openRect.anchoredPosition = new Vector2(18.0f, -18.0f);
        openButton.onClick.AddListener(ToggleMenu);

        _pausePanel = CreatePanel(canvasObject.transform);
        _pausePanel.SetActive(false);

        Text titleText = CreateText(_pausePanel.transform, font, "Pausenmenue", 42, TextAnchor.MiddleCenter);
        RectTransform titleRect = titleText.GetComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0.0f, 115.0f);
        titleRect.sizeDelta = new Vector2(420.0f, 60.0f);

        _statusText = CreateText(_pausePanel.transform, font, "Spiel laeuft", 28, TextAnchor.MiddleCenter);
        RectTransform statusRect = _statusText.GetComponent<RectTransform>();
        statusRect.anchoredPosition = new Vector2(0.0f, 62.0f);
        statusRect.sizeDelta = new Vector2(420.0f, 42.0f);

        _pauseButton = CreateButton(_pausePanel.transform, font, "Pause", new Vector2(250.0f, 58.0f));
        RectTransform pauseRect = _pauseButton.GetComponent<RectTransform>();
        pauseRect.anchoredPosition = new Vector2(0.0f, 0.0f);
        _pauseButton.onClick.AddListener(PauseGame);

        _resumeButton = CreateButton(_pausePanel.transform, font, "Fortsetzen", new Vector2(250.0f, 58.0f));
        RectTransform resumeRect = _resumeButton.GetComponent<RectTransform>();
        resumeRect.anchoredPosition = new Vector2(0.0f, -72.0f);
        _resumeButton.onClick.AddListener(ResumeGame);

        Button closeButton = CreateButton(_pausePanel.transform, font, "Schliessen", new Vector2(250.0f, 58.0f));
        RectTransform closeRect = closeButton.GetComponent<RectTransform>();
        closeRect.anchoredPosition = new Vector2(0.0f, -144.0f);
        closeButton.onClick.AddListener(CloseMenu);

        UpdateMenuState();
    }

    private void EnsureEventSystem()
    {
        if (EventSystem.current != null)
        {
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.transform.SetParent(transform, false);
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<InputSystemUIInputModule>();
    }

    private GameObject CreatePanel(Transform parent)
    {
        GameObject panelObject = new GameObject("Pause Panel");
        panelObject.transform.SetParent(parent, false);

        RectTransform rectTransform = panelObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(470.0f, 390.0f);
        rectTransform.anchoredPosition = Vector2.zero;

        Image image = panelObject.AddComponent<Image>();
        image.color = new Color(0.05f, 0.07f, 0.09f, 0.92f);

        return panelObject;
    }

    private Button CreateButton(Transform parent, Font font, string label, Vector2 size)
    {
        GameObject buttonObject = new GameObject(label + " Button");
        buttonObject.transform.SetParent(parent, false);

        RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = size;

        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color(0.12f, 0.39f, 0.32f, 0.95f);

        Button button = buttonObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.12f, 0.39f, 0.32f, 0.95f);
        colors.highlightedColor = new Color(0.17f, 0.53f, 0.43f, 1.0f);
        colors.pressedColor = new Color(0.08f, 0.25f, 0.22f, 1.0f);
        colors.disabledColor = new Color(0.25f, 0.25f, 0.25f, 0.55f);
        button.colors = colors;

        Text buttonText = CreateText(buttonObject.transform, font, label, 28, TextAnchor.MiddleCenter);
        RectTransform textRect = buttonText.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        return button;
    }

    private Text CreateText(Transform parent, Font font, string text, int fontSize, TextAnchor alignment)
    {
        GameObject textObject = new GameObject(text + " Text");
        textObject.transform.SetParent(parent, false);

        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300.0f, 50.0f);

        Text uiText = textObject.AddComponent<Text>();
        uiText.font = font;
        uiText.text = text;
        uiText.fontSize = fontSize;
        uiText.alignment = alignment;
        uiText.color = Color.white;

        return uiText;
    }

    private void ToggleMenu()
    {
        _pausePanel.SetActive(!_pausePanel.activeSelf);
    }

    private void CloseMenu()
    {
        _pausePanel.SetActive(false);
    }

    private void PauseGame()
    {
        SetPaused(true);
    }

    private void ResumeGame()
    {
        SetPaused(false);
        CloseMenu();
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0.0f : 1.0f;
        UpdateMenuState();
    }

    private void UpdateMenuState()
    {
        if (_statusText != null)
        {
            _statusText.text = IsPaused ? "Spiel pausiert" : "Spiel laeuft";
        }

        if (_pauseButton != null)
        {
            _pauseButton.interactable = !IsPaused;
        }

        if (_resumeButton != null)
        {
            _resumeButton.interactable = IsPaused;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            IsPaused = false;
            Time.timeScale = 1.0f;
            _instance = null;
        }
    }
}
