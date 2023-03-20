using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Picker"))
        {
            GetComponent<BoxCollider>().enabled = false;
            GameManager.Instance.SetGameStatus(GameManager.GameStatus.END);
            GameManager.Instance.LevelFinishedorFailed();
        }
    }
}
