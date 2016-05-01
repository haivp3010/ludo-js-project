using UnityEngine;
using System.Collections.Generic;
using System;

public enum HorseColor
{
    Red = 0,
    Blue = 1,
    Yellow = 2,
    Green = 3
}

public class GameStateBase
{
    // Constants
    /*
        Horse number:
        0  -  3 : Red
        4  -  7 : Blue
        8  - 11 : Yellow
        12 - 15 : Green
    */
    public const int NUMBER_OF_PLAYERS = 4;
    public const int NUMBER_OF_HORSES_PER_PLAYER = 4;
    public const int NUMBER_OF_HORSES = NUMBER_OF_PLAYERS * NUMBER_OF_HORSES_PER_PLAYER;
    public const int NUMBER_OF_CAGES_PER_COLOR = 6;

    /* 3 position groups:
        - Road:      0 - 55
        - Cages:    56 - 79
        - Spawns:   80 - 95
    */
    public const int NUMBER_OF_POSITIONS = 96;
    public const int NUMBER_OF_ROAD_POSITIONS = 56;
    public const int FIRST_SPAWN_POSITION = 80;

    // Struct for X-Y reference
    private struct XYPair
    {
        public int x, y;
        public XYPair(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    };

    // Private data
    private static SortedList<int, float> realPositionXList;
    private static SortedList<int, float> realPositionYList;
    private static SortedList<int, XYPair> realPositionXYRef;
    private static SortedList<int, Vector3> realPositionList;
    private static SortedList<HorseColor, int> startPositionList;
    private static SortedList<HorseColor, int> cageEntrancePositionList;
    private static SortedList<HorseColor, int> firstCagePositionList;
    
