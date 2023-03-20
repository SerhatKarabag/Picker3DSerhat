using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckPoint : Platform // checkpoint object which is platform part.
{
    // CHECKPOINT
    public TextMeshPro ThresholdText; // Text to show player how many object they collect.
    public int collectedObjectCount = 0; // Currently collected object for checkpoint.
    public int Threshold; // Target collect count.
    
    // BARRIERS
    [SerializeField] private GameObject barrierLeft; // Left barrier arm of checkpoint.
    [SerializeField] private GameObject barrierRight; // Right barrier arm of checkpoint.
    private const float barrierOpenDuration = 1f; // in how many seconds the barrier opens.
    private const float platformUpDuration = 1.2f; // in how many seconds the platform lifts up.
    private Vector3 leftBarrierRotateAngle = new Vector3(180f, -90, -90); // endValue of barrier left rotation.
    private Vector3 rightBarrierRotateAngle = new Vector3(180f, 90, 90); // endValue of barrier right rotation.
    private readonly Vector3 counterPlatformUpPosition = new Vector3(0f, 0.445f, 0f); // endValue of platform LocalPosition while lifting up.
    public override PlatformType platformType // Type of this platform - overrided.
    {
        get { return PlatformType.checkPoint; }
    }
    public IEnumerator WaitForObjectCount(BallCounter ballCounter) // Wait for ball count calculation. 
    {
        yield return new WaitForSeconds(1.5f);
        if (collectedObjectCount >= Threshold) // If player picked enough.
        {
            ballCounter.GetComponent<BoxCollider>().enabled = false; // Let the balls drop.
            ballCounter.transform.DOLocalMove(counterPlatformUpPosition, platformUpDuration).SetEase(Ease.Linear).OnComplete(() => // Lift the platform up.
            {
                GameManager.Instance.SetGameStatus(GameManager.GameStatus.PLAY);
                OpenTheBarrier();
            });
        }
        else
        {
            GameManager.Instance.SetGameStatus(GameManager.GameStatus.FAIL);
            GameManager.Instance.LevelFinishedorFailed();
        }
    }
    public void OpenTheBarrier() // Open the barrier if collected object count is enough for checkpoint.
    {
        barrierLeft.transform.DORotate(leftBarrierRotateAngle, barrierOpenDuration);
        barrierRight.transform.DORotate(rightBarrierRotateAngle, barrierOpenDuration);
    }
}
