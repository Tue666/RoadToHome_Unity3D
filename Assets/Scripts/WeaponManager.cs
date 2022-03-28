using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Gun[] guns;
    [SerializeField] private GunController gunController;

    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    public static Transform currentWeapon;
    public static Animator currentAnimator;
    public static string isActivating = "GUN";

    // Start is called before the first frame update
    void Start()
    {
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
            WeaponChange("GUN", "Rifle 05");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponChange("GUN", "Rifle 03");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponChange("GUN", "Rifle 02");
        }
    }

    void WeaponChange(string type, string name)
    {
        switch (type)
        {
            case "GUN":
                if (Helpers.ContainsKeyButValueNotNull(gunDictionary, name))
                    gunController.GunChange(gunDictionary[name]);
                break;
            default:
                break;
        }
    }
}
