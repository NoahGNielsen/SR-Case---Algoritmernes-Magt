using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;

namespace SR_Case___Algoritmernes_Magt
{

    /*To-DO
     * - The first few post shoud be random to get a good variety of posts in the beginning, before the algorithm has enough data to personalize the feed
     * - Bug Fix: If there is no data in the posts.json file, the app crashes. This is because the deserialization process fails when it encounters an empty file.
     */
    public static class GlobalConfig //settings
    {
        public readonly static bool feedModePersonalization = true; //default: true
        public readonly static bool debugMode = false; //default: false
        public readonly static int watchHistorySize = 10;
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
            startUp();
        }

















        static void startUp()
        {
            // Check if the posts.json, user.json and the assets folder exist, if not creates them
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\posts.json")))
            {
                File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\posts.json"));
            }

            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\users.json")))
            {
                File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\users.json"));
            }

            //checks if the images folder exists, if not create it
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\assets\\images\\"));

            return;
        }

        static int userId ()//stub
        {
            //This is gonna return the current user's userId
            return 1;
        
        }

        static int PTIS(int userId, int postId) //stub
        {
            /*
            // 1. Setup Paths
            string usersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\users.json");
            string postsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\posts.json");

            try
            {
                // 2. Read and Parse JSON
                var users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(usersPath));
                var posts = JsonSerializer.Deserialize<List<Post>>(File.ReadAllText(postsPath));

                // 3. Find the specific user and post
                var user = users.FirstOrDefault(u => u.userId == userId);
                var post = posts.FirstOrDefault(p => p.postId == postId);

                if (user == null || post == null || post.tags == null || post.tags.Count == 0)
                    return 0;

                int totalScore = 0;
                int matchedTagsCount = 0;

                // 4. Cross-reference tags (Case-Insensitive)
                foreach (var postTag in post.tags)
                {
                    // ToLower() and Trim() to handle the " Bilka" spacing in your JSON
                    string cleanPostTag = postTag.Trim().ToLower();

                    var match = user.pitsTags.FirstOrDefault(ut => ut.tag.ToLower() == cleanPostTag);

                    if (match != null)
                    {
                        totalScore += match.score;
                        matchedTagsCount++;
                    }
                }

                // 5. Calculate Average
                // If no tags matched, return 0 to avoid division by zero
                if (post.tags.Count == 0) return 0;

                // Your requirement: Divide total score by the amount of tags on the post
                return totalScore / post.tags.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing PTIS: {ex.Message}");
                return -1;
            }
            */
        }

        public static int requstNewPostToFeed(int userId) //stub
        {
            // This is gonna return the postId to next post
            // This is a "organizer" you know, like does all the handeling of requesting a new post to the feed.
            return -1;
        }

        static void displayPost(int postId) //stub
        {
            // This function would be responsible for displaying the post in the feed.
            return;
        }

        public static void likePost(int postId, int userId) //stub
        {
            // This function would handle the logic for when a user likes a post, including updating the post's like count and the user's interaction history.
            return;
        }

        public static void commentOnPost(int postId, int userId) //stub
        {
            // This function would handle the logic for when a user comments on a post, including updating the post's comment count and the user's interaction history.
            return;
        }

        public static void sharePost(int postId, int userId) //stub
        {
            // This function would handle the logic for when a user shares a post, including updating the post's share count and the user's interaction history.
            return;
        }

        /*
         * Calculates the value of a post that is Personally Relevant to the user 
         */
        static readonly Random _random = new Random(); // Creates a random number for the discovery roll
        static long postValue(bool newUser, int PTIS, int likes, int comments, int shares, int postEngagement, DateTime postDate)
        {
            //Weights
            double weightLikes = 1; //default: 1
            double weightComments = 3; //default: 3
            double weightShares = 5; //default: 5
            double weightGravity = 1.3; //default: 1.3
            double weightFinalScore = 1000; //default: 1000


            // Validate input values
            if (PTIS <= 0 || PTIS >= 100 || likes <= 0 || comments <= 0 || shares <= 0 || postEngagement <= 0 || postEngagement >= 1000)
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
            bool isDiscoveryRoll = _random.Next(1, 14) == 1; // 1 in 13 chance for discovery roll
            double socialValue = Math.Log10((likes* weightLikes) + (comments * weightComments) + (shares * weightShares) + 1);
            double interestValue = 1 + (PTIS / 50.0); // Normalize PTIS to a value between 1 and 2
            double engagementScore = Math.Log10(postEngagement + 1) * 1.5;
            double daysSincePost = (DateTime.Now - postDate).TotalDays;
            double gravity = Math.Pow(daysSincePost + 1, weightGravity);
            if (GlobalConfig.debugMode) {
                Console.WriteLine("Debug Mode | SocialValue: " + socialValue + " Post Engagement: " + postEngagement + " InterestValue: " + interestValue + " Gravity: " + gravity);
            }


            /*
             *THE algorithm
             */

            //checks if feed personalization is active
            if (GlobalConfig.feedModePersonalization == true && !isDiscoveryRoll && !newUser) 
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

        public static bool CreateNewPost(string title, string description, string imageFilePath, List<string> tags)
        {
            // Define the file paths
            string postsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\posts.json");
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\assets\\images\\");

            // Extract the file name and extension from the provided image file path
            string extension = Path.GetExtension(imageFilePath);

            // Takes existing posts and deserializes them

            // There is a bug where if no data is in the posts.json file, it will crash.
            List<Post> posts = new List<Post>();
            if (File.Exists(postsPath) && postsPath.Length != 0)
            {
                string existingJson = File.ReadAllText(postsPath);
                posts = JsonSerializer.Deserialize<List<Post>>(existingJson) ?? new List<Post>();
            }

            // New id, takes highest existing id and adds 1
            int newId = posts.Count > 0 ? posts.Max(p => p.postId) + 1 : 1;


            // Define the path for the new image
            string fullPathToImage = Path.Combine(imagesFolder, newId + extension);

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
                MessageBox.Show("Something went wrong there...");
                return false;
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
            return true;
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