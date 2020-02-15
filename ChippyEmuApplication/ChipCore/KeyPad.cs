using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ChipCore
{
    class KeyPad
    {
        bool[] _kbBuffer = new bool[16];

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

        public void SetKeyDown(int keyPos)
        {
            _kbBuffer[keyPos] = true;
        }

        public void SetKeyUp(int keyPos)
        {
            _kbBuffer[keyPos] = false;
        }

        public bool GetKeyBoardBuffer(int keyPos)
        {
            return _kbBuffer[keyPos];
        }

        public byte WaitForKey()
        {
            return 0;
            //Console.Read();
        }
    }
}
