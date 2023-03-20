using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class LevelEditorTab : EditorTab
{
    //LEVEL DATA
    private Object levelDataObject; // The object which holds our levelInfo.
    private LevelInfo currentLevelInfo; // Scriptable object that holds level info currently selected from EditorGUILayout.ObjectField.
    private readonly Dictionary<string, Texture> tileTextures = new Dictionary<string, Texture>(); // Textures for level design.
    

    // ENUMS
    private enum PlatformType
    {
        collectArea, // The area which is given for the player to collect objects.
        checkPoint, // Area to check that a sufficient number of objects have been collected.
        endGame // Area to indicate game finished.
    }
    private enum FillMode
    {
        Tile, // Fills only one tile with selected PlatformType.
        Row // Fills all the tiles with selected PlatformType.
    }
    private enum BallPackType
    {
        Horizontal,
        LetterMixed,
        StickMans
    }

    // HELPERS
    private Vector2 scrollPos;
    private int previousPlatformLenght;
    private const float LevelTileButtonSize = 90f;
    private PlatformType currentPlatformType;
    private FillMode currentFillMode;

    public LevelEditorTab(GameEditor editor) : base(editor)
    {
        DirectoryInfo editorImagesPath = new DirectoryInfo(Application.dataPath + "/Resources/Editor"); // set directory path.
        FileInfo[] fileInfo = editorImagesPath.GetFiles("*.png", SearchOption.TopDirectoryOnly); // get png files.
        foreach (FileInfo file in fileInfo)
        {
            string filename = Path.GetFileNameWithoutExtension(file.Name);
            tileTextures[filename] = FileUtils.LoadTexture(filename);// add pngs to dictionary.
        }
    }

    public override void Draw() // Draw the selected editor tab. virtual method.
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        float oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 90;
        GUILayout.Space(15);
        DrawSelectLevelField(); // Draws ObjectField for select LevelInfo scriptable obj.

        if (currentLevelInfo != null)
        {
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            DrawLevel(); // Draws level accordingly levelInfo object.
            GUILayout.EndHorizontal();
        }

        EditorGUIUtility.labelWidth = oldLabelWidth;
        EditorGUILayout.EndScrollView();

        if (currentLevelInfo != null && GUI.changed)
            EditorUtility.SetDirty(currentLevelInfo); // You can use SetDirty when you want to modify an object without creating an undo entry.
    }

    private void DrawSelectLevelField() // Draws ObjectField for select LevelInfo scriptable obj.
    {
        Object oldDb = levelDataObject;
        levelDataObject = EditorGUILayout.ObjectField("Level Info Asset", levelDataObject, typeof(LevelInfo), false, GUILayout.Width(340));
        if (levelDataObject != oldDb)
        {
            currentLevelInfo = (LevelInfo)levelDataObject; // Set current levelInfo.
        }
    }

    private void DrawLevel()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(500));

        GUIStyle style = new GUIStyle
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };
        EditorGUILayout.LabelField("Level Design", style);
        GUILayout.Space(10);
        GUILayout.BeginHorizontal(GUILayout.Width(600)); // How to use.
        EditorGUILayout.HelpBox(
            "How to use?" + "\n" + "Enter a value to Level Number input field in order to set which level is this." +
             " Next, determine the number of platform pieces. The platform consists of 3 different pieces. 'Collect Area' is the area where the player will collect objects before coming to the checkpoint." +
             "'Checkpoint' is the area that will check the numerical value of the collected objects. 'EndGame' indicates the end of the platform and level. " +
             "The platform will be created automatically according to the combinations made. On the left side of the 'Collect Area' box, there will be an area for you to choose the ball that can be collected." +
             " When you add a CheckPoint, you must enter the numerical value that this checkpoint will control on the left side of the box." + "\n" + "\n" +


             "Nasıl kullanılır?" + "\n" + "İlk olarak düzenlenen assetin hangi level'a ait olduğunu ayarlamak için Level Number karşısına değer girin." +
            " Sonrasında platform parça sayısını belirleyin. Platform 3 farklı parçadan oluşur. 'Collect Area' oyuncunun checkpoint'e gelmeden önce objeleri toplayacağı alandır." +
            "'Checkpoint' ise toplanan objelerin sayısal değerini kontrol edecek olan alandır. 'EndGame' platformun ve seviyenin bitişini belirtir. " +
            "Yapılan kombinasyonalra göre platform otomatik olarak oluşturulacaktır. 'Collect Area' eklediğiniz kutunun hemen sol yanında Collect edilebilecek top seçimi yapmanız için bir alan oluşur." +
            " CheckPoint eklediğiniz zaman ise hemen kutunun sol tarafında bu checkpoint noktasının kontrol edeceği sayısal değeri girmeniz gerekir.",
            MessageType.Info);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Level number", "The number of this level."), // Level number input field.
            GUILayout.Width(EditorGUIUtility.labelWidth));
        currentLevelInfo.LevelNumber = EditorGUILayout.IntField(currentLevelInfo.LevelNumber, GUILayout.Width(30));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        previousPlatformLenght = currentLevelInfo.PlatformLenght;

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Platform Lenght", "The number of parts the platform consist of."), // Platform lenght input field.
            GUILayout.Width(EditorGUIUtility.labelWidth));
        currentLevelInfo.PlatformLenght = EditorGUILayout.IntField(currentLevelInfo.PlatformLenght, GUILayout.Width(30));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Platform type", "The type of platform to design the level."), // Platform type enumpopup.
            GUILayout.Width(EditorGUIUtility.labelWidth));
        currentPlatformType = (PlatformType)EditorGUILayout.EnumPopup(currentPlatformType, GUILayout.Width(100));
        GUILayout.EndHorizontal();




        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Fill mode", "The way of filling the empty tiles produced up to the platform length."), // Select fill mode to fill one tile or row.
            GUILayout.Width(EditorGUIUtility.labelWidth));
        currentFillMode = (FillMode)EditorGUILayout.EnumPopup(currentFillMode, GUILayout.Width(100));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        if (currentLevelInfo.PlatformLenght != previousPlatformLenght)
        {
            if (currentLevelInfo.Tiles != null)
            {
                foreach (Tile tile in currentLevelInfo.Tiles)
                        Object.DestroyImmediate(tile, true);
            }

            if (currentLevelInfo.PlatformLenght > 0)
            {
                currentLevelInfo.Tiles = new List<Tile>();
                currentLevelInfo.Tiles.AddRange(Enumerable.Repeat<Tile>(null, currentLevelInfo.PlatformLenght)); // Add empty tiles to fill later
            }
        }

        if (currentLevelInfo.Tiles != null)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < currentLevelInfo.Tiles.Count; i++)
            {
                CreateButton(i); // create button for every tiles.
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }



    private void CreateButton(int index)
    {
        string tileTypeName = string.Empty;
        if (currentLevelInfo.Tiles.Count == 0) return;
        Tile tile = currentLevelInfo.Tiles[index];
        if (tile != null) // behave by tile type
        {
            CollectAreaTile collectAreaTile = tile as CollectAreaTile;
            if (collectAreaTile != null)
            {
                tileTypeName = "collectArea";
                GUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(new GUIContent("Collectable" + "\n" + "BallPacks", "Type of objects to be placed on Collect Area"),
                    GUILayout.Width(EditorGUIUtility.labelWidth), GUILayout.Height(60));
                collectAreaTile.ballPackType = (BallPack.BallPackType)EditorGUILayout.EnumPopup(collectAreaTile.ballPackType, GUILayout.Width(100)); // Ballpack input field for collectArea tiles.
                GUILayout.EndVertical();
            }

            CheckPointTile checkPointTile = tile as CheckPointTile;
            if (checkPointTile != null)
            {
                tileTypeName = "checkPoint";
                GUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(new GUIContent("Checkpoint"+ "\n" +"Threshold", "The amount of balls to be dropped at the checkpoint."),
                    GUILayout.Width(EditorGUIUtility.labelWidth), GUILayout.Height(60));
                checkPointTile.Threshold = EditorGUILayout.IntField(checkPointTile.Threshold, GUILayout.Width(30)); // Threshold input field for checkpoint tiles.
                GUILayout.EndVertical();
            }

            EndGameTile endGameTile = tile as EndGameTile;
            if (endGameTile != null)
            {
                tileTypeName = "endGame";
            }

            if (GUILayout.Button(tileTextures[tileTypeName], GUILayout.Width(LevelTileButtonSize), // Draw tile by type
                GUILayout.Height(LevelTileButtonSize)))
                DrawTileController(index);
        }
        else
        {
            if (GUILayout.Button("", GUILayout.Width(LevelTileButtonSize), // Draw empty box
                GUILayout.Height(LevelTileButtonSize)))
                DrawTileController(index);
        }
    }

    private void DrawTileController(int index) // Draw tile or row
    {
        switch (currentFillMode)
        {
            case FillMode.Tile:
                currentLevelInfo.Tiles[index] = DrawTile(currentLevelInfo.Tiles[index],index);
                break;

            case FillMode.Row:
                {
                    for (int i = 0; i < currentLevelInfo.Tiles.Count; i++)
                    {
                        currentLevelInfo.Tiles[i] = DrawTile(currentLevelInfo.Tiles[i], index);
                    }
                }
                break;
        }
    }

    private Tile DrawTile(Tile tile, int index)
    {
        Object.DestroyImmediate(tile, true);

        Tile tileInfo = CreateInstanceByType(); // Create scriptable obj instance
        
        if (tileInfo != null && !AssetDatabase.IsSubAsset(tileInfo))
            AssetDatabase.AddObjectToAsset(tileInfo, currentLevelInfo); // Add produced instance to levelData

        return tileInfo;
    }

    private Tile CreateInstanceByType()
    {
        switch (currentPlatformType) 
        {
            case PlatformType.collectArea:
                {
                    CollectAreaTile tile = ScriptableObject.CreateInstance<CollectAreaTile>();
                    tile.hideFlags = HideFlags.HideInHierarchy;
                    return tile;
                }

            case PlatformType.checkPoint:
                {
                    CheckPointTile tile = ScriptableObject.CreateInstance<CheckPointTile>();
                    tile.hideFlags = HideFlags.HideInHierarchy;
                    return tile;
                }

            case PlatformType.endGame:
                {
                    EndGameTile tile = ScriptableObject.CreateInstance<EndGameTile>();
                    tile.hideFlags = HideFlags.HideInHierarchy;
                    return tile;
                }
            default:
                return null;
        }
    }
}

