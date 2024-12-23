namespace OOP_System
{
    partial class frmUserAccount
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.metroTabControl2 = new MetroFramework.Controls.MetroTabControl();
            this.tabPageCreate = new MetroFramework.Controls.MetroTabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboUserType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPageChange = new MetroFramework.Controls.MetroTabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.txtPin2 = new System.Windows.Forms.ComboBox();
            this.txtConfirmPin = new System.Windows.Forms.TextBox();
            this.labelConfirmPassword = new System.Windows.Forms.Label();
            this.txtNewPin = new System.Windows.Forms.TextBox();
            this.labelNewPassword = new System.Windows.Forms.Label();
            this.txtOldPin = new System.Windows.Forms.TextBox();
            this.labelOldPassword = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.tabPageDelete = new MetroFramework.Controls.MetroTabPage();
            this.button6 = new System.Windows.Forms.Button();
            this.cboDeletePin = new System.Windows.Forms.ComboBox();
            this.labelDeleteUsername = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3.SuspendLayout();
            this.metroTabControl2.SuspendLayout();
            this.tabPageCreate.SuspendLayout();
            this.tabPageChange.SuspendLayout();
            this.tabPageDelete.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.metroTabControl2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1084, 647);
            this.panel3.TabIndex = 4;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // metroTabControl2
            // 
            this.metroTabControl2.Controls.Add(this.tabPageCreate);
            this.metroTabControl2.Controls.Add(this.tabPageChange);
            this.metroTabControl2.Controls.Add(this.tabPageDelete);
            this.metroTabControl2.Controls.Add(this.tabPage1);
            this.metroTabControl2.FontSize = MetroFramework.MetroTabControlSize.Tall;
            this.metroTabControl2.FontWeight = MetroFramework.MetroTabControlWeight.Regular;
            this.metroTabControl2.Location = new System.Drawing.Point(12, 16);
            this.metroTabControl2.Name = "metroTabControl2";
            this.metroTabControl2.SelectedIndex = 2;
            this.metroTabControl2.Size = new System.Drawing.Size(1060, 614);
            this.metroTabControl2.TabIndex = 13;
            this.metroTabControl2.UseSelectable = true;
            // 
            // tabPageCreate
            // 
            this.tabPageCreate.Controls.Add(this.button1);
            this.tabPageCreate.Controls.Add(this.txtName);
            this.tabPageCreate.Controls.Add(this.label6);
            this.tabPageCreate.Controls.Add(this.cboUserType);
            this.tabPageCreate.Controls.Add(this.label5);
            this.tabPageCreate.Controls.Add(this.txtPin);
            this.tabPageCreate.Controls.Add(this.label2);
            this.tabPageCreate.HorizontalScrollbarBarColor = true;
            this.tabPageCreate.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPageCreate.HorizontalScrollbarSize = 2;
            this.tabPageCreate.Location = new System.Drawing.Point(4, 44);
            this.tabPageCreate.Name = "tabPageCreate";
            this.tabPageCreate.Size = new System.Drawing.Size(1052, 566);
            this.tabPageCreate.TabIndex = 0;
            this.tabPageCreate.Text = "CREATE PIN";
            this.tabPageCreate.VerticalScrollbarBarColor = true;
            this.tabPageCreate.VerticalScrollbarHighlightOnWheel = false;
            this.tabPageCreate.VerticalScrollbarSize = 6;
            this.tabPageCreate.Click += new System.EventHandler(this.tabPageCreate_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(182)))), ((int)(((byte)(114)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 17F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(776, 365);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(172, 49);
            this.button1.TabIndex = 26;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.txtName.Location = new System.Drawing.Point(385, 268);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(563, 52);
            this.txtName.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(69)))));
            this.label6.Location = new System.Drawing.Point(89, 274);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 46);
            this.label6.TabIndex = 21;
            this.label6.Text = "Name:";
            // 
            // cboUserType
            // 
            this.cboUserType.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.cboUserType.FormattingEnabled = true;
            this.cboUserType.Items.AddRange(new object[] {
            "Admin",
            "Cashier"});
            this.cboUserType.Location = new System.Drawing.Point(385, 187);
            this.cboUserType.Name = "cboUserType";
            this.cboUserType.Size = new System.Drawing.Size(563, 53);
            this.cboUserType.TabIndex = 20;
            this.cboUserType.SelectedIndexChanged += new System.EventHandler(this.cboRole_SelectedIndexChanged);
            this.cboUserType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboRole_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(69)))));
            this.label5.Location = new System.Drawing.Point(89, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 46);
            this.label5.TabIndex = 19;
            this.label5.Text = "User Type:";
            // 
            // tabPageChange
            // 
            this.tabPageChange.Controls.Add(this.button2);
            this.tabPageChange.Controls.Add(this.txtPin2);
            this.tabPageChange.Controls.Add(this.txtConfirmPin);
            this.tabPageChange.Controls.Add(this.labelConfirmPassword);
            this.tabPageChange.Controls.Add(this.txtNewPin);
            this.tabPageChange.Controls.Add(this.labelNewPassword);
            this.tabPageChange.Controls.Add(this.txtOldPin);
            this.tabPageChange.Controls.Add(this.labelOldPassword);
            this.tabPageChange.Controls.Add(this.labelUsername);
            this.tabPageChange.HorizontalScrollbarBarColor = true;
            this.tabPageChange.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPageChange.HorizontalScrollbarSize = 2;
            this.tabPageChange.Location = new System.Drawing.Point(4, 44);
            this.tabPageChange.Name = "tabPageChange";
            this.tabPageChange.Size = new System.Drawing.Size(1052, 566);
            this.tabPageChange.TabIndex = 1;
            this.tabPageChange.Text = "UPDATE PIN";
            this.tabPageChange.VerticalScrollbarBarColor = true;
            this.tabPageChange.VerticalScrollbarHighlightOnWheel = false;
            this.tabPageChange.VerticalScrollbarSize = 6;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(182)))), ((int)(((byte)(114)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 17F);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(760, 437);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(172, 49);
            this.button2.TabIndex = 27;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtPin2
            // 
            this.txtPin2.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.txtPin2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(69)))));
            this.txtPin2.FormattingEnabled = true;
            this.txtPin2.Location = new System.Drawing.Point(369, 91);
            this.txtPin2.Name = "txtPin2";
            this.txtPin2.Size = new System.Drawing.Size(563, 53);
            this.txtPin2.TabIndex = 25;
            this.txtPin2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxUsername_KeyPress);
            // 
            // txtConfirmPin
            // 
            this.txtConfirmPin.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.txtConfirmPin.Location = new System.Drawing.Point(369, 353);
            this.txtConfirmPin.Name = "txtConfirmPin";
            this.txtConfirmPin.Size = new System.Drawing.Size(563, 52);
            this.txtConfirmPin.TabIndex = 22;
            this.txtConfirmPin.UseSystemPasswordChar = true;
            this.txtConfirmPin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConfirmPin_KeyPress);
            // 
            // labelConfirmPassword
            // 
            this.labelConfirmPassword.AutoSize = true;
            this.labelConfirmPassword.BackColor = System.Drawing.Color.White;
            this.labelConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.labelConfirmPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(69)))));
            this.labelConfirmPassword.Location = new System.Drawing.Point(83, 359);
            this.labelConfirmPassword.Name = "labelConfirmPassword";
            this.labelConfirmPassword.Size = new System.Drawing.Size(202, 46);
            this.labelConfirmPassword.TabIndex = 21;
            this.labelConfirmPassword.Text = "Confirm Pin:";
            // 
            // txtNewPin
            // 
            this.txtNewPin.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.txtNewPin.Location = new System.Drawing.Point(369, 263);
            this.txtNewPin.Name = "txtNewPin";
            this.txtNewPin.PasswordChar = '•';
            this.txtNewPin.Size = new System.Drawing.Size(563, 52);
            this.txtNewPin.TabIndex = 18;
            this.txtNewPin.UseSystemPasswordChar = true;
            this.txtNewPin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewPin_KeyPress);
            // 
            // labelNewPassword
            // 
            this.labelNewPassword.AutoSize = true;
            this.labelNewPassword.BackColor = System.Drawing.Color.White;
            this.labelNewPassword.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.labelNewPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(69)))));
            this.labelNewPassword.Location = new System.Drawing.Point(83, 269);
            this.labelNewPassword.Name = "labelNewPassword";
            this.labelNewPassword.Size = new System.Drawing.Size(150, 46);
            this.labelNewPassword.TabIndex = 17;
            this.labelNewPassword.Text = "New Pin:";
            // 
            // txtOldPin
            // 
            this.txtOldPin.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.txtOldPin.Location = new System.Drawing.Point(369, 175);
            this.txtOldPin.Name = "txtOldPin";
            this.txtOldPin.PasswordChar = '•';
            this.txtOldPin.Size = new System.Drawing.Size(563, 52);
            this.txtOldPin.TabIndex = 16;
            this.txtOldPin.UseSystemPasswordChar = true;
            this.txtOldPin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOldPin_KeyPress);
            // 
            // labelOldPassword
            // 
            this.labelOldPassword.AutoSize = true;
            this.labelOldPassword.BackColor = System.Drawing.Color.White;
            this.labelOldPassword.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.labelOldPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(69)))));
            this.labelOldPassword.Location = new System.Drawing.Point(83, 181);
            this.labelOldPassword.Name = "labelOldPassword";
            this.labelOldPassword.Size = new System.Drawing.Size(136, 46);
            this.labelOldPassword.TabIndex = 15;
            this.labelOldPassword.Text = "Old Pin:";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.BackColor = System.Drawing.Color.White;
            this.labelUsername.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.labelUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(69)))));
            this.labelUsername.Location = new System.Drawing.Point(83, 98);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(116, 46);
            this.labelUsername.TabIndex = 13;
            this.labelUsername.Text = "Name:";
            // 
            // tabPageDelete
            // 
            this.tabPageDelete.Controls.Add(this.button6);
            this.tabPageDelete.Controls.Add(this.cboDeletePin);
            this.tabPageDelete.Controls.Add(this.labelDeleteUsername);
            this.tabPageDelete.HorizontalScrollbarBarColor = true;
            this.tabPageDelete.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPageDelete.HorizontalScrollbarSize = 2;
            this.tabPageDelete.Location = new System.Drawing.Point(4, 44);
            this.tabPageDelete.Name = "tabPageDelete";
            this.tabPageDelete.Size = new System.Drawing.Size(1052, 566);
            this.tabPageDelete.TabIndex = 2;
            this.tabPageDelete.Text = "DELETE PIN";
            this.tabPageDelete.VerticalScrollbarBarColor = true;
            this.tabPageDelete.VerticalScrollbarHighlightOnWheel = false;
            this.tabPageDelete.VerticalScrollbarSize = 6;
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(101)))), ((int)(((byte)(103)))));
            this.button6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.Font = new System.Drawing.Font("Segoe UI", 17F);
            this.button6.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.Location = new System.Drawing.Point(728, 203);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(172, 49);
            this.button6.TabIndex = 28;
            this.button6.Text = "Delete";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // cboDeletePin
            // 
            this.cboDeletePin.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.cboDeletePin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(69)))));
            this.cboDeletePin.FormattingEnabled = true;
            this.cboDeletePin.Items.AddRange(new object[] {
            "System Administrator",
            "Cashier"});
            this.cboDeletePin.Location = new System.Drawing.Point(337, 119);
            this.cboDeletePin.Name = "cboDeletePin";
            this.cboDeletePin.Size = new System.Drawing.Size(563, 53);
            this.cboDeletePin.TabIndex = 25;
            this.cboDeletePin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxDeleteUsername_KeyPress);
            // 
            // labelDeleteUsername
            // 
            this.labelDeleteUsername.AutoSize = true;
            this.labelDeleteUsername.BackColor = System.Drawing.Color.White;
            this.labelDeleteUsername.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.labelDeleteUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(69)))));
            this.labelDeleteUsername.Location = new System.Drawing.Point(114, 122);
            this.labelDeleteUsername.Name = "labelDeleteUsername";
            this.labelDeleteUsername.Size = new System.Drawing.Size(116, 46);
            this.labelDeleteUsername.TabIndex = 13;
            this.labelDeleteUsername.Text = "Name:";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.tabPage1.Location = new System.Drawing.Point(4, 44);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1052, 566);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "USERS LIST";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(159)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 13F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridView1.ColumnHeadersHeight = 40;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column5,
            this.Column7});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 10.8F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(212)))), ((int)(((byte)(255)))));
            this.dataGridView1.Location = new System.Drawing.Point(30, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(986, 472);
            this.dataGridView1.TabIndex = 38;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(69)))));
            this.label2.Location = new System.Drawing.Point(89, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 46);
            this.label2.TabIndex = 13;
            this.label2.Text = "Pin:";
            // 
            // txtPin
            // 
            this.txtPin.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.txtPin.Location = new System.Drawing.Point(385, 99);
            this.txtPin.Name = "txtPin";
            this.txtPin.Size = new System.Drawing.Size(563, 52);
            this.txtPin.TabIndex = 14;
            this.txtPin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPin_KeyPress);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column1.HeaderText = "#";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 54;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "NAME";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column5.HeaderText = "PIN";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 75;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column7.HeaderText = "TYPE";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 88;
            // 
            // frmUserAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1084, 647);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmUserAccount";
            this.Text = "frmUserAccount";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmUserAccount_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUserAccount_KeyDown);
            this.Resize += new System.EventHandler(this.frmUserAccount_Resize);
            this.panel3.ResumeLayout(false);
            this.metroTabControl2.ResumeLayout(false);
            this.tabPageCreate.ResumeLayout(false);
            this.tabPageCreate.PerformLayout();
            this.tabPageChange.ResumeLayout(false);
            this.tabPageChange.PerformLayout();
            this.tabPageDelete.ResumeLayout(false);
            this.tabPageDelete.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private MetroFramework.Controls.MetroTabControl metroTabControl2;
        private MetroFramework.Controls.MetroTabPage tabPageCreate;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboUserType;
        private System.Windows.Forms.Label label5;
        private MetroFramework.Controls.MetroTabPage tabPageChange;
        private System.Windows.Forms.ComboBox txtPin2;
        private System.Windows.Forms.TextBox txtConfirmPin;
        private System.Windows.Forms.Label labelConfirmPassword;
        private System.Windows.Forms.TextBox txtNewPin;
        private System.Windows.Forms.Label labelNewPassword;
        private System.Windows.Forms.TextBox txtOldPin;
        private System.Windows.Forms.Label labelOldPassword;
        private System.Windows.Forms.Label labelUsername;
        private MetroFramework.Controls.MetroTabPage tabPageDelete;
        private System.Windows.Forms.ComboBox cboDeletePin;
        private System.Windows.Forms.Label labelDeleteUsername;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtPin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
    }
}