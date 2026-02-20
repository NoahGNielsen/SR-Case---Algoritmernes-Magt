using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace SR_Case___Algoritmernes_Magt
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btn_uploadImage_Click(object sender, EventArgs e)
        {
            string imagePath = Program.imageUploader();
            label_imagePath.Text = imagePath;
        }

        private void btn_post_Click(object sender, EventArgs e)
        {
            string title = txtBox_title.Text;
            string description = rtb_description.Text;
            string imagePath = label_imagePath.Text;
            var Tags = new List<string>(rtb_tags.Text.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries));

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(imagePath) || Tags.Count == 0)
            {
                MessageBox.Show("Please fill in all fields and upload an image before posting.");
                return;
            }

            bool postCreated = Program.CreateNewPost(title, description, imagePath, Tags);

            if (postCreated)
            {
                MessageBox.Show("Post created successfully!");
                this.Close();
            } else if (postCreated)
            {
                MessageBox.Show("Failed to create post. Please check your input and try again.");
            }
        }
    }
}
