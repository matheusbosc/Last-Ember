using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed = 10;
    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 lastInput;
    private bool isPaused = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isPaused = GameManager.instance.isPaused;

        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (!isPaused ||  !GameManager.instance.isPlaying)
        {
            if (horizontal == 0 && vertical == 0)
            {
                SoundManager.instance.isWalking = false;
                animator.SetBool("IsWalking", false);
            }
            else
            {
                SoundManager.instance.isWalking = true;
                animator.SetBool("IsWalking", true);
                lastInput = new Vector2(horizontal, vertical);
            }
        
            animator.SetFloat("xInput", lastInput.x);
            animator.SetFloat("yInput", lastInput.y);
        }
        
        Vector2 movement  = new Vector2(horizontal, vertical);
        movement.Normalize();
        
        rb.linearVelocity = isPaused ? Vector2.zero : movement * speed;
    }
}
