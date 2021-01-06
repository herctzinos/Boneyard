using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Global options")]
    [SerializeField]
    private bool globalSpawnEnemies;
    
    private bool pausedGame;
    private bool startedGame;
    private bool endedGame;

    private HudManager hudManager;
    private GameObject hud;

    private GameObject player;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        ResumeGame();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player) playerController=player.GetComponent<PlayerController>();
        hud = GameObject.FindGameObjectWithTag("HUD");
        if (hud) hudManager =hud.GetComponent<HudManager>();

        CheckGlobalEnemySpawn();
    }

    private void TogglePause()
    {
        if (pausedGame) ResumeGame();
        else PauseGame();
    }

    public void StartGame()
    {
        startedGame = true;
        SceneManager.LoadScene("GameScene");
    }

    public void ResetGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
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

    public void EndGame()
    {
        Time.timeScale = 0;
        endedGame = true;
        SaveData();
        SceneManager.LoadScene("GameOverScene");
    }

    public void HandleOpenLevelUpMenu()
    {
        PauseGame();
        hudManager.DisplayLevelUpMenu();
    }

    public void ProvidePlayerSkill(int projectile, int spread, int range)
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

    private void SaveData()
    {

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
