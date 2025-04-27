using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameUI : MonoBehaviour
{
    public GameObject gamewinUI;
    public GameObject gameloserUI;
    bool gameIsover;
    void Start()
    {
        guard.OnGuardHasSpottedPlayer += showGameLoseUI;
        
    }

    void Update()
    {
        if (gameIsover){
            if (Input.GetKeyDown(KeyCode.Space)){
                SceneManager.LoadScene (0);
            }
        }
    }
    public void showGameWinUI(){
        onGameOver(gamewinUI);
    }
    void showGameLoseUI(){
        onGameOver(gameloserUI);
    }
    void onGameOver(GameObject gameOverUI){
        gameOverUI.SetActive(true);
        gameIsover = true;
        guard.OnGuardHasSpottedPlayer -= showGameLoseUI;
    }
}
