using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_script : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public BoxCollider2D boxCollider;
    private float dirX = 0f;
    private float dirY = 0f;
    [SerializeField] private float moveSpeed = 64f;
    [SerializeField] private float jumpForce = 128f;
    [SerializeField] private LayerMask jumpableGround;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rigidBody.velocity = new Vector2(dirX * moveSpeed, rigidBody.velocity.y);
        /*
        if (Input.GetButtonDown("Jump") && Physics2D.BoxCast(boxCollider.bounds.center, 
            boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }
        */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }
    }
}
