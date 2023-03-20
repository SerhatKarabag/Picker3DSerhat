using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject endGamePopUp;
    [SerializeField] private Transform Canvas;
    [SerializeField] private TextMeshProUGUI levelText;
    public GameObject tapToStart;
    public int currentLevel;
    public void Start()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 1);
        levelText.text = "LEVEL " + currentLevel;
    }
    public void ShowEndGamePopUp()
    {
        GameObject popup = Instantiate(endGamePopUp, Canvas);
        popup.transform.GetComponentInChildren<Button>().onClick.AddListener(delegate () { GameManager.Instance.LoadGameScene(); });
        if (GameManager.Instance.gameStatus == GameManager.GameStatus.END) // level succesfully finished. Go next level.
        {
            PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
            popup.GetComponentInChildren<TextMeshProUGUI>().text = "NEXT LEVEL";
        }
        else // level failed so try again.
        {
            popup.GetComponentInChildren<TextMeshProUGUI>().text = "RESTART";
        }
    }
}
