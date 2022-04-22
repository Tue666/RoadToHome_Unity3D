using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private Hand currentHand;

    private Camera cameraFOV = null;
    private float nextAttackTime;
    private RaycastHit hit;
    private GameObject previousObject = null;
    private object hitObject = null;

    private WaitForSeconds waitAttackTakeEffect = new WaitForSeconds(0.4f);

    // Start is called before the first frame update
    void Start()
    {
        cameraFOV = PlayerManager.Instance.cameraRecoil;
        if (!currentHand.gameObject.activeSelf) currentHand.gameObject.SetActive(true);
        WeaponManager.currentWeapon = currentHand.gameObject.transform;
        WeaponManager.currentAnimator = currentHand.animator;
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
            if (hitObject == null || hit.transform.gameObject != previousObject)
            {
                if (hit.transform.tag == "Enemy")
                    hitObject = hit.transform.GetComponent<Target>();
                else if (hit.transform.tag == "Boss")
                    hitObject = hit.transform.GetComponent<Boss>();
                else
                {
                    hitObject = null;
                    yield break;
                }
                previousObject = hit.transform.gameObject;
            }
            if (hitObject != null)
            {
                if (hitObject is Target)
                    ((Target)hitObject).TakeDamage(currentHand.damage + PlayerManager.Instance.strength, hit.point);
                if (hitObject is Boss)
                    ((Boss)hitObject).TakeDamage(currentHand.damage + PlayerManager.Instance.strength, hit.point);
            }
        }
    }

    public void HandChange(Hand hand)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        AudioManager.Instance.PlayEffect("WEAPON", "Weapon Change");
        WeaponManager.currentWeapon = hand.gameObject.transform;
        WeaponManager.currentAnimator = hand.animator;
        WeaponManager.isActivating = "HAND";
        currentHand = hand;
        currentHand.gameObject.SetActive(true);
        currentHand.animator.SetTrigger("Get");
        MainUI.Instance.CurrentWeaponChanged(WeaponManager.isActivating, currentHand.handName);
    }
}
