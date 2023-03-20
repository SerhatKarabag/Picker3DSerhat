using System;
using System.Collections.Generic;
using UnityEngine;

public class PickerController : Singleton<PickerController>
{
    private const float moveSpeed = 0.01f; // picker obj speed.
    private Rigidbody rigidBody;
    [SerializeField]private List<GameObject> currentlyPickedObjects; // currently picking objects on picker.
    private readonly Vector3 forceVector = new Vector3(0f,50f,180f); // force to ball when came to checkpoint.
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate() // Move picker obj forward and left or right according to user input.
    {
        if (GameManager.Instance.gameStatus == GameManager.GameStatus.PLAY)
        {
            Vector3 nextPos = transform.position + new Vector3(InputManager.Instance.moveThreshold ? InputManager.Instance.swipeLeft ? moveSpeed : -moveSpeed : 0, 0f, moveSpeed);
            Vector3 clampedNexPos = new Vector3(Mathf.Clamp(nextPos.x,-0.35f,0.35f) , nextPos.y, nextPos.z);
            rigidBody.MovePosition(clampedNexPos);
        }
    }
    public void AddForceToPickedObject() // Force balls to shot them to checkpoint area.
    {
        for (int i = 0; i < currentlyPickedObjects.Count; i++)
        {
            currentlyPickedObjects[i].GetComponent<Rigidbody>().AddForce(forceVector);
        }
    }
    private void OnTriggerStay(Collider other) // Find currently picking objects.
    {
        if (other.TryGetComponent<Ball>(out Ball ball) && !currentlyPickedObjects.Contains(other.gameObject))
        {
            currentlyPickedObjects.Add(ball.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        currentlyPickedObjects.Remove(other.gameObject);
    }
}
