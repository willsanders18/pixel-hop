using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    private Rigidbody2D rb;
    private float jumpForce = 7.5f;
    private bool isOnGround;
    private float moveValue = 0.01f;
    private float lowBound = -5.5f;
    public Animator animator;
    private bool facingRight;
    private bool hasWon;
    public GameObject endUI;

    private AudioSource playerAudio;
    public AudioClip jump;
    public AudioClip crashSound;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = gameObject.GetComponent<AudioSource>();
        hasWon = false;
        facingRight = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Flip(horizontalInput);
        if (horizontalInput > moveValue && !hasWon)
        {
            transform.Translate(Vector2.right * horizontalInput * speed * Time.deltaTime);
            animator.SetFloat("Speed", Mathf.Abs(1));
        }

        else if (horizontalInput < -moveValue && !hasWon)
        {
            transform.Translate(Vector2.right * horizontalInput * speed * Time.deltaTime);
            animator.SetFloat("Speed", Mathf.Abs(1));
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
            
        if (Input.GetKeyDown(KeyCode.UpArrow) && isOnGround && !hasWon)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
            animator.SetBool("isJumping", true);
            playerAudio.PlayOneShot(jump);
        }

        if (transform.position.y < lowBound)
        {
            playerAudio.PlayOneShot(crashSound);
            Destroy(gameObject);
            GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            animator.SetBool("isJumping", false);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerAudio.PlayOneShot(crashSound);
            Destroy(gameObject);
            GameOver();
        }

    }
    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void GameOver()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Win Sensor"))
        {
            hasWon = true;
            endUI.SetActive(true);
        }
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            SceneManager.LoadScene("Level 2");
        }else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            SceneManager.LoadScene("Level 3");
        }else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            SceneManager.LoadScene("Main Menu");
        }

        hasWon = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
