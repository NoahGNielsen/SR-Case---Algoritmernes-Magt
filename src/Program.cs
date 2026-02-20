using System;
using System.Text.Json;
using System.IO;
using System.Windows.Forms;

namespace SR_Case___Algoritmernes_Magt
{
    public static class GlobalConfig //settings
    {
        public readonly static bool feedModePersonalization = true; //default: true
        public readonly static bool debugMode = false; //default: false
    }

    public class Post //define the class for a post
    {
        public int postId { get; set; }
        public required string title { get; set; }
        public required string description { get; set; }
        public required string imagePath { get; set; }
        public int likes { get; set; }
        public int comments { get; set; }
        public int shares { get; set; }
        public int engagement { get; set; }
        public required List<string> tags { get; set; }
        public DateTime PostDate { get; set; }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Windows Forms application code
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }


        /*
         * Calculates the value of a post that is Personally Relevant to the user 
         */
        private static readonly Random _random = new Random();
        static long postValue(int PTIS, int likes, int comments, int shares, int postEngagement, DateTime postDate)
        {
            //Weights
            double weightLikes = 1; //default: 1
            double weightComments = 3; //default: 3
            double weightShares = 5; //default: 5
            double weightGravity = 1.3; //default: 1.8
            double weightFinalScore = 1000; //default: 1000


            // Validate input values
            if (PTIS < 0 || PTIS > 100 || likes < 0 || comments < 0 || shares < 0 || postEngagement < 0 || postEngagement > 1000)
            {
                // If any input value is out of the expected range, return -1 to indicate an error
                // And log invalid input values for debugging
                Console.WriteLine("Invalid input values.");
                if (GlobalConfig.debugMode) {
                    Console.WriteLine("Debug Mode | PTIS: " + PTIS + " Likes: " + likes + " Comments: " + comments + " Shares: " + shares + " Post Engagement Value: " + postEngagement);
                }
                return -1;
            }

            //calculate the factors for the post value calculation
            bool isDiscoveryRoll = _random.Next(1, 14) == 1;
            double socialValue = Math.Log10((likes* weightLikes) + (comments * weightComments) + (shares * weightShares) + 1);
            double interestValue = 1 + (PTIS / 50.0); // Normalize PTIS to a value between 1 and 2
            double engagementScore = Math.Log10(postEngagement + 1) * 1.5;
            double daysSincePost = (DateTime.Now - postDate).TotalDays;
            double gravity = Math.Pow(daysSincePost + 1, weightGravity);
            if (GlobalConfig.debugMode) {
                Console.WriteLine("Debug Mode | SocialValue: " + socialValue + " Post Engagement: " + postEngagement + " InterestValue: " + interestValue + " Gravity: " + gravity);
            }


            /*
             * 
             *The algorithm to calculate the post value
             *
             */

            //checks if feed personalization is active
            if (GlobalConfig.feedModePersonalization == true && !isDiscoveryRoll) 
            {
                // Calculate the Final Post Score with personalization
                double FPS = (((socialValue + engagementScore) * interestValue) / gravity) * weightFinalScore;
                if (GlobalConfig.debugMode) {
                    Console.WriteLine("Debug Mode | FinalPostScore: " + FPS);
                }
                return (long)FPS;
            } 
            else 
            {
                // Calculate the Final Post Score without personalization
                double FPS = ((socialValue + engagementScore) / gravity) * weightFinalScore;
                if (GlobalConfig.debugMode) {
                    Console.WriteLine("Debug Mode | FinalPostScore: " + FPS);
                }
                return (long)FPS;
            }
        }

        public static void CreateNewPost(string title, string description, string imageFilePath, List<string> tags)
        {
            // Define the file paths
            string postsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\posts.json");
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\assets\\images\\");

            // Extract the file name and extension from the provided image file path
            string extension = Path.GetExtension(imageFilePath);

            // Takes existing posts and deserializes them
            List<Post> posts = new List<Post>();
            if (File.Exists(postsPath))
            {
                string existingJson = File.ReadAllText(postsPath);
                posts = JsonSerializer.Deserialize<List<Post>>(existingJson) ?? new List<Post>();
            }

            //creates a new id, takes highest existing id and adds 1
            int newId = posts.Count > 0 ? posts.Max(p => p.postId) + 1 : 1;



            string newImageName = newId + extension;
            string fullPathToImage = Path.Combine(imagesFolder, newImageName);

            if (File.Exists(imageFilePath))
            {
                // Ensure directory exists and copy the file
                Directory.CreateDirectory(imagesFolder);
                File.Copy(imageFilePath, fullPathToImage, true);
            } else
            {
                if (GlobalConfig.debugMode == true)
                {
                    Console.WriteLine("Debug Mode | Image file not found at path: " + imageFilePath + "\nFunction has stopped to prevent id numbers to get fucked up");
                }
                return;
            }



                //Creates a new post in the list
                Post newPost = new Post
                {
                    postId = newId,
                    title = title,
                    description = description,
                    imagePath = $"data/assets/images/{newId}{extension}",
                    likes = 0,
                    comments = 0,
                    shares = 0,
                    engagement = 0,
                    tags = tags,
                    PostDate = DateTime.Now
                };


            // save the new post 
            posts.Add(newPost); // Add the new post to the list
            var options = new JsonSerializerOptions { WriteIndented = true }; // For better readability of the JSON file
            string updatedJson = JsonSerializer.Serialize(posts, options); // Serialize the updated list back to JSON

            File.WriteAllText(postsPath, updatedJson); // Save it to the file

            Console.WriteLine("Successfully created Post, ID: " + newId + ", Title: "+ title);
        }

        public static string imageUploader()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            //config for the file dialog
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png"; // Limit to jpeg and png files
            fileDialog.Title = "Select Images to Upload";
            fileDialog.Multiselect = false; // Only one image


            if (fileDialog.ShowDialog() == DialogResult.OK) //if a user selects a file and clicks "OK"
            {
                if (GlobalConfig.debugMode)
                {
                    Console.WriteLine("Debug Mode | Selected file: " + fileDialog.FileName);
                }
                return fileDialog.FileName;
            }
            else //if the user cancels the file selection
            {
                Console.WriteLine("No file selected.");
                return "No file selected or something went wrong";
            }
        }
    }
}