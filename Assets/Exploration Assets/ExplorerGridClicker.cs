using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ExplorerGridClicker : MonoBehaviour
{
    private System.DateTime date = new System.DateTime(2066, 6, 1, 12, 0, 0);

    public float DateTimeScale = 5000;

    private Grid grid;
    private Tilemap tilemap;
    private ExplorerTileInfo tileInfoHold;

    public GameObject[] heroPrefabs;

    public Image overlayImage; // Reference to an UI Image (should cover entire screen)
    public Color morningColor = new Color(1f, 1f, 0.7f, 0.2f); // Light yellowish morning
    public Color middayColor = new Color(1f, 1f, 1f, 0f); // Fully transparent for midday
    public Color eveningColor = new Color(0.8f, 0.5f, 0.3f, 0.3f); // Orange tint for evening
    public Color nightColor = new Color(0f, 0f, 0.2f, 0.5f); // Dark blue tint for night

    private GameObject activeTile;
    private Vector3 activeTileWorldDestibation;
    private GUIStyle dateTimeStyle;

    void Awake()
    {
        dateTimeStyle = new GUIStyle();
        dateTimeStyle.fontSize = 24; // You can adjust this value to change the font size
        dateTimeStyle.normal.textColor = Color.white; // Optional: change text color if needed

        grid = GetComponent<Grid>();
        tilemap = GetComponentInChildren<Tilemap>();

        overlayImage.enabled = true;
    }

    void Update()
    {

        {
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

        if (activeTile != null)
        {
            date = date.AddSeconds(Time.deltaTime * DateTimeScale);

            var oldPosition = activeTile.transform.position;
            var dir = activeTileWorldDestibation - activeTile.transform.position;
            activeTile.transform.position += Time.deltaTime * dir.normalized * WalkingSpeed(date);

            var A = activeTileWorldDestibation - oldPosition;
            var B = activeTileWorldDestibation - activeTile.transform.position;
            bool overshoot = Vector3.Dot(A, B) <= 0;
            if (overshoot)
            {
                activeTile.transform.position = activeTileWorldDestibation;
                activeTile = null;
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
                if (info.clickedTile != null)
                {
                    Debug.Log($"Dragged from {tileInfoHold.clickedTile.name} to {info.clickedTile.name}");
                    activeTile = GameObject.Instantiate(heroPrefabs[0]);
                    Vector3Int cellPosition = tileInfoHold.cellPosition;
                    activeTile.gameObject.transform.position = GetMidpointForCell(cellPosition);
                    activeTileWorldDestibation = GetMidpointForCell(info.cellPosition);
                }
            }
            tileInfoHold = null;
        }
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

    public void OnGUI()
    {
        string dateTimeString = $"({GetTimeOfDayLabel(date)}) {date:yyyy MMMM} {GetDayWithSuffix(date.Day)} {date:HH:mm}";

        GUI.Label(new Rect(10, 10, 300, 50),  dateTimeString, dateTimeStyle);

    }
}
