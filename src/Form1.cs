namespace SR_Case___Algoritmernes_Magt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_newPost_Click(object sender, EventArgs e)
        {
            Form2 newPostForm = new Form2();
            newPostForm.ShowDialog();
        }
    }
}
