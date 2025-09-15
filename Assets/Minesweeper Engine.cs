using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using TMPro;
using System.Threading;
using System;

public class MinesweeperEngine : MonoBehaviour
{
    const int boardSize = 20;
    int length = boardSize;

    int[,] numberBoard = new int[boardSize, boardSize]; //left blank on start, values calculated at runtime
    bool[,] mineBoard = new bool[boardSize, boardSize]; // true if mine
    bool[,] shownBoard = new bool[boardSize, boardSize]; // what squares are viewable to the player

    bool[,] flagBoard = new bool[boardSize, boardSize];

    public TextMeshProUGUI NumberLayer;
    public TextMeshProUGUI ShownLayer;
    public TextMeshProUGUI MineLayer;
    public TextMeshProUGUI log;

    public TextMeshProUGUI higherStatus;
    public TextMeshProUGUI lowerStatus;
    public TextMeshProUGUI orthogonalStatus;

    public GameObject Tile;
    public GameObject one; // one symbol
    public GameObject two; // two symbol
    public GameObject three; // three symbol
    public GameObject four; // four symbol
    public GameObject five; // five symbol
    public GameObject six;
    public GameObject seven;
    public GameObject eight;
    public GameObject empty;
    public GameObject hidden;
    public GameObject flag;

    public GameObject solving;
    public GameObject error;
    public GameObject invis;
    public GameObject ajacentTile;


    public Transform GameBoard;
    public Transform DebugGrid;

    public bool gameGoing = false;

    // ui logic
    public bool higherFunctions;
    public bool lowerFunctions;
    public int higherFunctionsCount;
    public int lowerFunctionsCount;


    public bool orthogonal;
    public int orthogonalCount;
    public bool basicInference;
    public int basicInferenceCount;


    public int turn;





    // Start is called before the first frame update
    void Start()
    {


        GenerateMine();
        CalculateNumbers();
        UpdateDisp();
        UpdateDebugDisp();
        //ChainViewUpd();
        //StartCoroutine(PlayGame());
        //CreateBoardDisp();

        //gameGoing = true;
        //

    }
    /*
        void CreateBoardDisp()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int k = 0; k < boardSize; k++)
                {
                    Instantiate(Tile, GameBoard);
                }
            }
        }*/


