using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;

namespace SR_Case___Algoritmernes_Magt
{

    public static class GlobalConfig //settings
    {
        public readonly static bool feedModePersonalization = true; //default: true
        public readonly static bool debugMode = true; //default: false
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

        public static int userId()//stub
        {
            //This is gonna return the current user's userId
            return 1;
        
        }

        static int getPTIS(int userId, int postId)
        {
            string usersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\users.json");
            string postsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\posts.json");

            if (!File.Exists(usersPath) || !File.Exists(postsPath))
            {
                return 0;
            }
            /*
             * Stack Overflow
             */
            using JsonDocument usersDoc = JsonDocument.Parse(File.ReadAllText(usersPath));
            using JsonDocument postsDoc = JsonDocument.Parse(File.ReadAllText(postsPath));


            // Finder den aktuelle bruger og det aktuelle opslag
            var user = usersDoc.RootElement.EnumerateArray()
                .FirstOrDefault(u => u.GetProperty("userId").GetInt32() == userId);

            var post = postsDoc.RootElement.EnumerateArray()
                .FirstOrDefault(p => p.GetProperty("postId").GetInt32() == postId);

            // Checker om enten brugeren eller opslaget ikke findes i deres respektive JSON, og returnerer 0 i så fald
            if (user.ValueKind == JsonValueKind.Undefined || post.ValueKind == JsonValueKind.Undefined)
                return 0;

            // Laver det om til en dictionary for at gøre det nemmere at slå op i brugerens pitsTags baseret på opslagets tags
            // Laver det også om til lowercase og  trimer også
            var userPitsLookup = user.GetProperty("pitsTags").EnumerateArray()
                .Where(t => t.TryGetProperty("tag", out _))
                .ToDictionary(
                    t => t.GetProperty("tag").GetString()?.Trim().ToLower() ?? "",
                    t => t.GetProperty("score").GetInt32()
                );

            // Laver en liste over opslagets tags
            var postTags = post.GetProperty("tags").EnumerateArray().ToList();

            if (postTags.Count == 0 || userPitsLookup.Count == 0)
                return 0;

            int totalScore = 0;
            int maxScore = 0;
            int matchCount = 0;

            // Går igennem hvert tag i opslaget og tjekker om det findes i brugerens pitsTags, hvis det gør, så lægges den til scoren
            foreach (var tagElement in postTags)
            {
                string currentPostTag = tagElement.GetString()?.Trim().ToLower() ?? "";

                if (userPitsLookup.TryGetValue(currentPostTag, out int score))
                {
                    totalScore += score;
                    matchCount++;
                    if (score > maxScore)
                    {
                        maxScore = score;
                    }
                }
            }

            // Hvis der ikke er nogen matchende tags, returneres 0
            if (matchCount == 0) return 0;

            double average = totalScore / postTags.Count;
            int finalPTIS = (int)(average + maxScore) / 2;

            if (finalPTIS <= 100)
            {
                return finalPTIS;
            }
            else
            {
                return 100; // Burde aldrig ske, men bare for at være sikker
            }
            /*
             * End of Daniels code
             */
        }

        public static int requstNewPostToFeed(int userId)
        {
            string postsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\posts.json");
            string usersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\users.json");

            if (!File.Exists(postsPath) || !File.Exists(usersPath)) return -1;

            string postsJson = File.ReadAllText(postsPath);
            string usersJson = File.ReadAllText(usersPath);

            List<Post> allPosts = JsonSerializer.Deserialize<List<Post>>(postsJson) ?? new List<Post>();
            using JsonDocument usersDoc = JsonDocument.Parse(usersJson);


            // Checks the user's watch history and extracts the video IDs into a list
            var userElement = usersDoc.RootElement.EnumerateArray()
                .FirstOrDefault(u => u.GetProperty("userId").GetInt32() == userId);

            if (userElement.ValueKind == JsonValueKind.Undefined) return -1;

            // Extract lastWatchedVideos IDs
            List<int> watchHistory = new List<int>();
            if (userElement.TryGetProperty("lastWatchedVideos", out var historyArray))
            {
                watchHistory = historyArray.EnumerateArray()
                    .Select(v => int.Parse(v.GetProperty("videoId").GetString() ?? "0"))
                    .ToList();
            }


            // Checks if the user has a small watch history
            bool isNewUser = watchHistory.Count < 10;


            // We take the 'GlobalConfig.watchHistorySize' items from history to exclude
            var excludeIds = watchHistory.TakeLast(GlobalConfig.watchHistorySize).ToList();

            int bestPostId = -1;
            long highestScore = long.MinValue;

            // Calculate post scores for all posts
            foreach (var post in allPosts)
            {
                // Checks if the post is in the user's recent watch history
                if (excludeIds.Contains(post.postId)) continue;

                int ptisScore = getPTIS(userId, post.postId);
                long currentPostValueScore = postValue(isNewUser, ptisScore, post.likes, post.comments, post.shares, post.engagement, post.PostDate);

                if (currentPostValueScore > highestScore)
                {
                    highestScore = currentPostValueScore;
                    bestPostId = post.postId;
                }
            }
            if (GlobalConfig.debugMode) {
                Debug.WriteLine("Debug Mode | Best Post ID: " + bestPostId + " with a score of: " + highestScore);
            }
            return bestPostId;
        }

