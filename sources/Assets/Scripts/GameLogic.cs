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
    private List<int> sortingOrder = new List<int>();
    private List<float> currentYValues = new List<float>();
    private bool audio = true;
    private string message = "";

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
    public List<int> SortingOrder
    {
        get
        {
            return sortingOrder;
        }

        set
        {
            sortingOrder = value;
        }
    }
    public List<float> CurrentYValues
    {
        get
        {
            return currentYValues;
        }

        set
        {
            currentYValues = value;
        }
    }
    public bool Audio
    {
        get
        {
            return audio;
        }

        set
        {
            audio = value;
        }
    }
    public string Message
    {
        get
        {
            return message;
        }

        set
        {
            message = value;
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
        if (CurrentPlayer == HorseColor.Yellow)
            CurrentPlayer = HorseColor.Red;
        else
            CurrentPlayer++;
    }

    public void UpdateSortingOrder()
    {
        for (int i = 0; i < NUMBER_OF_HORSES; i++)
        {
            int count = 0;
            for (int j = 0; j < NUMBER_OF_HORSES; j++)
            {
                if (CurrentYValues[i] < CurrentYValues[j])
                    count++;
            }
            SortingOrder[i] = count;
        }
    }

    // Move cases
    // Check Move case in case immovable and two dice are equal
    private MoveCase CheckMoveCase(int horseNumber, int steps)
    {
        int currentPosition = HorsePosition[horseNumber];
        HorseColor horseColor = GetHorseColor(horseNumber);

        // If horse in the base or in the cage, cannot move
        if (currentPosition > 47)
            return MoveCase.Immovable;

        for (int i = 0; i < steps; i++)
        {
            currentPosition = PositionControl.GetNextPosition(horseColor, currentPosition);

            if (currentPosition == -1)
                return MoveCase.Immovable;
            if (HorsePosition.Contains(currentPosition))
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
        int currentPosition = HorsePosition[horseNumber];
        HorseColor horseColor = GetHorseColor(horseNumber);

        // Horse is in the cage
        if (currentPosition > 47 && currentPosition < 900)
        {
            // If horse is at the final position
            if (currentPosition % 100 == 6)
                return MoveCase.Immovable;

            if (FindHorseAt(currentPosition + 1) != -1)
                return MoveCase.Immovable;

            if ((currentPosition + 1) % 100 == steps || dice_1 == dice_2)
                return MoveCase.Movable;
            else
                return MoveCase.Immovable;
        }

        if (currentPosition >= 900)
        {
            if (dice_1 == dice_2 || (dice_1 == 6 && dice_2 == 1) || (dice_1 == 1 && dice_2 == 6))
            {
                if (HorsePosition.Contains(PositionControl.GetStartPosition(currentPlayer)))
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
            if (HorsePosition.Contains(currentPosition))
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
        for (int i = 0; i < NUMBER_OF_HORSES; i++)
        {
            if (i / 4 != (int)CurrentPlayer)
                Movable[i] = MoveCase.Immovable;
            else if (CheckMoveCase(i, Dice1 + 1, Dice2 + 1) == MoveCase.Immovable)
            {
                if (Dice1 == Dice2)
                    _movable[i] = CheckMoveCase(i, Dice1 + 1);
                else
                    _movable[i] = MoveCase.Immovable;
            }
            else
                _movable[i] = CheckMoveCase(i, Dice1 + 1, Dice2 + 1);
        }
    }

    public void KillHorse(int horseNumber)
    {
        HorsePosition[horseNumber] = PositionControl.GetSpawnPosition(horseNumber);
    }

    public int FindHorseAt(int position)
    {
        return HorsePosition.FindIndex(pos => pos == position);
    }

    public void ProcessDice(int horseNumber)
    {
        // If dice are not rolled, do nothing
        if (!_diceRolled)
            return;

        int dice_1 = Dice1 + 1;
        int dice_2 = Dice2 + 1;
        int currentPosition = HorsePosition[horseNumber];
        HorseColor horseColor = GetHorseColor(horseNumber);

        // If horse is in the base
        if (currentPosition >= 900)
        {
            if (dice_1 == dice_2 ||
               (dice_1 == 6 && dice_2 == 1) ||
               (dice_1 == 1 && dice_2 == 6))
            {

                int startPosition = PositionControl.GetStartPosition(horseColor);
                // If there is another horse at start position
                if (HorsePosition.Contains(startPosition))
                {
                    if (GetHorseColor(FindHorseAt(startPosition)) != horseColor)
                    {
                        KillHorse(FindHorseAt(startPosition));
                        // Horse Start
                        HorsePosition[horseNumber] = PositionControl.GetStartPosition(GetHorseColor(horseNumber));
                    }
                }
                else
                    // Horse Start
                    HorsePosition[horseNumber] = PositionControl.GetStartPosition(GetHorseColor(horseNumber));
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
                        HorsePosition[horseNumber] = PositionControl.GetNextPosition(horseColor, currentPosition);
                    else
                        // Move horse to target
                        HorsePosition[horseNumber] = targetPosition;
                    break;
                case MoveCase.Attackable:
                    if ((currentPosition > 47 && HorsePosition[horseNumber] % 100 == dice_1 + dice_2) || currentPosition <= 47)
                    {
                        KillHorse(FindHorseAt(targetPosition));
                        HorsePosition[horseNumber] = targetPosition;
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
                            HorsePosition[horseNumber] = target;
                        }
                    }
                    break;
            }
        }
    }

    public void CheckWinner()
    {
        if (HorsePosition.Contains(106) && HorsePosition.Contains(105) && HorsePosition.Contains(104) && HorsePosition.Contains(103))
            Instance.Winner = HorseColor.Red;
        else if (HorsePosition.Contains(206) && HorsePosition.Contains(205) && HorsePosition.Contains(204) && HorsePosition.Contains(203))
            Instance.Winner = HorseColor.Blue;
        else if (HorsePosition.Contains(306) && HorsePosition.Contains(305) && HorsePosition.Contains(304) && HorsePosition.Contains(303))
            Instance.Winner = HorseColor.Green;
        else if (HorsePosition.Contains(406) && HorsePosition.Contains(405) && HorsePosition.Contains(404) && HorsePosition.Contains(403))
            Instance.Winner = HorseColor.Yellow;
    }

    // Constructor and property to get singleton instance
    private GameState()
    {
        ResetGameState();    
    }

    private void ResetGameState()
    {
        CurrentPlayer = HorseColor.Red;
        HorsePosition = new List<int>(NUMBER_OF_HORSES);

        // Initialize list
        for (int i = 0; i < NUMBER_OF_HORSES; i++)
        {
            horsePosition.Add(0);
            SortingOrder.Add(0);
            currentYValues.Add(0);
        }

        for (int i = 0; i < NUMBER_OF_HORSES; i++)
        {
            HorsePosition[i] = PositionControl.GetSpawnPosition(i);
            Movable[i] = MoveCase.Immovable;
            SortingOrder[i] = 0;
        }

        HorseMoving = false;
        Winner = HorseColor.None;
        DiceRolled = false;
        Audio = true;
        Message = "";
    }

    public static GameState Instance
    {
        get { return _instance; }
    }
}
