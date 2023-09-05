using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameInput gameInput;

    private bool isWalking;

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 0.7f;
        bool canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Attemp only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can move only X
                moveDir = moveDirX;
            }

        }

        if (!canMove)
        {
            // Attemp only Z movement
            Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
            canMove = !Physics.CapsuleCast(transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirZ, moveDistance);

            if (canMove)
            {
                // Can move only Z
                moveDir = moveDirZ;
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
