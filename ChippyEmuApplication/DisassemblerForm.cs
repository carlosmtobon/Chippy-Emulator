using ChipCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChippyEmuApplication
{
    public partial class DisassemblerForm : Form
    {
        private Disassembler disassembler;

        public DisassemblerForm(RAM ram)
        {
            InitializeComponent();
            disassembler = new Disassembler(ram);
        }

        private void DisassemblerForm_Load(object sender, EventArgs e)
        {
            var asm = "";
            foreach (string s in disassembler.GetAsm())
                asm += s;
            richTextBox1.Text = asm;
        }
    }
}