    void GenerateMine()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int k = 0; k < boardSize; k++)
            {
                Random rnd = new Random();
                if (rnd.Next(5) == 1)
                {
                    mineBoard[i, k] = true;
                }
            }
        }

    }

    void CalculateNumbers()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int k = 0; k < boardSize; k++)
            {
                if (mineBoard[i, k] == true) // mine found
                {
                    for (int j = i - 1; j < i + 2; j++)
                    {
                        for (int n = k - 1; n < k + 2; n++)
                        {
                            if ((n < boardSize) && (n > -1))
                            {
                                if ((j < boardSize) && (j > -1)) // if search is inbounds)
                                {
                                    numberBoard[j, n] += 1;
                                }
                            }
                        }
                    }
                }

            }
        }
    }

    public void ShowError(int i, int k, int j, int n, int ajacenty, int ajacentx)
    {
        for (int a = DebugGrid.childCount - 1; a >= 0; a--)
        {
            Destroy(DebugGrid.GetChild(a).gameObject);
        }

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (y == i && x == k)
                {
                    Instantiate(solving, DebugGrid);
                }
                else if (y == j && x == n)
                {
                    Instantiate(error, DebugGrid);
                }
                else if (y == ajacenty && x == ajacentx)
                {
                    Instantiate(ajacentTile, DebugGrid);
                }
                else
                {
                    Instantiate(invis, DebugGrid);
                }
            }
            Instantiate(error, DebugGrid);
        }
    }

    public void UpdateDebugDisp()
    {
        NumberLayer.text = "";
        ShownLayer.text = "";
        for (int i = 0; i < boardSize; i++)
        {
            if (i > 9)
            {
                NumberLayer.text += i + ": ";
            }
            else
            {
                NumberLayer.text += i + ":  ";
            }

            for (int k = 0; k < boardSize; k++)
            {
                if (mineBoard[i, k])
                {
                    NumberLayer.text += "# ";
                }
                else
                {
                    NumberLayer.text += numberBoard[i, k] + " ";
                }

            }
            NumberLayer.text += "\n";
        }
    }

    public void UpdateDisp()
    {
        for (int i = GameBoard.childCount - 1; i >= 0; i--)
        {
            Destroy(GameBoard.GetChild(i).gameObject);
        }

        //NumberLayer.text = "";
        //ShownLayer.text = "";

        for (int i = 0; i < boardSize; i++)
        {
            for (int k = 0; k < boardSize; k++)
            {
                if (mineBoard[i, k] == false) // if not mine
                {
                    if (shownBoard[i, k] == true) // if shown
                    {
                        if (numberBoard[i, k] > 0)
                        {
                            //NumberLayer.text += numberBoard[i, k];
                            switch (numberBoard[i, k])
                            {
                                case 1:
                                    {
                                        Instantiate(one, GameBoard);
                                        break;
                                    }
                                case 2:
                                    {
                                        Instantiate(two, GameBoard);
                                        break;
                                    }
                                case 3:
                                    {
                                        Instantiate(three, GameBoard);
                                        break;
                                    }
                                case 4:
                                    {
                                        Instantiate(four, GameBoard);
                                        break;
                                    }
                                case 5:
                                    {
                                        Instantiate(five, GameBoard);
                                        break;
                                    }
                                case 6:
                                    {
                                        Instantiate(six, GameBoard);
                                        break;
                                    }
                                case 7:
                                    {
                                        Instantiate(seven, GameBoard);
                                        break;
                                    }
                                case 8:
                                    {
                                        Instantiate(eight, GameBoard);
                                        break;
                                    }

                            }
                        }
                        else
                        {
                            //NumberLayer.text += "0";
                            Instantiate(empty, GameBoard);
                        }
                    }
                    else
                    {
                        Instantiate(hidden, GameBoard);
                    }
                }
                else // if mine
                {
                    //NumberLayer.text += "X";
                    if (flagBoard[i, k] == true)
                    {
                        Instantiate(flag, GameBoard);
                    }
                    else
                    {
                        Instantiate(hidden, GameBoard);
                    }

                }
                /*
                if (shownBoard[i, k] == false)
                {
                    ShownLayer.text += "█";
                    Instantiate(hidden, GameBoard);
                }
                else
                {
                    ShownLayer.text += "-";
                }*/

            }
            //NumberLayer.text += "\n";
            //ShownLayer.text += "\n";
            Instantiate(eight, GameBoard);

        }
    }

    void UpdateTile(int i, int k, bool isFlagging)
    {
        if (i > -1 && i < boardSize && k > -1 && k < boardSize)
        {
            if (isFlagging == false)
            {
                if (mineBoard[i, k] == false)
                {
                    //all good
                    if (numberBoard[i, k] == 0) // if its 0
                    {
                        ChainViewUpd(i, k);
                    }
                    else
                    {
                        shownBoard[i, k] = true;
                    }
                    Debug.Log("Cleared: " + i + " " + k);


                }
                else
                {
                    if (flagBoard[i, k] == false)
                    {
                        //end game
                        Debug.LogError("Game over, clicked a mine: " + i + " " + k);
                        gameGoing = false;
                    }
                    else
                    {
                        Debug.LogWarning("Clicked a flag: " + i + " " + k);
                    }

                }
            }
            else
            {
                if (mineBoard[i, k] == true)
                {
                    Debug.Log("Flagged: " + i + " " + k);
                    flagBoard[i, k] = true;
                }
                else
                {
                    //end game
                    Debug.LogError("Game over, flagged a clear space: " + i + ", " + k);
                    gameGoing = false;
                }
            }
            //UpdateDisp();
        }
        turn++;
        

    }

    void ChainViewUpd(int i, int k)
    {
        if (numberBoard[i, k] == 0) // if its 0
        {
            for (int j = i - 1; j < i + 2; j++) // check surrounding 3x3 area
            {
                for (int n = k - 1; n < k + 2; n++)
                {
                    if ((n < boardSize) && (n > -1))
                    {
                        if ((j < boardSize) && (j > -1)) // if search is inbounds
                        {
                            if (!(j == i && n == k)) // if not starting point
                            {
                                if (numberBoard[j, n] == 0 && shownBoard[j, n] == false)
                                {
                                    ChainViewUpd(j, n);
                                }
                            }

                            shownBoard[j, n] = true;

                        }
                    }
                }
            }
        }
    }

    void UpdateStatus()
    {
        lowerStatus.text = "Basic Logic: " + lowerFunctions;
        higherStatus.text = "Basic Inference: " + basicInference;
        orthogonalStatus.text = "Orthogonal Extrapolation: " + orthogonal;
    }


    // ALGORITHM BEGINS HERE ------------------------------------------------------------------------------------------------------------

    // show tiles if and only if they are numbers and have been found

    public void StartOnClick()
    {
        StartCoroutine(PlayGame());
    }

    public void ToggleHigherFunctions(int mode)
    {
        switch (mode)
        {
            case 0:

                orthogonalCount++;
                if (orthogonalCount % 2 == 0)
                {

                    orthogonal = false;
                    Debug.Log("ORTHOGONAL EXTRAPOLATION: OFF");
                }
                else
                {
                    orthogonal = true;
                    Debug.Log("ORTHOGONAL EXTRAPOLATION: ON");
                }
                UpdateStatus();
                break;

            case 1:
                basicInferenceCount++;
                if (basicInferenceCount % 2 == 0)
                {

                    basicInference = false;
                    Debug.Log("BASIC INFERENCE: OFF");
                }
                else
                {
                    basicInference = true;
                    Debug.Log("BASIC INFERENCE: ON");
                }
                UpdateStatus();
                break;
            
        }
    }

    public void ToggleLowerFunctions()
    {
        lowerFunctionsCount++;
        if (lowerFunctionsCount % 2 == 0)
        {
            lowerFunctions = false;
            Debug.Log("LOWER FUNCTIONS: OFF");
        }
        else
        {
            lowerFunctions = true;
            Debug.Log("LOWER FUNCTIONS: ON");
        }
        UpdateStatus();
    }

    IEnumerator PlayGame()
    {
        StartGameReroll();

        UpdateDisp();

        while (gameGoing == true)
        {
            UpdateDisp();
            yield return new WaitForSecondsRealtime(1);
            Debug.Log("New Loop...");
            for (int i = 0; i < boardSize; i++)
            {
                for (int k = 0; k < boardSize; k++)
                {
                    if (shownBoard[i, k] == true)
                    {
                        if (lowerFunctions == true)
                        {
                            if (FindAjacentMines(i, k) == numberBoard[i, k]) // if number of ajacent hidden tiles is equal to the number, flag all
                            {
                                FlagAllAjacentTiles(i, k);
                            }

                            if (FindAjacentSafe(i, k) == numberBoard[i, k]) // if number of flagged tiles is equal to number then remaining are safe
                            {
                                ClearAllAjacentTiles(i, k);
                            }
                        }


                        if (orthogonal == true)
                        {
                            OrthogonalExtrapolation(i, k);
                        }

                        if (basicInference == true)
                        {
                            BasicInference(i, k);
                        }
                    }
                }
            }
            UpdateDisp();
        }
    }

    void StartGameReroll()
    {
        while (gameGoing == false)
        {
            Random rnd = new Random();
            int x = rnd.Next(boardSize);
            int y = rnd.Next(boardSize);
            if (numberBoard[y, x] == 0)
            {
                ChainViewUpd(y, x);
                gameGoing = true;
            }
        }
    }

    // flags all tiles ajacent to [i, k]
    void FlagAllAjacentTiles(int i, int k)
    {
        for (int j = i - 1; j < i + 2; j++) // check surrounding 3x3 area
        {
            for (int n = k - 1; n < k + 2; n++)
            {
                if ((n < boardSize) && (n > -1))
                {
                    if ((j < boardSize) && (j > -1)) // if search is inbounds
                    {
                        if (!(j == i && n == k)) // if not starting point
                        {
                            if (shownBoard[j, n] == false && flagBoard[j, n] == false) // for each tile around i, k - if it is hidden and not flagged - add 1
                            {
                                //yield return new WaitForSecondsRealtime(1);
                                UpdateTile(j, n, true);
                            }
                        }
                    }
                }
            }
        }
    }

    // clears all tiles ajacent to [i, k]
    void ClearAllAjacentTiles(int i, int k)
    {
        for (int j = i - 1; j < i + 2; j++) // check surrounding 3x3 area
        {
            for (int n = k - 1; n < k + 2; n++)
            {
                if ((n < boardSize) && (n > -1))
                {
                    if ((j < boardSize) && (j > -1)) // if search is inbounds
                    {
                        if (!(j == i && n == k)) // if not starting point
                        {
                            if (shownBoard[j, n] == false && flagBoard[j, n] == false) // for each tile around i, k - if it is hidden and not flagged - add 1
                            {
                                //yield return new WaitForSecondsRealtime(1);
                                UpdateTile(j, n, false);
                            }

                        }
                    }
                }
            }
        }
    }

    int FindAjacentMines(int i, int k) // input position and outputs 
    {
        int ajacentHidden = 0;
        for (int j = i - 1; j < i + 2; j++) // check surrounding 3x3 area
        {
            for (int n = k - 1; n < k + 2; n++)
            {
                if ((n < boardSize) && (n > -1))
                {
                    if ((j < boardSize) && (j > -1)) // if search is inbounds
                    {
                        if (!(j == i && n == k)) // if not starting point
                        {
                            if (shownBoard[j, n] == false/* && flagBoard[j, n] == false*/) // for each tile around i, k - if it is hidden and not flagged - add 1
                            {
                                ajacentHidden += 1;
                            }
                        }
                    }
                }
            }
        }
        return ajacentHidden;
    }

    int FindAjacentSafe(int i, int k)
    {
        int ajacentFlags = 0;
        for (int j = i - 1; j < i + 2; j++) // check surrounding 3x3 area
        {
            for (int n = k - 1; n < k + 2; n++)
            {
                if ((n < boardSize) && (n > -1))
                {
                    if ((j < boardSize) && (j > -1)) // if search is inbounds
                    {
                        if (!(j == i && n == k)) // if not starting point
                        {
                            if (flagBoard[j, n] == true/* && flagBoard[j, n] == false*/) // for each tile around i, k - if it is hidden and not flagged - add 1
                            {
                                ajacentFlags += 1;
                            }
                        }
                    }
                }
            }
        }
        return ajacentFlags;
    }

    int FindAjacentUnknown(int i, int k) // input position and returns (hidden tiles - flagged ones)
    {
        int unknown = 0;
        for (int j = i - 1; j < i + 2; j++) // check surrounding 3x3 area
        {
            for (int n = k - 1; n < k + 2; n++)
            {
                if ((n < boardSize) && (n > -1))
                {
                    if ((j < boardSize) && (j > -1)) // if search is inbounds
                    {
                        if (!(j == i && n == k)) // if not starting point
                        {
                            if (!shownBoard[j, n])
                            {
                                if (!flagBoard[j, n])
                                {
                                    unknown++;
                                }
                            }

                        }
                    }
                }
            }
        }
        return unknown;
        
    }

    int[] GetAjacentTileData(int i, int k)
    {
        // 0: not valid
        // 1: is shown
        // 2: is not known
        // 3: flagged

        // n of 2 & 3 = n of unknown

        int[] state = new int[9];
        int loopN = -1;
        for (int j = i - 1; j < i + 2; j++) // check surrounding 3x3 area
        {
            for (int n = k - 1; n < k + 2; n++)
            {
                loopN++;
                if ((n < boardSize) && (n > -1))
                {
                    if ((j < boardSize) && (j > -1)) // if search is inbounds
                    {
                        if (!(j == i && n == k)) // if not starting point
                        {
                            if (!shownBoard[j, n])
                            {
                                if (!flagBoard[j, n])
                                {
                                    state[loopN] = 2;
                                }
                                else
                                {
                                    state[loopN] = 3;
                                }
                            }
                            else
                            {
                                state[loopN] = 1;
                            }

                        }
                        else
                        {
                            state[loopN] = 0;
                        }
                    }
                    else
                    {
                        state[loopN] = 0;
                    }
                }
                else
                {
                    state[loopN] = 0;
                }
            }
        }
        return state;
    }


    void SimulatedPositioning(int i, int k, bool givePosition)
    {
        // simulated positioning

        // simulate each arrangement of n mines in available unknown tiles
        // 


        // for each of these
        //     test if each known number ajacent to each simulated mine supports the simulated pattern
        //         if there are multiple possible answers, flag the tiles that are always simulated to be mines
        //         if there is one, try it


        // CHECK OUT THE IMAGE IN THE ASSESTS

        // notation: mines]tiles; simulatedNum]domain
        int domain = Array.FindAll(GetAjacentTileData(i, k), x => x == 2).Length;                               // domain = n of unknown tiles surrounding i, k minus flagged ones
        int simulatedNum = numberBoard[i, k] - Array.FindAll(GetAjacentTileData(i, k), x => x == 3).Length; // simnumber = value - flagged tiles

        bool[] validTiles = new bool[9];
        for (int j = 0; j < 9; j++)
        {
            if (GetAjacentTileData(i, k)[j] == 0)
            {
                validTiles[j] = true;
            }
        }



        switch (simulatedNum) // switch (n) from a]n
        {
            case 2: // n of for loops changes depending on size of a
                for (int j = 1; j < domain; j++)
                {
                    for (int n = j + 1; n < domain + 1; j++)
                    {
                        bool[] simulatedGuess = new bool[9];
                        simulatedGuess[j] = true;
                        simulatedGuess[n] = true;
                        SimulateMines(i, k, simulatedGuess, validTiles);
                        // 2d arr of valid positions and the simulated mines

                    }
                }
                break;

            case 3:
                for (int j = 1; j < domain - 1; j++)
                {
                    for (int n = j + 1; n < domain; n++)
                    {
                        for (int m = n + 1; m < domain + 1; m++)
                        {
                            bool[] simulatedGuess = new bool[9];
                            simulatedGuess[j] = true;
                            simulatedGuess[n] = true;
                            simulatedGuess[m] = true;
                        }
                    }
                }
                break;

            case 4:
                for (int j = 1; j < domain - 2; j++)
                {
                    for (int n = j + 1; n < domain - 1; n++)
                    {
                        for (int m = n + 1; m < domain; m++)
                        {
                            for (int o = m + 1; o < domain + 1; o++)
                            {
                                bool[,] simulatedGuess = new bool[2, 9];
                                simulatedGuess[0, j] = true;
                                simulatedGuess[0, n] = true;
                                simulatedGuess[0, m] = true;
                                simulatedGuess[0, m] = true;
                            }
                        }
                    }
                }
                break;
        }
        


        


    }

    void SimulateMines(int i, int k, bool[] simMines, bool[] validPos)
    {
        
    }

    

    void BasicInference(int i, int k) // called per tile
    {
        int ajacentFlags = 0;
        int ajacentHidden = 0;
        int ajacentLowestNum = 9;
        int[] ajacentLowestPos = { -1, -1 };
        for (int j = i - 1; j < i + 2; j++) // check surrounding 3x3 area
        {
            for (int n = k - 1; n < k + 2; n++)
            {
                if ((n < boardSize) && (n > -1))
                {
                    if ((j < boardSize) && (j > -1)) // if search is inbounds
                    {
                        if (!(j == i && n == k)) // if not starting point
                        {
                            if (flagBoard[j, n] == true) // for each tile around i, k
                            {
                                ajacentFlags += 1;
                            }
                            else if (shownBoard[j, n] == false)
                            {
                                ajacentHidden += 1;
                            }
                            else if (shownBoard[j, n] == true)
                            {
                                if ((numberBoard[j, n] - FindAjacentSafe(j, n)) < ajacentLowestNum && (numberBoard[j, n] - FindAjacentSafe(j, n)) > 0)
                                {
                                    ajacentLowestNum = numberBoard[j, n] - FindAjacentSafe(j, n);
                                    ajacentLowestPos[0] = j;
                                    ajacentLowestPos[1] = n;

                                }
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("lowest ajacent val is at: " + ajacentLowestPos[0] + " " + ajacentLowestPos[1] + ". with a val of " + ajacentLowestNum + ". orig is at " + i + " " + k);
        int unknownTiles = 0;
        // orig ajalowest <= ajahidden 
        // better is         <=                                       <=
        if (ajacentLowestNum + 1 <= ajacentHidden && ajacentLowestPos[0] > -1 && ajacentLowestNum < numberBoard[i, k] - ajacentFlags)
        {
            Debug.Log(ajacentLowestNum + " is less than " + (numberBoard[i, k] - ajacentFlags));
            for (int l = ajacentLowestPos[0] - 1; l < ajacentLowestPos[0] + 2; l++) // check surrounding 3x3 area of the ajacent lowest tile
            {
                for (int m = ajacentLowestPos[1] - 1; m < ajacentLowestPos[1] + 2; m++)
                {
                    if ((m < boardSize) && (m > -1))
                    {
                        if ((l < boardSize) && (l > -1)) // if search is inbounds
                        {
                            // commented out because the starting point would have to intersect with the original tile's domain anyways
                            //if (!(l == ajacentLowestPos[0] && m == ajacentLowestPos[1])) // if not starting point
                            //{

                            // if, ignoring the tiles intersecting [i, k], [l, m] otherwise is solved; then all good
                            if (!(l <= i + 1 && l >= i - 1 && m <= k + 1 && m >= k - 1))
                            {
                                if (shownBoard[l, m] == false && flagBoard[l, m] == false)
                                {
                                    unknownTiles += 1;
                                }
                            }
                            //}
                        }
                    }
                }
            }
        }

        if (unknownTiles == 0)
        {

            for (int j = i - 1; j < i + 2; j++) // check surrounding 3x3 area
            {
                for (int n = k - 1; n < k + 2; n++)
                {
                    if ((j < boardSize) && (j > -1))
                    {
                        if ((n < boardSize) && (n > -1)) // if search is inbounds
                        {
                            if (!(j <= ajacentLowestPos[0] + 1 && j >= ajacentLowestPos[0] - 1 && n <= ajacentLowestPos[1] + 1 && n >= ajacentLowestPos[1] - 1)) // if not within domain of ajacent tile
                            {

                                if (shownBoard[j, n] == false && flagBoard[j, n] == false)
                                {
                                    Debug.Log("attempting to solve " + i + " " + k + ". clicking " + j + " " + n);
                                    ShowError(i, k, j, n, ajacentLowestPos[0], ajacentLowestPos[1]);
                                    UpdateTile(j, n, false);
                                    //if (FindAjacentMines(i, k) == numberBoard[i, k])
                                    // {
                                    ToggleHigherFunctions(1);
                                    return;
                                    //  }
                                }
                            }
                        }
                    }
                }
            }

        }



    }

    /*

    logged instances of it being right:

    format:

    before  >   after

    ___, | = wall


    ? F 3       4 F 3
    ? 2 1   >   ? 2 1
    ? 1 0       ? 1 0
    _____       _____

    revealed values

    4 # 3
    3 2 1
    # 1 0



    being wrong
        1 2 3 4 5
  
    1   2 3 2 1 0 
    2   F F 2 1 0  
    3   ? 4 F 2 1 
    4   ? ? ? ? ?

    attempted to solve 3, 2.
    2, 3 was the lower aja val
    clicked on 1, 3 which was a mine

        1 2 3 4 5
  
    1   2 3 2 1 0 
    2   # # 2 1 0  
    3   # 4 # 2 1 
    4   4 3 1 2 #

    */


    void OrthogonalExtrapolation(int i, int k)
    {
        if (numberBoard[i, k] > 0)
        {


            //Debug.Log(i + " " + k);
            if (PerpendicularDomainSafe(i, k, 4)) // if on left wall
            {
                if (k + 2 < boardSize)
                {
                    if (PerpendicularDomainSafe(i, k+1, 2) == false)
                    {

                        
                        int hiddenTiles = 0;
                        int flaggedTiles = 0;
                        if (shownBoard[i, k + 1] == true)
                        {
                            if (numberBoard[i, k + 1] == numberBoard[i, k]) // no new mines
                            {
                                Debug.Log("no new mines");
                                for (int j = i - 1; j < i + 2; j++)
                                {
                                    ShowError(i, k, j, k + 2, i, k + 1);
                                    Debug.Log("Attempting "+j+" "+(k+2));
                                    UpdateTile(j, k + 2, false);
                                }
                                //ToggleHigherFunctions();
                            }
                            else if (numberBoard[i, k + 1] - 3 == numberBoard[i, k]) // all 3 new tiles are mines
                            {
                                Debug.Log("all 3 new tiles are mines");
                                for (int j = i - 1; j < i + 2; j++)
                                {
                                    if (shownBoard[j, k + 2] == false && flagBoard[j, k + 2] == false)
                                    {
                                        ShowError(i, k, j, k + 2, i, k + 1);
                                        Debug.Log("Attempting "+j+" "+(k+2));
                                        UpdateTile(j, k + 2, true);

                                    }
                                }
                                //ToggleHigherFunctions();
                            }
                            else if (numberBoard[i, k + 1] > numberBoard[i, k])
                            {
                                for (int j = i - 1; j < i + 2; j++)
                                {
                                    if (j > -1 && j < boardSize)
                                    {
                                        if (shownBoard[j, k + 2] == false)
                                        {
                                            hiddenTiles += 1;
                                        }
                                    }

                                }
                                Debug.Log(i + " " + k);
                                if ((numberBoard[i, k + 1] - numberBoard[i, k]) == hiddenTiles) // all remaining new tiles are mines
                                {
                                    Debug.Log("all remaining new tiles are mines");
                                    for (int j = i - 1; j < i + 2; j++)
                                    {
                                        if (j > -1 && j < boardSize)
                                        {
                                            if (shownBoard[j, k + 2] == false && flagBoard[j, k + 2] == false)
                                            {
                                                ShowError(i, k, j, k + 2, i, k + 1);
                                                Debug.Log("Attempting "+j+" "+(k+2));
                                                UpdateTile(j, k + 2, true);
                                            }

                                        }
                                    }
                                    //ToggleHigherFunctions();
                                }
                            }
                        }

                    }

                }

            }
            else if (PerpendicularDomainSafe(i, k, 2)) // if on right wall
            {
                if (k - 2 > -1)
                {
                    if (PerpendicularDomainSafe(i, k-1, 4) == false)
                    {

                        
                        int hiddenTiles = 0;
                        int flaggedTiles = 0;
                        if (shownBoard[i, k - 1] == true)
                        {
                            if (numberBoard[i, k - 1] == numberBoard[i, k]) // no new mines
                            {
                                Debug.Log("no new mines");
                                for (int j = i - 1; j < i + 2; j++)
                                {
                                    ShowError(i, k, j, k - 2, i, k - 1);
                                    Debug.Log("Attempting "+j+" "+(k-2));
                                    UpdateTile(j, k - 2, false);
                                }
                                //ToggleHigherFunctions();
                            }
                            else if (numberBoard[i, k - 1] - 3 == numberBoard[i, k]) // all 3 new tiles are mines
                            {
                                Debug.Log("all 3 new tiles are mines");
                                for (int j = i - 1; j < i + 2; j++)
                                {
                                    if (shownBoard[j, k - 2] == false && flagBoard[j, k - 2] == false)
                                    {
                                        ShowError(i, k, j, k - 2, i, k - 1);
                                        Debug.Log("Attempting "+j+" "+(k-2));
                                        UpdateTile(j, k - 2, true);

                                    }
                                }
                                //ToggleHigherFunctions();
                            }
                            else if (numberBoard[i, k - 1] > numberBoard[i, k])
                            {
                                for (int j = i - 1; j < i + 2; j++)
                                {
                                    if (j > -1 && j < boardSize)
                                    {
                                        if (shownBoard[j, k - 2] == false)
                                        {
                                            hiddenTiles += 1;
                                        }
                                    }

                                }
                                //Debug.Log(i + " " + k);
                                if ((numberBoard[i, k - 1] - numberBoard[i, k]) == hiddenTiles) // all remaining new tiles are mines
                                {
                                    Debug.Log("all remaining new tiles are mines");
                                    for (int j = i - 1; j < i + 2; j++)
                                    {
                                        if (j > -1 && j < boardSize)
                                        {
                                            if (shownBoard[j, k - 2] == false && flagBoard[j, k - 2] == false)
                                            {
                                                ShowError(i, k, j, k - 2, i, k - 1);
                                                Debug.Log("Attempting "+j+" "+(k-2));
                                                UpdateTile(j, k - 2, true);
                                            }

                                        }
                                    }
                                    //ToggleHigherFunctions();
                                }
                            }
                        }

                    }

                }
            }
            else if (PerpendicularDomainSafe(i, k, 1)) // if on ceiling         i, k is origin, i+1, k is ajacent number, i+2, k[-1, 0, 1] is target
            {
                if (i + 2 < boardSize) // if inbounds
                {
                    if (PerpendicularDomainSafe(i+1, k, 3) == false) // does solving reveal anything
                    {

                        
                        int hiddenTiles = 0;
                        int flaggedTiles = 0;
                        if (shownBoard[i+1, k] == true)
                        {
                            if (numberBoard[i+1, k] == numberBoard[i, k]) // no new mines
                            {
                                Debug.Log("no new mines");
                                for (int n = k - 1; n < k + 2; n++)
                                {
                                    ShowError(i, k, i+2, n, i+1, k);
                                    Debug.Log("Attempting "+(i+2)+" "+n);
                                    UpdateTile(i+2, n, false);
                                }
                                //ToggleHigherFunctions();
                            }
                            else if (numberBoard[i+1, k] - 3 == numberBoard[i, k]) // all 3 new tiles are mines
                            {
                                Debug.Log("all 3 new tiles are mines");
                                for (int n = k - 1; n < k + 2; n++)
                                {
                                    if (shownBoard[i+2, n] == false && flagBoard[i+2, n] == false)
                                    {
                                        ShowError(i, k, i+2, n, i+1, k);
                                        Debug.Log("Attempting "+(i+2)+" "+n);
                                        UpdateTile(i+2, n, true);

                                    }
                                }
                                //ToggleHigherFunctions();
                            }
                            else if (numberBoard[i+1, k] > numberBoard[i, k])
                            {
                                for (int n = k - 1; n < k + 2; n++)
                                {
                                    if (n > -1 && n < boardSize)
                                    {
                                        if (shownBoard[i + 2, n] == false)
                                        {
                                            hiddenTiles += 1;
                                        }
                                    }

                                }
                                //Debug.Log(i + " " + k);
                                if ((numberBoard[i+1, k] - numberBoard[i, k]) == hiddenTiles) // all remaining new tiles are mines
                                {
                                    Debug.Log("all remaining new tiles are mines");
                                    for (int n = k - 1; n < k + 2; n++)
                                    {
                                        if (n > -1 && n < boardSize)
                                        {
                                            if (shownBoard[i+2, n] == false && flagBoard[i+2, n] == false)
                                            {
                                                ShowError(i, k, i+2, n, i+1, k);
                                                Debug.Log("Attempting "+(i+2)+" "+n);
                                                UpdateTile(i+2, n, true);

                                            }

                                        }
                                    }
                                    //ToggleHigherFunctions();
                                }
                            }
                        }

                    }

                }
            }
            else if (PerpendicularDomainSafe(i, k, 3)) // if on floor       i, k is origin, i+1, k is ajacent number, i+2, k[-1, 0, 1] is target
            {
                if (i - 2 > -1) // if inbounds
                {
                    if (PerpendicularDomainSafe(i-1, k, 1) == false) // does solving reveal anything
                    {

                        
                        int hiddenTiles = 0;
                        int flaggedTiles = 0;
                        if (shownBoard[i-1, k] == true)
                        {
                            if (numberBoard[i-1, k] == numberBoard[i, k]) // no new mines
                            {
                                Debug.Log("no new mines");
                                for (int n = k - 1; n < k + 2; n++)
                                {
                                    ShowError(i, k, i-2, n, i-1, k);
                                    Debug.Log("Attempting "+(i-2)+" "+n);
                                    UpdateTile(i-2, n, false);
                                }
                                //ToggleHigherFunctions();
                            }
                            else if (numberBoard[i-1, k] - 3 == numberBoard[i, k]) // all 3 new tiles are mines
                            {
                                Debug.Log("all 3 new tiles are mines");
                                for (int n = k - 1; n < k + 2; n++)
                                {
                                    if (shownBoard[i-2, n] == false && flagBoard[i-2, n] == false)
                                    {
                                        ShowError(i, k, i-2, n, i-1, k);
                                        Debug.Log("Attempting "+(i-2)+" "+n);
                                        UpdateTile(i-2, n, true);

                                    }
                                }
                                //ToggleHigherFunctions();
                            }
                            else if (numberBoard[i-1, k] > numberBoard[i, k])
                            {
                                for (int n = k - 1; n < k + 2; n++)
                                {
                                    if (n > -1 && n < boardSize)
                                    {
                                        if (shownBoard[i - 2, n] == false)
                                        {
                                            hiddenTiles += 1;
                                        }
                                    }

                                }
                                //Debug.Log(i + " " + k);
                                if ((numberBoard[i-1, k] - numberBoard[i, k]) == hiddenTiles) // all remaining new tiles are mines
                                {
                                    Debug.Log("all remaining new tiles are mines");
                                    for (int n = k - 1; n < k + 2; n++)
                                    {
                                        if (n > -1 && n < boardSize)
                                        {
                                            if (shownBoard[i-2, n] == false && flagBoard[i-2, n] == false)
                                            {
                                                ShowError(i, k, i-2, n, i-1, k);
                                                Debug.Log("Attempting "+(i-2)+" "+n);
                                                UpdateTile(i-2, n, true);

                                            }

                                        }
                                    }
                                    //ToggleHigherFunctions();
                                }
                            }
                        }

                    }

                }
            }
        }
        
    }

    // FLAGS DONT COUNT AS SAFE
    bool PerpendicularDomainSafe(int i, int k, int direction) //1 up, 2 right, 3 down, 4 left. direction of wall facing from origin point. true if direction is out of bounds or safe
    {
        switch (direction)
        {
            case 1:
                {
                    if (i > 0)
                    {
                        for (int n = k - 1; n < k + 2; n++)
                        {
                            if (n > -1 && n < boardSize)
                            {
                                if (shownBoard[i - 1, n] == false/* && flagBoard[i - 1, n] == false*/)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                    break;
                }
            case 2:
                {
                    if (k < boardSize - 1)
                    {
                        for (int j = i - 1; j < i + 2; j++)
                        {
                            if (j > -1 && j < boardSize)
                            {
                                if (shownBoard[j, k + 1] == false/* && flagBoard[j, k + 1] == false*/)
                                {
                                    return false;
                                }
                            }
                            
                        }
                    }
                    return true;
                    break;
                }
            case 3:
                {
                    if (i < boardSize - 1)
                    {
                        for (int n = k - 1; n < k + 2; n++)
                        {
                            if (n > -1 && n < boardSize)
                            {
                                if (shownBoard[i + 1, n] == false/* && flagBoard[i + 1, n] == false*/)
                                {
                                    return false;
                                }
                            }

                        }
                    }
                    return true;
                    break;
                }
            case 4:
                {
                    if (k > 0)
                    {
                        for (int j = i - 1; j < i + 2; j++)
                        {
                            if (j > -1 && j < boardSize)
                            {
                                if (shownBoard[j, k - 1] == false/* && flagBoard[j, k - 1] == false*/)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                    break;
                }
            default:
                {
                    return false;
                }
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}





















