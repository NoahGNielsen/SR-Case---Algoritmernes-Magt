using System.Diagnostics;
using System.Text.Json;

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
            }
            Program.UpdateUserTagScore(Program.userId(), currentPostId, Program.getPostTime());

            // Assign the image to the PictureBox control
            picBox_feed.Image = postImage;
            UpdatePostUI(currentPostId);

            if (GlobalConfig.debugMode == true)
            {
                Debug.WriteLine("Debug Mode | Post Image Posted");
            }
        }

        private void UpdatePostUI(int postId)
        {
            try
            {
                string postsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\posts.json");
                if (!File.Exists(postsPath)) return;

                string json = File.ReadAllText(postsPath);
                var posts = JsonSerializer.Deserialize<List<Post>>(json);
                var post = posts?.FirstOrDefault(p => p.postId == postId);

                if (post == null) return;

                label_author.Text = post.title;
                rtb_description.Text = post.description;
                label_totalLikes.Text = $"👍 {post.likes}";
                label_totalComments.Text = $"💬 {post.comments}";
                label_totalShares.Text = $"🔁 {post.shares}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in UpdatePostUI: " + ex.Message);
            }

            // Reset btn's for next post
            btn_like.Enabled = true;
            btn_like.Text = "Like";
            btn_comments.Enabled = true;
            btn_comments.Text = "Comment";
            btn_share.Enabled = true;
            btn_share.Text = "Share";
        }

        private void btn_like_Click(object sender, EventArgs e)
        {
            Program.UpdateUserEngagement(Program.userId(), currentPostId, "Like");
            btn_like.Enabled = false;
            btn_like.Text = "Liked!";
        }

        private void btn_comments_Click(object sender, EventArgs e)
        {
            Program.UpdateUserEngagement(Program.userId(), currentPostId, "Comment");
            btn_comments.Enabled = false;
            btn_comments.Text = "Commented!";
        }

        private void btn_share_Click(object sender, EventArgs e)
        {
            Program.UpdateUserEngagement(Program.userId(), currentPostId, "Share");
            btn_share.Enabled = false;
            btn_share.Text = "Shared!";
        }
    }
}
