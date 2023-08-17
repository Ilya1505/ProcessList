using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace ProcessList
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //gridProcess.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gridProcess.AutoResizeRows();
            PrintProcess();
        }

        public void PrintProcess()
        {
            Process[] localAll = Process.GetProcesses();
            foreach (Process p in localAll)
            {
                string s;
                try { s = p.StartTime.ToString(); } catch { s = "Отказано в доступе"; }
                gridProcess.Rows.Add(p.Id.ToString(), p.ProcessName, p.PeakWorkingSet64.ToString(), s, p.Threads.Count.ToString());
            }
        }
    }
}
