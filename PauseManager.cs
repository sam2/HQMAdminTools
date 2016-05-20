using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    public class PauseManager
    {
        int _faceoffTimer = 3;
        System.Timers.Timer _timer;       

        PauseState _state = PauseState.UnPaused;

        public void Update(Command newCommand)
        {
            if (newCommand != null && newCommand.Sender.IsAdmin)
            {
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
        }

        void Pause(string adminName)
        {
            Tools.PauseGame();
            Chat.SendMessage("GAME PAUSED BY " + adminName);
            _state = PauseState.Paused;
        }

        void Resume(string adminName)
        {
            Tools.ResumeGame();
            Chat.SendMessage("GAME RESUMED BY " + adminName);
            _state = PauseState.UnPaused;
        }

        void Faceoff(string adminName)
        {
            Tools.PauseGame();            
            Chat.SendMessage("FACEOFF INITIATED BY " + adminName);

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

        enum PauseState
        {
            UnPaused,
            Paused,
            FaceoffCountdown
        }
    }
}
