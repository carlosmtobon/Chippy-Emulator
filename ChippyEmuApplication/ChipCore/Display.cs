using System;

namespace ChipCore
{
    class Display
    {
        public byte[][] _displayPixels;

        internal bool DrawScreen { get; set; }
        internal bool ExtendedDisplay { get; set; }
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
               
                if (ExtendedDisplay)
                {
                    ushort b = (ushort)(spriteData[yIndex] << 8 | spriteData[yIndex + 1]);
                    for (int xIndex = 0; xIndex < 16; xIndex++)
                    {
                        var xCoord = x + xIndex;
                        xCoord %= Width;

                        var bitInBuff = _displayPixels[yCoord][xCoord];
                        byte bit = (byte)((b >> (15 - xIndex)) & 1);
                        _displayPixels[yCoord][xCoord] ^= bit;
                        var bitInBuffAfter = _displayPixels[yCoord][xCoord];
                        if ((bitInBuff == 1) && bitInBuff != bitInBuffAfter)
                            collision = true;
                    }
                }
                else
                {
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

        public void ScrollLeft()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x + 4 < Width; x++)
                {
                    _displayPixels[y][x] = _displayPixels[y][x + 4];
                }
            }
        }

        public void ScrollRight()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = Width - 1; x - 4 >= 0; x--)
                {
                    _displayPixels[y][x] = _displayPixels[y][x - 4];
                }
            }
        }

        public void ScrollDown(int num)
        {
            for (int y = Height - 1; y - num >= 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    _displayPixels[y][x] = _displayPixels[y - num][x];
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
