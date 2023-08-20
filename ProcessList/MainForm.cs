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
using NLog;
using NLog.Fluent;

namespace ProcessList
{
    public partial class MainForm : Form
    {
        private static Logger logger;
        private Process[] localAll;
        private string labelCountProcText;
        private string labelCommonMemoryText;
        private double coefMb;
        public MainForm()
        {
            logger = LogManager.GetCurrentClassLogger();
            InitializeComponent();
            setup();
            UpdateProcess();
        }

        public void setup()
        {
            logger.Info("Инициализация системы");
            coefMb = 1048576;
            gridProcessList.AutoResizeRows();
            gridProcessList.AllowUserToAddRows = false;
            labelCountProcText = "Всего процессов: ";
            labelCommonMemoryText = "Общая память: ";
        }
        public void UpdateProcess()
        {
            localAll = Process.GetProcesses();
            int time;
            string timeString;
            double memory;
            double totalMemory = 0;
            foreach (Process process in localAll)
            {
                memory = Math.Round(process.WorkingSet64 / coefMb, 2);
                try
                {
                    time = process.TotalProcessorTime.Seconds + process.UserProcessorTime.Seconds;
                    timeString = (time / 60).ToString() + ":" + (time % 60).ToString();
                } catch {
                    timeString = "Отказано в доступе";
                }
                gridProcessList.Rows.Add(process.Id.ToString(), process.ProcessName, memory.ToString(), timeString, process.Threads.Count.ToString());
                totalMemory += memory;
            }
            labelCountProc.Text = labelCountProcText + localAll.Length.ToString();
            labelCommonMemory.Text = labelCommonMemoryText + Math.Round(totalMemory, 2).ToString() + " Мб";
        }

        private void gridProcess_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            logger.Info("Выделение строки пользователем");
            gridProcessList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void gridProcessList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            logger.Info("Открытие формы ProcessInfoForm по двойному клику");
            try
            {
                ProccessInfoForm processInfoForm = new ProccessInfoForm(localAll[e.RowIndex]);
                processInfoForm.ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка открытия формы ProcessInfoForm");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridProcessList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 && gridProcessList.CurrentRow!=null)
            {
                logger.Info("Открытие формы ProcessInfoForm по нажатию Enter при выделенной строке");
                try
                {
                    ProccessInfoForm processInfoForm = new ProccessInfoForm(localAll[gridProcessList.CurrentRow.Index]);
                    processInfoForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    logger.Error("Ошибка открытия формы ProcessInfoForm");
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
