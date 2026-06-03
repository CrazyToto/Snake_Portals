using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class OpenSnakeSceneOnLoad
{
    private const string MainScenePath = "Assets/Scenes/Snake.unity";
    private const string SessionKey = "SnakePortals.OpenedMainScene";

    static OpenSnakeSceneOnLoad()
    {
        EditorApplication.delayCall += OpenMainSceneWhenProjectStartsEmpty;
    }

    private static void OpenMainSceneWhenProjectStartsEmpty()
    {
        if (SessionState.GetBool(SessionKey, false))
        {
            return;
        }

        SessionState.SetBool(SessionKey, true);

        Scene activeScene = SceneManager.GetActiveScene();
        if (!string.IsNullOrEmpty(activeScene.path))
        {
            return;
        }

        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(MainScenePath) == null)
        {
            return;
        }

        EditorSceneManager.OpenScene(MainScenePath);
    }
}
