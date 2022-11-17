using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Gomoku
{
    class Board
    {
        private static readonly Point NO_MATCH_MODE = new Point(-1, -1);
        private static readonly int OFFSET = 75;
        private static readonly int NODE_RADIUS = 10;
        private static readonly int NODE_DISTANCE = 75;
        public static readonly int NODE_COUNT = 9;

        private Piece[,] piece = new Piece[9,9];
        private Point lastPlaceNode = NO_MATCH_MODE;
        public Point LastPlaceNode { get { return lastPlaceNode; } }

        public PieceType GetPieceType(int nodeIdX, int nodeIdY)
        {
            if (piece[nodeIdX, nodeIdY] == null)
                return PieceType.NONE;
            else
                return piece[nodeIdX, nodeIdY].GetPieceType();
        }
        public bool canBePlaced(int x, int y)
        {
            if (x < 0 || x >= NODE_COUNT || y < 0 || y >= NODE_COUNT)
                return false;

            Point nodeId = new Point(x, y);
            //有的話，檢查是否有棋子存在
            if (piece[nodeId.X, nodeId.Y] != null)
                return false;
            else
                return true;
        }

        public Piece placeAPiece(int x, int y,PieceType type)
        {
            //找出最近的節點
            Point nodeId = new Point(x, y);
            //沒有的話，回應fasle
            if (nodeId == NO_MATCH_MODE)
                return null;
            //有的話，檢查是否有棋子存在
            if (piece[nodeId.X, nodeId.Y] != null)
                return null;
            //根據type產生不同的棋子
            Point formPos = convertToFormPosition(nodeId);
            if (type == PieceType.BLACK)
                piece[nodeId.X, nodeId.Y] = new BlackPiece(formPos.X,formPos.Y);
            else if (type == PieceType.WHITE)
                piece[nodeId.X, nodeId.Y] = new WhitePiece(formPos.X, formPos.Y);

            //紀錄最後下的棋子位置
            lastPlaceNode = nodeId;

            return piece[nodeId.X, nodeId.Y];
        }
        private Point convertToFormPosition(Point nodeId)
        {
            Point formPosition=new Point();
            formPosition.X = OFFSET + nodeId.X * NODE_DISTANCE;
            formPosition.Y = OFFSET + nodeId.Y * NODE_DISTANCE;
            return formPosition;
        }
        public Point FindTheCloseNode(int x, int y)
        {
            int nodeIdX = findTheCloseNode(x);
            if (nodeIdX == -1)
                return NO_MATCH_MODE;

            int nodeIdY = findTheCloseNode(y);
            if (nodeIdY == -1)
                return NO_MATCH_MODE;

            return new Point(nodeIdX, nodeIdY);
        }
        private int findTheCloseNode(int pos)
        {
            if (pos <= OFFSET - NODE_RADIUS)
                return -1;
            else if (pos >=OFFSET+8*NODE_DISTANCE+NODE_RADIUS)
                return -1;
            else
                pos -= OFFSET;
            int quotient = pos / NODE_DISTANCE;
            int remainder = pos % NODE_DISTANCE;

            if (remainder <= NODE_RADIUS)
                return quotient;
            else if (remainder >= NODE_DISTANCE - NODE_RADIUS)
                return quotient + 1;
            else
                return -1;
        }
        public void BoardReset()
        {
            piece = new Piece[9, 9];
            lastPlaceNode = NO_MATCH_MODE;
        }

    }
}
