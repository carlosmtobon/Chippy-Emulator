using ChipCore;
using System;
using System.Collections.Generic;

namespace ChippyEmuApplication
{
    internal class Disassembler
    {
        private RAM _ram;
        private List<String> _asm;
        public Disassembler(RAM ram)
        {
            _ram = ram;
            _asm = new List<string>();
        }

        internal List<string> GetAsm()
        {
            var memory = _ram.GetMemory();
            for (int i = 0x0200; i < memory.Length; i+=2)
            {
                var opcode = memory[i] << 8 | memory[i + 1];
                byte mostSigNib = (byte)((opcode & 0xF000) >> 12);
                byte leastSigNib = (byte)(opcode & 0x000F);
                byte leastSigByte = (byte)(opcode & 0x00FF);

                ushort addr = (ushort)(opcode & 0x0FFF);
                byte kk = (byte)(opcode & 0x00FF);
                byte x = (byte)((opcode & 0x0F00) >> 8);
                byte y = (byte)((opcode & 0x00F0) >> 4);

                String line = "";
                switch (mostSigNib)
                {
                    case 0:
                        if (opcode == 0x00E0)
                            line = "CLS";
                        else if (opcode == 0x00EE)
                            line = "RET";
                        //else if (opcode == 0x00FB)
                        //    //ScrollRight();
                        //else if (opcode == 0x00FC)
                        //    //ScrollLeft();
                        //else if (opcode == 0x00FD)
                        //    //Quit();
                        //else if (opcode == 0x00FE)
                        //    //SetChip8Graphics();
                        //else if (opcode == 0x00FF)
                        //    //SetSchipGraphics();
                        //else if (y == 0x000C)
                        //   // ScrollDown(leastSigNib);
                        //else
                            //SYS(addr);
                        break;
                    case 1:
                        line = String.Format("JP {0:X}", addr);
                        break;
                    case 2:
                        line = String.Format("JSR {0:X}", addr);
                        break;
                    case 3:
                        line = String.Format("SE V{0:X}, {1:X}", x,kk);
                        break;
                    case 4:
                        line = String.Format("SNE V{0:X}, {1:X}", x, kk);
                        break;
                    case 5:
                        if (leastSigNib == 0)
                            line = String.Format("SE V{0:X}, V{1:X}", x, y);
                        break;
                    case 6:
                        line = String.Format("LD V{0:X}, {1:X}", x, kk);
                        break;
                    case 7:
                        line = String.Format("ADD V{0:X}, {1:X}", x, kk);
                        break;
                    case 8:
                        if (leastSigNib == 0)
                            line = String.Format("LD V{0:X}, V{1:x}", x, y);
                        else if (leastSigNib == 1)
                            line = String.Format("OR V{0:X}, V{1:X}", x, y);
                        else if (leastSigNib == 2)
                            line = String.Format("AND V{0:X}, V{1:X}", x, y);
                        else if (leastSigNib == 3)
                            line = String.Format("XOR V{0:X}, V{1:X}", x, y);
                        else if (leastSigNib == 4)
                            line = String.Format("ADD V{0:X}, V{1:X}", x, y);
                        else if (leastSigNib == 5)
                            line = String.Format("SUB V{0:X}, V{1:X}", x, y);
                        else if (leastSigNib == 6)
                            line = String.Format("SHR V{0:X}", x);
                        else if (leastSigNib == 7)
                            line = String.Format("SUBN V{0:X}, V{1:X}", x, y);
                        else if (leastSigNib == 0xE)
                            line = String.Format("SHL V{0:X}", x);
                        break;
                    case 9:
                        if (leastSigNib == 0)
                            line = String.Format("SNE V{0:X}, V{1:X}", x, y);
                        break;
                    case 0xA:
                        line = String.Format("LDI {0:X}",addr);
                        break;
                    case 0xB:
                        line = String.Format("JMI {0:X}", addr);
                        break;
                    case 0xC:
                        line = String.Format("RND V{0:X}, {1:X}", x, kk);
                        break;
                    case 0xD:
                        line = String.Format("DRW V{0:X}, V{1:X} {2:X}", x, y, leastSigNib);
                        break;
                    case 0xE:
                        if (leastSigByte == 0x009E)
                            line = String.Format("SKP V{0:X}", x);
                        else if (leastSigByte == 0x00A1)
                            line = String.Format("SKNP V{0:X}", x);
                        break;
                    case 0xF:
                        if (leastSigByte == 0x0007)
                            line = String.Format("STDT V{0:X}", x);
                        else if (leastSigByte == 0x000A)
                            line = String.Format("LDKY V{0:X}", x);
                        else if (leastSigByte == 0x0015)
                            line = String.Format("LDDT V{0:X}", x);
                        else if (leastSigByte == 0x0018)
                            line = String.Format("LDST V{0:X}", x);
                        else if (leastSigByte == 0x001E)
                            line = String.Format("ADDI V{0:X}", x);
                        else if (leastSigByte == 0x0029)
                            line = String.Format("FONT V{0:X}", x);
                        else if (leastSigByte == 0x0033)
                            line = String.Format("BCD V{0:X}", x);
                        else if (leastSigByte == 0x0055)
                            line = String.Format("STR V0, V{0:X}", x);
                        else if (leastSigByte == 0x0065)
                            line = String.Format("LDR V0, V{0:X}", x);
                        //else if (leastSigByte == 0x0075)
                        //    LD_Vx_I_Schip(x);
                        //else if (leastSigByte == 0x0085)
                        //    LD_I_Vx_Schip(x);
                        break;
                    default:
                        Console.WriteLine("Unknown Opcode");
                        break;
                }
                if (!string.IsNullOrEmpty(line))
                    _asm.Add(line+'\r');
            }
            return _asm;
        }
    }
}