    // Constructor
    static GameStateBase()
    {
        realPositionXList = new SortedList<int, float>
        {
            { -7, -6.33f },
            { -6, -5.75f },
            { -5, -5.17f },
            { -4, -4.59f },
            { -3, -4.01f },
            { -2, -3.43f },
            { -1, -2.85f },
            {  0, -1.70f },
            {  1, -0.55f },
            {  2,  0.03f },
            {  3,  0.61f },
            {  4,  1.19f },
            {  5,  1.77f },
            {  6,  2.35f },
            {  7,  2.93f },
        };

        realPositionYList = new SortedList<int, float>
        {
            { -7, -4.57f },
            { -6, -3.99f },
            { -5, -3.41f },
            { -4, -2.83f },
            { -3, -2.25f },
            { -2, -1.67f },
            { -1, -1.09f },
            {  0,  0.06f },
            {  1,  1.21f },
            {  2,  1.79f },
            {  3,  2.37f },
            {  4,  2.95f },
            {  5,  3.53f },
            {  6,  4.11f },
            {  7,  4.69f },
        };

        realPositionXYRef = new SortedList<int, XYPair>
        {
            // Path - Red
            {  0, new XYPair( 7,  1) },
            {  1, new XYPair( 6,  1) },
            {  2, new XYPair( 5,  1) },
            {  3, new XYPair( 4,  1) },
            {  4, new XYPair( 3,  1) },
            {  5, new XYPair( 2,  1) },
            {  6, new XYPair( 1,  1) },
            {  7, new XYPair( 1,  2) },
            {  8, new XYPair( 1,  3) },
            {  9, new XYPair( 1,  4) },
            { 10, new XYPair( 1,  5) },
            { 11, new XYPair( 1,  6) },
            { 12, new XYPair( 1,  7) },
            { 13, new XYPair( 0,  7) },
            // Path - Blue
            { 14, new XYPair(-1,  7) },
            { 15, new XYPair(-1,  6) },
            { 16, new XYPair(-1,  5) },
            { 17, new XYPair(-1,  4) },
            { 18, new XYPair(-1,  3) },
            { 19, new XYPair(-1,  2) },
            { 20, new XYPair(-1,  1) },
            { 21, new XYPair(-2,  1) },
            { 22, new XYPair(-3,  1) },
            { 23, new XYPair(-4,  1) },
            { 24, new XYPair(-5,  1) },
            { 25, new XYPair(-6,  1) },
            { 26, new XYPair(-7,  1) },
            { 27, new XYPair(-7,  0) },
            // Path - Yellow
            { 28, new XYPair(-7, -1) },
            { 29, new XYPair(-6, -1) },
            { 30, new XYPair(-5, -1) },
            { 31, new XYPair(-4, -1) },
            { 32, new XYPair(-3, -1) },
            { 33, new XYPair(-2, -1) },
            { 34, new XYPair(-1, -1) },
            { 35, new XYPair(-1, -2) },
            { 36, new XYPair(-1, -3) },
            { 37, new XYPair(-1, -4) },
            { 38, new XYPair(-1, -5) },
            { 39, new XYPair(-1, -6) },
            { 40, new XYPair(-1, -7) },
            { 41, new XYPair(-0, -7) },
            // Path - Green
            { 42, new XYPair( 1, -7) },
            { 43, new XYPair( 1, -6) },
            { 44, new XYPair( 1, -5) },
            { 45, new XYPair( 1, -4) },
            { 46, new XYPair( 1, -3) },
            { 47, new XYPair( 1, -2) },
            { 48, new XYPair( 1, -1) },
            { 49, new XYPair( 2, -1) },
            { 50, new XYPair( 3, -1) },
            { 51, new XYPair( 4, -1) },
            { 52, new XYPair( 5, -1) },
            { 53, new XYPair( 6, -1) },
            { 54, new XYPair( 7, -1) },
            { 55, new XYPair( 7,  0) },

            // Cages - Red
            { 56, new XYPair( 6,  0) },
            { 57, new XYPair( 5,  0) },
            { 58, new XYPair( 4,  0) },
            { 59, new XYPair( 3,  0) },
            { 60, new XYPair( 2,  0) },
            { 61, new XYPair( 1,  0) },
            // Cages - Blue
            { 62, new XYPair( 0,  6) },
            { 63, new XYPair( 0,  5) },
            { 64, new XYPair( 0,  4) },
            { 65, new XYPair( 0,  3) },
            { 66, new XYPair( 0,  2) },
            { 67, new XYPair( 0,  1) },
            // Cages - Yellow
            { 68, new XYPair(-6,  0) },
            { 69, new XYPair(-5,  0) },
            { 70, new XYPair(-4,  0) },
            { 71, new XYPair(-3,  0) },
            { 72, new XYPair(-2,  0) },
            { 73, new XYPair(-1,  0) },
            // Cages - Green
            { 74, new XYPair(-0, -6) },
            { 75, new XYPair(-0, -5) },
            { 76, new XYPair(-0, -4) },
            { 77, new XYPair(-0, -3) },
            { 78, new XYPair(-0, -2) },
            { 79, new XYPair(-0, -1) },

            // Spawn - Red
            { 80, new XYPair( 5,  4) },
            { 81, new XYPair( 4,  4) },
            { 82, new XYPair( 4,  5) },
            { 83, new XYPair( 5,  5) },
            // Spawn - Blue
            { 84, new XYPair(-4,  5) },
            { 85, new XYPair(-4,  4) },
            { 86, new XYPair(-5,  4) },
            { 87, new XYPair(-5,  5) },
            // Spawn - Yellow
            { 88, new XYPair(-5, -4) },
            { 89, new XYPair(-4, -4) },
            { 90, new XYPair(-4, -5) },
            { 91, new XYPair(-5, -5) },
            // Spawn - Green
            { 92, new XYPair( 4,  -5) },
            { 93, new XYPair( 4,  -4) },
            { 94, new XYPair( 5,  -4) },
            { 95, new XYPair( 5,  -5) },
        };

        realPositionList = new SortedList<int, Vector3>();
        for (int i = 0; i < NUMBER_OF_POSITIONS; i++)
            realPositionList.Add(i, new Vector3(realPositionXList[realPositionXYRef[i].x], realPositionYList[realPositionXYRef[i].y]));

        startPositionList = new SortedList<HorseColor, int>
        {
            { HorseColor.Red, 0 },
            { HorseColor.Blue, 14 },
            { HorseColor.Yellow, 28 },
            { HorseColor.Green, 42 }
        };

        cageEntrancePositionList = new SortedList<HorseColor, int>
        {
            { HorseColor.Red, 55 },
            { HorseColor.Blue, 13 },
            { HorseColor.Yellow, 27 },
            { HorseColor.Green, 41 }
        };

        firstCagePositionList = new SortedList<HorseColor, int>
        {
            { HorseColor.Red, 56 },
            { HorseColor.Blue, 62 },
            { HorseColor.Yellow, 68 },
            { HorseColor.Green, 74 }
        };
    }

