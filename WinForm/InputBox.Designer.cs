namespace CSharpHelper.WinForm
{
	partial class InputBox
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.Btn_Cancel = new System.Windows.Forms.Button();
			this.Btn_Ok = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.80315F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.19685F));
			this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.Btn_Cancel, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.Btn_Ok, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.875F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.125F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 160);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// textBox1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.textBox1, 2);
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.textBox1.Location = new System.Drawing.Point(3, 49);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(502, 20);
			this.textBox1.TabIndex = 0;
			// 
			// Btn_Cancel
			// 
			this.Btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Btn_Cancel.Location = new System.Drawing.Point(430, 136);
			this.Btn_Cancel.Name = "Btn_Cancel";
			this.Btn_Cancel.Size = new System.Drawing.Size(75, 21);
			this.Btn_Cancel.TabIndex = 1;
			this.Btn_Cancel.Text = "Annuler";
			this.Btn_Cancel.UseVisualStyleBackColor = true;
			// 
			// Btn_Ok
			// 
			this.Btn_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Btn_Ok.Location = new System.Drawing.Point(301, 136);
			this.Btn_Ok.Name = "Btn_Ok";
			this.Btn_Ok.Size = new System.Drawing.Size(75, 21);
			this.Btn_Ok.TabIndex = 2;
			this.Btn_Ok.Text = "OK";
			this.Btn_Ok.UseVisualStyleBackColor = true;
			this.Btn_Ok.Click += new System.EventHandler(this.Btn_Ok_Click);
			// 
			// InputBox
			// 
			this.AcceptButton = this.Btn_Ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.Btn_Cancel;
			this.ClientSize = new System.Drawing.Size(508, 160);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "InputBox";
			this.Text = "InputBox";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button Btn_Cancel;
		private System.Windows.Forms.Button Btn_Ok;
	}
}