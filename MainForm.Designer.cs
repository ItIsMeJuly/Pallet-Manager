
namespace Tracker
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.btnNewQuerry = new System.Windows.Forms.Button();
            this.btnFindQuery = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 0;
            // 
            // btnNewQuerry
            // 
            this.btnNewQuerry.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewQuerry.Location = new System.Drawing.Point(120, 272);
            this.btnNewQuerry.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNewQuerry.Name = "btnNewQuerry";
            this.btnNewQuerry.Size = new System.Drawing.Size(186, 78);
            this.btnNewQuerry.TabIndex = 1;
            this.btnNewQuerry.Text = "New Publication";
            this.btnNewQuerry.UseVisualStyleBackColor = true;
            this.btnNewQuerry.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnFindQuery
            // 
            this.btnFindQuery.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFindQuery.Location = new System.Drawing.Point(499, 272);
            this.btnFindQuery.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnFindQuery.Name = "btnFindQuery";
            this.btnFindQuery.Size = new System.Drawing.Size(186, 78);
            this.btnFindQuery.TabIndex = 2;
            this.btnFindQuery.Text = "Show Publications";
            this.btnFindQuery.UseVisualStyleBackColor = true;
            this.btnFindQuery.Click += new System.EventHandler(this.btnFindQuery_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(153, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(581, 69);
            this.label2.TabIndex = 3;
            this.label2.Text = "BST Pallet Manager";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(779, 455);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFindQuery);
            this.Controls.Add(this.btnNewQuerry);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "BST";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNewQuerry;
        private System.Windows.Forms.Button btnFindQuery;
        private System.Windows.Forms.Label label2;
    }
}

