using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceStart : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("Instructions");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
