using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceInstruction : MonoBehaviour
{
    public void Continue() {
        SceneManager.LoadScene("InitialLevel");
    }
}
