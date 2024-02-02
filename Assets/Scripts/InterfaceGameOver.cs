using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceGameOver : MonoBehaviour
{
    public void StartGame() {
        CanvasHUD.health = 3;
        SceneManager.LoadScene("Start");
    }
}
