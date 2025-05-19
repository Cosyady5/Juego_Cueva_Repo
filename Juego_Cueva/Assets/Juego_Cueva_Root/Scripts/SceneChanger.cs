using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Libreria que permite usar metodos de carga/descarga de escenas

public class SceneChanger : MonoBehaviour
{
    public void SceneLoader(int sceneToLoad)
    {
        Time.timeScale = 1f;
        GameManager.Instance.currentGameState = GameManager.GameStatus.gameRunning;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitGame()
    {
        Application.Quit(); //Salir de la aplicación, cierra el juego completamente
    }

    [SerializeField] private string Escena;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(Escena);

        }
    }
}