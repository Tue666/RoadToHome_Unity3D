using UnityEngine;

public class SpaceBag : MonoBehaviour
{
    //private int space = 400;
    private int[] ammos = { 400, 400 }; // [5mm], [7mm]

    public int GetAmmo(int ammoType)
    {
        return ammos[ammoType];
    }

    public void SetAmmo(int ammoType, int ammoAmount)
    {
        ammos[ammoType] += ammoAmount;
    }
}
