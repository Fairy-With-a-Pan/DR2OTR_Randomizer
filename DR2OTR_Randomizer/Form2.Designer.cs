namespace DR2OTR_Randomizer
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            checkBox1 = new CheckBox();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            checkBox2 = new CheckBox();
            checkBox3 = new CheckBox();
            SuspendLayout();
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(12, 87);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(147, 19);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Allow Random Spawns";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(12, 137);
            button1.Name = "button1";
            button1.Size = new Size(100, 50);
            button1.TabIndex = 1;
            button1.Text = "Ok";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(272, 137);
            button2.Name = "button2";
            button2.Size = new Size(100, 50);
            button2.TabIndex = 2;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(360, 75);
            label1.TabIndex = 3;
            label1.Text = resources.GetString("label1.Text");
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(23, 112);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(172, 19);
            checkBox2.TabIndex = 4;
            checkBox2.Text = "Random Keys (Vehicle keys)";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(275, 87);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(97, 19);
            checkBox3.TabIndex = 5;
            checkBox3.Text = "Keep Vehicles";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 201);
            ControlBox = false;
            Controls.Add(checkBox3);
            Controls.Add(checkBox2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(checkBox1);
            Name = "Form2";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox checkBox1;
        private Button button1;
        private Button button2;
        private Label label1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
    }
}