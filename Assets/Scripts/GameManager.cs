using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Global options")]
    [SerializeField]
    private bool globalSpawnEnemies;

    int currentSceneIndex;
    [SerializeField] int timeToWait = 4;

    private bool pausedGame;
    private bool startedGame;
    private bool endedGame;

    private HudManager hudManager;
    private GameObject hud;

    private GameObject player;
    private PlayerController playerController;

    public GameData gd;

    // Start is called before the first frame update
    public void Start()
    {

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitForTime());
        }

        LoadData();
        ResumeGame();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player) playerController = player.GetComponent<PlayerController>();
        hud = GameObject.FindGameObjectWithTag("HUD");
        if (hud) hudManager = hud.GetComponent<HudManager>();

        CheckGlobalEnemySpawn();
    }
    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(timeToWait);
        LoadStartScreen();
    }

 
    private void TogglePause()
    {
        if (pausedGame) ResumeGame();
        else PauseGame();
    }

    /* public void StartGame()
     {
         startedGame = true;
         SceneManager.LoadScene("GameScene");
     }

     public void ResetGame()
     {
         Time.timeScale = 1;
         SceneManager.LoadScene("StartScene");
     }*/


    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadStartScreen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScreen");
    }
    
    public void LoadSplashScreen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SplashScreen");
    }

    public void LoadOptionsScreen()
    {
        SceneManager.LoadScene("OptionsScreen");
    } 
    
    public void OpenStatsScreen()
    {
        PauseGame();
        hudManager.DisplayStats();
    }

    public void CloseStatsScreen()
    {
        hudManager.HideStats();
        ResumeGame();
    }

    public void LoadGameScene1()
    {
        SceneManager.LoadScene("GameScene");

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pausedGame = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        pausedGame = false;
    }

    public void LoadGameOverScene()
    {
        Time.timeScale = 0;
        endedGame = true;
        LoadData();
        SaveData();
       // SaveDataOnKill();
        SceneManager.LoadScene("GameOverScene");
    }

    public void HandleOpenLevelUpMenu()
    {
        PauseGame();
        hudManager.DisplayLevelUpMenu();
    }

    public void AttachSelectedSkillToPlayer(int projectile, int spread, int range)
    {
        Weapon activeWeaponScript = playerController.GetActiveWeapon().GetComponent<Weapon>();
        activeWeaponScript.UpgradeStats(projectile, spread, range);
    }

    public void HandleCloseLevelUpMenu()
    {
        hudManager.HideLevelUpMenu();
        ResumeGame();
    }

    public bool IsGamePaused()
    {
        return pausedGame;
    }

    public bool IsGameStarted()
    {
        return startedGame;
    }

    public bool IsGameEnded()
    {
        return endedGame;
    }

    public void LoadData()
    {
        Debug.Log("GameData: " + PlayerPrefs.GetString("GameData"));

        if (PlayerPrefs.HasKey("GameData")&& PlayerPrefs.GetString("GameData")!="")
        {
            gd = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("GameData"));
        }
        else
        {
            gd = new GameData();
        }
    }

    public void SaveData()
    {
        gd.playerGold = playerController.GetGold();
        gd.maxLevelReached = playerController.GetLevel();
        gd.maxHealthReached = playerController.GetMaxHealthEarned();
        gd.maxManaReached = playerController.GetMaxManaEarned();
        gd.maxPowerReached = playerController.GetMaxPowerEarned();
        PlayerPrefs.SetString("GameData",JsonUtility.ToJson(gd));
    }

    public void SaveDataOnKill()
    {
        gd.playerGold = playerController.GetGold();
       
        PlayerPrefs.SetString("GameData", JsonUtility.ToJson(gd));
    }

    private bool CheckGlobalEnemySpawn()
    {

        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("enemyspawner");
        foreach (GameObject enemySpawner in enemySpawners)
        {
            enemySpawner.GetComponent<ItemSpawn>().SetGlobalEnabling(globalSpawnEnemies);
        }
        return globalSpawnEnemies;
    }


}
