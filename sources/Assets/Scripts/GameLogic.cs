using UnityEngine;
using System.Collections;
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
    public static bool AttackingHorse;
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
        8  - 11 : Green
        12 - 15 : Yellow
    */

    // Private variables
    private HorseColor currentPlayer;
    private List<int> horsePosition;
    private bool _horseMoving;
    private HorseColor _winner = HorseColor.None;
    private bool _diceRolled = false;
    private MoveCase[] _movable = new MoveCase[16];

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
    public bool HorseMoving
    {
        get
        {
            return _horseMoving;
        }

        set
        {
            _horseMoving = value;
        }
    }
    public HorseColor Winner
    {
        get
        {
            return _winner;
        }

        set
        {
            _winner = value;
        }
    }
    public bool DiceRolled
    {
        get
        {
            return _diceRolled;
        }

        set
        {
            _diceRolled = value;
        }
    }
    public MoveCase[] Movable
    {
        get
        {
            return _movable;
        }

        set
        {
            _movable = value;
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

    public void NextPlayer()
    {
        if (currentPlayer == HorseColor.Yellow)
            currentPlayer = HorseColor.Red;
        else
            currentPlayer++;
    }

    // Move cases
    // Check Move case in case immovable and two dice are equal
    private MoveCase CheckMoveCase(int horseNumber, int steps)
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
                    if (GetHorseColor(FindHorseAt(currentPosition)) == horseColor)
                        return MoveCase.Immovable;
                    else
                        return MoveCase.Attackable;
                }
            }
        }

        return MoveCase.Movable;
    }

    // Check move case according to two dice
    private MoveCase CheckMoveCase(int horseNumber, int dice_1, int dice_2)
    {
        int steps = dice_1 + dice_2;
        int currentPosition = Instance.HorsePosition[horseNumber];
        HorseColor horseColor = GetHorseColor(horseNumber);

        // Horse is in the cage
        if (currentPosition > 47 && currentPosition < 900)
        {
            if (currentPosition + 1 == steps || dice_1 == dice_2)
                return MoveCase.Movable;
            else
                return MoveCase.Immovable;
        }

        if (currentPosition >= 900)
        {
            if (dice_1 == dice_2 || (dice_1 == 6 && dice_2 == 1) || (dice_1 == 1 && dice_2 == 6))
            {
                if (Instance.HorsePosition.Contains(PositionControl.GetStartPosition(currentPlayer)))
                {
                    if (GetHorseColor(FindHorseAt(PositionControl.GetStartPosition(currentPlayer))) != horseColor)
                        return MoveCase.Attackable;
                    else
                        return MoveCase.Immovable;
                }
                else
                    return MoveCase.Movable;
            }
            else
                return MoveCase.Immovable;
        }

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

    public bool NoHorseCanMove()
    {
        for (int i = (int)currentPlayer * 4; i < (int)currentPlayer * 4 + 4; i++)
        {
            if (_movable[i] != MoveCase.Immovable)
                return false;
        }
        return true;
    }

    public void UpdateMovable()
    {
        for (int i = (int)currentPlayer * 4; i < (int)currentPlayer * 4 + 4; i++)
        {
            if (CheckMoveCase(i, Dice1 + 1, Dice2 + 1) == MoveCase.Immovable)
            {
                if (Dice1 == Dice2)
                    _movable[i] = CheckMoveCase(i, (Dice1 + Dice2) / 2);
                else
                    _movable[i] = MoveCase.Immovable;
            }
            else
                _movable[i] = CheckMoveCase(i, Dice1 + 1, Dice2 + 1);
        }
    }

    public void KillHorse(int horseNumber)
    {
        Instance.HorsePosition[horseNumber] = PositionControl.GetSpawnPosition(horseNumber);
    }

    public int FindHorseAt(int position)
    {
        return Instance.HorsePosition.FindIndex(pos => pos == position);
    }

    public void ProcessDice(int horseNumber)
    {
        // If dice are not rolled, do nothing
        if (!_diceRolled)
            return;

        int dice_1 = Dice1 + 1;
        int dice_2 = Dice2 + 1;
        int currentPosition = Instance.HorsePosition[horseNumber];
        HorseColor horseColor = GetHorseColor(horseNumber);

        // If horse is in the base
        if (Instance.HorsePosition[horseNumber] >= 900)
        {
            if (dice_1 == dice_2 ||
               (dice_1 == 6 && dice_2 == 1) ||
               (dice_1 == 1 && dice_2 == 6))
            {

                int startPosition = PositionControl.GetStartPosition(horseColor);
                // If there is another horse at start position
                if (Instance.HorsePosition.Contains(startPosition))
                {
                    if (GetHorseColor(FindHorseAt(startPosition)) != horseColor)
                    {
                        KillHorse(FindHorseAt(startPosition));
                        // Horse Start
                        Instance.HorsePosition[horseNumber] = PositionControl.GetStartPosition(GetHorseColor(horseNumber));
                    }
                }
                else
                    // Horse Start
                    Instance.HorsePosition[horseNumber] = PositionControl.GetStartPosition(GetHorseColor(horseNumber));
            }
        }
        else // If horse is not in the base
        {
            int targetPosition = PositionControl.GetTargetPosition(horseNumber, dice_1 + dice_2);
            Debug.Log("Target : " + targetPosition);
            Debug.Log(CheckMoveCase(horseNumber, dice_1, dice_2));

            switch (CheckMoveCase(horseNumber, dice_1, dice_2))
            {
                case MoveCase.Movable:
                    // If horse is in the cage
                    if (currentPosition > 47 && currentPosition < 900)
                    {
                        if ((currentPosition + 1) % 100 == dice_1 + dice_2 || dice_1 == dice_2)
                            Instance.HorsePosition[horseNumber] = PositionControl.GetNextPosition(horseColor, currentPosition);
                    }
                    else
                        // Move horse to target
                        Instance.HorsePosition[horseNumber] = targetPosition;
                    break;
                case MoveCase.Attackable:
                    if ((currentPosition > 47 && Instance.HorsePosition[horseNumber] % 100 == dice_1 + dice_2) || currentPosition <= 47)
                    {
                        KillHorse(FindHorseAt(targetPosition));
                        Instance.HorsePosition[horseNumber] = targetPosition;  //đi đến điểm gần mục tiêu
                    }
                    break;
                case MoveCase.Immovable:
                    if (dice_1 == dice_2 && currentPosition <= 47)
                    {
                        MoveCase move = CheckMoveCase(horseNumber, (dice_1 + dice_2) / 2);
                        if (move != MoveCase.Immovable)
                        {
                            int target = PositionControl.GetTargetPosition(horseNumber, (dice_1 + dice_2) / 2);
                            if (move == MoveCase.Attackable)
                            {
                                KillHorse(FindHorseAt(target));
                                Movable[horseNumber] = MoveCase.Attackable;
                            }
                            Instance.HorsePosition[horseNumber] = target;
                        }
                    }
                    break;
            }
        }
    }

    public void CheckWinner()
    {
        if (Instance.HorsePosition.Contains(106) && Instance.HorsePosition.Contains(105) && Instance.HorsePosition.Contains(104) && Instance.HorsePosition.Contains(103))
            Instance.Winner = HorseColor.Red;
        else if (Instance.HorsePosition.Contains(206) && Instance.HorsePosition.Contains(205) && Instance.HorsePosition.Contains(204) && Instance.HorsePosition.Contains(203))
            Instance.Winner = HorseColor.Blue;
        else if (Instance.HorsePosition.Contains(306) && Instance.HorsePosition.Contains(305) && Instance.HorsePosition.Contains(304) && Instance.HorsePosition.Contains(303))
            Instance.Winner = HorseColor.Green;
        else if (Instance.HorsePosition.Contains(406) && Instance.HorsePosition.Contains(405) && Instance.HorsePosition.Contains(404) && Instance.HorsePosition.Contains(403))
            Instance.Winner = HorseColor.Yellow;
    }

    // Constructor and property to get singleton instance
    private GameState()
    {
        currentPlayer = HorseColor.Red;
        horsePosition = new List<int>(NUMBER_OF_HORSES);
        
        // Initialize list
        for (int i = 0; i < NUMBER_OF_HORSES; i++)
            horsePosition.Add(0);
    }
    public static GameState Instance
    {
        get { return _instance; }
    }
}