        public static void likePost(int postId)
        {
            string postsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\data\\posts.json");

            if (!File.Exists(postsPath)) 
            { 
                return; 
            }

            try
            {
                // Get the posts, deserialize them, and find the target post
                string json = File.ReadAllText(postsPath);
                List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();

                // Find the post and update likes
                Post targetPost = posts.FirstOrDefault(p => p.postId == postId);
                if (targetPost != null)
                {
                    targetPost.likes++;

                    // Serialize and Save
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string updatedJson = JsonSerializer.Serialize(posts, options);
                    File.WriteAllText(postsPath, updatedJson);

                    if (GlobalConfig.debugMode)
                    {
                        Debug.WriteLine($"Debug Mode | Post {postId} liked. New count: {targetPost.likes}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error liking post: " + ex.Message);
            }
        }

        public static void commentOnPost(int postId) //stub
        {
            // This function would handle the logic for when a user comments on a post, including updating the post's comment count and the user's interaction history.
            return;
        }

        public static void sharePost(int postId) //stub
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


            // Validate input values
            if (PTIS < 0 || PTIS > 100 || likes < 0 || comments < 0 || shares < 0 || postEngagement < 0 || postEngagement > 1000)
            {
                // If any input value is out of the expected range, return -1 to indicate an error
                // And log invalid input values for debugging
                Debug.WriteLine("Invalid input values.");
                if (GlobalConfig.debugMode) {
                    Debug.WriteLine("Debug Mode | PTIS: " + PTIS + " Likes: " + likes + " Comments: " + comments + " Shares: " + shares + " Post Engagement Value: " + postEngagement);
                }
                return -1;
            }

            //calculate the factors for the post value calculation
            bool isDiscoveryRoll = _random.Next(1, 14) == 1; // 1 in 13 chance for discovery roll
            double socialValue = Math.Log10((likes* weightLikes) + (comments * weightComments) + (shares * weightShares) + 1);
            int interestValue = 1 + (PTIS / 25);
            double engagementScore = Math.Log10(postEngagement + 1) * 1.5;
            double daysSincePost = (DateTime.Now - postDate).TotalDays;
            double gravity = Math.Pow(daysSincePost + 1, weightGravity);
            if (GlobalConfig.debugMode) {
                Debug.WriteLine("Debug Mode | SocialValue: " + socialValue + " Post Engagement: " + postEngagement + " InterestValue: " + interestValue + " Gravity: " + gravity);
            }


            /*
             *THE algorithm
             */

            //checks if feed personalization is active
            if (GlobalConfig.feedModePersonalization == true && !isDiscoveryRoll && !newUser) 
            {
                // Calculate the Final Post Score with personalization
                double FPS = (((socialValue + engagementScore) * interestValue) / gravity);
                if (GlobalConfig.debugMode) {
                    Debug.WriteLine("Debug Mode | FinalPostScore: " + FPS);
                }
                return (long)FPS;
            } 
            else 
            {
                // Calculate the Final Post Score without personalization
                double FPS = ((socialValue + engagementScore) / gravity);
                if (GlobalConfig.debugMode) {
                    Debug.WriteLine("Debug Mode | FinalPostScore: " + FPS);
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
                    Debug.WriteLine("Debug Mode | Image file not found at path: " + imageFilePath + "\nFunction has stopped to prevent id numbers to get fucked up");
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

            Debug.WriteLine("Successfully created Post, ID: " + newId + ", Title: "+ title);
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
                    Debug.WriteLine("Debug Mode | Selected file: " + fileDialog.FileName);
                }
                return fileDialog.FileName;
            }
            else //if the user cancels the file selection
            {
                Debug.WriteLine("No file selected.");
                return "No file selected or something went wrong";
            }
        }
    }
}