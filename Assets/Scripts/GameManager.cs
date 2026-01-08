using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalEnemies = 10;    // koliko jih moraš ubiti
    private int enemiesKilled = 0;

    public int towerHealth = 100;

    // Klice se, ko ubiješ enega enemy
    public void EnemyKilled()
    {
        enemiesKilled++;
        CheckWinLose();
    }

    // Klice se, ko tower izgubi zdravje
    public void TowerDamaged(int damage)
    {
        towerHealth -= damage;
        CheckWinLose();
    }

    private void CheckWinLose()
    {
        if (enemiesKilled >= totalEnemies)
        {
            WinGame();
        }
        else if (towerHealth <= 0)
        {
            LoseGame();
        }
    }

    private void WinGame()
    {
        Debug.Log("YOU WIN!");
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("win_screen");  // ime tvoje Win scene
    }

    private void LoseGame()
    {
        Debug.Log("YOU LOSE!");
        SceneManager.LoadScene("los_screen"); // ime tvoje Lose scene
    }
}

