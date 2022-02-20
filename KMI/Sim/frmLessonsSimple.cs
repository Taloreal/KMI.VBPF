namespace KMI.Sim
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class frmLessonsSimple : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private Container components = null;
        private Label label1;
        private Label labMultiSimLesson;
        private ListBox lboLesson;
        private ListBox lboSim;
        protected string lessonFileName;
        private Hashtable multiSimLessons = new Hashtable();
        private bool NoLessonsFound = false;
        private Panel panel2;

        public frmLessonsSimple()
        {
            this.InitializeComponent();
            if (!Directory.Exists(Application.StartupPath + @"\Lessons\"))
            {
                this.NoLessonsFound = true;
            }
            else
            {
                string[] files = Directory.GetFiles(Application.StartupPath + @"\Lessons\", "*." + S.Instance.DataFileTypeExtension);
                if (files.Length == 0)
                {
                    this.NoLessonsFound = true;
                }
                else
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        files[i] = Path.GetFileNameWithoutExtension(files[i]);
                    }
                    Array.Sort(files, new LessonNameComparer());
                    foreach (string str in files)
                    {
                        string str2;
                        if (str.IndexOf(", Sim 1") > -1)
                        {
                            str2 = str.Substring(0, str.IndexOf(", Sim "));
                            this.lboLesson.Items.Add(str2);
                            this.multiSimLessons.Add(str2, 1);
                        }
                        else if (str.IndexOf(", Sim ") == -1)
                        {
                            this.lboLesson.Items.Add(str);
                        }
                        else
                        {
                            str2 = str.Substring(0, str.IndexOf(", Sim "));
                            this.multiSimLessons[str2] = ((int) this.multiSimLessons[str2]) + 1;
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.lboLesson.SelectedIndex <= -1)
            {
                MessageBox.Show("Please choose a lesson or hit cancel.", "No Lesson Selected");
            }
            else
            {
                if (this.lboSim.Visible)
                {
                    if (this.lboSim.SelectedIndex <= -1)
                    {
                        MessageBox.Show(S.Resources.GetString("The lesson you selected has multiple sims associated with it. Please choose one below as directed in your assignment."), S.Resources.GetString("Choose a Sim"));
                        return;
                    }
                    object[] objArray1 = new object[] { Application.StartupPath, @"\Lessons\", (string) this.lboLesson.SelectedItem, ", Sim ", this.lboSim.SelectedIndex + 1, ".", S.Instance.DataFileTypeExtension };
                    this.lessonFileName = string.Concat(objArray1);
                }
                else
                {
                    string[] textArray1 = new string[] { Application.StartupPath, @"\Lessons\", (string) this.lboLesson.SelectedItem, ".", S.Instance.DataFileTypeExtension };
                    this.lessonFileName = string.Concat(textArray1);
                }
                base.DialogResult = DialogResult.OK;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmLessonsSimple_Load(object sender, EventArgs e)
        {
            if (this.NoLessonsFound)
            {
                MessageBox.Show(S.Resources.GetString("No lessons found."), S.Resources.GetString("No Lessons Found"));
                base.Close();
            }
        }

        private void InitializeComponent()
        {
            this.panel2 = new Panel();
            this.labMultiSimLesson = new Label();
            this.lboSim = new ListBox();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.lboLesson = new ListBox();
            this.label1 = new Label();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.panel2.BorderStyle = BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labMultiSimLesson);
            this.panel2.Controls.Add(this.lboSim);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.lboLesson);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x1a8, 480);
            this.panel2.TabIndex = 0;
            this.panel2.Click += new EventHandler(this.btnOK_Click);
            this.labMultiSimLesson.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labMultiSimLesson.Location = new Point(80, 0x173);
            this.labMultiSimLesson.Name = "labMultiSimLesson";
            this.labMultiSimLesson.Size = new Size(120, 40);
            this.labMultiSimLesson.TabIndex = 5;
            this.labMultiSimLesson.Text = "Choose a Specific Sim:";
            this.labMultiSimLesson.TextAlign = ContentAlignment.MiddleLeft;
            this.labMultiSimLesson.Visible = false;
            this.lboSim.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lboSim.ItemHeight = 0x10;
            this.lboSim.Location = new Point(200, 0x173);
            this.lboSim.Name = "lboSim";
            this.lboSim.Size = new Size(0x40, 0x34);
            this.lboSim.TabIndex = 4;
            this.lboSim.Visible = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xe8, 0x1b3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Location = new Point(0x68, 0x1b3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.lboLesson.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lboLesson.ItemHeight = 0x10;
            this.lboLesson.Location = new Point(0x20, 0x3d);
            this.lboLesson.Name = "lboLesson";
            this.lboLesson.Size = new Size(360, 0x124);
            this.lboLesson.TabIndex = 1;
            this.lboLesson.SelectedIndexChanged += new EventHandler(this.lboLesson_SelectedIndexChanged);
            this.lboLesson.DoubleClick += new EventHandler(this.lboLesson_DoubleClick);
            this.label1.Font = new Font("Microsoft Sans Serif", 20.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(80, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x108, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose a Lesson:";
            this.label1.TextAlign = ContentAlignment.TopCenter;
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x1a8, 480);
            base.Controls.Add(this.panel2);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "frmLessonsSimple";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "frmLessons";
            base.Load += new EventHandler(this.frmLessonsSimple_Load);
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void lboLesson_DoubleClick(object sender, EventArgs e)
        {
            this.btnOK.PerformClick();
        }

        private void lboLesson_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lboSim.Visible = this.multiSimLessons.ContainsKey(this.lboLesson.SelectedItem);
            this.labMultiSimLesson.Visible = this.lboSim.Visible;
            if (this.lboSim.Visible)
            {
                this.lboSim.Items.Clear();
                for (int i = 1; i <= ((int) this.multiSimLessons[this.lboLesson.SelectedItem]); i++)
                {
                    this.lboSim.Items.Add("Sim " + i);
                }
            }
        }

        public string LessonFileName
        {
            get
            {
                return this.lessonFileName;
            }
        }

        public class LessonNameComparer : IComparer
        {
            public int Compare(object x1, object x2)
            {
                string str = (string) x1;
                string str2 = (string) x2;
                if (str == str2)
                {
                    return 0;
                }
                char[] separator = new char[] { ' ' };
                string[] strArray = str.Split(separator);
                char[] chArray2 = new char[] { ' ' };
                string[] strArray2 = str2.Split(chArray2);
                int num = int.Parse(strArray[1]);
                int num2 = int.Parse(strArray2[1]);
                if (num != num2)
                {
                    return (num - num2);
                }
                int num3 = int.Parse(strArray[strArray.Length - 1]);
                int num4 = int.Parse(strArray2[strArray2.Length - 1]);
                return (num3 - num4);
            }
        }
    }
}

