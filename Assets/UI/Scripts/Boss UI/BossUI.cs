using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUI : MonoBehaviour
{
    public static BossUI Instance { get; private set; }

    public GameObject healthBar;

    private Image avatar;
    private Image health;
    private Image virtualBlood;
    private TMP_Text bossName;

    private Animator animator;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        avatar = healthBar.transform.GetChild(0).GetComponent<Image>();
        health = healthBar.transform.GetChild(1).GetComponent<Image>();
        virtualBlood = healthBar.transform.GetChild(2).GetComponent<Image>();
        bossName = healthBar.transform.GetChild(3).GetComponent<TMP_Text>();
        animator = healthBar.GetComponent<Animator>();
    }

    public void Initialize(Sprite _avatar, string _bossName)
    {
        avatar.sprite = _avatar;
        bossName.text = _bossName;
    }

    public void ShowHealthBar()
    {
        healthBar.SetActive(true);
    }

    public void HideHealthBar()
    {
        StartCoroutine(HideHealthBarCoroutine());
    }

    IEnumerator HideHealthBarCoroutine()
    {
        animator.SetTrigger("Hide");
        yield return new WaitForSeconds(1f);
        healthBar.SetActive(false);
    }

    public void UpdateHealthBar(float fillAmount)
    {
        health.fillAmount = fillAmount;
    }

    public void ShowVirtualBlood()
    {
        virtualBlood.gameObject.SetActive(true);
    }

    public void HideVirtualBlood()
    {
        virtualBlood.gameObject.SetActive(false);
    }

    public void UpdateVirtualBlood(float fillAmount)
    {
        virtualBlood.fillAmount = fillAmount;
    }
}
