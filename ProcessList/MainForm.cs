using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using NLog;
using NLog.Fluent;
using NLog.LayoutRenderers;

namespace ProcessList
{
    public partial class MainForm : Form
    {
        private static Logger logger;
        private Process[] localAll;
        private string labelCountProcText;
        private string labelCommonMemoryText;
        private double coefMb;
        private Thread updateThread;
        private static ManualResetEvent mre;
        public MainForm()
        {
            InitializeComponent();
            setup();
        }
        ~MainForm()
        {
            updateThread.Abort();
        }
        public void setup()
        {
            logger = LogManager.GetCurrentClassLogger();
            logger.Info("Инициализация системы");
            coefMb = 1048576;
            gridProcessList.AutoResizeRows();
            gridProcessList.AllowUserToAddRows = false;
            labelCountProcText = "Всего процессов: ";
            labelCommonMemoryText = "Общая память: ";
            gridProcessList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            work();
            mre = new ManualResetEvent(true);
            updateThread = new Thread(UpdateProcess);
            updateThread.Start();
            mre.Reset();
        }
        public void UpdateProcess()
        {
            while (updateThread.IsAlive)
            {
                work();
            }
        }
        private void work()
        {
            localAll = Process.GetProcesses();
            int time;
            string timeString;
            double memory;
            double totalMemory = 0;
            DataGridViewRow[] rows = new DataGridViewRow[localAll.Length];
            int i = 0;
            foreach (Process process in localAll)
            {
                memory = Math.Round(process.WorkingSet64 / coefMb, 2);
                try
                {
                    time = process.TotalProcessorTime.Seconds + process.UserProcessorTime.Seconds;
                    timeString = (time / 60).ToString() + ":" + (time % 60).ToString();
                }
                catch
                {
                    timeString = "Отказано в доступе";
                }
                rows[i] = new DataGridViewRow();
                rows[i].CreateCells(gridProcessList, process.Id.ToString(), process.ProcessName, memory.ToString(), timeString, process.Threads.Count.ToString());
                totalMemory += memory;
                i++;
            }
            if (InvokeRequired)
            {
                Action actionClear = () => gridProcessList.Rows.Clear();
                Action actionAdd = () => gridProcessList.Rows.AddRange(rows);
                Action actionUpdateLabelCount = () => labelCountProc.Text = labelCountProcText + localAll.Length.ToString();
                Action actionUpdateLabelMemory = () => labelCommonMemory.Text = labelCommonMemoryText + Math.Round(totalMemory, 2).ToString() + " Мб";
                BeginInvoke(actionClear);
                BeginInvoke(actionAdd);
                BeginInvoke(actionUpdateLabelCount);
                BeginInvoke(actionUpdateLabelMemory);
                Thread.Sleep(1000);
                mre.WaitOne();
            }
            else
            {
                gridProcessList.Rows.Clear();
                gridProcessList.Rows.AddRange(rows);
                labelCountProc.Text = labelCountProcText + localAll.Length.ToString();
                labelCommonMemory.Text = labelCommonMemoryText + Math.Round(totalMemory, 2).ToString() + " Мб";
            }
        }
        private void gridProcess_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            logger.Info("Выделение строки пользователем");
            gridProcessList.Rows[e.RowIndex].Selected = true;
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

        private void startButton_Click(object sender, EventArgs e)
        {
            if (toolStrip.Items[0].Text == "Запуск")
            {
                logger.Info("Запуск потока на обновление процессов");
                toolStrip.Items[0].Text = "Стоп";
                mre.Set();
            }
            else
            {
                logger.Info("Остановка потока обновления процессов");
                toolStrip.Items[0].Text = "Запуск";
                mre.Reset();
            }
        }
    }
}
