using UnityEngine;

public static class FileUtils
{
    public static LevelInfo LoadLevel(int levelNumber)
    {
        return Resources.Load<LevelInfo>($"Levels/{levelNumber}");
    }

    public static Texture LoadTexture(string fileName)
    {
        return Resources.Load($"Editor/{fileName}") as Texture;
    }

    public static bool FileExists(string path)
    {
        LevelInfo level = Resources.Load<LevelInfo>(path);
        return level != null;
    }
    public static GameObject[] LoadFromResourcesPath(string path)
    {
        return Resources.LoadAll<GameObject>(path);
    }
}
