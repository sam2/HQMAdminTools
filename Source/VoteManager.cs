using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    class VoteManager : CommandProcessor
    {      
        Vote _currentVote;
        Dictionary<string, Action> VoteTypes = new Dictionary<string, Action>();

        public VoteManager()
        {
            _currentVote = null;
            VoteTypes = new Dictionary<string, Action>();
            VoteTypes["endgame"] = EndGame;
        }

        public void ProcessCommand(Command newCommand)
        {
            if (newCommand.Args.Length == 0)
                return;

            string voteType = newCommand.Args[0].ToLower();
            Action votePassed;
            if ((_currentVote == null || !_currentVote.IsActive) && VoteTypes.TryGetValue(voteType, out votePassed))
            {             
                _currentVote = new Vote(voteType, ServerInfo.PlayerCount, votePassed);                                       
                Chat.SendMessage(newCommand.Sender.Name + " has started a vote: "+voteType);
                Chat.SendMessage("type /vote "+voteType+" to vote yes");
                _currentVote.AddVote(newCommand.Sender);
            }
            else if(voteType == _currentVote.Type && !_currentVote.Votes.Contains(newCommand.Sender.Slot))
            {
                _currentVote.AddVote(newCommand.Sender);
            }
        }

        //kind of a hacky way to end the game. Should find out how to reset it.
        void EndGame()
        {
            GameInfo.Period = 3;
            GameInfo.RedScore = 1;
            GameInfo.BlueScore = 0;
            GameInfo.GameTime = new TimeSpan(0, 0, 0, 1);           
        }

        class Vote
        {
            public string Type = "";
            public List<int> Votes;
            public int RequiredVotes;

            System.Timers.Timer _timer;

            public bool IsActive;
            Action VotePassedAction;

            public Vote(string type, int requiredVotes, Action votepassedaction)
            {
                Type = type;
                RequiredVotes = requiredVotes;
                Votes = new List<int>();
                VotePassedAction = votepassedaction;

                IsActive = true;

                _timer = new System.Timers.Timer(30000); //30s timer
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(VoteFailed);
                _timer.Enabled = true;
            }

            public void AddVote(Player voter)
            {
                Votes.Add(voter.Slot);
                Chat.SendMessage(Type + " vote: " + Votes.Count + "/" + RequiredVotes);
                if (Votes.Count >= RequiredVotes)
                {
                    VotePassed();
                }
            }

            public void VoteFailed(object sender, EventArgs e)
            {
                Chat.SendMessage(Type + " vote failed.");                
                IsActive = false;
                _timer.Enabled = false;                  
            }

            public void VotePassed()
            {
                Chat.SendMessage(Type + " vote passed.");
                IsActive = false;
                _timer.Enabled = false;
                VotePassedAction();
            }
        }

        
    }
}
