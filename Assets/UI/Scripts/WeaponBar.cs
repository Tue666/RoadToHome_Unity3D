using UnityEngine;
using UnityEngine.UI;

public class WeaponBar : MonoBehaviour
{
    [SerializeField] private Text magazine;
    [SerializeField] private Text remainingAmmo;
    [SerializeField] private Image iconWeapon;

    private bool _noMagazine = false;
    private bool _noRemaining = false;

    public bool noMagazine
    {
        get { return _noMagazine; }
        set { _noMagazine = value; }
    }

    public bool noRemaining
    {
        get { return _noRemaining; }
        set { _noRemaining = value; }
    }

    public void UpdateMagazine(int newValue)
    {
        magazine.text = newValue.ToString();
        if (!_noMagazine)
        {
            magazine.fontStyle = FontStyle.Normal;
            magazine.color = Color.white;
            _noMagazine = true;
        }
        if (magazine.text == "0" && _noMagazine)
        {
            magazine.fontStyle = FontStyle.Bold;
            magazine.color = Color.red;
            _noMagazine = false;
        }
    }

    public void UpdateRemainingAmmo(int newValue)
    {
        remainingAmmo.text = newValue.ToString();
        if (!_noRemaining)
        {
            remainingAmmo.fontStyle = FontStyle.Normal;
            remainingAmmo.color = Color.white;
            _noRemaining = true;
        }
        if (remainingAmmo.text == "0" && _noRemaining)
        {
            remainingAmmo.fontStyle = FontStyle.Bold;
            remainingAmmo.color = Color.red;
            _noRemaining = false;
        }
    }

    public void UpdateIconWeapon(Sprite icon)
    {
        iconWeapon.sprite = icon;
    }
}
