using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceStart : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("InitialLevel");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
