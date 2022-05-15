using UnityEngine;

public class Teleport : MonoBehaviour
{
    public int currentIndex;
    public Transform startPosition;
    public Animator crossFadeAnimator = null;

    private float currentPrepareTime = 0f;

    void Start()
    {
        SceneLoader.crossFadeAnimator = crossFadeAnimator;
        SceneLoader.currentScene = currentIndex;

        PlayerManager.Instance.playerObj.transform.rotation = startPosition.rotation;
    }

    void Update()
    {
        PreparePosition();
    }

    void PreparePosition()
    {
        if (currentPrepareTime <= 0.5f)
        {
            currentPrepareTime += Time.deltaTime;
            PlayerManager.Instance.playerObj.transform.position = startPosition.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.playerObj)
        {
            AudioManager.Instance.PlayEffect("PLAYER", "Enter Teleport");
            UIManager.Instance.ShowView("Select Map UI");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.playerObj)
        {
            AudioManager.Instance.PlayEffect("PLAYER", "Enter Teleport");
            UIManager.Instance.HideView("Select Map UI");
        }
    }
}
