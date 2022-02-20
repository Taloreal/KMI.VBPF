namespace KMI.Sim.Academics
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;
    using System.IO;

    [Serializable]
    public class AcademicGod : ActiveObject
    {
        protected int academicLevel = -1;
        protected PageBank pageBank = PageBank.LoadFromXML(PageBankPath + Path.DirectorySeparatorChar + "Pages.xml");
        public static string PageBankPath;
        protected bool showedQuestionsRemain;

        public AcademicGod()
        {
            this.AcademicLevel = 0;
            S.Instance.Subscribe(this, Simulator.TimePeriod.Day);
        }

        public Question[] FindAllAskedQuestions(int level)
        {
            ArrayList list = new ArrayList();
            foreach (KMI.Sim.Academics.Page page in this.pageBank.Levels[level].Pages)
            {
                foreach (Question question in page.Questions)
                {
                    if (question.Answer != null)
                    {
                        list.Add(question);
                    }
                }
            }
            return (Question[])list.ToArray(typeof(Question));
        }

        protected KMI.Sim.Academics.Page FindNextPage(string power)
        {
            float score = this.Score;
            foreach (KMI.Sim.Academics.Page page in this.CurrentLevel.Pages)
            {
                if (((page.Questions[0].Answer == null) && (page.Power == power)) && (score >= page.MinScore))
                {
                    return page;
                }
            }
            return null;
        }

        public float GradeForLevel(int level)
        {
            float num = 0f;
            float num2 = 0f;
            foreach (KMI.Sim.Academics.Page page in this.pageBank.Levels[level].Pages)
            {
                foreach (Question question in page.Questions)
                {
                    num2++;
                    if (question.Correct)
                    {
                        num++;
                    }
                }
            }
            if (num2 == 0f)
            {
                return 0f;
            }
            return (num / num2);
        }

        public static void HandleLevelEnd(LevelEndTestMessage m)
        {
            MessageBox.Show(S.Resources.GetString("You have completed Level {0}. You will now be asked to answer review questions from this level. Hit Submit only after completing all questions.", new object[] { m.NewLevel }), S.Resources.GetString("Congratulations!!"));
            new frmQuestions(frmQuestions.Modes.LevelEndTest, m.Questions) { Text = S.Resources.GetString("Review Test for Level {0}", new object[] { m.NewLevel }) }.ShowDialog();
            if (MessageBox.Show(S.Resources.GetString("We suggest saving after each level. Do you want to save now?"), S.Resources.GetString("Save Now?"), MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                S.MainForm.mnuFileSaveAs.PerformClick();
            }
            if (!(m.LastLevel || !(m.Message != "")))
            {
                MessageBox.Show(m.Message, S.Resources.GetString("Welcome to Level {0}", new object[] { m.NewLevel + 1 }));
            }
            if (m.LastLevel)
            {
                MessageBox.Show(S.Resources.GetString("You have completed all the levels. Review your test scores under Options or use File->New to start again."), S.Resources.GetString("Congratulations"));
                S.MainForm.StopSimulation();
            }
        }

        public override void NewDay()
        {
            Player player = (Player) S.State.Player[""];
            if (this.TimeForNextLevel())
            {
                string s = this.QuestionPowersLeft();
                if (s != "")
                {
                    if (!this.showedQuestionsRemain)
                    {
                        player.SendModalMessage(S.Resources.GetString("You've reached the {0} goal for Level {1}. And you didn't even view all the available hints! To officially complete this level, please click {2} to see the remaining hints.", new object[] { Journal.ScoreSeriesName.ToLower(), this.AcademicLevel + 1, Utilities.FormatCommaSeries(s) }), S.Resources.GetString("Congratulations"), MessageBoxIcon.Exclamation);
                        this.showedQuestionsRemain = true;
                    }
                }
                else
                {
                    bool lastLevel = this.AcademicLevel == (this.pageBank.Levels.Length - 1);
                    player.SendModalMessage(new LevelEndTestMessage(player.PlayerName, this.CurrentLevel.LevelIntroMessage, this.AcademicLevel + 1, this.FindAllAskedQuestions(this.AcademicLevel), lastLevel));
                    this.AcademicLevel++;
                }
            }
        }

        public static void Prompt(object sender)
        {
            string power = Utilities.NoEllipsis(Utilities.NoAmpersand(((MenuItem)sender).Text)).Replace(" ", "");
            AcademicGod academicGod = S.Adapter.GetAcademicGod();
            KMI.Sim.Academics.Page page = academicGod.FindNextPage(power);
            if (page != null)
            {
                new frmPage(page).ShowDialog();
            }
            academicGod.NewDay();
        }

        protected string QuestionPowersLeft()
        {
            string str = "";
            foreach (KMI.Sim.Academics.Page page in this.CurrentLevel.Pages)
            {
                if (page.Questions[0].Answer == null)
                {
                    str = str + Utilities.AddSpaces(page.Power) + ", ";
                }
            }
            return str;
        }

        protected virtual bool TimeForNextLevel()
        {
            return (this.Score > this.CurrentLevel.Goal);
        }

        public int AcademicLevel
        {
            get
            {
                return this.academicLevel;
            }
            set
            {
                this.academicLevel = value;
                this.showedQuestionsRemain = false;
                PropertyInfo[] properties = S.Settings.GetType().GetProperties();
                foreach (string str in this.CurrentLevel.Powers)
                {
                    foreach (PropertyInfo info in properties)
                    {
                        MethodInfo setMethod = info.GetSetMethod();
                        if (info.Name == (str + "EnabledForOwner"))
                        {
                            lock (S.Adapter)
                            {
                                setMethod.Invoke(S.Settings, new object[] { true });
                            }
                            break;
                        }
                    }
                }
                S.MainForm.EnableDisable();
            }
        }

        protected Level CurrentLevel
        {
            get
            {
                return this.pageBank.Levels[Math.Min(this.AcademicLevel, this.pageBank.Levels.Length - 1)];
            }
        }

        protected float Score
        {
            get
            {
                ArrayList list = new ArrayList(S.State.Entity.Values);
                list.AddRange(S.State.RetiredEntity.Values);
                float num = 0f;
                foreach (Entity entity in list)
                {
                    if (!entity.AI)
                    {
                        num += entity.Journal.NumericDataSeriesLastEntry(Journal.ScoreSeriesName);
                    }
                }
                return num;
            }
        }
    }
}