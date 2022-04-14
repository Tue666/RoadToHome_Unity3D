using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private Hand currentHand;
    [SerializeField] private Hand handEquipped;

    private Camera cameraFOV = null;
    private float nextAttackTime;
    private RaycastHit hit;

    private WaitForSeconds waitAttackTakeEffect = new WaitForSeconds(0.4f);

    // Start is called before the first frame update
    void Start()
    {
        cameraFOV = PlayerManager.Instance.cameraRecoil;
        if (!currentHand.gameObject.activeSelf) currentHand.gameObject.SetActive(true);
        WeaponManager.currentWeapon = currentHand.gameObject.transform;
        WeaponManager.currentAnimator = currentHand.animator;
        MainUI.Instance.InitExtraWeaponBar(handEquipped);
    }

    // Update is called once per frame
    void Update()
    {
        if (WeaponManager.isActivating == "HAND")
        {
            // Must lock cursor to do the following
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                if (Input.GetButton("Fire1") && Time.time >= nextAttackTime)
                {
                    nextAttackTime = Time.time + currentHand.attackRate;
                    StartCoroutine(Attack("One"));
                }
                if (Input.GetButton("Fire2") && Time.time >= nextAttackTime)
                {
                    nextAttackTime = Time.time + currentHand.attackRate;
                    StartCoroutine(Attack("Two"));
                }
            }
        }
    }

    IEnumerator Attack(string type)
    {
        AudioManager.Instance.PlayEffect("WEAPON", currentHand.attackClipName);
        currentHand.animator.SetTrigger("Attack" + type);
        yield return waitAttackTakeEffect;
        if (Physics.Raycast(cameraFOV.transform.position, cameraFOV.transform.forward, out hit, currentHand.range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(currentHand.damage, hit.point);
            }
        }
    }

    public void HandChange(Hand hand)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        AudioManager.Instance.PlayEffect("WEAPON", "Weapon Change", true);
        WeaponManager.currentWeapon = hand.gameObject.transform;
        WeaponManager.currentAnimator = hand.animator;
        WeaponManager.isActivating = "HAND";
        currentHand = hand;
        currentHand.gameObject.SetActive(true);
        currentHand.animator.SetTrigger("Get");
        MainUI.Instance.CurrentWeaponChanged(WeaponManager.isActivating, currentHand.handName);
    }
}
