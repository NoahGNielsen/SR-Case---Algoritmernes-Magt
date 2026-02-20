namespace SR_Case___Algoritmernes_Magt
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
            txtBox_title = new TextBox();
            rtb_description = new RichTextBox();
            btn_uploadImage = new Button();
            btn_post = new Button();
            label_image = new Label();
            label_title = new Label();
            label_description = new Label();
            label_imagePath = new Label();
            rtb_tags = new RichTextBox();
            label_tags = new Label();
            label_separateNotice = new Label();
            SuspendLayout();
            // 
            // txtBox_title
            // 
            txtBox_title.Location = new Point(118, 57);
            txtBox_title.Name = "txtBox_title";
            txtBox_title.PlaceholderText = "I am a title...";
            txtBox_title.Size = new Size(323, 27);
            txtBox_title.TabIndex = 0;
            // 
            // rtb_description
            // 
            rtb_description.Location = new Point(118, 102);
            rtb_description.Name = "rtb_description";
            rtb_description.Size = new Size(323, 111);
            rtb_description.TabIndex = 1;
            rtb_description.Text = "";
            // 
            // btn_uploadImage
            // 
            btn_uploadImage.Location = new Point(118, 230);
            btn_uploadImage.Name = "btn_uploadImage";
            btn_uploadImage.Size = new Size(94, 29);
            btn_uploadImage.TabIndex = 2;
            btn_uploadImage.Text = "Upload Image";
            btn_uploadImage.UseVisualStyleBackColor = true;
            btn_uploadImage.Click += btn_uploadImage_Click;
            // 
            // btn_post
            // 
            btn_post.Location = new Point(118, 376);
            btn_post.Name = "btn_post";
            btn_post.Size = new Size(94, 29);
            btn_post.TabIndex = 3;
            btn_post.Text = "Post";
            btn_post.UseVisualStyleBackColor = true;
            btn_post.Click += btn_post_Click;
            // 
            // label_image
            // 
            label_image.AutoSize = true;
            label_image.Location = new Point(21, 234);
            label_image.Name = "label_image";
            label_image.Size = new Size(57, 20);
            label_image.TabIndex = 6;
            label_image.Text = "Image*";
            // 
            // label_title
            // 
            label_title.AutoSize = true;
            label_title.Location = new Point(21, 60);
            label_title.Name = "label_title";
            label_title.Size = new Size(44, 20);
            label_title.TabIndex = 4;
            label_title.Text = "Title*";
            // 
            // label_description
            // 
            label_description.AutoSize = true;
            label_description.Location = new Point(21, 105);
            label_description.Name = "label_description";
            label_description.Size = new Size(91, 20);
            label_description.TabIndex = 5;
            label_description.Text = "Description*";
            // 
            // label_imagePath
            // 
            label_imagePath.Location = new Point(227, 234);
            label_imagePath.Name = "label_imagePath";
            label_imagePath.Size = new Size(366, 20);
            label_imagePath.TabIndex = 7;
            // 
            // rtb_tags
            // 
            rtb_tags.Location = new Point(118, 280);
            rtb_tags.Name = "rtb_tags";
            rtb_tags.Size = new Size(323, 50);
            rtb_tags.TabIndex = 8;
            rtb_tags.Text = "";
            // 
            // label_tags
            // 
            label_tags.AutoSize = true;
            label_tags.Location = new Point(21, 280);
            label_tags.Name = "label_tags";
            label_tags.Size = new Size(44, 20);
            label_tags.TabIndex = 9;
            label_tags.Text = "Tags*";
            // 
            // label_separateNotice
            // 
            label_separateNotice.AutoSize = true;
            label_separateNotice.Font = new Font("Segoe UI", 7F);
            label_separateNotice.Location = new Point(118, 333);
            label_separateNotice.Name = "label_separateNotice";
            label_separateNotice.Size = new Size(161, 15);
            label_separateNotice.TabIndex = 10;
            label_separateNotice.Text = "Use commas to separate tags";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(605, 445);
            Controls.Add(label_separateNotice);
            Controls.Add(label_tags);
            Controls.Add(rtb_tags);
            Controls.Add(label_imagePath);
            Controls.Add(label_description);
            Controls.Add(label_title);
            Controls.Add(label_image);
            Controls.Add(btn_post);
            Controls.Add(btn_uploadImage);
            Controls.Add(rtb_description);
            Controls.Add(txtBox_title);
            Name = "Form2";
            Text = "Posting a post...";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtBox_title;
        private RichTextBox rtb_description;
        private Button btn_uploadImage;
        private Button btn_post;
        private Label label_image;
        private Label label_title;
        private Label label_description;
        private Label label_imagePath;
        private RichTextBox rtb_tags;
        private Label label_tags;
        private Label label_separateNotice;
    }
}