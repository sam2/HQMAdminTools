using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditSharp;
using RedditSharp.Things;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    public class StatsRecorder : ICommandProcessor
    {
        Reddit _reddit;
        Subreddit _subreddit;

        public StatsRecorder(string username, string password, string subreddit)
        {
            _reddit = new Reddit(username, password);
            _subreddit = _reddit.GetSubreddit(subreddit);
        }

        public void ProcessCommand(Command newCommand)
        {
           
        }

        void PostGameResults()
        {
            
        }


    }
}
