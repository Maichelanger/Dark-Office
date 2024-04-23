using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public string currentScene;
    public string nextScene;
    public GameObject keyObject;
    public GameObject screenCover;
    public GameObject endGameText;
    public AudioClip deathSound, doorSound;

    private bool hasKey;
    private bool disabled;
    private string collidingWith;
    private Vector2 movement;
    private Rigidbody2D rb;
    private PlayerControls inputs;
    private Animator animator;
    private AudioSource charAudio;

    private void Awake()
    {
        disabled = false;

        screenCover.SetActive(false);
        endGameText.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        charAudio = GetComponent<AudioSource>();

        inputs = new PlayerControls();

        charAudio.Play();
    }

    private void Update()
    {
        if (collidingWith == "Enemy" && !disabled)
        {
            EnemyCollision();
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Time.fixedDeltaTime * walkSpeed * movement);
    }

    #region Misc Functions

    private void LoadSame()
    {
        SceneManager.LoadScene(currentScene);
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }

    private void TurnRed()
    {
        screenCover.GetComponent<Image>().color = Color.red;
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Load1(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Level1");
    }

    private void Load2(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Level2");
    }

    private void Load3(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Level3");
    }

    private void EnemyCollision()
    {
        disabled = true;
        screenCover.SetActive(true);
        charAudio.Stop();
        charAudio.PlayOneShot(deathSound);

        Invoke("TurnRed", 3.8f);

        Invoke("LoadSame", 5);
    }
    #endregion

    #region Action Button

    private void PerformAction(InputAction.CallbackContext context)
    {
        switch (collidingWith)
        {
            case "Key":
                GetKey();
                break;
            case "Reader":
                UseKey();
                break;
            case "HDD":
                EndGame();
                break;
        }
    }

    private void EndGame()
    {
        disabled = true;
        screenCover.SetActive(true);
        endGameText.SetActive(true);
        charAudio.Stop();

        Invoke("LoadMenu", 5);
    }

    private void UseKey()
    {
        if (hasKey)
        {
            disabled = true;
            screenCover.SetActive(true);
            charAudio.Stop();
            charAudio.PlayOneShot(doorSound);

            Invoke("NextLevel", 3);
        }
    }

    private void GetKey()
    {
        Destroy(keyObject);
        keyObject = null;

        hasKey = true;
    }
    #endregion

    private void OnMovement(InputAction.CallbackContext context)
    {
        if (!disabled)
        {
            movement = context.ReadValue<Vector2>();
        }
        else
        {
            movement = Vector2.zero;
        }

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    #region OnEnable, OnDisable
    private void OnEnable()
    {
        inputs.Enable();

        inputs.InGame.Movement.performed += OnMovement;
        inputs.InGame.Movement.canceled += OnMovement;

        inputs.InGame.Action.performed += PerformAction;

        inputs.InGame.Level1.performed += Load1;
        inputs.InGame.Level2.performed += Load2;
        inputs.InGame.Level3.performed += Load3;
    }

    private void OnDisable()
    {
        inputs.Disable();

        inputs.InGame.Movement.performed -= OnMovement;
        inputs.InGame.Movement.canceled -= OnMovement;

        inputs.InGame.Action.performed -= PerformAction;

        inputs.InGame.Level1.performed -= Load1;
        inputs.InGame.Level2.performed -= Load2;
        inputs.InGame.Level3.performed -= Load3;
    }

    #endregion

    #region OnTrigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collidingWith = collision.gameObject.tag;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collidingWith = "";
    }
    #endregion
}
