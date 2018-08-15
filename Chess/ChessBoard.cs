using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessBoard
    {
        private ChessPiece[,] boardArray;
        private const int COLUMNS = 8;
        private int ROWS = 8;

        public ChessBoard()
        {

        }

        public int GetLength(int l)
        {
            return boardArray.GetLength(l);
        }

        public ChessPiece this[int x, int y]
        {
            get
            {
                return boardArray[x, y];
            }
        }

        private ChessBoard SetupBoard()
        {
            boardArray = new ChessPiece[COLUMNS, ROWS];
            string[] playerPieces = {
                "Rook", "Kinght", "Bishop", "Queen",
                "King", "Bishop", "Knight", "Rook",
                "Pawn", "Pawn", "Pawn", "Pawn",
                "Pawn", "Pawn", "Pawn", "Pawn" };

            for (int i = 0; i < COLUMNS; i++)
            {
                boardArray[i, 0] = (ChessPiece)Activator.CreateInstance(Type.GetType("Chess." + playerPieces[i]));
                boardArray[i, 1] = (ChessPiece)Activator.CreateInstance(Type.GetType("Chess." + playerPieces[i + COLUMNS]));

                boardArray[i, ROWS - 1] = (ChessPiece)Activator.CreateInstance(Type.GetType("Chess." + playerPieces[i]), new object[] { 1 });
                boardArray[i, ROWS - 2] = (ChessPiece)Activator.CreateInstance(Type.GetType("Chess." + playerPieces [i + COLUMNS]), new object[] { 2 });
            }
            return this;
        }

        public IEnumerable<Point> PieceActions(int x, int y, bool ignoreCheck = false, bool attackActions = true, bool moveAttacks = true, ChessPiece[,] boardArray = null)
        {
            if (boardArray == null)
            {
                boardArray = this.boardArray;
            }

            bool[,] legalActions = new bool[boardArray.GetLength(0), boardArray.GetLength(1)];
            List<Point> availableActions = new List<Point>();
            ChessPiece movingPiece = boardArray[x, y];

            if (attackActions)
            {
                foreach (Point[] direction in movingPiece.AvailableAttacks)
                {
                    foreach (Point attackPoint in direction)
                    {
                        Point adjustedPoint = new Point(attackPoint.x + x, attackPoint.y + y);
                        if (ValidatePoint(adjustedPoint))
                        {
                            if (boardArray[adjustedPoint.x, adjustedPoint.y] != null &&
                                boardArray[adjustedPoint.x, adjustedPoint.y].Player == 
                                movingPiece.Player) break;
                            if (boardArray[adjustedPoint.x, adjustedPoint.y] != null)
                            {
                                AddMove(availableActions, new Point(x, y), adjustedPoint, ignoreCheck);
                                break;
                            }
                        }
                    }
                }
            }

            if (moveAttacks)
            {
                foreach (Point[] direction in movingPiece.AvailableMoves)
                {
                    foreach (Point movePoint in direction)
                    {
                        Point adjustedPoint = new Point(movePoint.x + x, movePoint.y + y);
                        if (ValidatePoint(adjustedPoint))
                        {
                            if (boardArray[adjustedPoint.x, adjustedPoint.y] != null) break;
                            AddMove(availableActions, new Point(x, y), adjustedPoint, ignoreCheck);
                        }
                    }
                }
            }

            if (movingPiece is King && )
            {


            }
        }

        private void AddMove(List<Point> availableActions, Point fromPoint, Point toPoint, bool ignoreCheck = false)
        {
            bool kingInCheck = false;

            if (!ignoreCheck)
            {
                ChessPiece movingPiece = boardArray[fromPoint.x, fromPoint.y];
                ChessPiece[,] boardArrayBackup = (ChessPiece[,])boardArray.Clone();
                ActionPiece(fromPoint, toPoint, true);
                kingInCheck = KingInCheck(movingPiece.Player);
                boardArray = boardArrayBackup;
            }

            if (ignoreCheck || !kingInCheck) availableActions.Add(toPoint);
        }

        public bool KingInCheck(int player)
        {
            for (int x = 0; x < COLUMNS; x++)
            {
                for(int y = 0; y < ROWS; y++)
                {
                    ChessPiece chessPiece = boardArray[x, y];

                    if (chessPiece != null
                        && chessPiece.Player == player
                        && chessPiece is King)
                    {
                        if (CheckSquareVulnerable(x, y, player))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            throw new Exception("King wasn't found!");
        }

        public IEnumerable<Point> PieceActions(Point position, bool ignoreCheck = false, bool attackActions = true, bool moveActions = true, ChessPiece[,] boardArray = null)
        {
            return PieceActions(position.x, position.y, ignoreCheck, attackActions, moveActions, boardArray);
        }

        public bool ActionPiece(int fromX, int fromY, int toX, int toY)
        {
            return ActionPiece(new Point(fromX, fromY), new Point(toX, toY));
        }

        public bool ActionPiece(Point from, Point to, bool bypassValidation = false)
        {
            if (bypassValidation || PieceActions(from).Contains(to))
            {
                ChessPiece movingPiece = boardArray[from.x, from.y];
                if (movingPiece is Pawn)
                {
                    Pawn pawn = movingPiece;
                }
            }
            return false;
        }

        public bool CheckSquareVulnerable(int squareX, int squareY, int player, ChessPiece[,] boardArray = null)
        {
            return false;
        }

        private bool ValidateRange(int value, int high, int low = -1)
        {
            return value > low && value < high;
        }

        public bool ValidateX(int value)
        {
            return ValidateRange(value, boardArray.GetLength(0));
        }

        public bool ValidateY(int value)
        {
            return ValidateRange(value, boardArray.GetLength(1));
        }

        public bool ValidatePoint(Point point)
        {
            return ValidateX(point.x) && ValidateY(point.y);
        }
    }
}
