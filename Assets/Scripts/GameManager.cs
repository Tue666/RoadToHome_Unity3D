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
        {
            if (!isConfinedCursor) LockCursorChanged(CursorLockMode.Confined);
            else LockCursorChanged(CursorLockMode.Locked);
            isConfinedCursor = !isConfinedCursor;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isFreeCursor) LockCursorChanged(CursorLockMode.None);
            else LockCursorChanged(CursorLockMode.Locked);
            isFreeCursor = !isFreeCursor;
        }
    }

    void HandleStateChange()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StateChanged(GameState.Pause);
    }

    void HandleShortcutsUI()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            HandleToggleInventory();
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
    void HandleToggleInventory()
    {

    }
    #endregion
}
