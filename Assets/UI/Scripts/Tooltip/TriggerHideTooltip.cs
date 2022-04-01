using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerHideTooltip : MonoBehaviour, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
