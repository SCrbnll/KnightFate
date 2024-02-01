using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] CircleCollider2D m_groundSensor;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private bool canJump = false;

    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;

    private int lives = 3;
    public Vector3 initialPosition; // X:-19.9  Y:-5.165
    public float delay;
    private RigidbodyConstraints2D initialConfigurationRigidBody2D;


    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        initialConfigurationRigidBody2D = m_body2d.constraints;
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --

        //Death
        if (Input.GetKeyDown("e"))
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                Debug.Log("Enemy hitted");
                GameObject enemyObject = enemy.gameObject;
                Destroy(enemyObject);
            
            }

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        //Jump
        else if (Input.GetKeyDown("space"))
        {
            if (canJump)
            {
                m_animator.SetTrigger("Jump");
                canJump = false;
                m_animator.SetBool("Grounded", canJump);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            }
    }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canJump = true;
            m_animator.SetBool("Grounded", canJump);
        } 
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            lives--;
            if (lives <= 0)
            {   
                // Cambiar escena
                Debug.Log("You are dead");
            } 
            
            collision.enabled = false;
            m_animator.SetTrigger("Hurt");
            StartCoroutine(EnableCollider(delay, collision));
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
             // Cambiar escena
                Debug.Log("You pass the door");
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canJump = false;
            m_animator.SetBool("Grounded", canJump);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator EnableCollider(float delay, Collider2D collider)
     {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
     }
    
}
