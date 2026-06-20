using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private AudioSource audioSource;
    private bool isOnGround = true;
    private bool gameOver = false;

    public float jumpForce = 10f;
    public float duckScaleY = 0.5f;
    private Vector3 originalScale;

    public AudioClip jumpSound;
    public AudioClip crashSound;

    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem dirtParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        originalScale = transform.localScale;
        animator.SetFloat("Speed_f", 1.0f);
        animator.SetBool("Static_b", false);
        if (dirtParticle != null) dirtParticle.Play();
    }

    void Update()
{
    if (gameOver) return;
    
    RaycastHit hit;
   if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 3f))
{
    if (hit.collider.CompareTag("Ground") && rb.linearVelocity.y <= 0.1f)
        isOnGround = true;
    else if (rb.linearVelocity.y > 0.1f)
        isOnGround = false;
}
else
{
    isOnGround = false;
}

    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
        Debug.Log("Jump pressed - isOnGround: " + isOnGround + " velocity Y: " + rb.linearVelocity.y);
        if (isOnGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("After force - velocity Y: " + rb.linearVelocity.y);
            isOnGround = false;
            animator.SetTrigger("Jump_trig");
            audioSource.PlayOneShot(jumpSound);
            dirtParticle.Stop();
        }
    }

    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
        transform.localScale = new Vector3(
            originalScale.x, duckScaleY, originalScale.z);
        animator.SetBool("Crouch_b", true);
    }

    if (Input.GetKeyUp(KeyCode.DownArrow))
    {
        transform.localScale = originalScale;
        animator.SetBool("Crouch_b", false);
    }
}

void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        // Ελέγχει αν η επαφή είναι από κάτω (Y > 0.5)
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isOnGround = true;
                animator.SetBool("Grounded", true);
                dirtParticle.Clear();
                dirtParticle.Play();
                break;
            }
        }
    }

    if (collision.gameObject.CompareTag("Obstacle"))
    {
        audioSource.PlayOneShot(crashSound);
        explosionParticle.Play();
        if (dirtParticle != null) dirtParticle.Stop();
        GameManager.instance.TriggerGameOver();
    }
}

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Grounded", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reward"))
        {
            GameManager.instance.AddScore(1);
            Destroy(other.gameObject);
        }
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
        animator.SetFloat("Speed_f", 0);
        animator.SetBool("Death_b", true);
    }
        public void SetWin()
    {
        gameOver = true;
        animator.SetFloat("Speed_f", 0);
        animator.SetBool("Static_b", true);
    }



}