﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation_Lab_4
{
    public partial class Form1 : Form
    {
        Graphics graphics;
        int resolution;
        bool[,] field;
        int cols;
        int rows;

        public Form1()
        {
            InitializeComponent();
        }

        private void startGame()
        {
            if (timer1.Enabled) return;
            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            resolution = (int)nudResolution.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            field = new bool[cols, rows];
            Random random = new Random();
            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                    field[x, y] = random.Next((int)nudDensity.Value) == 0;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void nextGeneration()
        {
            graphics.Clear(Color.Black);

            var newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                {
                    var neigboursCount = countNeighbours(x, y);
                    bool hasLife = field[x, y];

                    if (!hasLife && neigboursCount == 3) newField[x, y] = true;
                    else if (hasLife && (neigboursCount < 2 || neigboursCount > 3)) newField[x, y] = false;
                    else newField[x, y] = field[x, y];

                    if (hasLife)
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                }
            field = newField;
            pictureBox1.Refresh();
        }

        private int countNeighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;
                    var isSelfChecking = col == x && row == y;
                    var hasLife = field[col, row];
                    if (hasLife && !isSelfChecking) count++;
                }

            return count;
        }

        private void stopGame()
        {
            if (!timer1.Enabled) return;
            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            startGame();
            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nextGeneration();
        }
    }
}
