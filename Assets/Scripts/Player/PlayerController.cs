using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private PlayerInput playerInput;
    private Vector2 moveInput;
    private float runMultiplier = 10f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private Interactor interactor;

    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        interactor = GetComponent<Interactor>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!interactor.isInteracting)
        {
            Vector2 moveInput = playerInput.Player.Move.ReadValue<Vector2>();
            if (moveInput.x == 0 && moveInput.y == 0)
            {
                animator.SetBool("isMoving", false);
            }
            else
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("horizontal", moveInput.x);
                animator.SetFloat("vertical", moveInput.y);
            }
            if(playerInput.Player.Run.ReadValue<float>() > 0)
            {
                runMultiplier = 1.2f;
            } else
            {
                runMultiplier = 1f;
            }
            rb.velocity = moveInput * moveSpeed * runMultiplier;
            //rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }

    }



}
