using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    public class PauseManager : ICommandProcessor
    {
        int _faceoffTimer = 3;
        System.Timers.Timer _timer;       

        PauseState _state = PauseState.UnPaused;

        public void ProcessCommand(Command newCommand)
        {         
            if (!newCommand.Sender.IsAdmin)
                return;

            string senderName = newCommand.Sender.Name;

            if (_state == PauseState.UnPaused)
            {
                switch (newCommand.Cmd)
                {
                    case "pause":
                        Pause(senderName);
                        break;
                    case "faceoff":
                        Faceoff(senderName);
                        break;
                }
            }
            else if (_state == PauseState.Paused)
            {
                switch (newCommand.Cmd)
                {
                    case "resume":
                        Resume(senderName);
                        break;
                    case "faceoff":
                        Faceoff(senderName);
                        break;
                }               
            }               
        }

        void Pause(string adminName)
        {
            Tools.PauseGame();
            Chat.SendMessage("Game paused by " + adminName);
            _state = PauseState.Paused;
        }

        void Resume(string adminName)
        {
            Tools.ResumeGame();
            Chat.SendMessage("Game resumed by " + adminName);
            _state = PauseState.UnPaused;
        }

        void Faceoff(string adminName)
        {
            Tools.PauseGame();            
            Chat.SendMessage("Faceoff initiated by " + adminName);

            _timer = new System.Timers.Timer(1000);
            _timer.AutoReset = true;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
            _timer.Enabled = true;

            _state = PauseState.FaceoffCountdown;
        }

        void TimerElapsed(object sender, EventArgs e)
        {
            Chat.SendMessage("" + _faceoffTimer);
            --_faceoffTimer;
            if(_faceoffTimer < 0)
            {
                _faceoffTimer = 3;
                _timer.Enabled = false;

                Tools.ResumeGame();
                Tools.ForceFaceoff();

                _state = PauseState.UnPaused;
            }
        }

        public void CheckForAutoResume()
        {
            if(_state == PauseState.Paused)
            {
                foreach (Player p in PlayerManager.Players)
                {
                    if (p.InServer)
                        return;
                }
                Resume("AdminTools"); 
            }            
        }

        enum PauseState
        {
            UnPaused,
            Paused,
            FaceoffCountdown
        }
    }
}
