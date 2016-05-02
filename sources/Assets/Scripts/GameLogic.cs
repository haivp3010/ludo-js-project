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

public enum MoveCase
{
    Movable,
    Immovable,
    Attackable
}

public class PositionControl
{
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

    public static Vector3 GetRealPosition(int position)
    {
        return PositionList.List[position];
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
        return horseNumber + 900;
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
    public static bool HorseMoving;
    // Singleton Instance
    private static GameState _instance = new GameState();

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
    private MoveCase CheckMoveCase(int horseNumber, int steps)
    {
        int currentPosition = Instance.HorsePosition[horseNumber];
        HorseColor horseColor = GetHorseColor(horseNumber);
        List<int> ListHorse = Instance.HorsePosition;

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
                    if (GetHorseColor(FindHorseAt(currentPosition)) == horseColor)
                        return MoveCase.Immovable;
                    else
                        return MoveCase.Attackable;
                }
            }
        }

        return MoveCase.Movable;
    }

    private void KillHorse(int horseNumber)
    {
        Instance.HorsePosition[horseNumber] = PositionControl.GetSpawnPosition(horseNumber);
    }

    private int FindHorseAt(int position)
    {
        return Instance.HorsePosition.FindIndex(pos => pos == position);
    }

    public void ProcessDice(int horseNumber)
    {
        int dice_1 = Dice1 + 1;
        int dice_2 = Dice2 + 1;
        int currentPosition = Instance.HorsePosition[horseNumber];

        // If horse is in the base
        if (Instance.HorsePosition[horseNumber] >= 900)
        {
            if (dice_1 == dice_2 ||
               (dice_1 == 6 && dice_2 == 1) ||
               (dice_1 == 1 && dice_2 == 6))
            {
                // If there is another horse at start position
                if (Instance.HorsePosition.Contains(PositionControl.GetStartPosition(GetHorseColor(horseNumber))))
                {
                    int startPosition = PositionControl.GetStartPosition(GetHorseColor(horseNumber));
                    KillHorse(FindHorseAt(startPosition));
                }

                // Horse Start
                Instance.HorsePosition[horseNumber] = PositionControl.GetStartPosition(GetHorseColor(horseNumber));
            }
        }
        else // If horse is not in the base
        {
            int targetPosition = PositionControl.GetTargetPosition(horseNumber, dice_1 + dice_2);
            Debug.Log("Target : " + targetPosition);
            Debug.Log(CheckMoveCase(horseNumber, dice_1 + dice_2));

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
                        KillHorse(FindHorseAt(targetPosition));
                        Instance.HorsePosition[horseNumber] = targetPosition;
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
        
        // Initialize list
        for (int i = 0; i < NUMBER_OF_HORSES; i++)
            horsePosition.Add(0);

        currentDiceValue = 1;
    }
    public static GameState Instance
    {
        get { return _instance; }
    }
}
