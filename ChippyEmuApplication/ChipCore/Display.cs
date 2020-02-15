using System;
using System.Collections.Generic;
using System.Text;

namespace ChipCore
{
    class Display
    {
        public byte[][] _displayPixels;
     
        RAM _ram;
        internal bool drawScreen ;

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

        public bool RenderSprite(int x, int y, Sprite sprite)
        {
            bool collision = false;
            var spriteData = sprite.GetSpriteData();
            for (int i = 0; i < spriteData.Length; i++)
            {
                byte[] row = _displayPixels[x];
                byte b = spriteData[i];
                for (int j = 0; j < 8; j++)
                {
                    var bitInBuff = _displayPixels[((x + j) % 64)][(y + i) % 32];
                    byte bit = (byte)((b >> (7 - j)) & 1);
                    _displayPixels[((x + j) % 64)][(y + i) % 32] ^= bit;
                    var bitInBuffAfter = _displayPixels[((x + j) % 64)][(y + i) % 32];
                    if ((bitInBuff == 1) && bitInBuff != bitInBuffAfter) 
                        collision = true; 
                }
            }
            return collision;
        }

        public void ClearScreen()
        {
            for (int i = 0; i < _displayPixels.Length; i++)
            {
                for (int j = 0; j < _displayPixels[0].Length; j++)
                {
                    _displayPixels[i][j] = 0;
                }
            }
        }

        public void ConsoleDisplay()
        {
            for (int i = 0; i < _displayPixels.Length; i++)
            {
                for (int j = 0; j < _displayPixels[0].Length; j++)
                {
                    Console.Write(_displayPixels[i][j] == 1 ? "*" : " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
