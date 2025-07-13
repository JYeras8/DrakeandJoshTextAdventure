namespace DrakeandJoshPart1;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
        writeTextBox = new System.Windows.Forms.TextBox();
        readTextBox = new System.Windows.Forms.TextBox();
        statTextBox = new System.Windows.Forms.TextBox();
        SuspendLayout();
        // 
        // writeTextBox
        // 
        writeTextBox.BackColor = System.Drawing.SystemColors.MenuText;
        writeTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
        writeTextBox.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)0));
        writeTextBox.ForeColor = System.Drawing.Color.Cyan;
        writeTextBox.Location = new System.Drawing.Point(0, 1014);
        writeTextBox.Name = "writeTextBox";
        writeTextBox.Size = new System.Drawing.Size(1904, 27);
        writeTextBox.TabIndex = 0;
        // 
        // readTextBox
        // 
        readTextBox.BackColor = System.Drawing.SystemColors.ControlText;
        readTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
        readTextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)0));
        readTextBox.ForeColor = System.Drawing.Color.Lime;
        readTextBox.Location = new System.Drawing.Point(0, 830);
        readTextBox.Multiline = true;
        readTextBox.Name = "readTextBox";
        readTextBox.ReadOnly = true;
        readTextBox.Size = new System.Drawing.Size(1904, 184);
        readTextBox.TabIndex = 1;
        // 
        // statTextBox
        // 
        statTextBox.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
        statTextBox.Dock = System.Windows.Forms.DockStyle.Top;
        statTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)0));
        statTextBox.ForeColor = System.Drawing.Color.Lime;
        statTextBox.Location = new System.Drawing.Point(0, 0);
        statTextBox.Multiline = true;
        statTextBox.Name = "statTextBox";
        statTextBox.ReadOnly = true;
        statTextBox.Size = new System.Drawing.Size(1904, 824);
        statTextBox.TabIndex = 2;
        statTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.ActiveCaptionText;
        ClientSize = new System.Drawing.Size(1904, 1041);
        Controls.Add(statTextBox);
        Controls.Add(readTextBox);
        Controls.Add(writeTextBox);
        Text = "Form1";
        Load += Form1_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.TextBox statTextBox;

    private System.Windows.Forms.TextBox readTextBox;

    private System.Windows.Forms.TextBox writeTextBox;
    private System.Windows.Forms.Label TextBox;

    #endregion
}