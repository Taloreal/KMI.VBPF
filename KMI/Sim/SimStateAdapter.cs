namespace KMI.Sim
{
    using KMI.Sim.Academics;
    using KMI.Sim.Drawables;
    using KMI.Sim.Surveys;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SimStateAdapter : MarshalByRefObject
    {
        public const int SOUND_NOT_ENTITY_SPECIFIC = -1;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event ModalMessageDelegate ModalMessageEvent;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event PlayerMessageDelegate PlayerMessageEvent;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event PlaySoundDelegate PlaySoundEvent;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void ChangeEntityOwner(long entityID, string newOwnerName)
        {
            CheckEntityExists(entityID);
            Entity entity = (Entity) this.simState.Entity[entityID];
            foreach (Player player in this.simState.Player.Values)
            {
                if (player.PlayerName == newOwnerName)
                {
                    entity.Player = player;
                    return;
                }
            }
            throw new SimApplicationException("Player name not found.");
        }

        public static Entity CheckEntityExists(long entityID)
        {
            if (S.State.Entity.Contains(entityID))
            {
                return (Entity) S.State.Entity[entityID];
            }
            object[] args = new object[] { S.Instance.EntityName };
            EntityNotFoundException exception = new EntityNotFoundException(S.Resources.GetString("{0} no longer exists.", args));
            throw exception;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CloseEntity(long entityID)
        {
            this.LogMethodCall(new object[0]);
            CheckEntityExists(entityID).Retire();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual Survey ConductAndAddSurvey(string playerName, long entityID, ArrayList questions, int numToSurvey, float cost)
        {
            string[] entityNames = new string[S.State.Entity.Count];
            int num = 0;
            foreach (Entity entity in S.State.Entity.Values)
            {
                entityNames[num++] = entity.Name;
            }
            Survey survey = S.Instance.SimFactory.CreateSurvey(entityID, this.simState.Now, entityNames, questions);
            survey.Execute(numToSurvey);
            ArrayList surveys = ((Player) S.State.Player[playerName]).Surveys;
            surveys.Add(survey);
            if (surveys.Count > Survey.MaxSurveys)
            {
                surveys.RemoveAt(0);
            }
            if ((entityID != -1L) && S.State.Entity.ContainsKey(entityID))
            {
                Entity entity2 = (Entity) S.State.Entity[entityID];
                if (Survey.BillForSurveys)
                {
                    entity2.GL.Post("Surveys", cost, "Cash");
                }
                object[] args = new object[] { Utilities.FU(survey.Responses.Count), Survey.SurveyableObjectName };
                entity2.Journal.AddEntry(S.Resources.GetString("Conducted survey of {0} {1}.", args));
            }
            return survey;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Player CreateClientPlayer(string playerName, string password)
        {
            if ((S.State.Multiplayer && S.Instance.UserAdminSettings.PasswordsForMultiplayer) && ((password.Length < 3) || !S.State.ValidateMultiplayerTeamPassword(playerName, password)))
            {
                throw new frmJoinMultiplayerSession.BadTeamPasswordException();
            }
            return this.CreatePlayer(playerName, PlayerType.Human);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Player CreatePlayer(string playerName, PlayerType playerType)
        {
            object[] args = new object[] { playerName, playerType };
            this.LogMethodCall(args);
            Player player = null;
            Simulator instance = Simulator.Instance;
            foreach (Player player2 in this.simState.Player.Values)
            {
                if (player2.PlayerName.ToUpper() == playerName.ToUpper())
                {
                    player = player2;
                }
            }
            if (player == null)
            {
                player = S.Instance.SimFactory.CreatePlayer(playerName, playerType);
                this.simState.Player.Add(playerName, player);
                return player;
            }
            if (!this.simState.RoleBasedMultiplayer)
            {
                object[] objArray2 = new object[] { player.PlayerName };
                player.SendMessage(S.Resources.GetString("Welcome back, {0}.", objArray2), "", NotificationColor.Green);
            }
            return player;
        }

        public void FireModalMessageEvent(ModalMessage message)
        {
            if (S.Instance.Client)
            {
                throw new Exception("FireModalMessageEvent called from client.");
            }
            if (this.ModalMessageEvent != null)
            {
                foreach (ModalMessageDelegate delegate2 in this.ModalMessageEvent.GetInvocationList())
                {
                    try
                    {
                        delegate2.BeginInvoke(message, new AsyncCallback(this.ModalMessageCallback), delegate2);
                    }
                    catch (SocketException)
                    {
                        this.ModalMessageEvent = (ModalMessageDelegate) Delegate.Remove(this.ModalMessageEvent, delegate2);
                    }
                }
            }
        }

        public void FirePlayerMessageEvent(PlayerMessage message)
        {
            if (S.Instance.Client)
            {
                throw new Exception("FirePlayerMessageEvent called from client.");
            }
            if (this.PlayerMessageEvent != null)
            {
                foreach (PlayerMessageDelegate delegate2 in this.PlayerMessageEvent.GetInvocationList())
                {
                    try
                    {
                        delegate2.BeginInvoke(message, new AsyncCallback(this.PlayerMessageCallback), delegate2);
                    }
                    catch (SocketException)
                    {
                        this.PlayerMessageEvent = (PlayerMessageDelegate) Delegate.Remove(this.PlayerMessageEvent, delegate2);
                    }
                }
            }
        }

        public void FirePlaySoundEvent(string fileName, long entityID, string viewName)
        {
            if (S.Instance.Client)
            {
                throw new Exception("FirePlaySoundEvent called from client.");
            }
            if (this.PlaySoundEvent != null)
            {
                foreach (PlaySoundDelegate delegate2 in this.PlaySoundEvent.GetInvocationList())
                {
                    try
                    {
                        delegate2.BeginInvoke(fileName, entityID, viewName, new AsyncCallback(this.PlaySoundCallback), delegate2);
                    }
                    catch (SocketException)
                    {
                        this.PlaySoundEvent = (PlaySoundDelegate) Delegate.Remove(this.PlaySoundEvent, delegate2);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public AcademicGod GetAcademicGod()
        {
            return S.State.GetAcademicGod();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public frmActionsJournal.Input getActionsJournal(long entityID)
        {
            Entity current;
            if (entityID != -1L)
            {
                CheckEntityExists(entityID);
            }
            frmActionsJournal.Input input = new frmActionsJournal.Input {
                Journals = new ArrayList()
            };
            if (entityID == -1L)
            {
                IEnumerator enumerator = S.State.Entity.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    current = (Entity) enumerator.Current;
                    input.Journals.Add(current.Journal);
                }
                foreach (Entity entity2 in S.State.RetiredEntity.Values)
                {
                    input.Journals.Add(entity2.Journal);
                }
            }
            else
            {
                current = (Entity) S.State.Entity[entityID];
                input.Journals.Add(current.Journal);
            }
            input.StartDate = S.State.SimSettings.StartDate;
            input.EndDate = S.State.Now;
            return input;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public long GetAnEntityIdForPlayer(string playerName)
        {
            Entity[] playersEntities = S.State.GetPlayersEntities(playerName);
            if (playersEntities.Length == 0)
            {
                return -1L;
            }
            return playersEntities[0].ID;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int getCurrentWeek()
        {
            this.LogMethodCall(new object[0]);
            return Simulator.Instance.SimState.CurrentWeek;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetEntityPlayer(long entityID)
        {
            object[] args = new object[] { entityID };
            this.LogMethodCall(args);
            if (this.simState.Entity.Count == 0)
            {
                return null;
            }
            CheckEntityExists(entityID);
            Entity entity = (Entity) this.simState.Entity[entityID];
            return entity.Player.PlayerName;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public GeneralLedger GetGL(long entityID)
        {
            CheckEntityExists(entityID);
            return ((Entity) this.simState.Entity[entityID]).GL;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int getHumanPlayerCount()
        {
            this.LogMethodCall(new object[0]);
            int num = 0;
            foreach (Entity entity in S.State.Entity.Values)
            {
                if (entity.Player.PlayerType == PlayerType.Human)
                {
                    num++;
                }
            }
            return num;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual float getHumanScore(string seriesName)
        {
            object[] args = new object[] { seriesName };
            this.LogMethodCall(args);
            float num = 0f;
            foreach (Entity entity in S.State.Entity.Values)
            {
                if (entity.Player.PlayerType == PlayerType.Human)
                {
                    num += entity.Journal.NumericDataSeriesLastEntry(seriesName);
                }
            }
            foreach (Entity entity2 in S.State.RetiredEntity.Values)
            {
                if (entity2.Player.PlayerType == PlayerType.Human)
                {
                    num += entity2.Journal.NumericDataSeriesLastEntry(seriesName);
                }
            }
            return num;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool getMultiplayer()
        {
            this.LogMethodCall(new object[0]);
            return this.simState.Multiplayer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string[] GetOtherOwnedEntities(long entityID)
        {
            Entity entity = CheckEntityExists(entityID);
            ArrayList list = new ArrayList();
            foreach (Entity entity2 in S.State.Entity.Values)
            {
                if ((entity != entity2) && (entity.Player == entity2.Player))
                {
                    list.Add(entity2.Name);
                }
            }
            return (string[]) list.ToArray(typeof(string));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public byte[] getPdfAssignment()
        {
            this.LogMethodCall(new object[0]);
            return this.simState.SimSettings.PdfAssignment;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public frmRunTo.Input GetRunTo()
        {
            return new frmRunTo.Input { runTo = S.State.RunToDate, now = S.State.Now };
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual frmScoreboard.Input getScoreboard(bool showAIOwnedEntities)
        {
            this.LogMethodCall(new object[0]);
            frmScoreboard.Input input = new frmScoreboard.Input {
                ScoreFriendlyName = Journal.ScoreSeriesName
            };
            Hashtable hashtable = new Hashtable();
            ArrayList list = new ArrayList(S.State.Entity.Values);
            foreach (Entity entity in list)
            {
                if (showAIOwnedEntities || !entity.AI)
                {
                    ArrayList list2 = entity.Journal.NumericDataSeries(Journal.ScoreSeriesName);
                    string playerName = entity.Player.PlayerName;
                    if (entity.AI)
                    {
                        playerName = entity.Name;
                    }
                    if (hashtable.ContainsKey(playerName))
                    {
                        ArrayList list3 = (ArrayList) hashtable[playerName];
                        for (int i = 0; i < list2.Count; i++)
                        {
                            if (i < list3.Count)
                            {
                                list3[i] = ((float) list3[i]) + ((float) list2[i]);
                            }
                            else
                            {
                                list3.Add(list2[i]);
                            }
                        }
                    }
                    else
                    {
                        hashtable.Add(playerName, list2.Clone());
                    }
                }
            }
            input.EntityNames = new string[hashtable.Count];
            hashtable.Keys.CopyTo(input.EntityNames, 0);
            input.Scores = new ArrayList[hashtable.Count];
            hashtable.Values.CopyTo(input.Scores, 0);
            return input;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public SimSettings getSimSettings()
        {
            this.LogMethodCall(new object[0]);
            return this.simState.SimSettings;
        }

        public Guid GetSimulatorID()
        {
            return S.Instance.GUID;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public ArrayList getSurveys(string playerName)
        {
            return ((Player) S.State.Player[playerName]).Surveys;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual ViewUpdate GetViewUpdate(string viewName, long entityID, params object[] args)
        {
            Entity entity = null;
            if ((entityID != -1L) || !S.Instance.SafeViewsForNoEntity.Contains(viewName))
            {
                entity = CheckEntityExists(entityID);
            }
            ViewUpdate update = new ViewUpdate {
                Drawables = S.Instance.View(viewName).BuildDrawables(entityID, args),
                Now = S.State.Now,
                CurrentWeek = S.State.CurrentWeek
            };
            if (entity != null)
            {
                update.Cash = entity.CriticalResourceBalance();
            }
            update.EntityNames = S.State.EntityNameTable();
            return update;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsUniqueEntityName(string name)
        {
            foreach (Entity entity in S.State.Entity.Values)
            {
                if (entity.Name == name)
                {
                    return false;
                }
            }
            foreach (Entity entity2 in S.State.RetiredEntity.Values)
            {
                if (entity2.Name == name)
                {
                    return false;
                }
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int Level()
        {
            return S.Settings.Level;
        }

        protected void LogMethodCall(params object[] args)
        {
            if (S.MainForm.DesignerMode && !S.Instance.Client)
            {
                MethodBase method = new StackFrame(1).GetMethod();
                ParameterInfo[] parameters = method.GetParameters();
                S.MainForm.SaveMacroAction(new MacroAction(method, args, S.State.Now));
            }
        }

        public void ModalMessageCallback(IAsyncResult ar)
        {
            ((ModalMessageDelegate) ar.AsyncState).EndInvoke(ar);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Ping()
        {
            return true;
        }

        public void PlayerMessageCallback(IAsyncResult ar)
        {
            ((PlayerMessageDelegate) ar.AsyncState).EndInvoke(ar);
        }

        public void PlaySoundCallback(IAsyncResult ar)
        {
            ((PlaySoundDelegate) ar.AsyncState).EndInvoke(ar);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void ProvideCash(long entityID, float amount)
        {
            CheckEntityExists(entityID).GL.Post("Cash", amount, "Paid-in Capital");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RenameEntity(long entityID, string newName)
        {
            CheckEntityExists(entityID).Name = newName;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool RoleBasedMultiplayer()
        {
            this.LogMethodCall(new object[0]);
            return this.simState.RoleBasedMultiplayer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendMessage(string fromPlayerName, string toPlayerName, string message)
        {
            ((Player) S.State.Player[toPlayerName]).SendMessage(message, fromPlayerName, NotificationColor.Blue);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void SetRunTo(DateTime date)
        {
            S.State.RunToDate = date;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void SetRunTo(int daysAhead)
        {
            S.State.RunToDate = new DateTime(S.State.Now.Year, S.State.Now.Month, S.State.Now.Day).AddDays((double) daysAhead);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Entity TryAddEntity(string playerName, string entityName)
        {
            foreach (Entity entity2 in S.State.Entity.Values)
            {
                if (entity2.Name.ToUpper() == entityName.ToUpper())
                {
                    throw new SimApplicationException(S.Resources.GetString("That name is already taken. Please try another."));
                }
            }
            foreach (Entity entity3 in S.State.RetiredEntity.Values)
            {
                if (entity3.Name.ToUpper() == entityName.ToUpper())
                {
                    object[] args = new object[] { S.Instance.EntityName.ToLower() };
                    throw new SimApplicationException(S.Resources.GetString("A previously existing {0} had that name. Please try another.", args));
                }
            }
            Entity entity = S.Instance.SimFactory.CreateEntity((Player) S.State.Player[playerName], entityName);
            S.State.Entity.Add(entity.ID, entity);
            return entity;
        }

        protected SimState simState
        {
            get
            {
                return Simulator.Instance.SimState;
            }
        }

        [Serializable]
        public class ViewUpdate
        {
            public object AppData;
            public float Cash;
            public int CurrentWeek;
            public Drawable[] Drawables;
            public Hashtable EntityNames;
            public DateTime Now;
        }
    }
}

