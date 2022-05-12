
namespace RoutechToFiveAxis
{
    partial class MainView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ToLarryBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ToGrantBtn = new System.Windows.Forms.Button();
            this.ToMikeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Image = global::RoutechToFiveAxis.Properties.Resources.cancel_update_24;
            this.button1.Name = "button1";
            this.button1.Click += new System.EventHandler(this.OnCloseClick);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ToLarryBtn
            // 
            resources.ApplyResources(this.ToLarryBtn, "ToLarryBtn");
            this.ToLarryBtn.Name = "ToLarryBtn";
            this.ToLarryBtn.UseVisualStyleBackColor = true;
            this.ToLarryBtn.Click += new System.EventHandler(this.ToLarryBtn_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ToGrantBtn
            // 
            resources.ApplyResources(this.ToGrantBtn, "ToGrantBtn");
            this.ToGrantBtn.Name = "ToGrantBtn";
            this.ToGrantBtn.UseVisualStyleBackColor = true;
            this.ToGrantBtn.Click += new System.EventHandler(this.ToGrantBtn_Click);
            // 
            // ToMikeBtn
            // 
            resources.ApplyResources(this.ToMikeBtn, "ToMikeBtn");
            this.ToMikeBtn.Name = "ToMikeBtn";
            this.ToMikeBtn.UseVisualStyleBackColor = true;
            this.ToMikeBtn.Click += new System.EventHandler(this.ToMikeBtn_Click);
            // 
            // MainView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ToMikeBtn);
            this.Controls.Add(this.ToGrantBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ToLarryBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "MainView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ToLarryBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ToGrantBtn;
        private System.Windows.Forms.Button ToMikeBtn;
    }
}