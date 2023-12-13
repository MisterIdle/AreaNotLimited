using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour
{
    private float speed = 3.5f;
    private float force = 2f;
    private float maxDistance = 5f;

    private CharacterController controller;
    private Vector2 moveInput = Vector2.zero;
    private LineRenderer lineRenderer;

    private Camera mainCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.material.color = Color.blue;

        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;

        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector2 move = new Vector2(moveInput.x, moveInput.y);
        controller.Move(move * Time.deltaTime * speed);

        UpdatePlayerDistance();
        CameraFollow();

        lineRenderer.SetPosition(0, transform.position);
    }

    void CameraFollow()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            Vector3 averagePosition = Vector3.zero;

            for (int i = 0; i < players.Length; i++)
            {
                for (int j = i + 1; j < players.Length; j++)
                {
                    averagePosition += (players[i].transform.position + players[j].transform.position) / 2;
                }
            }

            averagePosition /= (players.Length * (players.Length - 1) / 2);

            mainCamera.transform.position = new Vector3(averagePosition.x, averagePosition.y, mainCamera.transform.position.z);
        }
    }

    void UpdatePlayerDistance()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player != gameObject)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance > maxDistance)
                {
                    Vector3 direction = (player.transform.position - transform.position).normalized;

                    float adjustmentStrength = force * Time.deltaTime;
                    float adjustedDistance = Mathf.Clamp(distance - adjustmentStrength, 0f, Mathf.Infinity);

                    Vector3 newPosition = transform.position + direction * adjustedDistance;
                    controller.Move((newPosition - transform.position) * Time.deltaTime * speed);
                }

                lineRenderer.SetPosition(1, player.transform.position);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
