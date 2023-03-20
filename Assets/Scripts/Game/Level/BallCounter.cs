using UnityEngine;

public class BallCounter : MonoBehaviour
{
    public CheckPoint checkPoint;
    private void Start()
    {
        checkPoint = transform.root.GetComponent<CheckPoint>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out Ball ball))
        {
            if (ball.collectable)
            {
                ball.collectable = false;
                checkPoint.collectedObjectCount++;
                checkPoint.ThresholdText.text = checkPoint.collectedObjectCount + " / " + checkPoint.Threshold;
            }
        }
    }
}
