using System;
using System.Collections.Generic;
using System.Text;

namespace Chippy_Emulator
{
    class KeyBoard
    {
        byte _kbBuffer;

        public const int key0 = 0x0;
        public const int key1 = 0x1;
        public const int key2 = 0x2;
        public const int key3 = 0x3;
        public const int key4 = 0x4;
        public const int key5 = 0x5;
        public const int key6 = 0x6;
        public const int key7 = 0x7;
        public const int key8 = 0x8;
        public const int key9 = 0x9;
        public const int keyA = 0xA;
        public const int keyB = 0xB;
        public const int keyC = 0xC;
        public const int keyD = 0xD;
        public const int keyE = 0xE;
        public const int keyF = 0xF;

        public void SetKeyBuffer(byte kbBuffer)
        {
            _kbBuffer = kbBuffer;
        }

        public byte GetKeyBoardBuffer()
        {
            return _kbBuffer;
        }
    }
}
