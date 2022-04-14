using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    public static int currentScene;
    public static Animator crossFadeAnimator;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void LoadMap(int sceneIndex)
    {
        StartCoroutine(LoadMapCoroutine(sceneIndex));
    }

    IEnumerator LoadMapCoroutine(int sceneIndex)
    {
        crossFadeAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneIndex);
    }
}
