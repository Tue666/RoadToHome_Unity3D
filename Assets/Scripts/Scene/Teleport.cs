using UnityEngine;

public class Teleport : MonoBehaviour
{
    public int currentIndex;
    public Animator crossFadeAnimator = null;

    void Start()
    {
        SceneLoader.crossFadeAnimator = crossFadeAnimator;
        SceneLoader.currentScene = currentIndex;

        PlayerManager.Instance.player.transform.position = gameObject.transform.position + (Vector3.forward * 10);
        PlayerManager.Instance.player.transform.rotation = gameObject.transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.player)
        {
            AudioManager.Instance.PlayEffect("PLAYER", "Opening/Closing Inventory");
            UIManager.Instance.ShowView("Select Map UI");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.player)
        {
            AudioManager.Instance.PlayEffect("PLAYER", "Opening/Closing Inventory");
            UIManager.Instance.HideView("Select Map UI");
        }
    }
}
