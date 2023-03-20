using System.Linq;
using UnityEngine;

public class LevelGenerator : Singleton<LevelGenerator>
{
    private LevelInfo levelData;
    private GameObject[] platformParts; // collect area, checkpoint, end area etc..
    private GameObject[] ballPacks; // Horizontal, LetterMixed, Stickman etc..
    private GameObject createdObject; // currently creating platform part.
    private GameObject previousCreatedObject; // previous createrd platform part.
    private void Awake()
    {
        GetAssetFromResources(); // Get asset then create level.
    }
    public void InitializeLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("currentLevel", 1);
        if (currentLevel > 10)
        {
            currentLevel = Random.Range(1, 11);
        }
        GetLevelData(currentLevel);

    }
    private void GetLevelData(int levelNum) // Get levelData which we create from level editor.
    {
        levelData = FileUtils.LoadLevel(levelNum);
        CreateLevel();
    }
    private void CreateLevel() // Creates level by levelData. Joins the platform parts end-to-end. Creates balls on CollectArea and sets checkpoint threshold.
    {
        for (int i = 0; i < levelData.Tiles.Count; i++)
        {
            System.Type type = levelData.Tiles[i].GetType();
            switch (type)
            {
                case System.Type t when type == typeof(CollectAreaTile):
                    createdObject = Instantiate(platformParts.Where(o => o.TryGetComponent<CollectArea>(out CollectArea ca))
                        .FirstOrDefault(), Vector3.zero, Quaternion.identity);
                    if (i > 0)
                    {
                        createdObject.transform.position = previousCreatedObject.transform.position + new Vector3(0f, 0f, previousCreatedObject.transform.localScale.z / 2) +
                        new Vector3(0f, 0f, createdObject.transform.localScale.z / 2);
                    }
                    CollectAreaTile collectAreaTile = levelData.Tiles[i] as CollectAreaTile;
                    Instantiate(ballPacks.Where(b => b.GetComponent<BallPack>().type ==collectAreaTile.ballPackType) // create collectable balls on collectArea
                      .FirstOrDefault(), createdObject.transform);
                    previousCreatedObject = createdObject;
                    break;
                case System.Type t when type == typeof(CheckPointTile):
                    createdObject = Instantiate(platformParts.Where(o => o.TryGetComponent<CheckPoint>(out CheckPoint cp))
                       .FirstOrDefault(), Vector3.zero, Quaternion.identity);
                    CheckPointTile checkPointTile = levelData.Tiles[i] as CheckPointTile;
                    CheckPoint checkPoint = createdObject.GetComponent<CheckPoint>();
                    checkPoint.Threshold = checkPointTile.Threshold;
                    checkPoint.ThresholdText.text = checkPoint.collectedObjectCount + " / " + checkPoint.Threshold.ToString();
                    if (i > 0)
                    {
                        createdObject.transform.position = previousCreatedObject.transform.position + new Vector3(0f, 0f, previousCreatedObject.transform.localScale.z / 2) +
                        new Vector3(0f, 0f, createdObject.transform.localScale.z / 2);
                    }
                    previousCreatedObject = createdObject;
                    break;
                case System.Type t when type == typeof(EndGameTile):
                    createdObject = Instantiate(platformParts.Where(o => o.TryGetComponent<EndGame>(out EndGame eg))
                       .FirstOrDefault(), Vector3.zero, Quaternion.identity);
                    if (i > 0)
                    {
                        createdObject.transform.position = previousCreatedObject.transform.position + new Vector3(0f, 0f, previousCreatedObject.transform.localScale.z / 2) +
                        new Vector3(0f, 0f, createdObject.transform.localScale.z / 2);
                    }
                    previousCreatedObject = createdObject;
                    break;
                default:
                    break;
            }
        }
    }
    private void GetAssetFromResources()
    {
        platformParts = FileUtils.LoadFromResourcesPath("PlatformParts");
        ballPacks = FileUtils.LoadFromResourcesPath("BallPacks");
        InitializeLevel();
    }
}