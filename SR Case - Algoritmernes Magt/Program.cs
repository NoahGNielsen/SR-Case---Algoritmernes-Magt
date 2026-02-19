namespace SR_Case___Algoritmernes_Magt
{
    internal static class Program
    {
        //settings


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
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
                Console.WriteLine("PTIS: " + PTIS + " Likes: " + likes + " Comments: " + comments + " Shares: " + shares + " Post Engagement Value: " + postEngagement + " Old(days): " + daysSincePost);
                return -1;
            }

            /*
            /The algorithm to calculate the post value
           */
            double socialValue = Math.Log10((likes* weightLikes) + (comments * weightComments) + (shares * weightShares) + 1);
            double engagementFactor = postEngagement / 1000.0;
            double interestValue = PTIS / 100.0; // Normalize PTIS to a value between 0 and 1
            double gravity = Math.Pow(daysSincePost + 1, weightGravity);

            // Calculate the Final Post Score
            double FPS = (((socialValue + engagementFactor) * interestValue) / gravity) * weightFinalScore;

            return (long)FPS;
        }
    }
}