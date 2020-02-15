using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChipCore
{
    class Interpreter 
    {
        public RAM _ram;
        public Display _display;
        public CPU _cpu;
        public KeyPad _kb;
        public Stopwatch stopwatch;

        public void Init()
        {
            _ram = new RAM(@"C:\Users\Carlos\Desktop\c8games\TETRIS");
            _display = new Display(_ram);
            _kb = new KeyPad();
            _cpu = new CPU(_ram, _display, _kb);
            stopwatch = new Stopwatch();

            int instruction = 0;
            while (_cpu.GetProgramCounter() < _ram.GetMemSize())
            {
               // stopwatch.Restart();
                _cpu.ExecuteCycle();
                instruction++;
                if (instruction == 9)
                {
                    _cpu.UpdateTimers();
                    instruction = 0;
                }

                UpdateScreen(_display);
                Thread.Sleep(2);

                //var timeElapsed = stopwatch.ElapsedTicks / (Stopwatch.Frequency/ (1000 * 1000));
                //while (timeElapsed < 1852)
                //{
                //     timeElapsed = stopwatch.ElapsedTicks / (Stopwatch.Frequency / (1000 * 1000));
                //} 
            }
        }

        private void UpdateScreen(Display display)
        {
            Bitmap image = new Bitmap(64, 32);
            byte[][] pixels = _display._displayPixels;

            for (int x = 0; x < pixels.Length; x++)
            {
                for (int y = 0; y < pixels[0].Length; y++)
                {
                    if (pixels[x][y] == 1)
                    {
                        image.SetPixel(x, y, Color.White);
                    }
                }
            }

            PictureBox pictureBox = new PictureBox(); 
            pictureBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            pictureBox.Location = new System.Drawing.Point(48, 56);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new System.Drawing.Size(864, 392);
            pictureBox.TabIndex = 1;
            pictureBox.TabStop = false;

        }

        public void SetKeys(byte key)
        {
            _kb.SetKeyDown(key);
        }
    }
}
