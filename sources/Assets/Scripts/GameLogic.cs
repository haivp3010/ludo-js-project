using UnityEngine;
using System.Collections.Generic;

public enum HorseColor
{
    None = -1,
    Red = 0,
    Blue = 1,
    Green = 2,
    Yellow = 3
    
}

public class PositionControl
{
    static PositionList PL = new PositionList();
    public static SortedList<int, Vector3> positionList = PL.List;
    private const int FirstSpawnPosition = 80;

    private static SortedList<HorseColor, int> StartPositionList = new SortedList<HorseColor, int>
    {
        { HorseColor.Red, 0 },
        { HorseColor.Blue, 12 },
        { HorseColor.Green, 24 },
        { HorseColor.Yellow, 36 }
        
    };
    private static SortedList<HorseColor, int> FirstCagePositionList = new SortedList<HorseColor, int>
    {
        { HorseColor.Red, 101 },
        { HorseColor.Blue, 201 },
        { HorseColor.Green, 301 },
        { HorseColor.Yellow, 401 }
        
    };
    private static SortedList<HorseColor, int> CageEntranceList = new SortedList<HorseColor, int>
    {
        { HorseColor.Red, 47 },
        { HorseColor.Blue, 11 },
        { HorseColor.Green, 23 },
        { HorseColor.Yellow, 35 }
    };
    public static Vector3 GetRealPosition(int position)
    {
        return positionList[position];
    }
    public static int GetStartPosition(HorseColor color)
    {
        return StartPositionList[color];
    }
    public static int GetCagePosition(HorseColor color, int cageNumber)
    {
        return FirstCagePositionList[color] + cageNumber - 1;
    }
    public static int GetNextPosition(HorseColor color, int currentPosition)
    {
        if (currentPosition == 47 && color != HorseColor.Red)
            return 0;
        if (CageEntranceList[color] == currentPosition)
            return FirstCagePositionList[color];
        else if (FirstCagePositionList.ContainsValue(currentPosition - 5))
            return -1;
        else
            return currentPosition + 1;
    }
    public static int GetSpawnPosition(int horseNumber)
    {
        return horseNumber + FirstSpawnPosition - 1;
    }

}

public class GameLogic : MonoBehaviour
{
    public const int NUMBER_OF_PLAYERS = 4; // Red, Blue, Yellow, Green
    public const int NUMBER_OF_HORSES_PER_PLAYER = 4;
    public const int NUMBER_OF_HORSES = NUMBER_OF_PLAYERS * NUMBER_OF_HORSES_PER_PLAYER;
    public static HorseColor GetHorseColor(int horseNumber)
    {
        if ((horseNumber >= 0) && (horseNumber < GameLogic.NUMBER_OF_HORSES))
        {
            return (HorseColor)(horseNumber / GameLogic.NUMBER_OF_HORSES_PER_PLAYER);
        }
        return HorseColor.None;
    }

    void Awake()
    {

    }
}
