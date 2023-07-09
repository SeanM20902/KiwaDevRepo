
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;

    private float dirX = 0f;
    private float ddt;
    KeyCode preInput;
    private bool isDash;
    private float dashDistancex = 50f;
    private float dashDistancey = 30f;
    private int maxDash = 1;
    private int dashCharge = 1;
    private float dashCd = 0.5f;
    private float preDash;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private LayerMask jumpableGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // sprite dir
        dirX = Input.GetAxisRaw("Horizontal");
        if (dirX < 0)
            sprite.flipX = true;
        else if (dirX > 0)
            sprite.flipX = false;

        // jump
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGround())
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // dash
        if (canDash())
            dash();

        // + vel on move to ground
        if (Input.GetKey(KeyCode.S))
            rb.velocity = new Vector2(rb.velocity.x, -1 * jumpForce);

        isGround();
    }

    bool canDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDash && dashCharge > 0)
            return true;
        return false;
    }

    bool handleDash(int[] nDashList, KeyCode KeyCode1, KeyCode KeyCode2)
    {
        if (Input.GetKey(KeyCode1))
        {
            StartCoroutine(nDash(nDashList[0], nDashList[1], 0.75f));
            return true;
        }
        else if (Input.GetKey(KeyCode2))
        {
            StartCoroutine(nDash(nDashList[2], nDashList[3], 0.75f));
            return true;
        }
        return false;
    }

    void dash()
    {
        if (Input.GetKey(KeyCode.A))
        {
            int[] nDashList = { -1, 1, -1, -1 };
            if (!handleDash(nDashList, KeyCode.W, KeyCode.S))
                StartCoroutine(Dashx(-1));
        }

        if (Input.GetKey(KeyCode.D))
        {
            int[] nDashList = { 1, 1, 1, -1 };
            if (!handleDash(nDashList, KeyCode.W, KeyCode.S))
                StartCoroutine(Dashx(1));
        }

        if (Input.GetKey(KeyCode.S))
        {
            int[] nDashList = { -1, -1, 1, -1 };
            if (!handleDash(nDashList, KeyCode.A, KeyCode.D))
                StartCoroutine(Dashy(-1));
        }

        if (Input.GetKey(KeyCode.W))
        {
            int[] nDashList = { -1, 1, 1, 1 };
            if (!handleDash(nDashList, KeyCode.A, KeyCode.D))
                StartCoroutine(Dashy(1));
        }
        dashCharge--;
    }

    void FixedUpdate()
    {
        if (!isDash)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
    }

    bool isGround()
    {
        if (Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround))
        {
            dashReset();
            return true;
        }
        else
        {
            return false;
        }
    }

    void dashReset()
    {
        if (preDash <= Time.time)
        {
            dashCharge = maxDash;
            preDash = Time.time + dashCd;
        }
    }

    IEnumerator Dashx(float dir)
    {
        isDash = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistancex * dir, 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.2f);
        isDash = false;
        rb.gravityScale = gravity;
    }

    IEnumerator Dashy(float dir)
    {
        isDash = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(rb.velocity.x, dashDistancey * dir), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.1f);
        isDash = false;
        rb.gravityScale = gravity;
    }

    IEnumerator nDash(float x, float y, float scale)
    {
        isDash = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistancex * x * scale, dashDistancey * y * scale), ForceMode2D.Impulse);
        float gravity = 4;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.2f);
        isDash = false;
        rb.gravityScale = gravity;
    }
}