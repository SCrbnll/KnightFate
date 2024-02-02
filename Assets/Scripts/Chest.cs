using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject llavePrefab;

    private Animator animator;

    private bool cofreAbierto = false;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!cofreAbierto) {
            OpenChest();
        }
    }

    void OpenChest()
    {
        if (llavePrefab != null)
        {
            animator.SetBool("IsOpened", true);
            StartCoroutine(OpenDelay());
        }
    }

    IEnumerator OpenDelay()
     {
        yield return new WaitForSeconds(0.4f);
        Vector3 posicionCofre = transform.position;
            Vector3 posicionLlave = new Vector3(posicionCofre.x, posicionCofre.y + 1f, posicionCofre.z);
            Instantiate(llavePrefab, posicionLlave, Quaternion.identity);
            cofreAbierto = true;
     }
}
