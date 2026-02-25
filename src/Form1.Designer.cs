namespace SR_Case___Algoritmernes_Magt
{
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_skip = new Button();
            btn_like = new Button();
            btn_share = new Button();
            rtb_description = new RichTextBox();
            label_author = new Label();
            btn_comments = new Button();
            label_totalShares = new Label();
            label_totalComments = new Label();
            label_totalLikes = new Label();
            picBox_feed = new PictureBox();
            btn_newPost = new Button();
            ((System.ComponentModel.ISupportInitialize)picBox_feed).BeginInit();
            SuspendLayout();
            // 
            // btn_skip
            // 
            btn_skip.Location = new Point(191, 714);
            btn_skip.Margin = new Padding(3, 2, 3, 2);
            btn_skip.Name = "btn_skip";
            btn_skip.Size = new Size(232, 22);
            btn_skip.TabIndex = 1;
            btn_skip.Text = "Next";
            btn_skip.UseVisualStyleBackColor = true;
            btn_skip.Click += btn_skip_Click;
            // 
            // btn_like
            // 
            btn_like.Location = new Point(485, 459);
            btn_like.Margin = new Padding(3, 2, 3, 2);
            btn_like.Name = "btn_like";
            btn_like.Size = new Size(82, 22);
            btn_like.TabIndex = 2;
            btn_like.Text = "Like";
            btn_like.UseVisualStyleBackColor = true;
            btn_like.Click += btn_like_Click;
            // 
            // btn_share
            // 
            btn_share.Location = new Point(485, 568);
            btn_share.Margin = new Padding(3, 2, 3, 2);
            btn_share.Name = "btn_share";
            btn_share.Size = new Size(82, 22);
            btn_share.TabIndex = 4;
            btn_share.Text = "Share";
            btn_share.UseVisualStyleBackColor = true;
            btn_share.Click += btn_share_Click;
            // 
            // rtb_description
            // 
            rtb_description.Location = new Point(12, 664);
            rtb_description.Margin = new Padding(3, 2, 3, 2);
            rtb_description.Name = "rtb_description";
            rtb_description.Size = new Size(478, 46);
            rtb_description.TabIndex = 6;
            rtb_description.Text = "Place Holder";
            // 
            // label_author
            // 
            label_author.BackColor = Color.Transparent;
            label_author.Font = new Font("Segoe UI", 13F);
            label_author.Location = new Point(12, 641);
            label_author.Name = "label_author";
            label_author.Size = new Size(478, 21);
            label_author.TabIndex = 7;
            label_author.Text = "Place Holder";
            label_author.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btn_comments
            // 
            btn_comments.Location = new Point(485, 513);
            btn_comments.Margin = new Padding(3, 2, 3, 2);
            btn_comments.Name = "btn_comments";
            btn_comments.Size = new Size(82, 22);
            btn_comments.TabIndex = 8;
            btn_comments.Text = "Comments";
            btn_comments.UseVisualStyleBackColor = true;
            btn_comments.Click += btn_comments_Click;
            // 
            // label_totalShares
            // 
            label_totalShares.Location = new Point(485, 547);
            label_totalShares.Name = "label_totalShares";
            label_totalShares.Size = new Size(82, 19);
            label_totalShares.TabIndex = 9;
            label_totalShares.Text = "Place Holder";
            label_totalShares.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_totalComments
            // 
            label_totalComments.Location = new Point(485, 492);
            label_totalComments.Name = "label_totalComments";
            label_totalComments.Size = new Size(82, 19);
            label_totalComments.TabIndex = 10;
            label_totalComments.Text = "Place Holder";
            label_totalComments.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label_totalLikes
            // 
            label_totalLikes.Location = new Point(485, 438);
            label_totalLikes.Name = "label_totalLikes";
            label_totalLikes.Size = new Size(82, 19);
            label_totalLikes.TabIndex = 11;
            label_totalLikes.Text = "Place Holder";
            label_totalLikes.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picBox_feed
            // 
            picBox_feed.Cursor = Cursors.No;
            picBox_feed.Location = new Point(1, -1);
            picBox_feed.Margin = new Padding(3, 2, 3, 2);
            picBox_feed.Name = "picBox_feed";
            picBox_feed.Size = new Size(577, 640);
            picBox_feed.SizeMode = PictureBoxSizeMode.Zoom;
            picBox_feed.TabIndex = 12;
            picBox_feed.TabStop = false;
            // 
            // btn_newPost
            // 
            btn_newPost.Location = new Point(495, 9);
            btn_newPost.Margin = new Padding(3, 2, 3, 2);
            btn_newPost.Name = "btn_newPost";
            btn_newPost.Size = new Size(72, 22);
            btn_newPost.TabIndex = 14;
            btn_newPost.Text = "New Post";
            btn_newPost.UseVisualStyleBackColor = true;
            btn_newPost.Click += btn_newPost_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(578, 738);
            Controls.Add(btn_newPost);
            Controls.Add(label_totalLikes);
            Controls.Add(label_totalComments);
            Controls.Add(label_totalShares);
            Controls.Add(btn_comments);
            Controls.Add(label_author);
            Controls.Add(rtb_description);
            Controls.Add(btn_share);
            Controls.Add(btn_like);
            Controls.Add(btn_skip);
            Controls.Add(picBox_feed);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "User UI";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)picBox_feed).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btn_skip;
        private Button btn_like;
        private Button btn_share;
        private RichTextBox rtb_description;
        private Label label_author;
        private Button btn_comments;
        private Label label_totalShares;
        private Label label_totalComments;
        private Label label_totalLikes;
        private PictureBox picBox_feed;
        private Button btn_newPost;
    }
}
