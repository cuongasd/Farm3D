using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public float moveSpeed = 5f;
    [SerializeField] private CharacterController characterController;

    public Animator animator;
    public void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontalMove = joystick.Horizontal;
        float verticalMove = joystick.Vertical;
        Vector3 moveDirection = new Vector3(horizontalMove, 0, verticalMove);
        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("walk", true);
            animator.gameObject.transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            animator.SetBool("walk", false);
        }
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= moveSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
