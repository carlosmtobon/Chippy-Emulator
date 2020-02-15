using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChipCore
{
    class CPU
    {
        byte[] _byteBuff = new byte[2];
        ushort _opcode = 0x0;
        ushort _programCounter = 0x200;
        int _stackPointer = 0;
        ushort[] _stack = new ushort[16];

        byte v0Index = 0x0;
        byte vfIndex = 0xF;

        RAM _ram;
        Display _display;
        KeyPad _keypad;

        // Registers
        byte[] _vRegisters = new byte[16]; 

        // Register to store mem address (12 right most bits)
        ushort _iRegister;

        // sound timer register
        byte _soundTimerRegister;

        // delay timer register
        byte _delayTimerRegister;

        public CPU(RAM ram, Display display, KeyPad keypad)
        {
            _ram = ram;
            _display = display;
            _keypad = keypad;
        }

        public void ExecuteCycle()
        {
            //_opcode = (ushort)(_ram.LoadFromMemory(_programCounter++) << 8 | _ram.LoadFromMemory(_programCounter++));
            _byteBuff[1] = _ram.LoadFromMemory(_programCounter++);
            _byteBuff[0] = _ram.LoadFromMemory((ushort)(_programCounter++));

            _opcode = (BitConverter.ToUInt16(_byteBuff, 0));
            ProcessOpcode(); 
        }

        internal ushort GetProgramCounter()
        {
            return _programCounter;
        }

        public ushort GetOpcode()
        {
            return _opcode;
        }

        internal byte[] GetRegisters()
        {
            return _vRegisters;
        }

        internal object GetStackPointer()
        {
            return _stackPointer;
        }

        internal void SetOpcode(ushort v)
        {
            _opcode = v;
        }

        internal void IncrementPC()
        {
            _programCounter++;
        }

        internal void UpdateTimers()
        {
            if (_soundTimerRegister > 0)
                _soundTimerRegister--;

            if (_delayTimerRegister > 0)
                _delayTimerRegister--;
        }

        public void ProcessOpcode()
        {
            byte mostSigNib = (byte)((_opcode & 0xF000) >> 12);
            byte leastSigNib = (byte)(_opcode & 0x000F);
            byte leastSigByte = (byte)(_opcode & 0x00FF);

            ushort addr = (ushort)(_opcode & 0x0FFF); 
            byte kk = (byte)(_opcode & 0x00FF);
            byte x = (byte)((_opcode & 0x0F00) >> 8);
            byte y = (byte)((_opcode & 0x00F0) >> 4);

            switch (mostSigNib)
            {
                case 0:
                    if (_opcode == 0x00E0)
                        CLS();
                    else if (_opcode == 0x00EE)
                        RET();
                    else
                        SYS(addr);
                    break;
                case 1:
                    JP_addr(addr);
                    break;
                case 2:
                    CALL_addr(addr);
                    break;
                case 3:
                    SE_Vx_byte(x, kk);
                    break;
                case 4:
                    SNE_Vx_byte(x, kk);
                    break;
                case 5:
                    if (leastSigNib == 5)
                        SE_Vx_Vy(x, y);
                    break;
                case 6:
                    LD_Vx_byte(x, kk);
                    break;
                case 7:
                    ADD_Vx_byte(x, kk);
                    break;
                case 8:
                    if (leastSigNib == 0)
                        LD_Vx_Vy(x, y);
                    else if (leastSigNib == 1)
                        OR_Vx_Vy(x, y);
                    else if (leastSigNib == 2)
                        AND_Vx_Vy(x, y);
                    else if (leastSigNib == 3)
                        XOR_Vx_Vy(x, y);
                    else if (leastSigNib == 4)
                        ADD_Vx_Vy(x, y);
                    else if (leastSigNib == 5)
                        SUB_Vx_Vy(x, y);
                    else if (leastSigNib == 6)
                        SHR_Vx_Vy(x, y);
                    else if (leastSigNib == 7)
                        SUBN_Vx_Vy(x, y);
                    else if (leastSigNib == 0xE)
                        SHL_Vx(x, y);
                    break;
                case 9:
                    if (leastSigNib == 0)
                        SNE_Vx_Vy(x, y);
                    break;
                case 0xA:
                    LD_I_addr(addr);
                    break;
                case 0xB:
                    JP_V0_addr(addr);
                    break;
                case 0xC:
                    RND_Vx_byte(x, kk);
                    break;
                case 0xD:
                    DRW_Vx_Vy_nibble(x , y, leastSigNib);
                    break;
                case 0xE:
                    if (leastSigByte == 0x009E)
                        SKP_Vx(x);
                    else if (leastSigByte == 0x00A1)
                        SKNP_Vx(x);
                    break;
                case 0xF:
                    if (leastSigByte == 0x0007)
                        LD_Vx_DT(x);
                    else if (leastSigByte == 0x000A)
                        LD_Vx_K(x);
                    else if (leastSigByte == 0x0015)
                        LD_DT_Vx(x);
                    else if (leastSigByte == 0x0018)
                        LD_ST_Vx(x);
                    else if (leastSigByte == 0x001E)
                        ADD_I_Vx(x);
                    else if (leastSigByte == 0x0029)
                        LD_F_Vx(x);
                    else if (leastSigByte == 0x0033)
                        LD_B_Vx(x);
                    else if (leastSigByte == 0x0055)
                        LD_I_Vx(x);
                    else if (leastSigByte == 0x0065)
                        LD_Vx_I(x);
                    break;
                default:
                    Console.WriteLine("Unknown Opcode");
                    break;
            }
        }

        private void SYS(ushort addr)
        {
            Debug.WriteLine("SYS()");
        }

        private void CLS()
        {
            _display.ClearScreen();
        }

        private void RET()
        {
            _programCounter = _stack[--_stackPointer];
        }

        private void JP_addr(ushort addr)
        {
            _programCounter = addr;
        }

        private void CALL_addr(ushort addr)
        {
            _stack[_stackPointer++] = _programCounter;
            _programCounter = addr;
        }

        private void SE_Vx_byte(byte x, byte kk)
        {
            if (_vRegisters[x] == kk)
                _programCounter += 2;
        }

        private void SNE_Vx_byte(byte x, byte kk)
        {
            if (_vRegisters[x] != kk)
                _programCounter += 2;
        }

        private void SE_Vx_Vy(byte x, ushort y)
        {
            if (_vRegisters[x] == _vRegisters[y])
                _programCounter += 2;
        }

        private void LD_Vx_byte(byte x, byte kk)
        {
            _vRegisters[x] = kk;
        }

        private void ADD_Vx_byte(byte x, byte kk)
        {
            _vRegisters[x] += kk;
        }

        private void LD_Vx_Vy(byte x, byte y)
        {
            _vRegisters[x] = _vRegisters[y];
        }

        private void OR_Vx_Vy(byte x, byte y)
        {
            _vRegisters[x] = (byte)(_vRegisters[x] | _vRegisters[y]);
        }

        private void AND_Vx_Vy(byte x, byte y)
        {
            _vRegisters[x] = (byte)(_vRegisters[x] & _vRegisters[y]);
        }

        private void XOR_Vx_Vy(byte x, byte y)
        {
            _vRegisters[x] = (byte)(_vRegisters[x] ^ _vRegisters[y]);
        }

        private void ADD_Vx_Vy(byte x, byte y)
        {
            if ((_vRegisters[x] + _vRegisters[y]) > 255)
                _vRegisters[vfIndex] = 1;
            else
                _vRegisters[vfIndex] = 0;

            _vRegisters[x] += _vRegisters[y];
        }

        private void SUB_Vx_Vy(byte x, byte y)
        {
            if (_vRegisters[x] > _vRegisters[y])
                _vRegisters[vfIndex] = 1;
            else
                _vRegisters[vfIndex] = 0;
            _vRegisters[x] -= _vRegisters[y];
        }

        // If the least-significant bit of Vx is 1, then VF is set to 1, otherwise 0. Then Vx is divided by 2.
        private void SHR_Vx_Vy(byte x, byte y)
        {
            _vRegisters[vfIndex] = (byte)(_vRegisters[x] & 1);
            _vRegisters[x] = (byte)(_vRegisters[y] >> 1);
        }

        //Set Vx = Vy - Vx, set VF = NOT borrow.
        //If Vy > Vx, then VF is set to 1, otherwise 0. Then Vx is subtracted from Vy, and the results stored in Vx.
        private void SUBN_Vx_Vy(byte x, byte y)
        {
            if (_vRegisters[y] > _vRegisters[x])
                _vRegisters[vfIndex] = 1;
            else
                _vRegisters[vfIndex] = 0;
            _vRegisters[x] = (byte)(_vRegisters[y] - _vRegisters[x]);
        }

        //Set Vx = Vx SHL 1.

        //If the most-significant bit of Vx is 1, then VF is set to 1, otherwise to 0. Then Vx is multiplied by 2.
        private void  SHL_Vx(byte x, byte y)
        {
            _vRegisters[vfIndex] = (byte)((_vRegisters[x] & 0x8) >> 3);
            _vRegisters[x] =  (byte)(_vRegisters[y] << 1);
        }

        //Skip next instruction if Vx != Vy.

        //The values of Vx and Vy are compared, and if they are not equal, the program counter is increased by 2.
        private void SNE_Vx_Vy(byte x, byte y)
        {
            if (_vRegisters[x] != _vRegisters[y])
                _programCounter += 2;
        }

        /*Annn - LD I, addr
        Set I = nnn.

        The value of register I is set to nnn.
        */
        private void LD_I_addr(ushort addr)
        {
            _iRegister = addr;
        }

        /*
        Bnnn - JP V0, addr
        Jump to location nnn + V0.

        The program counter is set to nnn plus the value of V0.
        */
        private void JP_V0_addr(ushort addr)
        {
            _programCounter = (ushort)(addr + _vRegisters[v0Index]);
        }

        /*
        Cxkk - RND Vx, byte
        Set Vx = random byte AND kk.

        The interpreter generates a random number from 0 to 255, which is then ANDed with the value kk. 
        The results are stored in Vx. See instruction 8xy2 for more information on AND.
         */
        private void RND_Vx_byte(byte x, byte kk)
        {
            Random random = new Random();
            _vRegisters[x] = (byte)(random.Next(0, 255) & kk);
        }

        /*
        Dxyn - DRW Vx, Vy, nibble
        Display n-byte sprite starting at memory location I at (Vx, Vy), set VF = collision.

        The interpreter reads n bytes from memory, starting at the address stored in I. 
        These bytes are then displayed as sprites on screen at coordinates (Vx, Vy).
        Sprites are XORed onto the existing screen. 
        If this causes any pixels to be erased, VF is set to 1, otherwise it is set to 0. 
        If the sprite is positioned so part of it is outside the coordinates of the display, it wraps around to the opposite side of the screen.
        See instruction 8xy3 for more information on XOR, and section 2.4, Display, for more information on the Chip-8 screen and sprites.
         */
        private void DRW_Vx_Vy_nibble(byte x, byte y, byte n)
        {
            byte[] spriteData = _ram.LoadMemBlock(_iRegister, n);
            Sprite sprite = new Sprite(spriteData);
            var xStart = _vRegisters[x];
            var yStart = _vRegisters[y];
            bool collision =_display.RenderSprite(xStart, yStart, sprite);
            if (collision)
                _vRegisters[vfIndex] = 1;
            else
                _vRegisters[vfIndex] = 0;
            _display.drawScreen = true;
        }

        private void SKP_Vx(byte x)
        {
            bool keyPressed = _keypad.GetKeyBoardBuffer(_vRegisters[x]);
            if (keyPressed)
                _programCounter += 2;
        }

        private void SKNP_Vx(byte x)
        {
           
            bool keyPressed = _keypad.GetKeyBoardBuffer(_vRegisters[x]);
            if (!keyPressed)
                _programCounter += 2;
        }

        private void LD_Vx_DT(byte x)
        {
            _vRegisters[x] = _delayTimerRegister;
        }

        /*
         Wait for a key press, store the value of the key in Vx.

         All execution stops until a key is pressed, then the value of that key is stored in Vx.
        */
        private void LD_Vx_K(byte x)
        {
            _vRegisters[x] = _keypad.WaitForKey();
        }

        private void LD_DT_Vx(byte x)
        {
            _delayTimerRegister = _vRegisters[x];
        }

        private void LD_ST_Vx(byte x)
        {
            _soundTimerRegister = _vRegisters[x];
        }

        private void ADD_I_Vx(byte x)
        {
            _iRegister += x;
        }

        private void LD_F_Vx(byte x)
        {
           
            _iRegister = (ushort)(_vRegisters[x] * 5);
        }

        private void LD_B_Vx(byte x)
        {
            byte val = _vRegisters[x];
            byte hundred = (byte)((val / 100) % 10);
            byte ten = (byte)((val / 10) % 10);
            byte one = (byte)(val  % 10);

            _ram.StoreInMemory(_iRegister, hundred);
            _ram.StoreInMemory((ushort)(_iRegister+1), ten);
            _ram.StoreInMemory((ushort)(_iRegister+2), one);
        }

        private void LD_I_Vx(byte x)
        {
            ushort currentAddr = _iRegister;
            for (int i = 0; i <= x; i++)
                _ram.StoreInMemory(currentAddr++, _vRegisters[i]);
        }

        private void LD_Vx_I(byte x)
        {
            ushort currentAddr = _iRegister;
            for (int i = 0; i <= x; i++)
                _vRegisters[i] = _ram.LoadFromMemory(currentAddr++);
        }
    }
}
