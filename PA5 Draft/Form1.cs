using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace PA5_Draft
{
    public partial class MainForm : Form
    {
        private int Step = 1, holder = 0;
        private readonly SnakeGame Game;
        private int NumberOfApples = 2000, eatenApples = 0;
        private SoundPlayer eatSound, hitWall, hitSelf;
        public MainForm()
        {
            UserInput stupid = new UserInput();
            stupid.ShowDialog(this);

            if (stupid.DialogResult == DialogResult.OK)
            {
                InitializeComponent();
                string rootLocation = typeof(Program).Assembly.Location;
                eatSound = new SoundPlayer();
                hitWall = new SoundPlayer();
                hitSelf = new SoundPlayer();
                eatSound.SoundLocation = @"C:\Users\Asterisk\Documents\2020 Fall Semester\Advanced Windows Programming\PA5\PA5 Draft\PA5 Draft\Pew.wav";
                hitWall.SoundLocation = @"C:\Users\Asterisk\Documents\2020 Fall Semester\Advanced Windows Programming\PA5\PA5 Draft\PA5 Draft\Hitwall.wav";
                hitSelf.SoundLocation = @"C:\Users\Asterisk\Documents\2020 Fall Semester\Advanced Windows Programming\PA5\PA5 Draft\PA5 Draft\Eatself.wav";
                progressBar1.Value = 1;
                progressBar1.Step = 1;
                NumberOfApples = stupid.TakeInput();
                Game = new SnakeGame(new System.Drawing.Point((Field.Width - 20) / 2, Field.Height / 2), 40, NumberOfApples, Field.Size);
                Field.Image = new Bitmap(Field.Width, Field.Height);
                Game.EatAndGrow += Game_EatAndGrow;
                Game.HitWallAndLose += Game_HitWallAndLose;
                Game.HitSnakeAndLose += Game_HitSnakeAndLose;
            }
        }

        private void Game_HitWallAndLose()
        {
            hitWall.Play();
            mainTimer.Stop();
            MessageBox.Show("You ate: " + eatenApples + " apples!!!");
            Field.Refresh();
        }
        private void Game_HitSnakeAndLose()
        {
            hitSelf.Play();
            mainTimer.Stop();
            MessageBox.Show("You ate: " + eatenApples + " apples!!!");
            Field.Refresh();
        }

        private void Field_Click(object sender, EventArgs e)
        {
            int temp;
            temp = Step;
            Step = holder;
            holder = temp;
        }

        private void Game_EatAndGrow()
        {
            eatSound.Play();
            eatenApples++;
            if (eatenApples % 10 == 0 && Step != 10)
            {
                Step++;
                progressBar1.PerformStep();
            }
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            Game.Move(Step);
            Field.Invalidate();
        }

        private void Field_Paint(object sender, PaintEventArgs e)
        {
            Pen PenForObstacles = new Pen(Color.FromArgb(40,40,40),2);
            Pen PenForSnake = new Pen(Color.FromArgb(100, 100, 100), 2);
            Brush BackGroundBrush = new SolidBrush(Color.FromArgb(150, 250, 150));
            Brush AppleBrush = new SolidBrush(Color.FromArgb(250, 50, 50));
            Brush AppleBrush2 = new SolidBrush(Color.FromArgb(0,0,100));
            using (Graphics g = Graphics.FromImage(Field.Image))
            {
                g.FillRectangle(BackGroundBrush,new Rectangle(0,0,Field.Width,Field.Height));
                foreach (Point Apple in Game.Apples)
                    g.FillEllipse(AppleBrush, new Rectangle(Apple.X - SnakeGame.AppleSize / 2, Apple.Y - SnakeGame.AppleSize / 2,
                        SnakeGame.AppleSize, SnakeGame.AppleSize));
                foreach (LineSeg Obstacle in Game.Obstacles)
                    g.DrawLine(PenForObstacles, new System.Drawing.Point(Obstacle.Start.X, Obstacle.Start.Y)
                        , new System.Drawing.Point(Obstacle.End.X, Obstacle.End.Y));
                foreach (LineSeg Body in Game.SnakeBody)
                    g.DrawLine(PenForSnake, new System.Drawing.Point((int)Body.Start.X, (int)Body.Start.Y)
                        , new System.Drawing.Point((int)Body.End.X, (int)Body.End.Y));
            }
        }



        private void Snakes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    Game.Move(Step, Direction.UP);
                    break;
                case Keys.Down:
                    Game.Move(Step, Direction.DOWN);
                    break;
                case Keys.Left:
                    Game.Move(Step, Direction.LEFT);
                    break;
                case Keys.Right:
                    Game.Move(Step, Direction.RIGHT);
                    break;
            }
        }
    }
}
