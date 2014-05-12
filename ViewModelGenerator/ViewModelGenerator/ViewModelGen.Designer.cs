namespace ViewModelGenerator
{
    partial class ViewModelGen
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
            this.DBSettings = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rowVersion = new System.Windows.Forms.CheckBox();
            this.modifiedDate = new System.Windows.Forms.CheckBox();
            this.createdDate = new System.Windows.Forms.CheckBox();
            this.HiddenInputAnn = new System.Windows.Forms.CheckBox();
            this.DisplayAnn = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.RequiredAnn = new System.Windows.Forms.CheckBox();
            this.StringLengthAnn = new System.Windows.Forms.CheckBox();
            this.modifiedBy = new System.Windows.Forms.CheckBox();
            this.createdBy = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IncludeLabel = new System.Windows.Forms.Label();
            this.GenB = new System.Windows.Forms.Button();
            this.CodeText = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ViewModelNameTB = new System.Windows.Forms.TextBox();
            this.TableCB = new System.Windows.Forms.ComboBox();
            this.TableNameLabel = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.isPartial = new System.Windows.Forms.CheckBox();
            this.ActionsResult = new System.Windows.Forms.RichTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ControllerNameTB = new System.Windows.Forms.TextBox();
            this.RepoNameTB = new System.Windows.Forms.TextBox();
            this.FKGV = new System.Windows.Forms.DataGridView();
            this.PKGV = new System.Windows.Forms.DataGridView();
            this.TableGrid = new System.Windows.Forms.DataGridView();
            this.CTableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CRegionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CRepoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CController = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isPartialView = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.QueueBtn = new System.Windows.Forms.Button();
            this.RepoCodeText = new System.Windows.Forms.RichTextBox();
            this.RepoBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.RegionTB = new System.Windows.Forms.TextBox();
            this.TableNameRepo = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.DatabaseCB = new System.Windows.Forms.ComboBox();
            this.TestB = new System.Windows.Forms.Button();
            this.PassTB = new System.Windows.Forms.TextBox();
            this.UserTB = new System.Windows.Forms.TextBox();
            this.DatabaseLabel = new System.Windows.Forms.Label();
            this.ServeTB = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.DBSettings.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FKGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PKGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TableGrid)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DBSettings
            // 
            this.DBSettings.Controls.Add(this.tabPage2);
            this.DBSettings.Controls.Add(this.tabPage3);
            this.DBSettings.Controls.Add(this.tabPage1);
            this.DBSettings.Location = new System.Drawing.Point(8, 8);
            this.DBSettings.Margin = new System.Windows.Forms.Padding(2);
            this.DBSettings.Name = "DBSettings";
            this.DBSettings.SelectedIndex = 0;
            this.DBSettings.Size = new System.Drawing.Size(1336, 741);
            this.DBSettings.TabIndex = 0;
            this.DBSettings.SelectedIndexChanged += new System.EventHandler(this.DBSettings_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rowVersion);
            this.tabPage2.Controls.Add(this.modifiedDate);
            this.tabPage2.Controls.Add(this.createdDate);
            this.tabPage2.Controls.Add(this.HiddenInputAnn);
            this.tabPage2.Controls.Add(this.DisplayAnn);
            this.tabPage2.Controls.Add(this.checkBox1);
            this.tabPage2.Controls.Add(this.RequiredAnn);
            this.tabPage2.Controls.Add(this.StringLengthAnn);
            this.tabPage2.Controls.Add(this.modifiedBy);
            this.tabPage2.Controls.Add(this.createdBy);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.IncludeLabel);
            this.tabPage2.Controls.Add(this.GenB);
            this.tabPage2.Controls.Add(this.CodeText);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.ViewModelNameTB);
            this.tabPage2.Controls.Add(this.TableCB);
            this.tabPage2.Controls.Add(this.TableNameLabel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(1328, 715);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Generate View Model ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rowVersion
            // 
            this.rowVersion.AutoSize = true;
            this.rowVersion.Location = new System.Drawing.Point(399, 86);
            this.rowVersion.Margin = new System.Windows.Forms.Padding(2);
            this.rowVersion.Name = "rowVersion";
            this.rowVersion.Size = new System.Drawing.Size(78, 17);
            this.rowVersion.TabIndex = 13;
            this.rowVersion.Text = "rowVersion";
            this.rowVersion.UseVisualStyleBackColor = true;
            // 
            // modifiedDate
            // 
            this.modifiedDate.AutoSize = true;
            this.modifiedDate.Checked = true;
            this.modifiedDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.modifiedDate.Location = new System.Drawing.Point(246, 104);
            this.modifiedDate.Margin = new System.Windows.Forms.Padding(2);
            this.modifiedDate.Name = "modifiedDate";
            this.modifiedDate.Size = new System.Drawing.Size(88, 17);
            this.modifiedDate.TabIndex = 13;
            this.modifiedDate.Text = "modifiedDate";
            this.modifiedDate.UseVisualStyleBackColor = true;
            // 
            // createdDate
            // 
            this.createdDate.AutoSize = true;
            this.createdDate.Checked = true;
            this.createdDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.createdDate.Location = new System.Drawing.Point(246, 86);
            this.createdDate.Margin = new System.Windows.Forms.Padding(2);
            this.createdDate.Name = "createdDate";
            this.createdDate.Size = new System.Drawing.Size(85, 17);
            this.createdDate.TabIndex = 13;
            this.createdDate.Text = "createdDate";
            this.createdDate.UseVisualStyleBackColor = true;
            // 
            // HiddenInputAnn
            // 
            this.HiddenInputAnn.AutoSize = true;
            this.HiddenInputAnn.Checked = true;
            this.HiddenInputAnn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HiddenInputAnn.Location = new System.Drawing.Point(111, 170);
            this.HiddenInputAnn.Margin = new System.Windows.Forms.Padding(2);
            this.HiddenInputAnn.Name = "HiddenInputAnn";
            this.HiddenInputAnn.Size = new System.Drawing.Size(116, 17);
            this.HiddenInputAnn.TabIndex = 13;
            this.HiddenInputAnn.Text = "HiddenInput for PK";
            this.HiddenInputAnn.UseVisualStyleBackColor = true;
            // 
            // DisplayAnn
            // 
            this.DisplayAnn.AutoSize = true;
            this.DisplayAnn.Checked = true;
            this.DisplayAnn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayAnn.Location = new System.Drawing.Point(111, 148);
            this.DisplayAnn.Margin = new System.Windows.Forms.Padding(2);
            this.DisplayAnn.Name = "DisplayAnn";
            this.DisplayAnn.Size = new System.Drawing.Size(97, 17);
            this.DisplayAnn.TabIndex = 13;
            this.DisplayAnn.Text = "Display (Name)";
            this.DisplayAnn.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(111, 149);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(77, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "modifiedBy";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // RequiredAnn
            // 
            this.RequiredAnn.AutoSize = true;
            this.RequiredAnn.Checked = true;
            this.RequiredAnn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RequiredAnn.Location = new System.Drawing.Point(245, 170);
            this.RequiredAnn.Margin = new System.Windows.Forms.Padding(2);
            this.RequiredAnn.Name = "RequiredAnn";
            this.RequiredAnn.Size = new System.Drawing.Size(163, 17);
            this.RequiredAnn.TabIndex = 13;
            this.RequiredAnn.Text = "Required for not null columns";
            this.RequiredAnn.UseVisualStyleBackColor = true;
            // 
            // StringLengthAnn
            // 
            this.StringLengthAnn.AutoSize = true;
            this.StringLengthAnn.Checked = true;
            this.StringLengthAnn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StringLengthAnn.Location = new System.Drawing.Point(246, 149);
            this.StringLengthAnn.Margin = new System.Windows.Forms.Padding(2);
            this.StringLengthAnn.Name = "StringLengthAnn";
            this.StringLengthAnn.Size = new System.Drawing.Size(86, 17);
            this.StringLengthAnn.TabIndex = 13;
            this.StringLengthAnn.Text = "StringLength";
            this.StringLengthAnn.UseVisualStyleBackColor = true;
            // 
            // modifiedBy
            // 
            this.modifiedBy.AutoSize = true;
            this.modifiedBy.Checked = true;
            this.modifiedBy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.modifiedBy.Location = new System.Drawing.Point(111, 107);
            this.modifiedBy.Margin = new System.Windows.Forms.Padding(2);
            this.modifiedBy.Name = "modifiedBy";
            this.modifiedBy.Size = new System.Drawing.Size(77, 17);
            this.modifiedBy.TabIndex = 13;
            this.modifiedBy.Text = "modifiedBy";
            this.modifiedBy.UseVisualStyleBackColor = true;
            // 
            // createdBy
            // 
            this.createdBy.AutoSize = true;
            this.createdBy.Checked = true;
            this.createdBy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.createdBy.Location = new System.Drawing.Point(111, 89);
            this.createdBy.Margin = new System.Windows.Forms.Padding(2);
            this.createdBy.Name = "createdBy";
            this.createdBy.Size = new System.Drawing.Size(74, 17);
            this.createdBy.TabIndex = 13;
            this.createdBy.Text = "createdBy";
            this.createdBy.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 149);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Annotations";
            // 
            // IncludeLabel
            // 
            this.IncludeLabel.AutoSize = true;
            this.IncludeLabel.Location = new System.Drawing.Point(19, 90);
            this.IncludeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.IncludeLabel.Name = "IncludeLabel";
            this.IncludeLabel.Size = new System.Drawing.Size(90, 13);
            this.IncludeLabel.TabIndex = 12;
            this.IncludeLabel.Text = "Include Defaults?";
            // 
            // GenB
            // 
            this.GenB.Location = new System.Drawing.Point(111, 202);
            this.GenB.Margin = new System.Windows.Forms.Padding(2);
            this.GenB.Name = "GenB";
            this.GenB.Size = new System.Drawing.Size(366, 25);
            this.GenB.TabIndex = 11;
            this.GenB.Text = "Generate and Copy to Clipboard";
            this.GenB.UseVisualStyleBackColor = true;
            this.GenB.Click += new System.EventHandler(this.GenB_Click);
            // 
            // CodeText
            // 
            this.CodeText.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CodeText.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.CodeText.Location = new System.Drawing.Point(22, 242);
            this.CodeText.Margin = new System.Windows.Forms.Padding(2);
            this.CodeText.Name = "CodeText";
            this.CodeText.Size = new System.Drawing.Size(1257, 446);
            this.CodeText.TabIndex = 10;
            this.CodeText.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 59);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "ViewModel Name";
            // 
            // ViewModelNameTB
            // 
            this.ViewModelNameTB.Location = new System.Drawing.Point(111, 56);
            this.ViewModelNameTB.Margin = new System.Windows.Forms.Padding(2);
            this.ViewModelNameTB.Name = "ViewModelNameTB";
            this.ViewModelNameTB.Size = new System.Drawing.Size(366, 20);
            this.ViewModelNameTB.TabIndex = 8;
            // 
            // TableCB
            // 
            this.TableCB.FormattingEnabled = true;
            this.TableCB.Location = new System.Drawing.Point(111, 14);
            this.TableCB.Margin = new System.Windows.Forms.Padding(2);
            this.TableCB.Name = "TableCB";
            this.TableCB.Size = new System.Drawing.Size(366, 21);
            this.TableCB.TabIndex = 7;
            this.TableCB.SelectedIndexChanged += new System.EventHandler(this.TableCB_SelectedIndexChanged);
            this.TableCB.SelectedValueChanged += new System.EventHandler(this.TableCB_SelectedValueChanged);
            this.TableCB.Click += new System.EventHandler(this.TableCB_Click);
            // 
            // TableNameLabel
            // 
            this.TableNameLabel.AutoSize = true;
            this.TableNameLabel.Location = new System.Drawing.Point(19, 19);
            this.TableNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TableNameLabel.Name = "TableNameLabel";
            this.TableNameLabel.Size = new System.Drawing.Size(34, 13);
            this.TableNameLabel.TabIndex = 6;
            this.TableNameLabel.Text = "Table";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.isPartial);
            this.tabPage3.Controls.Add(this.ActionsResult);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.ControllerNameTB);
            this.tabPage3.Controls.Add(this.RepoNameTB);
            this.tabPage3.Controls.Add(this.FKGV);
            this.tabPage3.Controls.Add(this.PKGV);
            this.tabPage3.Controls.Add(this.TableGrid);
            this.tabPage3.Controls.Add(this.QueueBtn);
            this.tabPage3.Controls.Add(this.RepoCodeText);
            this.tabPage3.Controls.Add(this.RepoBtn);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.RegionTB);
            this.tabPage3.Controls.Add(this.TableNameRepo);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1328, 715);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Generate Repo & Controller";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // isPartial
            // 
            this.isPartial.AutoSize = true;
            this.isPartial.Location = new System.Drawing.Point(623, 132);
            this.isPartial.Margin = new System.Windows.Forms.Padding(2);
            this.isPartial.Name = "isPartial";
            this.isPartial.Size = new System.Drawing.Size(84, 17);
            this.isPartial.TabIndex = 29;
            this.isPartial.Text = "PartialView?";
            this.isPartial.UseVisualStyleBackColor = true;
            // 
            // ActionsResult
            // 
            this.ActionsResult.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionsResult.ForeColor = System.Drawing.Color.DarkGreen;
            this.ActionsResult.Location = new System.Drawing.Point(898, 195);
            this.ActionsResult.Margin = new System.Windows.Forms.Padding(2);
            this.ActionsResult.Name = "ActionsResult";
            this.ActionsResult.Size = new System.Drawing.Size(406, 412);
            this.ActionsResult.TabIndex = 28;
            this.ActionsResult.Text = "";
            this.ActionsResult.Click += new System.EventHandler(this.ActionsResult_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(895, 180);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Controller Code";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(477, 180);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Repo Code";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(487, 111);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Controller Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(487, 93);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Repo Name";
            // 
            // ControllerNameTB
            // 
            this.ControllerNameTB.Location = new System.Drawing.Point(623, 108);
            this.ControllerNameTB.Margin = new System.Windows.Forms.Padding(2);
            this.ControllerNameTB.Name = "ControllerNameTB";
            this.ControllerNameTB.Size = new System.Drawing.Size(333, 20);
            this.ControllerNameTB.TabIndex = 24;
            // 
            // RepoNameTB
            // 
            this.RepoNameTB.Location = new System.Drawing.Point(623, 90);
            this.RepoNameTB.Margin = new System.Windows.Forms.Padding(2);
            this.RepoNameTB.Name = "RepoNameTB";
            this.RepoNameTB.Size = new System.Drawing.Size(333, 20);
            this.RepoNameTB.TabIndex = 25;
            this.RepoNameTB.Text = "Repo";
            // 
            // FKGV
            // 
            this.FKGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FKGV.Location = new System.Drawing.Point(269, 487);
            this.FKGV.Name = "FKGV";
            this.FKGV.Size = new System.Drawing.Size(194, 150);
            this.FKGV.TabIndex = 19;
            this.FKGV.Visible = false;
            // 
            // PKGV
            // 
            this.PKGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PKGV.Location = new System.Drawing.Point(12, 487);
            this.PKGV.Name = "PKGV";
            this.PKGV.Size = new System.Drawing.Size(240, 150);
            this.PKGV.TabIndex = 19;
            this.PKGV.Visible = false;
            // 
            // TableGrid
            // 
            this.TableGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TableGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CTableName,
            this.CRegionName,
            this.CRepoName,
            this.CController,
            this.isPartialView});
            this.TableGrid.Location = new System.Drawing.Point(12, 18);
            this.TableGrid.Name = "TableGrid";
            this.TableGrid.Size = new System.Drawing.Size(463, 646);
            this.TableGrid.TabIndex = 18;
            // 
            // CTableName
            // 
            this.CTableName.HeaderText = "Table Name";
            this.CTableName.Name = "CTableName";
            // 
            // CRegionName
            // 
            this.CRegionName.HeaderText = "Region Name";
            this.CRegionName.Name = "CRegionName";
            // 
            // CRepoName
            // 
            this.CRepoName.HeaderText = "Repo Name";
            this.CRepoName.Name = "CRepoName";
            // 
            // CController
            // 
            this.CController.HeaderText = "Controller";
            this.CController.Name = "CController";
            // 
            // isPartialView
            // 
            this.isPartialView.HeaderText = "PartialView?";
            this.isPartialView.Name = "isPartialView";
            // 
            // QueueBtn
            // 
            this.QueueBtn.Location = new System.Drawing.Point(480, 153);
            this.QueueBtn.Margin = new System.Windows.Forms.Padding(2);
            this.QueueBtn.Name = "QueueBtn";
            this.QueueBtn.Size = new System.Drawing.Size(490, 25);
            this.QueueBtn.TabIndex = 17;
            this.QueueBtn.Text = "<---- Add to queue";
            this.QueueBtn.UseVisualStyleBackColor = true;
            this.QueueBtn.Click += new System.EventHandler(this.QueueBtn_Click);
            // 
            // RepoCodeText
            // 
            this.RepoCodeText.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RepoCodeText.ForeColor = System.Drawing.Color.Chocolate;
            this.RepoCodeText.Location = new System.Drawing.Point(480, 195);
            this.RepoCodeText.Margin = new System.Windows.Forms.Padding(2);
            this.RepoCodeText.Name = "RepoCodeText";
            this.RepoCodeText.Size = new System.Drawing.Size(395, 412);
            this.RepoCodeText.TabIndex = 15;
            this.RepoCodeText.Text = "";
            this.RepoCodeText.Click += new System.EventHandler(this.RepoCodeText_Click);
            // 
            // RepoBtn
            // 
            this.RepoBtn.Location = new System.Drawing.Point(480, 623);
            this.RepoBtn.Margin = new System.Windows.Forms.Padding(2);
            this.RepoBtn.Name = "RepoBtn";
            this.RepoBtn.Size = new System.Drawing.Size(846, 41);
            this.RepoBtn.TabIndex = 14;
            this.RepoBtn.Text = "Generate";
            this.RepoBtn.UseVisualStyleBackColor = true;
            this.RepoBtn.Click += new System.EventHandler(this.RepoBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(487, 51);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Region Name";
            // 
            // RegionTB
            // 
            this.RegionTB.Location = new System.Drawing.Point(623, 48);
            this.RegionTB.Margin = new System.Windows.Forms.Padding(2);
            this.RegionTB.Name = "RegionTB";
            this.RegionTB.Size = new System.Drawing.Size(333, 20);
            this.RegionTB.TabIndex = 12;
            // 
            // TableNameRepo
            // 
            this.TableNameRepo.FormattingEnabled = true;
            this.TableNameRepo.Location = new System.Drawing.Point(623, 17);
            this.TableNameRepo.Margin = new System.Windows.Forms.Padding(2);
            this.TableNameRepo.Name = "TableNameRepo";
            this.TableNameRepo.Size = new System.Drawing.Size(333, 21);
            this.TableNameRepo.TabIndex = 11;
            this.TableNameRepo.SelectedIndexChanged += new System.EventHandler(this.TableNameRepo_SelectedIndexChanged);
            this.TableNameRepo.Click += new System.EventHandler(this.TableNameRepo_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(1148, 180);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(156, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Click inside to copy to clipboard";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(719, 180);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(156, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Click inside to copy to clipboard";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(487, 22);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Table";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DatabaseCB);
            this.tabPage1.Controls.Add(this.TestB);
            this.tabPage1.Controls.Add(this.PassTB);
            this.tabPage1.Controls.Add(this.UserTB);
            this.tabPage1.Controls.Add(this.DatabaseLabel);
            this.tabPage1.Controls.Add(this.ServeTB);
            this.tabPage1.Controls.Add(this.PasswordLabel);
            this.tabPage1.Controls.Add(this.UserNameLabel);
            this.tabPage1.Controls.Add(this.ServerLabel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(1328, 715);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DB Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // DatabaseCB
            // 
            this.DatabaseCB.FormattingEnabled = true;
            this.DatabaseCB.Location = new System.Drawing.Point(143, 84);
            this.DatabaseCB.Margin = new System.Windows.Forms.Padding(2);
            this.DatabaseCB.Name = "DatabaseCB";
            this.DatabaseCB.Size = new System.Drawing.Size(219, 21);
            this.DatabaseCB.TabIndex = 5;
            this.DatabaseCB.Click += new System.EventHandler(this.DatabaseCB_Click);
            // 
            // TestB
            // 
            this.TestB.Location = new System.Drawing.Point(143, 114);
            this.TestB.Margin = new System.Windows.Forms.Padding(2);
            this.TestB.Name = "TestB";
            this.TestB.Size = new System.Drawing.Size(217, 23);
            this.TestB.TabIndex = 4;
            this.TestB.Text = "Test and Save";
            this.TestB.UseVisualStyleBackColor = true;
            this.TestB.Click += new System.EventHandler(this.TestB_Click);
            // 
            // PassTB
            // 
            this.PassTB.Location = new System.Drawing.Point(143, 62);
            this.PassTB.Margin = new System.Windows.Forms.Padding(2);
            this.PassTB.Name = "PassTB";
            this.PassTB.PasswordChar = '*';
            this.PassTB.Size = new System.Drawing.Size(219, 20);
            this.PassTB.TabIndex = 3;
            // 
            // UserTB
            // 
            this.UserTB.Location = new System.Drawing.Point(143, 42);
            this.UserTB.Margin = new System.Windows.Forms.Padding(2);
            this.UserTB.Name = "UserTB";
            this.UserTB.Size = new System.Drawing.Size(219, 20);
            this.UserTB.TabIndex = 3;
            // 
            // DatabaseLabel
            // 
            this.DatabaseLabel.AutoSize = true;
            this.DatabaseLabel.Location = new System.Drawing.Point(20, 86);
            this.DatabaseLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DatabaseLabel.Name = "DatabaseLabel";
            this.DatabaseLabel.Size = new System.Drawing.Size(53, 13);
            this.DatabaseLabel.TabIndex = 2;
            this.DatabaseLabel.Text = "Database";
            // 
            // ServeTB
            // 
            this.ServeTB.Location = new System.Drawing.Point(143, 22);
            this.ServeTB.Margin = new System.Windows.Forms.Padding(2);
            this.ServeTB.Name = "ServeTB";
            this.ServeTB.Size = new System.Drawing.Size(219, 20);
            this.ServeTB.TabIndex = 3;
            this.ServeTB.TextChanged += new System.EventHandler(this.ServeTB_TextChanged);
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(20, 66);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.PasswordLabel.TabIndex = 2;
            this.PasswordLabel.Text = "Password";
            // 
            // UserNameLabel
            // 
            this.UserNameLabel.AutoSize = true;
            this.UserNameLabel.Location = new System.Drawing.Point(20, 45);
            this.UserNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.UserNameLabel.Name = "UserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(57, 13);
            this.UserNameLabel.TabIndex = 1;
            this.UserNameLabel.Text = "UserName";
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(20, 27);
            this.ServerLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(38, 13);
            this.ServerLabel.TabIndex = 0;
            this.ServerLabel.Text = "Server";
            // 
            // ViewModelGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1345, 717);
            this.Controls.Add(this.DBSettings);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ViewModelGen";
            this.Text = "MVC Code Helper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewModelGen_FormClosing);
            this.Load += new System.EventHandler(this.ViewModelGen_Load);
            this.DBSettings.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FKGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PKGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TableGrid)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl DBSettings;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button TestB;
        private System.Windows.Forms.TextBox PassTB;
        private System.Windows.Forms.TextBox UserTB;
        private System.Windows.Forms.TextBox ServeTB;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label UserNameLabel;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox DatabaseCB;
        private System.Windows.Forms.Label DatabaseLabel;
        private System.Windows.Forms.ComboBox TableCB;
        private System.Windows.Forms.Label TableNameLabel;
        private System.Windows.Forms.Button GenB;
        private System.Windows.Forms.RichTextBox CodeText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ViewModelNameTB;
        private System.Windows.Forms.CheckBox rowVersion;
        private System.Windows.Forms.CheckBox modifiedDate;
        private System.Windows.Forms.CheckBox createdDate;
        private System.Windows.Forms.CheckBox modifiedBy;
        private System.Windows.Forms.CheckBox createdBy;
        private System.Windows.Forms.Label IncludeLabel;
        private System.Windows.Forms.CheckBox DisplayAnn;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox StringLengthAnn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox HiddenInputAnn;
        private System.Windows.Forms.CheckBox RequiredAnn;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox RepoCodeText;
        private System.Windows.Forms.Button RepoBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RegionTB;
        private System.Windows.Forms.DataGridView TableGrid;
        private System.Windows.Forms.Button QueueBtn;
        private System.Windows.Forms.ComboBox TableNameRepo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView FKGV;
        private System.Windows.Forms.DataGridView PKGV;
        private System.Windows.Forms.RichTextBox ActionsResult;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ControllerNameTB;
        private System.Windows.Forms.TextBox RepoNameTB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox isPartial;
        private System.Windows.Forms.DataGridViewTextBoxColumn CTableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CRegionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CRepoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CController;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isPartialView;
    }
}