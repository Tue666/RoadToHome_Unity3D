using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    public static CharacterUI Instance { get; private set; }

    public Transform inventory;
    public GameObject itemTemplate;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void ClearInventory()
    {
        foreach (Transform inventoryItem in inventory)
        {
            Destroy(inventoryItem.gameObject);
        }
    }

    public void InitializeInventory()
    {
        ClearInventory();

        foreach (InventoryItem inventoryItem in InventoryManager.Instance.inventoryItems)
        {
            if (inventoryItem.quantity == 0) continue;

            GameObject itemObject = Instantiate(itemTemplate, inventory);
            Image itemIcon = itemObject.transform.GetChild(0).GetComponent<Image>();
            TMP_Text itemQuantity = itemObject.transform.GetChild(1).GetComponent<TMP_Text>();

            itemIcon.sprite = inventoryItem.item.icon;
            itemQuantity.text = inventoryItem.quantity.ToString();

            Button itemObjButton = itemObject.GetComponent<Button>();
            itemObjButton.onClick.AddListener(() =>
            {
                ItemSO item = inventoryItem.item;
                string content = string.Format("[{0}]\nRanking: [{1}]\n«««« ▼ »»»»\n\n{2}\n\nSell price: {3:#,##0} coin",
                    item.itemName, item.rank, item.description, item.sellPrice);
                SystemWindowManager.Instance.ShowWindowSystem(item.rank.ToString(), content, "MOUSE", false);
            });
        }
    }
}
