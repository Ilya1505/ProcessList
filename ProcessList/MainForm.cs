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
using System.Collections.Concurrent;
using NLog;
using NLog.Fluent;
using NLog.LayoutRenderers;

namespace ProcessList
{
    public partial class MainForm : Form// класс главной формы приложения
    {
        private static Logger logger;// для вывода логов
        private Process[] newProcesses;// список текущих процессов в системе
        // словарь процессов с предыдущей итерации: int - ID процесса, Process - сам процесс
        private ConcurrentDictionary<int, Process> prevProcesses;
        // словарь активных процессов с предыдущей итерации: int - ID процесса, bool - флаг, если true - процесс до сих пор активен
        private ConcurrentDictionary<int, bool> isActive;
        private string labelCountProcText;// изменяемый текст на форме
        private string labelCommonMemoryText;
        private const double coefMb = 1048576;// коэффициент для перевода байт в МБайт
        private Thread updateThread;// вторичный поток обновления списка процессов
        private static ManualResetEvent mre;//
        public MainForm()
        {
            InitializeComponent();
            setup();
        }
        ~MainForm()
        {
            updateThread.Abort();// остановка потока обновления процессов
        }
        // метод инициализации системы
        public void setup()
        {
            logger = LogManager.GetCurrentClassLogger();
            logger.Info("Инициализация системы");
            gridProcessList.AutoResizeRows();
            gridProcessList.AllowUserToAddRows = false;
            labelCountProcText = "Всего процессов: ";
            labelCommonMemoryText = "Общая память: ";
            gridProcessList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // получение списка текущих процессов в системе и заполнение словарей
            // для дальнейшей проверки на появление нового процесса или исчезновение процесса из списка
            newProcesses = Process.GetProcesses();
            prevProcesses = new ConcurrentDictionary<int, Process>();
            isActive = new ConcurrentDictionary<int, bool>();
            foreach (Process process in newProcesses)
            {
                prevProcesses.TryAdd(process.Id, process);
                isActive.TryAdd(process.Id, false);
            }
            work();// вывод на форму текущих процессов
            mre = new ManualResetEvent(true);
            // запуск потока на обновление процессов
            updateThread = new Thread(UpdateProcess);
            updateThread.Start();
            mre.Reset();
        }
        // метод обновление процессов в новом потоке
        public void UpdateProcess()
        {
            //цикл пока новый поток активен
            while (updateThread.IsAlive)
            {
                work();
            }
        }
        //обновление списка процессов
        private void work()
        {
            newProcesses = Process.GetProcesses();
            // словари для временного хранения обновленного списка текущих процессов
            ConcurrentDictionary<int, Process> currentProcess = new ConcurrentDictionary<int, Process>();
            ConcurrentDictionary<int, bool> currentActive = new ConcurrentDictionary<int, bool>();
            DataGridViewRow[] rows = new DataGridViewRow[newProcesses.Length];// новые строки таблицы
            int time;
            string timeString;
            double memory;
            double totalMemory = 0;
            int i = 0;// индекс для итерации по строкам
            foreach (Process process in newProcesses)
            {
                //вычисление занятой процессом памяти с переводом в MB и округлением до двух знаков
                memory = Math.Round(process.WorkingSet64 / coefMb, 2);
                try
                {
                    // вычисление общего времени работы процесса
                    time = process.TotalProcessorTime.Seconds + process.UserProcessorTime.Seconds;
                    // перевод секунд в мин:сек
                    timeString = (time / 60).ToString() + ":" + (time % 60).ToString();
                }
                catch
                {
                    timeString = "Отказано в доступе";
                }
                // создание новой строки с информацией о процессе
                rows[i] = new DataGridViewRow();
                rows[i].CreateCells(gridProcessList, process.Id.ToString(), process.ProcessName, memory.ToString(), timeString, process.Threads.Count.ToString());
                totalMemory += memory;
                i++;
                // проверка, что процесс из нового списка существует в предыдущем списке
                if (isActive.ContainsKey(process.Id)) isActive[process.Id] = true;
                // если нет, вывод сообщения в лог, что это новый процесс
                else logger.Info($"Появился новый процесс, ID: {process.Id}, Name: {process.ProcessName}");
                // добавление процесса в список временного хранения
                currentProcess.TryAdd(process.Id, process);
                currentActive.TryAdd(process.Id, false);
            }
            // если код выполняется во вторичном потоке
            if (InvokeRequired)
            {
                // ассинхранный вывод информации на контролы формы
                Action actionClear = () => gridProcessList.Rows.Clear();
                Action actionAdd = () => gridProcessList.Rows.AddRange(rows);
                Action actionUpdateLabelCount = () => labelCountProc.Text = labelCountProcText + newProcesses.Length.ToString();
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
                // если это главный поток, обычный вывод на форму
                gridProcessList.Rows.Clear();
                gridProcessList.Rows.AddRange(rows);
                labelCountProc.Text = labelCountProcText + newProcesses.Length.ToString();
                labelCommonMemory.Text = labelCommonMemoryText + Math.Round(totalMemory, 2).ToString() + " Мб";
            }
            // поиск процесса в словаре с флагом false - значит процесс завершил работу
            foreach(var prevProcess in prevProcesses)
            {
                if (!isActive[prevProcess.Key])
                {
                    logger.Info($"Процесс ID: {prevProcess.Key} Name: {prevProcess.Value.ProcessName}, завершил работу");
                }
            }
            prevProcesses.Clear();
            isActive.Clear();
            // сохранение словарей с новыми процессами, как уже предыдущих
            prevProcesses = currentProcess;
            isActive = currentActive;
        }
        // обработка события выделения строки таблицы пользователем
        private void gridProcess_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            logger.Info("Выделение строки пользователем");
            gridProcessList.Rows[e.RowIndex].Selected = true;
        }

        // открытие формы подбробных сведений о процессе по двойному клику мыши по строке таблицы
        private void gridProcessList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            logger.Info("Открытие формы ProcessInfoForm по двойному клику");
            try
            {
                // передача в новую форму выбранного процесса из списка
                ProccessInfoForm processInfoForm = new ProccessInfoForm(newProcesses[e.RowIndex]);
                processInfoForm.ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Error("Ошибка открытия формы ProcessInfoForm");
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // открытие формы подбробных сведений о процессе по нажатию enter на выделенной строке
        private void gridProcessList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 && gridProcessList.CurrentRow != null)
            {
                logger.Info("Открытие формы ProcessInfoForm по нажатию Enter");
                try
                {
                    ProccessInfoForm processInfoForm = new ProccessInfoForm(newProcesses[gridProcessList.CurrentRow.Index]);
                    processInfoForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    logger.Error("Ошибка открытия формы ProcessInfoForm");
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // метод приостановки и продолжения вторичного потока обновления процессов
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

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
