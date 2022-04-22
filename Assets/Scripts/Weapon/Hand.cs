using UnityEngine;

public class Hand : MonoBehaviour
{
    public string handName;
    public int damage = 10;
    public float range = 5;
    public float attackRate = 1f;
    public string attackClipName;

    public Animator animator;

    #region Events
    public void PlayRunEffect()
    {
        AudioManager.Instance.PlayEffect("PLAYER", "Run");
    }
    #endregion
}
