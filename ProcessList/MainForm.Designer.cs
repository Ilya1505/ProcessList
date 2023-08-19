namespace ProcessList
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridProcessList = new System.Windows.Forms.DataGridView();
            this.labelCountProc = new System.Windows.Forms.Label();
            this.labelCommonMemory = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.memory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.threads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridProcessList)).BeginInit();
            this.SuspendLayout();
            // 
            // gridProcessList
            // 
            this.gridProcessList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridProcessList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridProcessList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridProcessList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridProcessList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridProcessList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.name,
            this.memory,
            this.time,
            this.threads});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridProcessList.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridProcessList.Location = new System.Drawing.Point(0, 0);
            this.gridProcessList.Name = "gridProcessList";
            this.gridProcessList.RowHeadersVisible = false;
            this.gridProcessList.RowHeadersWidth = 51;
            this.gridProcessList.RowTemplate.Height = 24;
            this.gridProcessList.Size = new System.Drawing.Size(1263, 630);
            this.gridProcessList.TabIndex = 0;
            this.gridProcessList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridProcess_CellContentClick);
            this.gridProcessList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridProcessList_CellMouseDoubleClick);
            this.gridProcessList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridProcessList_KeyDown);
            // 
            // labelCountProc
            // 
            this.labelCountProc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCountProc.AutoSize = true;
            this.labelCountProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCountProc.Location = new System.Drawing.Point(12, 640);
            this.labelCountProc.Name = "labelCountProc";
            this.labelCountProc.Size = new System.Drawing.Size(293, 38);
            this.labelCountProc.TabIndex = 1;
            this.labelCountProc.Text = "Всего процессов: ";
            // 
            // labelCommonMemory
            // 
            this.labelCommonMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCommonMemory.AutoSize = true;
            this.labelCommonMemory.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCommonMemory.Location = new System.Drawing.Point(12, 688);
            this.labelCommonMemory.Name = "labelCommonMemory";
            this.labelCommonMemory.Size = new System.Drawing.Size(262, 38);
            this.labelCommonMemory.TabIndex = 2;
            this.labelCommonMemory.Text = "Общая память: ";
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // name
            // 
            this.name.HeaderText = "Имя";
            this.name.MinimumWidth = 6;
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // memory
            // 
            this.memory.HeaderText = "Память (Мб)";
            this.memory.MinimumWidth = 6;
            this.memory.Name = "memory";
            this.memory.ReadOnly = true;
            // 
            // time
            // 
            this.time.HeaderText = "Время работы (мин:сек)";
            this.time.MinimumWidth = 6;
            this.time.Name = "time";
            this.time.ReadOnly = true;
            // 
            // threads
            // 
            this.threads.HeaderText = "Число потоков";
            this.threads.MinimumWidth = 6;
            this.threads.Name = "threads";
            this.threads.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 748);
            this.Controls.Add(this.labelCommonMemory);
            this.Controls.Add(this.labelCountProc);
            this.Controls.Add(this.gridProcessList);
            this.MinimumSize = new System.Drawing.Size(1280, 795);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Список процессов";
            ((System.ComponentModel.ISupportInitialize)(this.gridProcessList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView gridProcessList;
        private System.Windows.Forms.Label labelCountProc;
        private System.Windows.Forms.Label labelCommonMemory;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn memory;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn threads;
    }
}

