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

public enum MoveCase
{
    Movable,
    Immovable,
    Attackable
}

public class PositionControl
{
    // TODO: change spawn position
    private const int FirstSpawnPosition = 80;
    // TODO: change real position
    private static SortedList<int, Vector3> RealPositionList = new SortedList<int, Vector3>
    {
        { 0, new Vector3(2.93f, 1.21f) },
        { 1, new Vector3(2.35f, 1.21f) },
        { 2, new Vector3(1.77f, 1.21f) },
        { 3, new Vector3(1.19f, 1.21f) }
    };
    // TODO: change start position
    private static SortedList<HorseColor, int> StartPositionList = new SortedList<HorseColor, int>
    {
        { HorseColor.Red, 0 },
        { HorseColor.Blue, 14 },
        { HorseColor.Yellow, 28 },
        { HorseColor.Green, 42 }
    };
    // TODO: change first cage position
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
    // TODO: change get spawn position method
    public static int GetSpawnPosition(int horseNumber)
    {
        return horseNumber + FirstSpawnPosition - 1;
    }

    // Cage entrance list
    private static SortedList<HorseColor, int> CageEntranceList = new SortedList<HorseColor, int>
    {
        { HorseColor.Red, 47 },
        { HorseColor.Blue, 11 },
        { HorseColor.Green, 23 },
        { HorseColor.Yellow, 35 }
    };
    // Get next position to move from the current position
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
    // Get target position
    public static int GetTargetPosition(int horseNumber, int diceNumber)
    {
        int targetPosition = GameState.Instance.HorsePosition[horseNumber];

        for (int i = 0; i < diceNumber; i++)
        {
            targetPosition = GetNextPosition(GameState.GetHorseColor(horseNumber), targetPosition);
        }

        return targetPosition;
    }
}

public class GameState
{
    public static int Dice1;
    public static int Dice2;
    public static bool AnimatingDice;
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


    // Move cases
    private static MoveCase CheckMoveCase(int horseNumber, int steps)
    {
        int currentPosition = Instance.HorsePosition[horseNumber];
        HorseColor horseColor = GetHorseColor(horseNumber);

        for (int i = 0; i < steps; i++)
        {
            currentPosition = PositionControl.GetNextPosition(horseColor, currentPosition);
            if (currentPosition == -1)
                return MoveCase.Immovable;
            if (Instance.HorsePosition.Contains(currentPosition))
            {
                // Check if movable
                if (i != steps - 1)
                {
                    return MoveCase.Immovable;
                }
                else // If there is another horse at the target position
                {
                    // Check if the two horses have the same color
                    if (GetHorseColor(currentPosition) == horseColor)
                        return MoveCase.Immovable;
                    else
                        return MoveCase.Attackable;
                }
            }
        }

        return MoveCase.Movable;
    }

    public static void ProcessDice(int horseNumber)
    {
        // TODO: assign GameState properties to dice
        int dice_1 = 0;
        int dice_2 = 0;
        int currentPosition = Instance.HorsePosition[horseNumber];

        // If horse is in the base
        if (Instance.HorsePosition[horseNumber] < 0)
        {
            if (dice_1 == dice_2 ||
               (dice_1 == 6 && dice_2 == 1) ||
               (dice_1 == 1 && dice_2 == 6))
                Instance.HorsePosition[horseNumber] = PositionControl.GetStartPosition(GetHorseColor(horseNumber));
        }
        else // If horse is not in the base
        {
            int targetPosition = PositionControl.GetTargetPosition(horseNumber, dice_1 + dice_2);

            switch (CheckMoveCase(horseNumber, dice_1 + dice_2))
            {
                case MoveCase.Movable:
                    // If horse is in the cage
                    // TODO: implement logic for double dice, in cage...
                    if (currentPosition > 47)
                    {
                        if (Instance.HorsePosition[horseNumber] % 100 == dice_1 + dice_2)
                            Instance.HorsePosition[horseNumber] = targetPosition;
                    }
                    else
                        // Move horse to target
                        Instance.HorsePosition[horseNumber] = targetPosition;
                    break;
                case MoveCase.Attackable:
                    if ((currentPosition > 47 && Instance.HorsePosition[horseNumber] % 100 == dice_1 + dice_2) || currentPosition <= 47)
                    {
                        Instance.HorsePosition[horseNumber] = targetPosition;
                        int targetHorse = Instance.HorsePosition.FindIndex(position => position == targetPosition);
                        Instance.HorsePosition[targetHorse] = PositionControl.GetSpawnPosition(targetHorse);
                    }
                    break;
            }
        }
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
