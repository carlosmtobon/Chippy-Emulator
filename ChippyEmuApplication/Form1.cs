using ChipCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChippyEmuApplication
{
    public partial class Form1 : Form
    {
        // private Interpreter  _intrepreter;
        RAM _ram;
        Display _display;
        CPU _cpu;
        KeyPad _kb;
      

        public Form1()
        {
            InitializeComponent();
        }

        public void Init(String fileName)
        {
            _ram = new RAM(fileName);
            _display = new Display(_ram);
            _kb = new KeyPad();
            _cpu = new CPU(_ram, _display, _kb);
            GameTick();
        }

        private void GameTick()
        {
            Stopwatch total = new Stopwatch();
            Stopwatch stopwatch = new Stopwatch();

            int instruction = 0;
            int frames = 0;
            stopwatch.Start();
            total.Start();
            while (_cpu.GetProgramCounter() < _ram.GetMemSize())
            {
                stopwatch.Restart();
                _cpu.ExecuteCycle();
                instruction++;
               
                if (instruction == 9)
                {
                    frames++;
                    _cpu.UpdateTimers();
                    if (_display.drawScreen)
                    {
                        UpdateScreen(_display);
                        _display.drawScreen = false;
                    }
                    instruction = 0;
                }
                
                var timeElapsed = stopwatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
                while (timeElapsed < 1852)
                {
                    timeElapsed = stopwatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
                }

                if (total.ElapsedMilliseconds > 1000)
                {
                    Console.WriteLine(frames);
                    frames = 0;
                    total.Restart();
                }
            }
        }

        private void UpdateScreen(Display display)
        {
            try
            {
                var size = 5;
                Bitmap image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                using (Graphics g = Graphics.FromImage(image))
                {

                    Brush brsh = new SolidBrush(Color.Red);

                    byte[][] pixels = _display._displayPixels;

                    for (int y = 0; y < pixels[0].Length; y++)
                    {
                        for (int x = 0; x < pixels.Length; x++)
                        {
                            if (pixels[x][y] == 1)
                            {
                                g.FillRectangle(brsh, (pictureBox1.Width/64) * x, (pictureBox1.Height / 32) * y , size, size);
                            }
                        }
                    }
                    pictureBox1.Image = image;
                    _display.ConsoleDisplay();
                }
            }
            catch (Exception ex)
            {

            }
        }
          

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int pos = GetKeyValueHex(e.KeyCode);
            if (pos != -1)
                _kb.SetKeyDown(pos);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            int pos = GetKeyValueHex(e.KeyCode);
            if (pos != -1)
                _kb.SetKeyUp(pos);
        }

        private int GetKeyValueHex(Keys keyValue)
        {
            int pos = -1;
            switch (keyValue)
            {
                case Keys.X:
                    pos = 0;
                    break;
                case Keys.D1:
                    pos = 1;
                    break;
                case Keys.D2:
                    pos = 2;
                    break;
                case Keys.D3:
                    pos = 3;
                    break;
                case Keys.Q:
                    pos = 4;
                    break;
                case Keys.W:
                    pos = 5;
                    break;
                case Keys.E:
                    pos = 6;
                    break;
                case Keys.A:
                    pos = 7;
                    break;
                case Keys.S:
                    pos = 8;
                    break;
                case Keys.D:
                    pos = 9;
                    break;
                case Keys.Z:
                    pos = 10;
                    break;
                case Keys.C:
                    pos = 11;
                    break;
                case Keys.D4:
                    pos = 12;
                    break;
                case Keys.R:
                    pos = 13;
                    break;
                case Keys.F:
                    pos = 14;
                    break;
                case Keys.V:
                    pos = 15;
                    break;
            }

            return pos;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Thread t = new Thread(()=> Init(((OpenFileDialog)sender).FileName));
            t.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
