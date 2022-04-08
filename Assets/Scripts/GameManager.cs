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
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
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
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }
    #endregion

    #region HandleShortcutsUI
    public void HandleToggleInventory()
    {
        AudioManager.Instance.PlayEffect("PLAYER", "Opening/Closing Inventory");
        UIManager.Instance.ShowView("Character UI");
        if (UIManager.Instance.isShowing("Character UI"))
            CharacterUI.Instance.InitializeInventory();
    }

    public void HandleUsePotion(int potionSlot)
    {
        AudioManager.Instance.PlayEffect("PLAYER", "Use Potion");
        switch (potionSlot)
        {
            case 1:
                StartCoroutine(MainUI.Instance.UsePotion(reference => isPotionOneUsing = reference, potionSlot));
                break;
            case 2:
                StartCoroutine(MainUI.Instance.UsePotion(reference => isPotionTwoUsing = reference, potionSlot));
                break;
            default:
                break;
        }
    }
    #endregion
}
