using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ExplorerGridClicker : MonoBehaviour
{
    enum GameState 
    { 
        Init = 0,
        MissionUIShown,
        OnTheWayToDestination,
        ConversationShown,
        ConversationCloset,
        OnTheWayHome,
        ReachedHome
    }

    GameState state = GameState.Init;

    private System.DateTime date = new System.DateTime(2066, 6, 1, 12, 0, 0);

    public float DateTimeScale = 5000;

    private Grid grid;
    private Tilemap tilemap;
    private ExplorerTileInfo tileInfoHold;


    public Image overlayImage; // Reference to an UI Image (should cover entire screen)
    public Color morningColor = new Color(1f, 1f, 0.7f, 0.2f); // Light yellowish morning
    public Color middayColor = new Color(1f, 1f, 1f, 0f); // Fully transparent for midday
    public Color eveningColor = new Color(0.8f, 0.5f, 0.3f, 0.3f); // Orange tint for evening
    public Color nightColor = new Color(0f, 0f, 0.2f, 0.5f); // Dark blue tint for night

    public GameObject heroPrefab;
    public GameObject missionUI;

    public TMPro.TMP_Text txtTimeOfDay;

    private Vector3Int[] destinationTilePositions = new Vector3Int[] { };
    private Vector3Int[] homeTilePositions = new Vector3Int[] { };
    private Vector3Int[] heroTilePositions = new Vector3Int[] { };
    private GameObject[] activeTiles = new GameObject[] { };
    private Vector3[] activeTilesWorldDestibations = new Vector3[] { };
    private GUIStyle dateTimeStyle;

    private ExplorerTileInfo tileStart;
    private ExplorerTileInfo tileEnd;

    public static void ShuffleArray<T>(T[] array)
    {
        var rng = new System.Random();
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            // Swap elements
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    void Awake()
    {
        dateTimeStyle = new GUIStyle();
        dateTimeStyle.fontSize = 24; // You can adjust this value to change the font size
        dateTimeStyle.normal.textColor = Color.white; // Optional: change text color if needed

        grid = GetComponent<Grid>();
        tilemap = GetComponentInChildren<Tilemap>();

        overlayImage.enabled = true;
    }

    void FindAllTiles()
    {
        // Get the bounds of the Tilemap
        BoundsInt bounds = tilemap.cellBounds;
        // Create a list to store tile GameObjects
        
        var destPositions = new System.Collections.Generic.List<Vector3Int>();
        //var heroPositions = new System.Collections.Generic.List<Vector3Int>();
        var homePositions = new System.Collections.Generic.List<Vector3Int>();

        Vector3Int abuelaTilePos = Vector3Int.zero;
        Vector3Int lanceTilePos = Vector3Int.zero;
        Vector3Int stellaTilePos = Vector3Int.zero;

        // Iterate through each cell in the Tilemap bounds
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            // Get the tile at the current position
            TileBase tileBase = tilemap.GetTile(pos);
            Tile tile = tileBase as Tile;
            if (tile != null)
            {
                if (tile.name.StartsWith("arrowUp"))
                {
                    destPositions.Add(pos);
                }
                else if (tile.name.StartsWith("home"))
                {
                    homePositions.Add(pos);
                }
                else
                {
                    if (tile.name.ToLower().StartsWith("abuela"))
                        abuelaTilePos = pos;
                    else if (tile.name.ToLower().StartsWith("lance"))
                        lanceTilePos = pos;
                    else if (tile.name.ToLower().StartsWith("stella"))
                        stellaTilePos = pos;
                }
            }
        }

        destinationTilePositions = destPositions.ToArray();
        homeTilePositions = homePositions.ToArray();
        heroTilePositions = new Vector3Int[] { abuelaTilePos, lanceTilePos, stellaTilePos };
    }

    private void Start()
    {
        FindAllTiles();
    }

    void Update()
    {
        string dateTimeString = $"({GetTimeOfDayLabel(date)}) {date:yyyy MMMM} {GetDayWithSuffix(date.Day)} {date:HH:mm}";
        txtTimeOfDay.text = dateTimeString;

        if (state != GameState.OnTheWayHome && state != GameState.OnTheWayToDestination)
        {
            overlayImage.gameObject.SetActive(false);
        }
        else
        {
            overlayImage.gameObject.SetActive(true);

            // Cycle through a 24-hour period
            float hoursAsFloat = (float)date.TimeOfDay.TotalHours;
            float cycleDuration = 24f; // 24 hours in seconds for demonstration
            float timeOfDay = hoursAsFloat / cycleDuration;

            // Smoothly interpolate colors based on time of day
            if (timeOfDay < 0.25f) // Morning (6 AM to 12 PM)
                overlayImage.color = Color.Lerp(nightColor, morningColor, timeOfDay / 0.25f);
            else if (timeOfDay < 0.5f) // Midday (12 PM to 6 PM)
                overlayImage.color = Color.Lerp(morningColor, middayColor, (timeOfDay - 0.25f) / 0.25f);
            else if (timeOfDay < 0.75f) // Evening (6 PM to 12 AM)
                overlayImage.color = Color.Lerp(middayColor, eveningColor, (timeOfDay - 0.5f) / 0.25f);
            else // Night (12 AM to 6 AM)
                overlayImage.color = Color.Lerp(eveningColor, nightColor, (timeOfDay - 0.75f) / 0.25f);
        }

        if (state == GameState.OnTheWayToDestination || state == GameState.OnTheWayHome)
        {
            date = date.AddSeconds(Time.deltaTime * DateTimeScale);

            int heroReachedDestination = 0;

            for (int i = 0; i != activeTiles.Length; ++i)
            {
                var activeTile = activeTiles[i];
                var activeTileWorldDestibation = activeTilesWorldDestibations[i];

                var oldPosition = activeTile.transform.position;
                var dir = activeTileWorldDestibation - activeTile.transform.position;
                activeTile.transform.position += Time.deltaTime * dir.normalized * WalkingSpeed(date);

                var A = activeTileWorldDestibation - oldPosition;
                var B = activeTileWorldDestibation - activeTile.transform.position;
                bool overshoot = Vector3.Dot(A, B) <= 0.1f;
                if (overshoot)
                {
                    activeTile.transform.position = activeTileWorldDestibation;
                    heroReachedDestination++;
                }
            }

            if (heroReachedDestination == 2)
            {
                if (state == GameState.OnTheWayToDestination)
                {
                    state = GameState.ConversationShown;

                    var missionDialogue = ExplorationManager.Instance.GetMissionDialogue();
                    ExplorationManager.Instance.StartDialogue(missionDialogue, OnEndMissionDialogue);
                }
                else
                {
                    state = GameState.ReachedHome;
                    SceneManager.LoadScene("Camp");
                }
            }
            return;
        }

        bool isMouseLeftDown = Input.GetMouseButtonDown(0);
        bool isMouseLeftUp = Input.GetMouseButtonUp(0);
        //bool isMouseRight = Input.GetMouseButtonDown(1);
        if (isMouseLeftDown) // Detect left mouse click
        {
            ExplorerTileInfo info = GetTileInfoAtMouse();

            if (info.clickedTile != null)
            {
                Debug.Log($"Clicked on cell: {info.cellPosition} with tile: {info.clickedTile.name}");
                tileInfoHold = info;
            }
            else
            {
                Debug.Log($"Clicked on empty cell: {info.cellPosition}");
            }

        }
        if (isMouseLeftUp)
        {
            if (tileInfoHold?.clickedTile != null)
            {
                ExplorerTileInfo info = GetTileInfoAtMouse();

                bool fromHeroToForest = tileInfoHold.IsHero&& info.IsForest;
                bool fromHeroToHome   = tileInfoHold.IsHero&& info.IsHome;
                bool fromForestToHero = tileInfoHold.IsForest&& info.IsHero;
                bool fromForestToHome = tileInfoHold.IsForest&& info.IsHome;

                if (fromHeroToForest && state == GameState.Init && GameManager.Instance.GetCurrentMission() == Mission.GatherWood 
                 || fromHeroToHome && state == GameState.Init && GameManager.Instance.GetCurrentMission() == Mission.SetUpShelter)
                {
                    state = GameState.MissionUIShown;
                    missionUI.SetActive(true);

                    tileStart = tileInfoHold;
                    tileEnd   = info;
                }
            }
            tileInfoHold = null;

            if (state == GameState.MissionUIShown && missionUI.activeSelf == false)
            {
                state = GameState.OnTheWayToDestination;

                ShuffleArray(destinationTilePositions);

                if (GameManager.Instance.GetCurrentMission() == Mission.SetUpShelter)
                {
                    // HACK for second mission, just stay at the home base
                    destinationTilePositions = new Vector3Int[] {
                        new Vector3Int(-1, 0),new Vector3Int(0, 0),new Vector3Int(-1, 1),new Vector3Int(0, 1),
                    };
                }

                //Get selected characters
                List<int> characterIndice = new List<int>();

                foreach (var character in ExplorationManager.Instance.SelectedCharacters)
                {
                    switch (character.Name)
                    {
                        case CharacterNames.Abuela:
                            characterIndice.Add(0);
                            break;
                        case CharacterNames.Lance:
                            characterIndice.Add(1);
                            break;
                        case CharacterNames.Stella:
                            characterIndice.Add(2);
                            break;
                    }
                }

                List<Vector3> destinations = new List<Vector3>();
                List<GameObject> gameObjects = new List<GameObject>();
                foreach(int i in characterIndice)
                {
                    //var activeTile = activeTiles[i];
                    var activeDestination = destinationTilePositions[i];

                    //Debug.Log($"Dragged from {tileStart.clickedTile.name} to {tileEnd.clickedTile.name}");
                    var activeTile = GameObject.Instantiate(heroPrefab);
                    var spriteRenderer = activeTile.GetComponent<SpriteRenderer>();
                    {
                        //Tile tile = (Tile)tileStart.clickedTile;
                        Tile tile = (Tile)tilemap.GetTile(heroTilePositions[i]);
                        spriteRenderer.sprite = tile.sprite;
                        var newTile = ScriptableObject.Instantiate((Tile)tilemap.GetTile(homeTilePositions[0]));
                        newTile.sprite = ((Tile)tilemap.GetTile(homeTilePositions[0])).sprite;
                        tilemap.SetTile(heroTilePositions[i], newTile);
                    }

                    Vector3Int cellPosition = tileStart.cellPosition;
                    activeTile.gameObject.transform.position = GetMidpointForCell(cellPosition);
                    //activeTile.gameObject.transform.position = grid.CellToWorld(cellPosition);
                    //activeTileWorldDestibation = GetMidpointForCell(tileEnd.cellPosition);
                    destinations.Add(GetMidpointForCell(activeDestination));
                    gameObjects.Add(activeTile);
                }
                activeTilesWorldDestibations = destinations.ToArray();
                activeTiles = gameObjects.ToArray();
            }
            if (state == GameState.ConversationCloset)
            {
                state = GameState.OnTheWayHome;

                FindAllTiles();
                ShuffleArray(homeTilePositions);

                List<GameObject> gameObjects = new List<GameObject>();
                for (int i = 0; i != 2; ++i)
                {
                    //var activeTile = activeTiles[i];
                    var activeDestination = homeTilePositions[i];

                    Debug.Log($"Dragged from {tileStart.clickedTile.name} to {tileEnd.clickedTile.name}");
                    /*
                    var activeTile = activeTiles[i];
                    var spriteRenderer = activeTile.GetComponent<SpriteRenderer>();
                    {
                        //Tile tile = (Tile)tileStart.clickedTile;
                        Tile tile = (Tile)tilemap.GetTile(heroTilePositions[i]);
                        spriteRenderer.sprite = tile.sprite;
                        var newTile = ScriptableObject.Instantiate((Tile)tilemap.GetTile(homeTilePositions[0]));
                        newTile.sprite = ((Tile)tilemap.GetTile(homeTilePositions[0])).sprite;
                        tilemap.SetTile(heroTilePositions[i], newTile);
                    }

                    Vector3Int cellPosition = tileStart.cellPosition;
                    activeTile.gameObject.transform.position = GetMidpointForCell(cellPosition);
                    //activeTile.gameObject.transform.position = grid.CellToWorld(cellPosition);
                    //activeTileWorldDestibation = GetMidpointForCell(tileEnd.cellPosition);
                    destinations.Add(GetMidpointForCell(activeDestination));
                    gameObjects.Add(activeTile);
                    */
                    activeTilesWorldDestibations[i] = activeDestination;
                }
            }
        }
    }

    private void OnEndMissionDialogue()
    {
        state = GameState.ConversationCloset;
    }

    private Vector3 GetMidpointForCell(Vector3Int cellPosition)
    {
        return (grid.CellToWorld(cellPosition) + grid.CellToWorld(cellPosition + new Vector3Int(1,1,0))) * 0.5f;
    }

    private ExplorerTileInfo GetTileInfoAtMouse()
    {
        Vector3 screenPosition = Input.mousePosition;
        return GetTileInfoAtScreenPosition(screenPosition);
    }

    private ExplorerTileInfo GetTileInfoAtScreenPosition(Vector3 screenPosition)
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        mouseWorldPosition.z = 0;

        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPosition);

        // Optionally, check if a tile exists
        TileBase clickedTile = tilemap.GetTile(cellPosition);

        ExplorerTileInfo info = new ExplorerTileInfo();
        info.cellPosition = cellPosition;
        info.clickedTile = clickedTile;
        return info;
    }

    // Method to get the day with an ordinal suffix
    private string GetDayWithSuffix(int day)
    {
        switch (day % 10)
        {
            case 1: return day + "st";
            case 2: return day + "nd";
            case 3: return day + "rd";
            case 11:
            case 12:
            case 13:
            default: return day + "th";
        }
    }

    public static string GetTimeOfDayLabel(System.DateTime dateTime)
    {
        int hour = dateTime.Hour;

        return hour switch
        {
            >= 5 and < 10 => "Morning",
            10 or > 10 and < 12 => "Daytime",
            12 => "Noon",
            > 12 and < 17 => "Afternoon",
            >= 17 and < 21 => "Evening",
            _ => "Night"
        };
    }

    public static float WalkingSpeed(System.DateTime dateTime)
    {
        int hour = dateTime.Hour;

        return hour switch
        {
            >= 22 or < 5 => 0f, // Night (Resting speed)
            >= 12 and <= 14 => 1f, // Midday (Top speed)
            >= 5 and < 12 => Mathf.Lerp(0f, 1f, (hour - 5) / 7f), // Morning (Increasing speed)
            _ => Mathf.Lerp(1f, 0f, (hour - 14) / 8f) // Afternoon/Evening (Decreasing speed)
        };
    }
}
