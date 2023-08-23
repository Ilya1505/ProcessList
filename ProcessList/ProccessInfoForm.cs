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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using NLog;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProcessList
{
    public partial class ProccessInfoForm : Form
    {
        private static Logger logger;
        private Process process;
        private double coefMb;
        public ProccessInfoForm(Process process)
        {
            logger = LogManager.GetCurrentClassLogger();
            InitializeComponent();
            Setup(process);

        }
        public void Setup(Process process)
        {
            logger.Info("Инициализация формы ProcessInfoForm");
            this.process = process;
            coefMb = 1048576;
            gridProcess.AutoResizeRows();
            gridProcess.AllowUserToAddRows = false;
            int time;
            gridProcess.Rows.Clear();
            gridProcess.Rows.Add("ID", process.Id.ToString());
            gridProcess.Rows.Add("Имя", process.ProcessName);
            gridProcess.Rows.Add("Физическая память", Math.Round(process.WorkingSet64 / coefMb, 2).ToString() + "Мб");
            gridProcess.Rows.Add("Виртуальная память", Math.Round(process.VirtualMemorySize64 / coefMb, 2).ToString() + "Мб");
            gridProcess.Rows.Add("Максимальная память", Math.Round(process.PeakWorkingSet64 / coefMb, 2).ToString() + "Мб");
            try { gridProcess.Rows.Add("Время старта", process.StartTime.ToString()); }
            catch { gridProcess.Rows.Add("Время старта", "Отказано в доступе"); }
            try
            {
                time = process.TotalProcessorTime.Seconds + process.UserProcessorTime.Seconds;
                gridProcess.Rows.Add("Время работы", (time / 60).ToString() + "мин " + (time % 60).ToString() + "сек");
            }
            catch { gridProcess.Rows.Add("Время работы", "Отказано в доступе"); }
            gridProcess.Rows.Add("Число потоков", process.Threads.Count.ToString());
        }
        private void killButton_Click(object sender, EventArgs e)
        {
            try
            {
                process.Kill();
                MessageBox.Show("Процесс успешно завершен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Info($"Процесс {process.Id}: {process.ProcessName} успешно завершен");
                this.Close();
            }
            catch 
            {
                logger.Error($"Не удалось завершить процесс {process.Id}: {process.ProcessName}, отказано в доступе ");
                MessageBox.Show("Не удалось выполнить операцию", "Отказано в доступе", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            logger.Info("Закрытие формы ProcessInfoForm");
            this.Close();
        }
    }
}
