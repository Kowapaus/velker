// Assets/Scripts/PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    private CharacterController controller;
    private Vector3 velocity;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0; right.y = 0;
        forward.Normalize(); right.Normalize();

        Vector3 moveDirection = forward * vertical + right * horizontal;
        moveDirection = moveDirection.normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
            animator?.SetBool("IsWalking", true);
        }
        else
        {
            animator?.SetBool("IsWalking", false);
        }

        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = -0.1f;
        }
        controller.Move(velocity * Time.deltaTime);

        Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Coin"))
            {
                GameManager.Instance.CollectCoin();
                Destroy(hit.gameObject);
            }
        }
    }
}