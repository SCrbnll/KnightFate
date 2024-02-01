using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
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

    public float delay;
    private RigidbodyConstraints2D initialConfigurationRigidBody2D;

    private int gems = 7;
    public GameObject puertaSinAnimar;

    private Vector3 posicionInicialJugador;

    private float movementButton = 0.0f;
    


    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        initialConfigurationRigidBody2D = m_body2d.constraints;
        posicionInicialJugador = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0) // CAMBIAR A movementButton
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
            
        else if (inputX < 0) // CAMBIAR A movementButton
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
        //m_body2d.velocity = new Vector2(movementButton * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --

        //Attack
        if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f)
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
        else if (Mathf.Abs(inputX) > Mathf.Epsilon) // CAMBIAR A movementButton
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

    public void Move(float amount) 
    {
        movementButton = amount;
    }

    public void Jump() 
    {
        if (canJump)
            {
                m_animator.SetTrigger("Jump");
                canJump = false;
                m_animator.SetBool("Grounded", canJump);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            }
    }

    public void Attack() 
    {
        if (m_timeSinceAttack > 0.25f)
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
            GameManager.lives--;
            if (GameManager.lives == 1)
            {   
                Debug.Log("You are dead");
                m_animator.SetBool("noBlood", false);
                m_animator.SetTrigger("Death");
                collision.enabled = false;
                m_body2d.bodyType = RigidbodyType2D.Static;
                // CAMBIAR ESCENA
            } 
            else
            {
                collision.enabled = false;
                m_animator.SetTrigger("Hurt");
                StartCoroutine(EnableCollider(delay, collision));
            }    
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            GameManager.lives = 4;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("You pass the door");
        } 

        if (collision.gameObject.tag == "Gem") 
        {
            Destroy(collision.gameObject);
            gems--;
            Debug.Log(gems);
            if (gems <= 0) {
                GameObject puertaAnimada = GameObject.FindGameObjectWithTag("AnimatedDoor");
                if (puertaAnimada != null) {
                    Destroy(puertaAnimada);
                    Instantiate(puertaSinAnimar, puertaAnimada.transform.position, puertaAnimada.transform.rotation);
                }
            }
        } 

        if (collision.gameObject.tag == "Trap") {
            GameManager.lives--;
            transform.position = posicionInicialJugador;

            if (GameManager.lives == 1)
            {   
                m_animator.SetBool("noBlood", false);
                m_animator.SetTrigger("Death");
                collision.enabled = false;
                m_body2d.constraints = RigidbodyConstraints2D.FreezePosition;
                // CAMBIAR ESCENA
            } 
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
        if(collider != null)
        {
            collider.enabled = true;
        }
     }
    
}
