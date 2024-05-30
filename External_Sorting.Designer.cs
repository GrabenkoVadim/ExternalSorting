using System;
using System.Windows.Forms;

namespace KP
{
    partial class ExternalSorting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExternalSorting));
            this.Tittle = new System.Windows.Forms.Label();
            this.Number_Of_Elemens_label = new System.Windows.Forms.Label();
            this.Generation_Button = new System.Windows.Forms.Button();
            this.Full_Input_file_button = new System.Windows.Forms.Button();
            this.Natural_Merge_Button = new System.Windows.Forms.RadioButton();
            this.Balanced_Multiway_Merge_Button = new System.Windows.Forms.RadioButton();
            this.Polyphase_Sort_Button = new System.Windows.Forms.RadioButton();
            this.Full_Input_file_label = new System.Windows.Forms.Label();
            this.Sort_button = new System.Windows.Forms.Button();
            this.Full_Output_File_Label = new System.Windows.Forms.Label();
            this.Full_Output_File_Button = new System.Windows.Forms.Button();
            this.Input_list = new System.Windows.Forms.RichTextBox();
            this.Output_list = new System.Windows.Forms.RichTextBox();
            this.NumberOfAccessesList = new System.Windows.Forms.TextBox();
            this.NumberOfAccessesLabel = new System.Windows.Forms.Label();
            this.From_Label = new System.Windows.Forms.Label();
            this.To_Label = new System.Windows.Forms.Label();
            this.From_List = new System.Windows.Forms.TextBox();
            this.To_List = new System.Windows.Forms.TextBox();
            this.Elements_Input_Field = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Tittle
            // 
            resources.ApplyResources(this.Tittle, "Tittle");
            this.Tittle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Tittle.Name = "Tittle";
            this.Tittle.Click += new System.EventHandler(this.label1_Click);
            // 
            // Number_Of_Elemens_label
            // 
            this.Number_Of_Elemens_label.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.Number_Of_Elemens_label, "Number_Of_Elemens_label");
            this.Number_Of_Elemens_label.ForeColor = System.Drawing.SystemColors.Window;
            this.Number_Of_Elemens_label.Name = "Number_Of_Elemens_label";
            this.Number_Of_Elemens_label.Click += new System.EventHandler(this.label2_Click);
            // 
            // Generation_Button
            // 
            resources.ApplyResources(this.Generation_Button, "Generation_Button");
            this.Generation_Button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Generation_Button.Name = "Generation_Button";
            this.Generation_Button.UseVisualStyleBackColor = true;
            this.Generation_Button.Click += new System.EventHandler(this.Generation_Button_Click);
            // 
            // Full_Input_file_button
            // 
            resources.ApplyResources(this.Full_Input_file_button, "Full_Input_file_button");
            this.Full_Input_file_button.ForeColor = System.Drawing.SystemColors.InfoText;
            this.Full_Input_file_button.Name = "Full_Input_file_button";
            this.Full_Input_file_button.UseVisualStyleBackColor = true;
            this.Full_Input_file_button.Click += new System.EventHandler(this.Open_Input_File_Button_Click);
            // 
            // Natural_Merge_Button
            // 
            resources.ApplyResources(this.Natural_Merge_Button, "Natural_Merge_Button");
            this.Natural_Merge_Button.Name = "Natural_Merge_Button";
            this.Natural_Merge_Button.TabStop = true;
            this.Natural_Merge_Button.UseVisualStyleBackColor = true;
            // 
            // Balanced_Multiway_Merge_Button
            // 
            resources.ApplyResources(this.Balanced_Multiway_Merge_Button, "Balanced_Multiway_Merge_Button");
            this.Balanced_Multiway_Merge_Button.Name = "Balanced_Multiway_Merge_Button";
            this.Balanced_Multiway_Merge_Button.TabStop = true;
            this.Balanced_Multiway_Merge_Button.UseVisualStyleBackColor = true;
            // 
            // Polyphase_Sort_Button
            // 
            resources.ApplyResources(this.Polyphase_Sort_Button, "Polyphase_Sort_Button");
            this.Polyphase_Sort_Button.Name = "Polyphase_Sort_Button";
            this.Polyphase_Sort_Button.TabStop = true;
            this.Polyphase_Sort_Button.UseVisualStyleBackColor = true;
            // 
            // Full_Input_file_label
            // 
            resources.ApplyResources(this.Full_Input_file_label, "Full_Input_file_label");
            this.Full_Input_file_label.Name = "Full_Input_file_label";
            // 
            // Sort_button
            // 
            resources.ApplyResources(this.Sort_button, "Sort_button");
            this.Sort_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Sort_button.Name = "Sort_button";
            this.Sort_button.UseVisualStyleBackColor = true;
            this.Sort_button.Click += new System.EventHandler(this.Sort_Button_Click);
            // 
            // Full_Output_File_Label
            // 
            resources.ApplyResources(this.Full_Output_File_Label, "Full_Output_File_Label");
            this.Full_Output_File_Label.Name = "Full_Output_File_Label";
            // 
            // Full_Output_File_Button
            // 
            resources.ApplyResources(this.Full_Output_File_Button, "Full_Output_File_Button");
            this.Full_Output_File_Button.ForeColor = System.Drawing.SystemColors.InfoText;
            this.Full_Output_File_Button.Name = "Full_Output_File_Button";
            this.Full_Output_File_Button.UseVisualStyleBackColor = true;
            this.Full_Output_File_Button.Click += new System.EventHandler(this.Open_Output_File_Button_Click);
            // 
            // Input_list
            // 
            resources.ApplyResources(this.Input_list, "Input_list");
            this.Input_list.Name = "Input_list";
            this.Input_list.TextChanged += new System.EventHandler(this.Input_list_TextChanged);
            // 
            // Output_list
            // 
            resources.ApplyResources(this.Output_list, "Output_list");
            this.Output_list.Name = "Output_list";
            // 
            // NumberOfAccessesList
            // 
            resources.ApplyResources(this.NumberOfAccessesList, "NumberOfAccessesList");
            this.NumberOfAccessesList.Name = "NumberOfAccessesList";
            // 
            // NumberOfAccessesLabel
            // 
            resources.ApplyResources(this.NumberOfAccessesLabel, "NumberOfAccessesLabel");
            this.NumberOfAccessesLabel.Name = "NumberOfAccessesLabel";
            this.NumberOfAccessesLabel.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // From_Label
            // 
            resources.ApplyResources(this.From_Label, "From_Label");
            this.From_Label.Name = "From_Label";
            // 
            // To_Label
            // 
            resources.ApplyResources(this.To_Label, "To_Label");
            this.To_Label.Name = "To_Label";
            // 
            // From_List
            // 
            resources.ApplyResources(this.From_List, "From_List");
            this.From_List.Name = "From_List";
            // 
            // To_List
            // 
            resources.ApplyResources(this.To_List, "To_List");
            this.To_List.Name = "To_List";
            // 
            // Elements_Input_Field
            // 
            resources.ApplyResources(this.Elements_Input_Field, "Elements_Input_Field");
            this.Elements_Input_Field.Name = "Elements_Input_Field";
            // 
            // ExternalSorting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.Elements_Input_Field);
            this.Controls.Add(this.To_List);
            this.Controls.Add(this.From_List);
            this.Controls.Add(this.To_Label);
            this.Controls.Add(this.From_Label);
            this.Controls.Add(this.NumberOfAccessesLabel);
            this.Controls.Add(this.NumberOfAccessesList);
            this.Controls.Add(this.Output_list);
            this.Controls.Add(this.Input_list);
            this.Controls.Add(this.Full_Output_File_Button);
            this.Controls.Add(this.Full_Output_File_Label);
            this.Controls.Add(this.Sort_button);
            this.Controls.Add(this.Full_Input_file_label);
            this.Controls.Add(this.Polyphase_Sort_Button);
            this.Controls.Add(this.Balanced_Multiway_Merge_Button);
            this.Controls.Add(this.Natural_Merge_Button);
            this.Controls.Add(this.Full_Input_file_button);
            this.Controls.Add(this.Generation_Button);
            this.Controls.Add(this.Number_Of_Elemens_label);
            this.Controls.Add(this.Tittle);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "ExternalSorting";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox Elements_Input_Field;

        private System.Windows.Forms.TextBox To_List;
        
        private System.Windows.Forms.TextBox From_List;
        
        private System.Windows.Forms.TextBox NumberOfAccessesList;

        private System.Windows.Forms.RichTextBox Input_list;
        
        private System.Windows.Forms.RichTextBox Output_list;
        
        private System.Windows.Forms.Label From_Label;
        
        private System.Windows.Forms.Label To_Label;
        
        private System.Windows.Forms.Label Number_Of_Elemens_label;

        private System.Windows.Forms.Label NumberOfAccessesLabel;

        private System.Windows.Forms.Label Full_Input_file_label;
        
        private System.Windows.Forms.Label Tittle;

        private System.Windows.Forms.Label Full_Output_File_Label;
        
        private System.Windows.Forms.RadioButton Natural_Merge_Button;
        
        private System.Windows.Forms.RadioButton Balanced_Multiway_Merge_Button;
        
        private System.Windows.Forms.RadioButton Polyphase_Sort_Button;
        
        private System.Windows.Forms.Button Full_Output_File_Button;
        
        private System.Windows.Forms.Button Generation_Button;
        
        private System.Windows.Forms.Button Sort_button;
        
        private System.Windows.Forms.Button Full_Input_file_button;

        #endregion
    }
}