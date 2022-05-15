using UnityEngine;

public class SavePoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerManager.Instance.playerObj)
        {
            if (Boss.beingChallenged)
            {
                SystemWindowManager.Instance.ShowWindowSystem("DANGER", "Player is challenging the boss, can't save the game!");
                return;
            }
            InventoryManager.Instance.SaveInventory();
            PlayerManager.Instance.player.SavePlayer();
            PlayerPrefs.SetInt("Map", SceneLoader.currentScene);
            SystemWindowManager.Instance.ShowWindowSystem("SUCCESS", "Game saved!");
        }
    }
}
