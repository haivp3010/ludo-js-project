using UnityEngine;
using System.Collections.Generic;

public enum HorseColor
{
    None = -1,
    Red = 0,
    Blue = 1,
    Yellow = 2,
    Green = 3
}

public class PositionControl
{
    private const int FirstSpawnPosition = 80;
    private static SortedList<int, Vector3> RealPositionList = new SortedList<int, Vector3>
    {
        { 0, new Vector3(2.93f, 1.21f) },
        { 1, new Vector3(2.35f, 1.21f) },
        { 2, new Vector3(1.77f, 1.21f) },
        { 3, new Vector3(1.19f, 1.21f) }
    };
    private static SortedList<HorseColor, int> StartPositionList = new SortedList<HorseColor, int>
    {
        { HorseColor.Red, 0 },
        { HorseColor.Blue, 14 },
        { HorseColor.Yellow, 28 },
        { HorseColor.Green, 42 }
    };
    private static SortedList<HorseColor, int> FirstCagePositionList = new SortedList<HorseColor, int>
    {
        { HorseColor.Red, 56 },
        { HorseColor.Blue, 62 },
        { HorseColor.Yellow, 68 },
        { HorseColor.Green, 74 }
    };
    public static Vector3 GetRealPosition(int position)
    {
        return RealPositionList[position];
    }
    public static int GetStartPosition(HorseColor color)
    {
        return StartPositionList[color];
    }
    public static int GetCagePosition(HorseColor color, int cageNumber)
    {
        return FirstCagePositionList[color] + cageNumber - 1;
    }
    public static int GetSpawnPosition(int horseNumber)
    {
        return horseNumber + FirstSpawnPosition - 1;
    }

}

public class GameState
{
    // Singleton Instance
    private static GameState instance = null;

    // Constants
    public const int NUMBER_OF_PLAYERS = 4; // Red, Blue, Yellow, Green
    public const int NUMBER_OF_HORSES_PER_PLAYER = 4;
    public const int NUMBER_OF_HORSES = NUMBER_OF_PLAYERS * NUMBER_OF_HORSES_PER_PLAYER;
    /*
        Horse number:
        0  -  3 : Red
        4  -  7 : Blue
        8  - 11 : Yellow
        12 - 15 : Green
    */

    // Private variables
    private HorseColor currentPlayer;
    private List<int> horsePosition;
    private int currentDiceValue; // 1 - 6

    // Properties
    public HorseColor CurrentPlayer
    {
        get
        {
            return currentPlayer;
        }
        set
        {
            currentPlayer = value;
        }
    }
    public List<int> HorsePosition
    {
        get
        {
            return horsePosition;
        }
        set
        {
            horsePosition = value;
        }
    }
    public int CurrentDiceValue
    {
        get
        {
            return currentDiceValue;
        }
        set
        {
            currentDiceValue = value;
        }
    }

    // Methods
    public static HorseColor GetHorseColor(int horseNumber)
    {
        if ((horseNumber >= 0) && (horseNumber < NUMBER_OF_HORSES))
        {
            return (HorseColor)(horseNumber / NUMBER_OF_HORSES_PER_PLAYER);
        }
        return HorseColor.None;
    }

    public void MoveHorseForward(int horseNumber, int steps)
    {
        
    }

    // Constructor and property to get singleton instance
    private GameState()
    {
        currentPlayer = HorseColor.Red;
        horsePosition = new List<int>(NUMBER_OF_HORSES);
        currentDiceValue = 1;
    }
    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameState();
            }
            return instance;
        }
    }
}
