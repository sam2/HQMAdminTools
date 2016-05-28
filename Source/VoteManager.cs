using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    class VoteManager : ICommandProcessor
    {      
        Vote _currentVote;
        Dictionary<string, Action> VoteTypes = new Dictionary<string, Action>();
        bool _enabled = false;

        public VoteManager()
        {
            _currentVote = null;
            VoteTypes = new Dictionary<string, Action>();
            VoteTypes["endgame"] = EndGame;
        }

        public void ProcessCommand(Command newCommand)
        {
            bool voteActive = _currentVote != null && _currentVote.IsActive;

            if (newCommand.Args.Length > 0)
            {
                string voteType = newCommand.Args[0].ToLower();

                if (newCommand.Sender.IsAdmin)
                {
                    if (voteType == "enable" && !_enabled)
                    {
                        _enabled = true;
                        Chat.SendMessage("Voting enabled.");
                        return;
                    }
                    else if(voteType == "disable" && _enabled)
                    {
                        _enabled = false;
                        Chat.SendMessage("Voting disabled.");
                        return;
                    }
                }                
                
                Action votePassed;
                if (_enabled && !voteActive && VoteTypes.TryGetValue(voteType, out votePassed))
                {
                    _currentVote = new Vote(voteType, 0.75f, votePassed);
                    Chat.SendMessage("VOTE STARTED BY "+newCommand.Sender.Name+": " + voteType);
                    Chat.SendMessage("Type /vote for yes");
                    _currentVote.AddVote(newCommand.Sender);
                }
            }
            
            else if(_enabled && voteActive && !_currentVote.Votes.Contains(newCommand.Sender.Slot))
            {
                _currentVote.AddVote(newCommand.Sender);
            }

            if(!_enabled)
            {
                Chat.SendMessage("Voting is disabled.");
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
            public float RequiredPercent;

            System.Timers.Timer _timer;

            public bool IsActive;
            Action VotePassedAction;

            public Vote(string type, float requiredPercentage, Action votepassedaction)
            {
                Type = type;
                RequiredPercent = requiredPercentage;
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
                Chat.SendMessage(Type + " vote - " + Votes.Count + "/" + Math.Ceiling(ServerInfo.PlayerCount * RequiredPercent));
                if (Votes.Count >= ServerInfo.PlayerCount * RequiredPercent)
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
