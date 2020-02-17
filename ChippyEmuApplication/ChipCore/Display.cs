using System;

namespace ChipCore
{
    class Display
    {
        public byte[][] _displayPixels;
        readonly RAM _ram;
        public int Height = 64;
        public int Width = 124;
        internal bool drawScreen;

        public Display(RAM ram)
        {
            _ram = ram;
            InitDisplay();
        }

        private void InitDisplay()
        {
            _displayPixels = new byte[Height][];
            for (int i = 0; i < _displayPixels.Length; i++)
            {
                _displayPixels[i] = new byte[Width];
            }
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
                    var bitInBuff = _displayPixels[(y + i) % Width][(x + j) % Height];
                    byte bit = (byte)((b >> (7 - j)) & 1);
                    _displayPixels[(y + i) % Width][(x + j) % Height] ^= bit;
                    var bitInBuffAfter = _displayPixels[(y + i) % Width][(x + j) % Height];
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
                    Console.Write(_displayPixels[i][j] == 1 ? "*" : "_");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
