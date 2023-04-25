using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WindMovement : MonoBehaviour
{
    public float speed;
    public float weight;
    private Rigidbody2D rb;
    public int windCharges = 10;
    private float windDuration = 1f;
    private bool usingWind = false;
    private float xinput = 0;
    private float yinput = 0;
    public Text uiText;
    public Text timerText;
    public ParticleSystem uiParticles;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;
    private float timeSinceStart = 0f;
    private bool timerStarted;
    private bool directionHeld;
    SpriteRenderer sprite;
    float score;
    private float axisRawHorizontal;
    private float axisRawVertical;
    private bool spaceButtonPressed;
    private bool leftShiftPressed;
    private bool rKeyPressed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        uiText.text = windCharges.ToString();
        velocityModule = uiParticles.velocityOverLifetime;
        velocityModule.x = 0;
        velocityModule.y = 0;
        uiParticles.gameObject.SetActive(false);
        timerStarted = false;
        timerText.text = "0";
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        axisRawHorizontal = Input.GetAxisRaw("Horizontal");
        axisRawVertical = Input.GetAxisRaw("Vertical");
        CheckKeyPresses();

        if (timerStarted)
        {
            timeSinceStart += Time.deltaTime;
            timerText.text = timeSinceStart.ToString();
        }
    }

    private void CheckKeyPresses()
    {
        if (Input.GetKeyDown("space"))
        {
            spaceButtonPressed = true;
            if (!timerStarted)
            {
                timerStarted = true;
            }
        }

        if (Input.GetKey("left shift"))
        {
            leftShiftPressed = true;
        }

        if (Input.GetKeyDown("r"))
        {
            rKeyPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (axisRawHorizontal == 0 && axisRawVertical == 0)
        {
            directionHeld = false;
        }
        else
        {
            directionHeld = true;
        }

        if (spaceButtonPressed && windCharges > 0 && directionHeld)
        {
            usingWind = true;
            windDuration = 1f;
            xinput = axisRawHorizontal * speed;
            yinput = axisRawVertical * speed;
            windCharges--;
            Debug.Log(windCharges);
            uiText.text = windCharges.ToString();
            uiParticles.gameObject.SetActive(true);
            velocityModule.x = 0;
            velocityModule.y = 0;
            velocityModule.x = axisRawHorizontal * 5;
            velocityModule.y = axisRawVertical * 10;

        }

        if (usingWind && windDuration > 0)
        {
            rb.gravityScale = 1;
            windDuration -= Time.fixedDeltaTime;
            if (!leftShiftPressed)
            {
                rb.AddForce(new Vector2(xinput, yinput));
                rb.gravityScale = 0.1f;
            }
        }

        if (windDuration <= 0)
        {
            usingWind = false;
            windDuration = 1f;
            rb.gravityScale = 1;
            uiParticles.gameObject.SetActive(false);
        }

        if (leftShiftPressed)
        {
            sprite.color = new Color(0, 0, 1);
        }
        else
        {
            sprite.color = new Color(0, 1, 0);
        }

        if (rKeyPressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        ResetInputs();
    }

    private void ResetInputs()
    {
        if (spaceButtonPressed || leftShiftPressed || rKeyPressed)
        {
            spaceButtonPressed = false;
            leftShiftPressed = false;
            rKeyPressed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "LevelEnd")
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().buildIndex.ToString(), timeSinceStart);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if(collision.gameObject.tag == "LevelRestart")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
