using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatVolverBehaviour : StateMachineBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    private Vector3 puntoInicial;
    private Bat murcielago;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        murcielago = animator.gameObject.GetComponent<Bat>();
        puntoInicial = murcielago.puntoInicial;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, puntoInicial, velocidadMovimiento * Time.deltaTime);
        murcielago.Girar(puntoInicial);
        if (animator.transform.position.x == puntoInicial.x && animator.transform.position.y == puntoInicial.y) {
            animator.SetTrigger("Llegar");
        }
    }
}
