using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            Vector2 moveVelocity = new Vector2(Input.GetAxisRaw("MoveHorizontal"), Input.GetAxisRaw("MoveVertical")).normalized * speed;
            rb.AddForce(moveVelocity);
        }
    }
}