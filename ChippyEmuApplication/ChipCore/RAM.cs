using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ChipCore
{
    class RAM
    {
        const int _memSize = 4096;
        private byte[] _memory = new byte[_memSize];
        const int _startAddr = 0x200;

        public byte[,] fonts = new byte[16,5] 
        {
            { 0xF0, 0x90, 0x90, 0x90, 0xF0 },
            { 0x20, 0x60, 0x20, 0x20, 0x70 },
            { 0xF0, 0x10, 0xF0, 0x80, 0xF0 },
            { 0xF0, 0x10, 0xF0, 0x10, 0xF0 },
            { 0x90, 0x90, 0xF0, 0x10, 0x10 },
            { 0xF0, 0x80, 0xF0, 0x10, 0xF0 },
            { 0xF0, 0x80, 0xF0, 0x90, 0xF0 },
            { 0xF0, 0x10, 0x20, 0x40, 0x40 },
            { 0xF0, 0x90, 0xF0, 0x90, 0xF0 },
            { 0xF0, 0x90, 0xF0, 0x10, 0xF0 },
            { 0xF0, 0x90, 0xF0, 0x90, 0x90 },
            { 0xE0, 0x90, 0xE0, 0x90, 0xE0 },
            { 0xF0, 0x80, 0x80, 0x80, 0xF0 },
            { 0xE0, 0x90, 0x90, 0x90, 0xE0 },
            { 0xF0, 0x80, 0xF0, 0x80, 0xF0 },
            { 0xF0, 0x80, 0xF0, 0x80, 0x80 }
        };

        public RAM(string fileName)
        {
            StoreFontsInMemory();
            LoadProgram(fileName);
        }

        public void LoadProgram(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                fs.Read(_memory, _startAddr, _memory.Length - _startAddr);
                fs.Close();
                Console.WriteLine("Program Loaded");
            } 
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
                memBlock[i] = LoadFromMemory((ushort)(addr +  i));
            }
            return memBlock;
        }

        private void StoreFontsInMemory()
        {
            ushort addr = 0;
           for (int i = 0; i < fonts.GetLength(0); i++)
            {
                for (int j = 0; j < fonts.GetLength(1); j++)
                {
                    StoreInMemory(addr++, fonts[i, j]);
                }
            }
        }
    }
}

