using UnityEngine;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
=======
>>>>>>> 93db0d9460945cdc64037a88f726ff855d86f13f

public enum HorseColor
{
    None = -1,
    Red = 0,
    Blue = 1,
<<<<<<< HEAD
    Green = 2,
    Yellow = 3
    
=======
    Yellow = 2,
    Green = 3
>>>>>>> 93db0d9460945cdc64037a88f726ff855d86f13f
}

public class PositionControl
{
<<<<<<< HEAD
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
=======
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
>>>>>>> 93db0d9460945cdc64037a88f726ff855d86f13f
    }
    public static int GetStartPosition(HorseColor color)
    {
        return StartPositionList[color];
    }
    public static int GetCagePosition(HorseColor color, int cageNumber)
    {
        return FirstCagePositionList[color] + cageNumber - 1;
    }
<<<<<<< HEAD
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
=======
>>>>>>> 93db0d9460945cdc64037a88f726ff855d86f13f
    public static int GetSpawnPosition(int horseNumber)
    {
        return horseNumber + FirstSpawnPosition - 1;
    }

}

<<<<<<< HEAD
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

public class BoardControl
{
    private static int[] _horsePositions = new int[16];

    // TODO: implement randomization
    private static int _dice = 10;

    public static void UpdatePositions(int horseNumber, int position)
    {
        _horsePositions[horseNumber] = position;
    }

    public static void PositionsLog()
    {
        for (int i = 0; i < 16; i++)
        {
            Debug.Log(_horsePositions[i]);
        }
    }

    // TODO: double attack logic
    public static int MoveControl(int horseNumber)
    {
        int currentPosition = _horsePositions[horseNumber];
        bool movable = true;

        // Check if movable in between
        for (int i = 0; i < _dice - 1; i++)
        {
            if (_horsePositions.Contains(PositionControl.GetNextPosition(GameLogic.GetHorseColor(horseNumber), currentPosition + i)))
            {
                movable = false;
                break;
            }
        }

        // Check if attackable
        if (movable)
        {
            int target = PositionControl.GetNextPosition(GameLogic.GetHorseColor(horseNumber), currentPosition + _dice - 1);
            if (_horsePositions.Contains(target))
            {
                if (GameLogic.GetHorseColor(target) == GameLogic.GetHorseColor(horseNumber))
                    // Not attackable
                    return -1;
                else
                    // Attackable
                    return 1;
            }
            else
                // Movable
                return 0;
        }
        else
            // Not movable
            return -1;

=======
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
>>>>>>> 93db0d9460945cdc64037a88f726ff855d86f13f
    }
}
