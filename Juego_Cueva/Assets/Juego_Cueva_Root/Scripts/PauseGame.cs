using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject menuPause;
    bool cursorActive = false;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
            if (GameManager.Instance.currentGameState == GameManager.GameStatus.gameRunning)
            {
                PauseMenu();
            }
            else if(GameManager.Instance.currentGameState == GameManager.GameStatus.gamePaused)
            {
                ResumeGame();
            }
        }
    }

    public void ToggleCursor()
    {
        cursorActive = !cursorActive;
        Cursor.lockState = cursorActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = cursorActive;
    }

    public void HideCursor()
    {
        cursorActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ResumeGame()
    {
        GameManager.Instance.currentGameState = GameManager.GameStatus.gameRunning;
        Time.timeScale = 1f;
        menuPause.SetActive(false);
    }
    public void PauseMenu()
    {
        GameManager.Instance.currentGameState = GameManager.GameStatus.gamePaused;
        Time.timeScale = 0f;
        menuPause.SetActive(true);
    }
}
