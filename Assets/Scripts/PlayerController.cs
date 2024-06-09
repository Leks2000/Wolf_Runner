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
    public float laneDistance = 10f; // Distance between two lanes

    public float jumpForce;
    public float gravity;

    public float fallSpeed;

    private Vector3 targetPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        // Gradually increase the forward speed
        forwardSpeed += SpeedModifier * Time.deltaTime;
        forwardSpeed = Mathf.Min(forwardSpeed, maxSpeed);
        displayedSpeed = forwardSpeed * 2.5f;

        direction.z = forwardSpeed;

        PerformJump();
        PerformTurn();

        targetPosition = new Vector3((desiredLane - 1) * laneDistance, transform.position.y, transform.position.z + forwardSpeed * Time.deltaTime);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
        controller.Move((smoothedPosition - transform.position) + direction * Time.deltaTime);
    }

    private void PerformTurn()
    {
        TurnRight();
        TurnLeft();
    }

    private void TurnLeft()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A )|| SwipeManager.swipeLeft)
        {
            FindObjectOfType<AudioManager>().PlaySound("Turn");

            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }
    }

    private void TurnRight()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || SwipeManager.swipeRight)
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
            direction.y = -1;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || SwipeManager.swipeUp)
                Jump();
        }
        else
        {
            direction.y += gravity * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || SwipeManager.swipeDown)
                direction.y = -fallSpeed;
        }
    }

    private void Jump()
    {
        FindObjectOfType<AudioManager>().PlaySound("Jump");
        direction.y = jumpForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Obstacle")
        {
            var am = FindObjectOfType<AudioManager>();
            am.PlaySound("Crash");
            StartCoroutine(AudioManager.FadeOut(am.GetComponent<AudioSource>(), 2, 0.0f));

            PlayerManager.gameOver = true;
        }
    }
}