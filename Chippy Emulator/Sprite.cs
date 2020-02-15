using System;
using System.Collections.Generic;
using System.Text;

namespace Chippy_Emulator
{
    class Sprite
    {
        byte[] _spriteData;

        public Sprite(byte[] spriteData)
        {
            _spriteData = spriteData;
        }

        public byte[] GetSpriteData()
        {
            return _spriteData;
        }
    }
}
