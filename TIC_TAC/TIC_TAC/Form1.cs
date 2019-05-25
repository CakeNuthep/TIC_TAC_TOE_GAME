using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TIC_TAC
{
    public partial class Form1 : Form
    {
        const int WIN = -10;
        const int LOST = 10;
        const int TIE = 0;
        const char PlAYER_SYMBOLE = 'X';
        const char COMPUTER_SYMBOLE = 'O';
        bool isGameEnd;
        List<int[]> winCombos = new List<int[]>() {
            new int[]{ 0, 1, 2},
            new int[]{3, 4, 5},
            new int[]{6, 7, 8},
            new int[]{0, 3, 6},
            new int[]{1, 4, 7},
            new int[]{2, 5, 8},
            new int[]{0, 4, 8},
            new int[]{6, 4, 2}
        };
        bool isYouTurn;
        char[] board;


        public class StateNode
        {
            public char[] currentBoard;
            public StateNode parent;
            public List<StateNode> childrenState;
            public StateNode bestChilState;
            public int MiniMaxValue
            {
                get
                {
                    return minimaxValue;
                }
                set
                {
                    isMinMaxValue = true;
                    minimaxValue = value;
                }
            }
            private int minimaxValue;
           
            public bool isMinMaxValue;
            public int level;
            public bool isPlayer;
            public int indexCellPut;
            public StateNode(char[] board, bool isPlayer, int level,int indexCellPut)
            {
                currentBoard = copyBoard(board);
                this.isPlayer = isPlayer;
                this.level = level;
                this.isMinMaxValue = false;
                childrenState = new List<StateNode>();
                this.indexCellPut = indexCellPut;
            }

            private char[] copyBoard(char[] board)
            {
                char[] copyBoard = new char[9];
                for (int indexBoard = 0; indexBoard < board.Length; indexBoard++)
                {
                    copyBoard[indexBoard] = board[indexBoard];
                }
                return copyBoard;
            }

            public void createChileNode()
            {
                for(int indexCellBoard=0; indexCellBoard<currentBoard.Count();indexCellBoard++)
                {
                    char symbole = currentBoard[indexCellBoard];
                    if(symbole != PlAYER_SYMBOLE && symbole != COMPUTER_SYMBOLE)
                    {
                        char[] childBoard = copyBoard(currentBoard);
                        bool nextTurn = !isPlayer;
                        if (isPlayer)
                        {
                            childBoard[indexCellBoard] = PlAYER_SYMBOLE;
                        }
                        else
                        {
                            childBoard[indexCellBoard] = COMPUTER_SYMBOLE;
                        }
                        StateNode childState = new StateNode(childBoard, nextTurn, level + 1, indexCellBoard);
                        childState.parent = this;
                        childrenState.Add(childState);
                    }
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            uiStopGame();
        }

        private void createBoard()
        {
            isGameEnd = false;
            board = new Char[9];
            isYouTurn = true;
        }
        
        private void uiStartGame()
        {
            label_message.Text = "TIC TAC TOE";

            button_xo0.Enabled = true;
            button_xo0.Text = "";

            button_xo1.Enabled = true;
            button_xo1.Text = "";

            button_xo2.Enabled = true;
            button_xo2.Text = "";

            button_xo3.Enabled = true;
            button_xo3.Text = "";

            button_xo4.Enabled = true;
            button_xo4.Text = "";

            button_xo5.Enabled = true;
            button_xo5.Text = "";

            button_xo6.Enabled = true;
            button_xo6.Text = "";

            button_xo7.Enabled = true;
            button_xo7.Text = "";

            button_xo8.Enabled = true;
            button_xo8.Text = "";
        }

        private void uiStopGame()
        {
            button_xo0.Enabled = false;
            button_xo1.Enabled = false;
            button_xo2.Enabled = false;
            button_xo3.Enabled = false;
            button_xo4.Enabled = false;
            button_xo5.Enabled = false;
            button_xo6.Enabled = false;
            button_xo7.Enabled = false;            
            button_xo8.Enabled = false;
        }

        private void uiEndGame(int result)
        {
            uiStopGame();
            if(result == WIN)
            {
                label_message.Text = "WIN";
            }
            else if(result == LOST)
            {
                label_message.Text = "LOST";
            }
            else
            {
                label_message.Text = "TIE";
            }
            button_start.Text = "Start";
        }

        private void changeTurn()
        {
            isYouTurn = !isYouTurn;
        }

        private Button selectCellBoard(int index)
        {
            if(index == 0)
            {
                return button_xo0;
            }
            else if(index == 1)
            {
                return button_xo1;
            }
            else if (index == 2)
            {
                return button_xo2;
            }
            else if (index == 3)
            {
                return button_xo3;
            }
            else if (index == 4)
            {
                return button_xo4;
            }
            else if (index == 5)
            {
                return button_xo5;
            }
            else if (index == 6)
            {
                return button_xo6;
            }
            else if (index == 7)
            {
                return button_xo7;
            }
            else
            {
                return button_xo8;
            }
        }

        private void putTacToc(int index)
        {
            Button button = selectCellBoard(index);
            if (isYouTurn)
            {
                button.Text = PlAYER_SYMBOLE.ToString();
                board[index] = PlAYER_SYMBOLE;
            }
            else
            {
                button.Text = COMPUTER_SYMBOLE.ToString();
                board[index] = COMPUTER_SYMBOLE;
            }
            button.Enabled = false;
            int isWinOrLost = checkIsWinOrLost(board);
            if (isWinOrLost == WIN || isWinOrLost == LOST)
            {
                uiEndGame(isWinOrLost);
                isGameEnd = true;
            }
            changeTurn();
        }

        private bool isFullBoard()
        {
            for(int indexCellBoard = 0;indexCellBoard < board.Length;indexCellBoard++)
            {
                if(board[indexCellBoard]!=PlAYER_SYMBOLE && board[indexCellBoard] != COMPUTER_SYMBOLE)
                {
                    return false;
                }
            }
            return true;
        }

        private int checkIsWinOrLost(char[] board)
        {
            int[] indexWinCombosArray;
            for (int indexList = 0;indexList< winCombos.Count;indexList++)
            {
                char symbole1, symbole2, symbole3;
                indexWinCombosArray = winCombos[indexList];
                symbole1 = board[indexWinCombosArray[0]];
                symbole2 = board[indexWinCombosArray[1]];
                symbole3 = board[indexWinCombosArray[2]];
                if(symbole1 == symbole2 && symbole2 == symbole3 && symbole3 == PlAYER_SYMBOLE)
                {
                    return WIN;
                }
                else if(symbole1 == symbole2 && symbole2 == symbole3 && symbole3 == COMPUTER_SYMBOLE)
                {
                    return LOST;
                }
            }
            return TIE;
        }
       
        private StateNode genertateAllState()
        {
            Queue<StateNode> deptFirstSearch = new Queue<StateNode>();
            StateNode currentState = new StateNode(board, isYouTurn,0,-1);
            deptFirstSearch.Enqueue(currentState);
            while(deptFirstSearch.Count!=0)
            {
                StateNode node = deptFirstSearch.Dequeue();
                int result = checkIsWinOrLost(node.currentBoard);
                    if (result == TIE)
                    {
                        node.createChileNode();
                        if (node.childrenState.Count == 0)
                        {
                            node.MiniMaxValue = result;
                        }
                        else
                        {
                            for (int indexChildrenState = 0; indexChildrenState < node.childrenState.Count(); indexChildrenState++)
                            {
                                deptFirstSearch.Enqueue(node.childrenState[indexChildrenState]);
                            }
                        }
                    }
                    else
                    {
                        node.MiniMaxValue = result;
                    }
            }
            return currentState;
        }

        

        private StateNode miniMax(StateNode currentState)
        {
            if(currentState.childrenState.Count == 0)
            {
                return currentState;
            }

            if (currentState.isPlayer)
            {
                for (int i = 0; i < currentState.childrenState.Count; i++)
                {
                    StateNode state = miniMax(currentState.childrenState[i]);
                    if (currentState.bestChilState == null)
                    {
                        currentState.bestChilState = state;
                        currentState.MiniMaxValue = state.MiniMaxValue;
                    }
                    else
                    {
                        if (currentState.MiniMaxValue > state.MiniMaxValue)
                        {
                            currentState.bestChilState = state;
                            currentState.MiniMaxValue = state.MiniMaxValue;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < currentState.childrenState.Count; i++)
                {
                    StateNode state = miniMax(currentState.childrenState[i]);
                    if (currentState.bestChilState == null)
                    {
                        currentState.bestChilState = state;
                        currentState.MiniMaxValue = state.MiniMaxValue;
                    }
                    else
                    {
                        if (currentState.MiniMaxValue < state.MiniMaxValue)
                        {
                            currentState.bestChilState = state;
                            currentState.MiniMaxValue = state.MiniMaxValue;
                        }
                    }
                }
            }
            return currentState;
        }

        private void computerPlay()
        {
            if(isFullBoard())
            {
                isGameEnd = true;
            }
            if (!isGameEnd)
            {
                StateNode allState = genertateAllState();
                StateNode state = miniMax(allState);
                int indexCell = state.bestChilState.indexCellPut;
                putTacToc(indexCell);
            }
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if (button_start.Text.ToLower() == "start")
            {
                button_start.Text = "Stop";
                createBoard();
                uiStartGame();
                if(isYouTurn==false)
                {
                    computerPlay();
                }
            }
            else
            {
                button_start.Text = "Start";
                uiStopGame();
            }

        }

        private void button_xo0_Click(object sender, EventArgs e)
        {
            putTacToc(0);
            computerPlay();
        }

        private void button_xo1_Click(object sender, EventArgs e)
        {
            putTacToc(1);
            computerPlay();
        }

        private void button_xo2_Click(object sender, EventArgs e)
        {
            putTacToc(2);
            computerPlay();
        }

        private void button_xo3_Click(object sender, EventArgs e)
        {
            putTacToc(3);
            computerPlay();
        }

        private void button_xo4_Click(object sender, EventArgs e)
        {
            putTacToc(4);
            computerPlay();
        }

        private void button_xo5_Click(object sender, EventArgs e)
        {
            putTacToc(5);
            computerPlay();
        }

        private void button_xo6_Click(object sender, EventArgs e)
        {
            putTacToc(6);
            computerPlay();
        }

        private void button_xo7_Click(object sender, EventArgs e)
        {
            putTacToc(7);
            computerPlay();
        }

        private void button_xo8_Click(object sender, EventArgs e)
        {
            putTacToc(8);
            computerPlay();
        }
    }
}
