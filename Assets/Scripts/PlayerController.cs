using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;

    public float forwardSpeed;
    public float maxSpeed;
    public const float SpeedModifier = 0.1f;
    public float displayedSpeed = 0;

    private int desiredLane = 1; // Start in the middle lane
    public float laneDistance = 4; // Distance between two lanes

    public float jumpForce;
    public float gravity;

    public float fallSpeed;

    private Vector3 targetPosition;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetPosition = transform.position;
    }

    void Update()
    {
        // Gradually increase the forward speed
        forwardSpeed += SpeedModifier * Time.deltaTime;
        forwardSpeed = Mathf.Min(forwardSpeed, maxSpeed);
        displayedSpeed = forwardSpeed * 10;

        direction.z = forwardSpeed;

        PerformJump();
        PerformTurn();

        // Calculate the target position for smooth lane change
        targetPosition = new Vector3((desiredLane - 1) * laneDistance, transform.position.y, transform.position.z + forwardSpeed * Time.deltaTime);

        // Smoothly move the player to the target lane position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, 10 * Time.deltaTime);
        controller.Move((smoothedPosition - transform.position) + direction * Time.deltaTime);
    }



    private void PerformTurn()
    {
        TurnRight();
        TurnLeft();
    }

    private void TurnLeft()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
        {
            FindObjectOfType<AudioManager>().PlaySound("Turn");

            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }
    }

    private void TurnRight()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
        {
            FindObjectOfType<AudioManager>().PlaySound("Turn");

            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
    }

    private void PerformJump()
    {
        if (controller.isGrounded)
        {
            direction.y = -1; // Ensure the player stays grounded
            if (Input.GetKeyDown(KeyCode.UpArrow) || SwipeManager.swipeUp)
                Jump();
        }
        else
        {
            direction.y += gravity * Time.deltaTime;

            // If the player is in the air and a downward swipe is performed, accelerate the fall
            if (Input.GetKeyDown(KeyCode.DownArrow) || SwipeManager.swipeDown)
                direction.y = -fallSpeed;
        }
    }

    private void Jump()
    {
        FindObjectOfType<AudioManager>().PlaySound("Jump");
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            var am = FindObjectOfType<AudioManager>();
            am.PlaySound("Crash");
            StartCoroutine(AudioManager.FadeOut(am.GetComponent<AudioSource>(), 2, 0.0f));

            PlayerManager.gameOver = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        Vector3 newPosition = other.transform.localPosition;
        newPosition.z = Mathf.Lerp(other.transform.localPosition.z, transform.localPosition.z, Time.deltaTime * 1);

        other.transform.localPosition = newPosition;
    }
}
