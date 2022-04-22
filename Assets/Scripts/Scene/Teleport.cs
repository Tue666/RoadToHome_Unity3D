using UnityEngine;

public class Teleport : MonoBehaviour
{
    public int currentIndex;
    public Transform startPosition;
    public Animator crossFadeAnimator = null;

    void Start()
    {
        SceneLoader.crossFadeAnimator = crossFadeAnimator;
        SceneLoader.currentScene = currentIndex;

        PlayerManager.Instance.player.transform.position = startPosition.position;
        PlayerManager.Instance.player.transform.rotation = startPosition.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.player)
        {
            AudioManager.Instance.PlayEffect("PLAYER", "Enter Teleport");
            UIManager.Instance.ShowView("Select Map UI");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.player)
        {
            AudioManager.Instance.PlayEffect("PLAYER", "Enter Teleport");
            UIManager.Instance.HideView("Select Map UI");
        }
    }
}
