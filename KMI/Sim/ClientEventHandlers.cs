namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientEventHandlers : MarshalByRefObject
    {
        public static ClientEventHandlers Instance = new ClientEventHandlers();

        private ClientEventHandlers()
        {
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void ModalMessageHandler(ModalMessage message)
        {
            if (((message.To == S.Instance.ThisPlayerName) || (message.To == S.Resources.GetString("All Players"))) && !S.MainForm.OptOutModalMessageHook(message))
            {
                object[] args = new object[] { message };
                S.MainForm.Invoke(new AddModalMessageDelegate(S.MainForm.ShowModalMessage), args);
            }
        }

        public void PlayerMessageHandler(PlayerMessage message)
        {
            if ((((message.To == S.Instance.ThisPlayerName) && ((S.Instance.MultiplayerRole == null) || S.Instance.MultiplayerRole.ReceivesMessages)) || ((message.To == S.Resources.GetString("All Players")) || S.Instance.Host)) || S.MainForm.DesignerMode)
            {
                object[] args = new object[] { message };
                S.MainForm.Invoke(new AddPlayerMessageDelegate(S.MainForm.AddPlayerMessage), args);
            }
        }

        public void PlaySoundHandler(string fileName, long entityID, string viewName)
        {
            if ((S.MainForm.SoundOn && ((S.MainForm.CurrentEntityID == entityID) || (entityID == -1L))) && (S.MainForm.CurrentViewName == viewName))
            {
                Sound.PlaySoundFromFile(fileName);
            }
        }

        private delegate void AddModalMessageDelegate(ModalMessage message);

        private delegate void AddPlayerMessageDelegate(PlayerMessage message);
    }
}

