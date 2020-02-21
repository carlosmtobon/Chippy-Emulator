using System;

namespace ChipCore
{
    class Display
    {
        public byte[][] _displayPixels;
      
        internal bool drawScreen;
        public int Height { get; set; }
        public int Width { get; set; }
        public int Scale { get; set; }


        public Display(int col, int row, int scale)
        {
            Width = col;
            Height = row;
            Scale = scale;
            InitDisplay();
        }

        private void InitDisplay()
        {
            _displayPixels = new byte[Height][];
            for (int i = 0; i < Height; i++)
            {
                _displayPixels[i] = new byte[Width];
            }
        }

        public bool RenderSprite(int x, int y, Sprite sprite)
        {
            bool collision = false;
            var spriteData = sprite.GetSpriteData();
             for (int yIndex = 0; yIndex < spriteData.Length; yIndex++)
            {
                var yCoord = y + yIndex;
                yCoord %= Height;
                byte b = spriteData[yIndex];
                for (int xIndex = 0; xIndex < 8; xIndex++)
                {
                    var xCoord = x + xIndex;
                    xCoord %= Width;

                    var bitInBuff = _displayPixels[yCoord][xCoord];
                    byte bit = (byte)((b >> (7 - xIndex)) & 1);
                    _displayPixels[yCoord][xCoord] ^= bit;
                    var bitInBuffAfter = _displayPixels[yCoord][xCoord];
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
