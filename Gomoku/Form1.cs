using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Gomoku
{
    public partial class Form1 : Form
    {
        private GameManager game=new GameManager();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void reset()
        {
            this.Controls.Clear();
            game=new GameManager();
        }
        
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (game.CurrentPlayer!=PieceType.BLACK) return;

            Point point=game.FindTheCloseNode(e.X, e.Y);
            Piece piece = game.PlaceAPiece(point.X, point.Y);
            if (piece == null) return;
            
            label1.Visible = false;
            this.Controls.Add(piece);

            //檢查是否有人獲勝
            if (game.Winner == PieceType.BLACK)
            {
                if (MessageBox.Show("Black wins!\nContinue?", "Result", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    reset();
                else
                    System.Environment.Exit(0);
                return;
            }
            else if (game.Winner == PieceType.WHITE)
            {
                if (MessageBox.Show("White wins!\nContinue?", "Result", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    reset();
                else
                    System.Environment.Exit(0);
                return;
            }

            Piece piece2 = game.ComputerPlay();
            this.Controls.Add(piece2);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //label1.Text = "(" + e.X + "," + e.Y + ")";
            Point point = game.FindTheCloseNode(e.X, e.Y);
            if (game.CanBePlaced(point.X, point.Y))
                this.Cursor = Cursors.Hand;
            else
                this.Cursor = Cursors.Default;
        }
    }
}
