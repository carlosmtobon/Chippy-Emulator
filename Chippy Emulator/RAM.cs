using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Chippy_Emulator
{
    class RAM
    {
        const int _memSize = 4096;
        private byte[] _memory = new byte[_memSize];
        const int _startAddr = 0x200;

        public byte[] sprite0 = new byte[] { 0xF0, 0x90, 0x90, 0x90, 0xF0 };
        public byte[] sprite1 = new byte[] { 0x20, 0x60, 0x20, 0x20, 0x70 };
        public byte[] sprite2 = new byte[] { 0xF0, 0x10, 0xF0, 0x80, 0xF0 };
        public byte[] sprite3 = new byte[] { 0xF0, 0x10, 0xF0, 0x10, 0xF0 };
        public byte[] sprite4 = new byte[] { 0x90, 0x90, 0xF0, 0x10, 0x10 };
        public byte[] sprite5 = new byte[] { 0xF0, 0x80, 0xF0, 0x10, 0xF0 };
        public byte[] sprite6 = new byte[] { 0xF0, 0x80, 0xF0, 0x90, 0xF0 };
        public byte[] sprite7 = new byte[] { 0xF0, 0x10, 0x20, 0x40, 0x40 };
        public byte[] sprite8 = new byte[] { 0xF0, 0x90, 0xF0, 0x90, 0xF0 };
        public byte[] sprite9 = new byte[] { 0xF0, 0x90, 0xF0, 0x10, 0xF0 };
        public byte[] spriteA = new byte[] { 0xF0, 0x90, 0xF0, 0x90, 0x90 };
        public byte[] spriteB = new byte[] { 0xE0, 0x90, 0xE0, 0x90, 0xE0 };
        public byte[] spriteC = new byte[] { 0xF0, 0x80, 0x80, 0x80, 0xF0 };
        public byte[] spriteD = new byte[] { 0xE0, 0x90, 0x90, 0x90, 0xE0 };
        public byte[] spriteE = new byte[] { 0xF0, 0x80, 0xF0, 0x80, 0xF0 };
        public byte[] spriteF = new byte[] { 0xF0, 0x80, 0xF0, 0x80, 0x80 };

        public RAM(string fileName)
        {
            LoadProgram(fileName);
        }

        public void LoadProgram(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            fs.Read(_memory, _startAddr, _memory.Length - _startAddr);
            Console.WriteLine("Program Loaded");
        }

        public void StoreInMemory(ushort addr, byte val)
        {
            _memory[addr] = val;
        }

        public byte LoadFromMemory(ushort addr)
        {
           return _memory[addr];
        }

        public int GetMemSize()
        {
            return _memSize;
        }

        public byte[] LoadMemBlock(ushort addr, byte num)
        {
            byte[] memBlock = new byte[num];
            for (int i = 0; i < num; i++)
            {
                memBlock[i] = LoadFromMemory((ushort)(addr + i));
            }
            return memBlock;
        }

        private void StoreFontsInMemory()
        {

        }
    }
}