    // Methods
    public static HorseColor GetHorseColor(int horseNumber)
    {
        return (HorseColor)(horseNumber / NUMBER_OF_HORSES_PER_PLAYER);
    }
    public static bool IsSameColor(int horseNumber1, int horseNumber2)
    {
        return (GetHorseColor(horseNumber1) == GetHorseColor(horseNumber2));
    }
    public static Vector3 GetRealPosition(int position)
    {
        return realPositionList[position];
    }
    public static int GetSpawnPosition(int horseNumber)
    {
        return horseNumber + FIRST_SPAWN_POSITION;
    }
    public static int GetStartPosition(HorseColor color)
    {
        return startPositionList[color];
    }
    public static int GetStartPosition(int horseNumber)
    {
        return GetStartPosition(GetHorseColor(horseNumber));
    }
    public static int GetCageEntrancePosition(HorseColor color)
    {
        return cageEntrancePositionList[color];
    }
    public static int GetCageEntrancePosition(int horseNumber)
    {
        return GetCageEntrancePosition(GetHorseColor(horseNumber));
    }
    public static int GetCagePosition(HorseColor color, int cageNumber)
    {
        return firstCagePositionList[color] + cageNumber - 1;
    }
    public static int GetCagePosition(int horseNumber, int cageNumber)
    {
        return GetCagePosition(GetHorseColor(horseNumber), cageNumber);
    }
    public static int GetNextPosition(int horseNumber, int currentPosition)
    {
        // The method will return -1 if horse is at the 6th cage or input is invalid

        if (currentPosition >= NUMBER_OF_POSITIONS)
            return -1;
        else if (currentPosition >= FIRST_SPAWN_POSITION)
        {
            // If horse is at spawn -> Next position = start position of the horse
            if (currentPosition == GetSpawnPosition(horseNumber))
                return GetStartPosition(horseNumber);
            return -1;
        }
        else if (currentPosition >= NUMBER_OF_ROAD_POSITIONS)
        {
            // If horse is in the cage and is not at the 6th cage -> Next position = position of the next cage
            if ((currentPosition >= GetCagePosition(horseNumber, 1)) && (currentPosition <= GetCagePosition(horseNumber, NUMBER_OF_CAGES_PER_COLOR - 1)))
                return currentPosition + 1;
            else
                return -1;
        }
        else if (currentPosition >= 0) // If horse is on the road
        {
            foreach (HorseColor color in Enum.GetValues(typeof(HorseColor)))
                if (currentPosition == GetCageEntrancePosition(color)) // If horse is at a cage entrance
                    if (GetHorseColor(horseNumber) == color) // If it is the correct cage entrance
                        return GetCagePosition(horseNumber, 1); // Next position = Position of the 1st cage
                    else // Else the horse will skip the start position following the cage entrance
                        return (currentPosition + 2) % NUMBER_OF_ROAD_POSITIONS;
            // If horse is not at any cage entrance -> Next position = the position next to the current position
            return (currentPosition + 1) % NUMBER_OF_ROAD_POSITIONS; 
        }
        else return -1;
    }
    public static int GetTargetPosition(int horseNumber, int currentPosition, int steps)
    {
        int targetPosition = currentPosition;
        for (int i = 0; i < steps; i++)
            targetPosition = GetNextPosition(horseNumber, targetPosition);
        return targetPosition;
    }
}

public class GameState : GameStateBase
{
    // Private variables
    private static GameState instance = null; // Singleton Instance
    private HorseColor currentPlayer;
    private SortedList<int, int> horsePosition;
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
    public SortedList<int, int> HorsePosition
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
            if ((value >= 0) && (value <= 6))
                currentDiceValue = value;
        }
    }

    // Methods
    private List<int> GetBlockHorses(int horseNumber, int steps)
    {
        List<int> blockHorses = new List<int>();
        int currentPosition = HorsePosition[horseNumber];
        int targetPosition = currentPosition;
        int blockHorseNumber;
        for (int i = 0; i < steps; i++)
        {
            targetPosition = GetNextPosition(horseNumber, targetPosition);
            blockHorseNumber = HorsePosition.IndexOfValue(targetPosition);
            if (blockHorseNumber != -1)
                blockHorses.Add(blockHorseNumber);
        }
            
        return blockHorses;
    }
    public void ResetDiceAndChangePlayer()
    {
        // If dice value != 6, switch to the next player
        if (CurrentDiceValue != 6)
            CurrentPlayer = (HorseColor)(((int)CurrentPlayer + 1) % NUMBER_OF_PLAYERS);
        CurrentDiceValue = 0;
    }
    public void ProcessDice(int horseNumber)
    {
        int currentPosition = HorsePosition[horseNumber];
        int targetPosition = GetTargetPosition(horseNumber, currentPosition, CurrentDiceValue);
        List<int> blockHorses;

        /* Cases that nothing changed
            - Dice value == 0
            - Target position == -1
            - Horse is on the road and not at the cage entrance position, but target position is off the road (caused by a large dice value)
        */
        if (CurrentDiceValue == 0) return;
        if (targetPosition == -1) return;
        if ((currentPosition < NUMBER_OF_ROAD_POSITIONS) && (currentPosition != GetCageEntrancePosition(horseNumber)) && (targetPosition >= NUMBER_OF_ROAD_POSITIONS)) return;
        
        if (currentPosition >= FIRST_SPAWN_POSITION)
        {
            // If the horse is at spawn and dice value == 1 -> Move it to start position
            if ((currentPosition == GetSpawnPosition(horseNumber)) && (CurrentDiceValue == 1))
            {
                HorsePosition[horseNumber] = GetStartPosition(horseNumber);
                ResetDiceAndChangePlayer();
            }
        }
        else if (currentPosition >= NUMBER_OF_ROAD_POSITIONS)
        {
            // If the horse is in the cage -> Check if any horse block it
            blockHorses = GetBlockHorses(horseNumber, CurrentDiceValue);
            if (blockHorses.Count == 0)
            {
                HorsePosition[horseNumber] = targetPosition;
                ResetDiceAndChangePlayer();
            }
        }
        else if (currentPosition >= 0)
        {
            // If the horse is on the road -> Check if any horse block it
            blockHorses = GetBlockHorses(horseNumber, CurrentDiceValue);
            if (blockHorses.Count == 0)
            {
                HorsePosition[horseNumber] = targetPosition;
                ResetDiceAndChangePlayer();
            }
            else if (blockHorses.Count == 1) 
            {
                int blockHorseNumber = blockHorses[0];
                // Check if the block horse can be kicked
                if ((HorsePosition[blockHorseNumber] == targetPosition) && !IsSameColor(blockHorseNumber, horseNumber))
                {
                    HorsePosition[horseNumber] = targetPosition;
                    HorsePosition[blockHorseNumber] = GetSpawnPosition(blockHorseNumber);
                    ResetDiceAndChangePlayer();
                }
            }
        }
    }

    // Constructor and property to get singleton instance
    private GameState()
    {
        CurrentPlayer = HorseColor.Red;
        HorsePosition = new SortedList<int, int>();
        for (int horseNumber = 0; horseNumber < NUMBER_OF_HORSES; horseNumber++)
            HorsePosition.Add(horseNumber, GetSpawnPosition(horseNumber));
        currentDiceValue = 0;
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
