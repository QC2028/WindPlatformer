using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public int windCharges = 10;
    private float windDuration = 1f;
    private bool usingWind = false;
    private float xinput = 0;
    private float yinput = 0;
    private float axisRawHorizontal;
    private float axisRawVertical;
    private bool spaceButtonPressed;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        axisRawHorizontal = Input.GetAxisRaw("Horizontal");
        axisRawVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown("space"))
        {
            spaceButtonPressed = true;
        }
    }

    private void FixedUpdate()
    {

        if (spaceButtonPressed && windCharges > 0)
        {
            usingWind = true;
            windDuration = 1f;
            xinput = axisRawHorizontal * speed;
            yinput = axisRawVertical * speed;
            windCharges--;
        }

        if (usingWind && windDuration > 0)
        {
            windDuration -= Time.fixedDeltaTime;
            rb.AddForce(new Vector2(xinput, yinput));
        }

        if (windDuration <= 0)
        {
            usingWind = false;
            windDuration = 1f;
        }
        spaceButtonPressed = false;
    }
}
