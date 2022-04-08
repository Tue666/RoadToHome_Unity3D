using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerHideOnMouseLeave : MonoBehaviour, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        SystemWindowManager.Instance.HideSystemWindow();
    }
}
