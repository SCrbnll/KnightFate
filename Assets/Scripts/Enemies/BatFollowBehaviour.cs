using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatFollowBehaviour : StateMachineBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float tiempoBase;
    private float tiempoSeguir;
    private Transform jugador;
    private Bat murcielago;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        tiempoSeguir = tiempoBase;
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        murcielago = animator.gameObject.GetComponent<Bat>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, jugador.position, velocidadMovimiento * Time.deltaTime);
        murcielago.Girar(jugador.position);
        tiempoSeguir -= Time.deltaTime;
        if (tiempoSeguir <= 0) {
            animator.SetTrigger("Volver");
        }
    }
}
