using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [SerializeField] private Hand[] hands;
    [SerializeField] private HandController handController;
    [SerializeField] private Gun[] guns;
    [SerializeField] private GunController gunController;

    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private string previousWeaponName;

    public static Transform currentWeapon;
    public static Animator currentAnimator;
    public static string isActivating = "HAND";

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        foreach (Hand hand in hands)
        {
            if (hand != null)
                handDictionary.Add(hand.handName, hand);
        }
        foreach (Gun gun in guns)
        {
            if (gun != null)
                gunDictionary.Add(gun.gunName, gun);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PrepareWeapon("GUN", "Rifle 03");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PrepareWeapon("GUN", "Rifle 05");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PrepareWeapon("HAND", "Knife");
        }
    }

    void PrepareWeapon(string type, string name)
    {
        switch (isActivating)
        {
            case "GUN":
                // Cancel reload if it's happening
                gunController.CancelReload();
                // Cancel scope if it's happening
                StartCoroutine(gunController.OnUnScoped());
                break;
            default:
                break;
        }
        if (previousWeaponName == name)
        {
            WeaponChange("HAND", "Hand");
            return;
        }
        WeaponChange(type, name);
    }

    void WeaponChange(string type, string name)
    {
        switch (type)
        {
            case "GUN":
                if (Helpers.ContainsKeyButValueNotNull(gunDictionary, name))
                    gunController.GunChange(gunDictionary[name]);
                break;
            case "HAND":
                if (Helpers.ContainsKeyButValueNotNull(handDictionary, name))
                    handController.HandChange(handDictionary[name]);
                break;
            default:
                break;
        }
        previousWeaponName = name;
        MainUI.Instance.SwitchCrosshair(isActivating);
    }
}
