using UnityEngine;

public class StopPickerTrigger : MonoBehaviour
{
    private CheckPoint basePoint;
    [SerializeField]private BallCounter ballCounter;
    private void Start()
    {
        basePoint = GetComponentInParent<CheckPoint>();
    }
    private void OnTriggerEnter(Collider other) // Stops picker object when near to checkpoint.
    {
        if (other.gameObject.CompareTag("Picker"))
        {
            GameManager.Instance.gameStatus = GameManager.GameStatus.WAIT;
            PickerController.Instance.AddForceToPickedObject();
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(basePoint.WaitForObjectCount(ballCounter));
        }
    }
    
}
