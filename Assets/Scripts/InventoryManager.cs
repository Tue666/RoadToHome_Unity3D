using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
class InventoryItemData
{
    public string id;
    public int quantity;

    public InventoryItemData(InventoryItem inventoryItem)
    {
        this.id = inventoryItem.item.id;
        this.quantity = inventoryItem.quantity;
    }
}

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
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SaveInventory()
    {
        List<InventoryItemData> inventoryData = new List<InventoryItemData>();
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            inventoryData.Add(new InventoryItemData(inventoryItem));
        }
        SaveSystem.Save("inventory", inventoryData);
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.GetString("GAME_MODE") == "continue")
        {
            List<InventoryItemData> inventoryData = SaveSystem.Load("inventory") as List<InventoryItemData>;
            foreach (InventoryItemData inventoryItemData in inventoryData)
            {
                inventoryItems.Add(new InventoryItem(GameManager.Instance.FindItemById(inventoryItemData.id), inventoryItemData.quantity));
            }
        }
        else
            inventoryItems.Add(new InventoryItem(GameManager.Instance.FindItemById("W1"), 30));
    }

    public bool ItemExists(ItemSO _item)
    {
        InventoryItem item = inventoryItems.Where(inventoryItem => inventoryItem.item == _item).FirstOrDefault();
        return item != null;
    }

    public InventoryItem GetItem(ItemSO item)
    {
        return inventoryItems.Where(inventoryItem => inventoryItem.item == item).SingleOrDefault();
    }

    public void InsertItem(TableItem tableItem)
    {
        InventoryItem item = inventoryItems.Where(inventoryItem => inventoryItem.item == tableItem.item).FirstOrDefault();
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
        if (tableItem.item.id == "W1" || tableItem.item.id == "W2")
            MainUI.Instance.UpdateRemainingAmmo();
        if (tableItem.item.id == "P1" || tableItem.item.id == "P2")
            MainUI.Instance.UpdateRemainingPotion();
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
