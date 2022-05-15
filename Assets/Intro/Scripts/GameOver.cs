using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
    public GameObject menu;
    public GameObject persits; 
    public GameObject SceneLoader;
    public GameObject GameManager;
    public GameObject AudioManager;
    public GameObject WeaponManager;
    public GameObject InventoryManager;
    public GameObject UIManager;
    public GameObject SystemManager;
    public GameObject Canvanes;
    public GameObject moitruong;
    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    public void goHome()
    {
        SceneManager.LoadScene("MainMenu");
        menu.SetActive(false);
        //Destroy(persits);
        //Destroy(SceneLoader);
        //Destroy(GameManager);
        //Destroy(AudioManager);
        //Destroy(WeaponManager);
        //Destroy(InventoryManager);
        //Destroy(UIManager);
        //Destroy(SystemManager);
        //Destroy(Canvanes);
        //Destroy(moitruong);
    }

}
