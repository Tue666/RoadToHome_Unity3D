using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DroppedItemsUI : MonoBehaviour
{
    public static DroppedItemsUI Instance { get; private set; }

    public Transform dropTable;
    public GameObject itemTemplate;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void ClearDropTable()
    {
        foreach (Transform droppedItem in dropTable)
        {
            Destroy(droppedItem.gameObject);
        }
    }

    public void InitializeDropTable(ref List<TableItem> tableItems)
    {
        List<TableItem> refTableItems = tableItems; // assign refTableItems to address of tableItems
        ClearDropTable();
        foreach (TableItem tableItem in tableItems)
        {
            GameObject itemObject = Instantiate(itemTemplate, dropTable);
            Image itemIcon = itemObject.transform.GetChild(0).GetComponent<Image>();
            TMP_Text itemName = itemObject.transform.GetChild(1).GetComponent<TMP_Text>();
            TMP_Text itemQuantity = itemObject.transform.GetChild(2).GetComponent<TMP_Text>();

            itemIcon.sprite = tableItem.item.icon;
            itemName.text = tableItem.item.itemName;
            itemQuantity.text = "x" + tableItem.quantity;

            Button itemObjButton = itemObject.GetComponent<Button>();
            itemObjButton.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayEffect("PLAYER", "Collect Item");
                InventoryManager.Instance.InsertItem(tableItem);
                refTableItems.Remove(tableItem); // refTableItems will solved can't use ref in lambda expression issue
                Destroy(itemObject);
            });
        }
    }
}
