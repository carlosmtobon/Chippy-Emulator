using System;
using System.Collections.Generic;
using System.Text;

namespace Chippy_Emulator
{
    class Display
    {
        byte[][] _displayPixels;
     
        RAM _ram;

        public Display(RAM ram)
        {
            _ram = ram;
            InitDisplay();
        }

        private void InitDisplay()
        {
            _displayPixels = new byte[32][];
            for (int i = 0; i < _displayPixels.Length; i++)
            {
                _displayPixels[i] = new byte[64];
            }
        }

        public void DisplayScreen()
        {
            for (int i = 0; i < _displayPixels.Length; i++)
            {
                for (int j = 0; j < _displayPixels[i].Length; j++)
                {
                    if (_displayPixels[i][j] == 1)
                    {
                        Console.Write("*");
                    }
                    else
                        Console.Write("_");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        public bool RenderSprite(int x, int y, Sprite sprite)
        {
            bool collision = false;
            var spriteData = sprite.GetSpriteData();
            for (int i = 0; i < spriteData.Length; i++)
            {
                byte b = spriteData[i];
                for (int j = 0; j < 8; j++)
                {
                    var bitInBuff = _displayPixels[((y + i) % 32)][(x + j) % 64];
                    var bit = GetBit(b, j);
                    _displayPixels[((y + i) % 32)][(x + j) % 64] ^= bit;
                    var bitInBuffAfter = _displayPixels[((y + i) % 32)][(x + j) % 64];
                    if ((bitInBuff == 1) && bitInBuff != bitInBuffAfter) 
                        collision = true; 
                }
            }
            return collision;
        }

        public byte GetBit(byte b, int pos)
        {
            byte val = 0x0;
            if (pos == 0)
                val = 0x80;
            else if (pos == 1)
                val = 0x40;
            else if (pos == 2)
                val = 0x20;
            else if (pos == 3)
                val = 0x10;
            else if (pos == 4)
                val = 0x8;
            else if (pos == 5)
                val = 0x4;
            else if (pos == 6)
                val = 0x2;
            else if (pos == 7)
                val = 0x1;
            return (byte)((b & val) >>  (7 - pos));
        }

        public void ClearScreen()
        {
            for (int i = 0; i < _displayPixels.Length; i++)
            {
                for (int j = 0; j < _displayPixels[i].Length; j++)
                {
                    _displayPixels[i][j] = 0;
                }
            }
        }

    }
}
