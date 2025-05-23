using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance != null) Debug.Log("There is a GameManager already!");
            return instance;
        }
    }

    public enum GameStatus { gameOver, gamePaused, gameRunning };
    public GameStatus currentGameState = GameStatus.gameRunning;

    public float currentLife;
    public float fullLife = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}