using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemSO item;
    public int quantity;

    public InventoryItem(ItemSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public InventoryItem GetItem(ItemSO item)
    {
        return inventoryItems.Where(inventoryItem => inventoryItem.item == item).SingleOrDefault();
    }

    public void InsertItem(TableItem tableItem)
    {
        InventoryItem item = inventoryItems.Where(itemInventory => itemInventory.item == tableItem.item).FirstOrDefault();
        if (item != null)
        {
            if (!item.item.isUnique)
                item.quantity += tableItem.quantity;
        }
        else
            inventoryItems.Add(new InventoryItem(tableItem.item, tableItem.quantity));

        // Refresh inventory if it's showing
        if (UIManager.Instance.isShowing("Character UI"))
            CharacterUI.Instance.InitializeInventory();
    }

    public void EditItem(InventoryItem newItem)
    {
        InventoryItem item = inventoryItems.Where(inventoryItem => inventoryItem.item == newItem.item).SingleOrDefault();
        item.quantity = newItem.quantity;

        // Refresh inventory if it's showing
        if (UIManager.Instance.isShowing("Character UI"))
            CharacterUI.Instance.InitializeInventory();
    }
}
