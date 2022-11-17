using System.Drawing;

namespace Gomoku
{
    class GameManager
    {
        private Board board = new Board();
        private Point endPoint;
        private Point lastComPoint = new Point(-2,-2);
        private bool isAlive = false;

        private PieceType currentPlayer = PieceType.BLACK;
        public PieceType CurrentPlayer { get { return currentPlayer; } }

        private PieceType winner = PieceType.NONE;
        public PieceType Winner { get { return winner; } }

        public bool CanBePlaced(int x, int y)  
        {
            return board.canBePlaced(x, y);
        }
        public Point FindTheCloseNode(int x, int y)
        {
            return board.FindTheCloseNode(x, y);
        }
        public Piece PlaceAPiece(int x, int y)
        {
            Piece piece = board.placeAPiece(x, y, currentPlayer);
            if (piece != null)
            {
                //檢查勝利者出現與否
                if(checkWinner(5,board.LastPlaceNode,currentPlayer)) winner=currentPlayer;
                //交換選手
                if (currentPlayer == PieceType.BLACK)
                    currentPlayer = PieceType.WHITE;
                else if (currentPlayer == PieceType.WHITE)
                    currentPlayer = PieceType.BLACK;

                return piece;
            }

            return null;
        }

        public Piece ComputerPlay()
        {
            Point point=new Point();
            Point lastPoint = board.LastPlaceNode;
            Point compare = new Point(-1, -1);

            if (checkWinner(4, lastComPoint, PieceType.WHITE) && endPoint != compare)
                point = endPoint;
            else if (checkWinner(4, lastPoint, PieceType.BLACK) && endPoint != compare)
                point = endPoint;
            else if (checkWinner(3, lastComPoint, PieceType.WHITE) && endPoint != compare && isAlive)
                point = endPoint;
            else if (checkWinner(3, lastPoint, PieceType.BLACK) && endPoint != compare && isAlive)
                point = endPoint;
            else if (checkWinner(3, lastComPoint, PieceType.WHITE) && endPoint != compare)
                point = endPoint;
            else if (checkWinner(3, lastPoint, PieceType.BLACK) && endPoint != compare)
                point = endPoint;
            else if (canPlaceBeside(lastComPoint) != compare)
                point = canPlaceBeside(lastComPoint);
            else if (canPlaceBeside(lastPoint) != compare)
                point = canPlaceBeside(lastPoint);
            else
            {
                for (int i = 0; i < Board.NODE_COUNT; i++)
                {
                    for(int j=0;j < Board.NODE_COUNT; j++)
                    {
                        if (board.canBePlaced(i, j))
                            point = new Point(i, j);
                    }
                }
            }

            Piece piece = PlaceAPiece(point.X, point.Y);
            lastComPoint = point;
            return piece;
        }

        private bool checkWinner(int num,Point center,PieceType pieceType)
        {
            endPoint = new Point(-1, -1);
            isAlive= false;
            //檢查八個不同方向
            for (int xDir = 0; xDir <= 1; xDir++)
            {
                for (int yDir = -1; yDir <= 1; yDir++)
                {
                    //排除中間的狀況
                    if ((xDir == 0 && yDir == 0)|| (xDir == 0 && yDir == -1))
                        continue;
                    
                    //紀錄現在看到的相同棋子數
                    int count = 1;
                    count += countLine(num,center,xDir,yDir,pieceType);
                    count += countLine(num, center, 0-xDir, 0-yDir,pieceType);

                    //檢查是否看到五顆棋子
                    if (count >= num) return true;

                    endPoint = new Point(-1, -1);
                }
            }
            return false;
        }

        private Point canPlaceBeside(Point center)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    if (board.canBePlaced(center.X + i, center.Y + j))
                        return new Point(center.X + i, center.Y + j);
                }
            }
            return new Point(-1,-1);
        }

        private int countLine(int num, Point center, int xDir, int yDir,PieceType pieceType)
        {
            int count = 0;
            int targetX = center.X;
            int targetY = center.Y;
            bool isOutRange = false;

            while (count < num)
            {
                targetX += xDir;
                targetY += yDir;

                if (targetX < 0 || targetX >= Board.NODE_COUNT || targetY < 0 || targetY >= Board.NODE_COUNT)
                {
                    isOutRange = true;
                    break;
                }
                //檢查顏色是否相同
                else if (board.GetPieceType(targetX, targetY) == pieceType)
                    count++;
                else
                    break;
            }
            if (!isOutRange && board.GetPieceType(targetX, targetY) == PieceType.NONE)
            {
                if (endPoint != new Point(-1, -1)) isAlive = true;
                else
                    endPoint = new Point(targetX, targetY);
            }
    
            return count;
        }
        public void GameReset()
        {
            winner = PieceType.NONE;
            board.BoardReset();
            currentPlayer = PieceType.BLACK;
        }
    }
}
