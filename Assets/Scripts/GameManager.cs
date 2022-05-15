using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Pause
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ItemSO[] items;

    private bool isConfinedCursor = false;
    private bool isFreeCursor = false;

    private bool isPotionOneUsing = false;
    private bool isPotionTwoUsing = false;

    private Dictionary<string, ItemSO> itemsDictionary = new Dictionary<string, ItemSO>();

    IEnumerator pauseGameCoroutine;

    void InitDictionary()
    {
        foreach (ItemSO item in items)
        {
            if (item != null)
                itemsDictionary.Add(item.id, item);
        }
    }
    void ConfinedCursor()
    {
        if (!isConfinedCursor) LockCursorChanged(CursorLockMode.Confined);
        else LockCursorChanged(CursorLockMode.Locked);
        isConfinedCursor = !isConfinedCursor;
    }
    void FreeCursor()
    {
        if (!isFreeCursor) LockCursorChanged(CursorLockMode.None);
        else LockCursorChanged(CursorLockMode.Locked);
        isFreeCursor = !isFreeCursor;
    }
    public ItemSO FindItemById(string id)
    {
        if (!itemsDictionary.ContainsKey(id)) return null;
        return itemsDictionary[id];
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        InitDictionary();
        LockCursorChanged(CursorLockMode.Locked);
    }

    void HandleLockCursor()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            ConfinedCursor();
        if (Input.GetKeyDown(KeyCode.Escape))
            FreeCursor();
    }

    void HandleStateChange()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StateChanged(GameState.Pause);
    }

    void HandleShortcutsUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            HandleToggleInventory();
        if (Input.GetKeyDown(KeyCode.C))
            HandleToggleShop();
        if (Input.GetKeyDown(KeyCode.Z) && !isPotionOneUsing)
            HandleUsePotion(0);
        if (Input.GetKeyDown(KeyCode.X) && !isPotionTwoUsing)
            HandleUsePotion(1);
    }

    // Update is called once per frame
    void Update()
    {
        HandleLockCursor();
        HandleStateChange();
        HandleShortcutsUI();
    }

    #region HandleLockCursor
    void LockCursorChanged(CursorLockMode cursorLockMode)
    {
        Cursor.lockState = cursorLockMode;
    }
    #endregion

    #region HandleStateChange
    void StateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Pause:
                StartPauseGameCoroutine();
                break;
            default:
                break;
        }
    }

    public void StartPauseGameCoroutine()
    {
        if (pauseGameCoroutine != null)
            StopCoroutine(pauseGameCoroutine);
        pauseGameCoroutine = HandlePausegame();
        StartCoroutine(pauseGameCoroutine);
    }
    IEnumerator HandlePausegame()
    {
        if (UIManager.Instance.isShowing("Pause Menu"))
        {
            Time.timeScale = 1;
            UIManager.Instance.HideView("Pause Menu");
        }
        else
        {
            UIManager.Instance.ShowView("Pause Menu");
            yield return new WaitForSeconds(1f);
            Time.timeScale = 0;
        }
    }
    #endregion

    #region HandleShortcutsUI
    public void HandleToggleInventory()
    {
        AudioManager.Instance.PlayEffect("PLAYER", "Opening/Closing Inventory");
        UIManager.Instance.ShowView("Character UI", true);
        if (UIManager.Instance.isShowing("Character UI"))
            CharacterUI.Instance.InitializeInventory();
    }

    public void HandleToggleShop()
    {
        SystemWindowManager.Instance.ShowWindowSystem("DEFAULT", "The shop is not open yet, come back later!");
    }

    public void HandleUsePotion(int potionUsing)
    {
        ItemSO potion = PlayerManager.Instance.potionsEquipped[potionUsing];
        if (!InventoryManager.Instance.ItemExists(potion) || InventoryManager.Instance.GetItem(potion).quantity <= 0)
        {
            SystemWindowManager.Instance.ShowWindowSystem("DANGER", "Out of quantity, go and get more!");
            return;
        }
        AudioManager.Instance.PlayEffect("PLAYER", "Use Potion");
        potion.PlayFunction();
        InventoryManager.Instance.EditItem(new InventoryItem(potion, InventoryManager.Instance.GetItem(potion).quantity - 1));
        MainUI.Instance.PotionCountChanged(potionUsing, InventoryManager.Instance.GetItem(potion).quantity);
        switch (potionUsing)
        {
            case 0:
                StartCoroutine(MainUI.Instance.UsePotion(reference => isPotionOneUsing = reference, potionUsing));
                break;
            case 1:
                StartCoroutine(MainUI.Instance.UsePotion(reference => isPotionTwoUsing = reference, potionUsing));
                break;
            default:
                break;
        }
    }
    #endregion
}
