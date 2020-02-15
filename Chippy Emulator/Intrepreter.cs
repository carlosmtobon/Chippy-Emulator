using System;
using System.Collections.Generic;
using System.Text;

namespace Chippy_Emulator
{
    class Intrepreter
    {
        public RAM _ram;
        public Display _display;
        public CPU _cpu;

        public void Init()
        {
             _ram = new RAM(@"C:\Users\Carlos\Desktop\c8games\TETRIS");
             _display = new Display(_ram);
             _cpu = new CPU(_ram, _display, false);
            while (_cpu.GetProgramCounter() < _ram.GetMemSize())
            {
                Execute(_ram, _cpu);

                // UpdateScreen(display);
            }
            Console.Read();



            RenderFonts(_display, _ram);
        }

        private void UpdateScreen(Display display)
        {
            display.DisplayScreen();
        }

        private void Execute(RAM ram, CPU cpu)
        {
            if (cpu._debug)
            {
                DebugInfo(cpu);
            }

            byte firstByte = ram.LoadFromMemory(cpu.GetProgramCounter());
            cpu.IncrementPC();

            byte secondByte = ram.LoadFromMemory(cpu.GetProgramCounter());
            cpu.IncrementPC();

            cpu.SetOpcode(BitConverter.ToUInt16(new byte[2] { secondByte, firstByte }, 0));
            cpu.ProcessOpcode();


        }
        public void DebugInfo(CPU cpu)
        {
            Console.WriteLine($"OPCODE: {cpu.GetOpcode().ToString("X4")} PC: {cpu.GetProgramCounter()} SP: {cpu.GetStackPointer()}");

            byte[] registers = cpu.GetRegisters();
            for (int i = 0; i < registers.Length; i++)
            {
                if (i % 4 == 0)
                {
                    Console.WriteLine();
                }
                Console.Write($" V{i} : {registers[i]} ");
            }
            Console.WriteLine();
        }

        public void RenderFonts(Display display, RAM ram)
        {
            display.RenderSprite(0, 0, new Sprite(ram.sprite0));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.sprite1));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.sprite2));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.sprite3));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.sprite4));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.sprite5));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.sprite6));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.sprite7));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.sprite8));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.sprite9));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.spriteA));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.spriteB));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.spriteC));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.spriteD));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.spriteE));
            display.DisplayScreen();
            display.ClearScreen();
            display.RenderSprite(0, 0, new Sprite(ram.spriteF));
            display.DisplayScreen();
            display.ClearScreen();
        }
    }
}
