using UnityEngine;

public class InventoryToolbar : MonoBehaviour
{
    public void PlayEffect()
    {
        AudioManager.Instance.PlayEffect("PLAYER", "Opening/Closing Inventory");
    }
}
