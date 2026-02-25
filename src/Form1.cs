using System.Diagnostics;

namespace SR_Case___Algoritmernes_Magt
{
    public partial class Form1 : Form
    {
        private int currentPostId = -1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btn_skip_Click(this, EventArgs.Empty);
        }

        private void btn_newPost_Click(object sender, EventArgs e)
        {
            Form2 newPostForm = new Form2();
            newPostForm.ShowDialog();
        }

        private void btn_skip_Click(object sender, EventArgs e)
        {
            currentPostId = Program.requstNewPostToFeed(Program.userId()); ;
            if (currentPostId <= 0)
            {
                if (GlobalConfig.debugMode == true)
                {
                    Debug.WriteLine("Debug Mode | nextPost value: " + currentPostId);
                }
                MessageBox.Show("Error, try again or restart...");
                return;
            }

            Image postImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"..\\..\\..\\..\\data\\assets\\images\\{currentPostId}.jpg"));

            // Check if an image is rotated, and if so, rotate it to the correct orientation
            // Check if the image contains the EXIF property (0x0112) - StackOverflow
            if (postImage.PropertyIdList.Contains(0x0112))
            {
                var prop = postImage.GetPropertyItem(0x0112);

                // Check if the Value array exists
                if (prop != null && prop.Value != null && prop.Value.Length >= 2)
                {
                    int rotationValue = BitConverter.ToUInt16(prop.Value, 0);

                    // Rotate the image based on the rotation value
                    switch (rotationValue)
                    {
                        case 3: postImage.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                        case 6: postImage.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                        case 8: postImage.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
                    }
                }
                Program.UpdateUserTagScore(Program.userId(), currentPostId, Program.getPostTime());
            }

            // Assign the image to the PictureBox control
            picBox_feed.Image = postImage;

            if (GlobalConfig.debugMode == true)
            {
                Debug.WriteLine("Debug Mode | Post Image Posted");
            }
        }

        private void btn_like_Click(object sender, EventArgs e)
        {
            Program.UpdateUserEngagement(Program.userId(), currentPostId, "Like");
        }

        private void btn_comments_Click(object sender, EventArgs e)
        {
            Program.UpdateUserEngagement(Program.userId(), currentPostId, "Comment");
        }

        private void btn_share_Click(object sender, EventArgs e)
        {
            Program.UpdateUserEngagement(Program.userId(), currentPostId, "Share");
        }
    }
}
