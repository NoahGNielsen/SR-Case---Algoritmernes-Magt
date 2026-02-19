namespace SR_Case___Algoritmernes_Magt
{
    public static class GlobalConfig
    {
        // These act as your global variables
        public readonly static bool feedModePersonalization = false; //default: true
        public readonly static bool debugMode = false; //default: false
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
        static long postValue(int PTIS, int likes, int comments, int shares, int postEngagement, int daysSincePost)
        {
            //Weights
            double weightLikes = 1; //default: 1
            double weightComments = 3; //default: 3
            double weightShares = 5; //default: 5
            double weightGravity = 1.8; //default: 1.8
            double weightFinalScore = 1000; //default: 1000


            // Validate input values
            if (PTIS < 0 || PTIS > 100 || likes < 0 || comments < 0 || shares < 0 || postEngagement < 0 || postEngagement > 1000 || daysSincePost < 0)
            {
                // If any input value is out of the expected range, return -1 to indicate an error
                // And log invalid input values for debugging
                Console.WriteLine("Invalid input values.");
                if (GlobalConfig.debugMode) {
                    Console.WriteLine("Debug Mode | PTIS: " + PTIS + " Likes: " + likes + " Comments: " + comments + " Shares: " + shares + " Post Engagement Value: " + postEngagement + " Old(days): " + daysSincePost);
                }
                return -1;
            }


            //calculate the factors for the post value calculation
            double socialValue = Math.Log10((likes* weightLikes) + (comments * weightComments) + (shares * weightShares) + 1);
            double engagementFactor = postEngagement / 1000.0;
            double interestValue = PTIS / 100.0; // Normalize PTIS to a value between 0 and 1
            double gravity = Math.Pow(daysSincePost + 1, weightGravity);
            if (GlobalConfig.debugMode) {
                Console.WriteLine("Debug Mode | SocialValue: " + socialValue + " EngagementFactor: " + engagementFactor + " InterestValue: " + interestValue + " Gravity: " + gravity);
            }


            /*
             * 
             *The algorithm to calculate the post value
             *
             */

            //checks if feed personalization is active
            if (GlobalConfig.feedModePersonalization == true) 
            {
                // Calculate the Final Post Score with personalization
                double FPS = (((socialValue + engagementFactor) * interestValue) / gravity) * weightFinalScore;
                if (GlobalConfig.debugMode) {
                    Console.WriteLine("Debug Mode | FinalPostScore: " + FPS);
                }
                return (long)FPS;
            } 
            else 
            {
                // Calculate the Final Post Score without personalization
                double FPS = ((socialValue + engagementFactor) / gravity) * weightFinalScore;
                if (GlobalConfig.debugMode) {
                    Console.WriteLine("Debug Mode | FinalPostScore: " + FPS);
                }
                return (long)FPS;
            }
        }
    }
}