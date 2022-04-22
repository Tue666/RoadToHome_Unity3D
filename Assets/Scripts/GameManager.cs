using UnityEngine;

public enum GameState
{
    Pause
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isConfinedCursor = false;
    private bool isFreeCursor = false;

    private bool isPotionOneUsing = false;
    private bool isPotionTwoUsing = false;

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

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
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
            HandleUsePotion(1);
        if (Input.GetKeyDown(KeyCode.X) && !isPotionTwoUsing)
            HandleUsePotion(2);
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
                HandlePausegame();
                break;
            default:
                break;
        }
    }

    void HandlePausegame()
    {
        //Time.timeScale = Time.timeScale == 1 ? 0 : 1;
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

    public void HandleUsePotion(int potionSlot)
    {
        int index = 0;
        AudioManager.Instance.PlayEffect("PLAYER", "Use Potion");
        ItemSO potion = null;
        switch (potionSlot)
        {
            case 1:
                {
                    index = 0;
                    potion = PlayerManager.Instance.potionsEquipped[0];
                    potion.PlayFunction();
                    StartCoroutine(MainUI.Instance.UsePotion(reference => isPotionOneUsing = reference, potionSlot));
                }
                break;
            case 2:
                {
                    index = 1;
                    potion = PlayerManager.Instance.potionsEquipped[1];
                    
                    potion.PlayFunction();
                    StartCoroutine(MainUI.Instance.UsePotion(reference => isPotionTwoUsing = reference, potionSlot));
                }
                break;
            default:
                break;
        }
        if (potion != null)
        {
            if (InventoryManager.Instance.GetItem(potion).quantity <= 0)
            {
                SystemWindowManager.Instance.ShowWindowSystem("DANGER", "Item out of stock, please get more before using!");
                return;
            }
            InventoryManager.Instance.EditItem(new InventoryItem(potion, InventoryManager.Instance.GetItem(potion).quantity - 1));
            MainUI.Instance.PotionCountChanged(index, InventoryManager.Instance.GetItem(potion).quantity);
        }
    }
    #endregion
}
