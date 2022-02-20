namespace KMI.Sim
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class Resource
    {
        protected SortedList cursorTable = new SortedList();
        protected SortedList imageTable = new SortedList();
        protected SortedList pageTable = new SortedList();
        //private ArrayList resourceManagers = new ArrayList();
        private readonly ArrayList resourceManagers = new ArrayList();

        public Resource(params ResourceManager[] rms)
        {
            foreach (ResourceManager manager in rms)
            {
                this.resourceManagers.Add(manager);
            }
        }

        public Cursor GetCursor(string cursorName)
        {
            Cursor cursor = (Cursor) this.cursorTable[cursorName];
            if (cursor == null)
            {
                throw new Exception("Could not find cursor " + cursorName);
            }
            return cursor;
        }

        public Bitmap GetImage(string imageName)
        {
            Bitmap bitmap = (Bitmap) this.imageTable[imageName];
            if (bitmap == null)
            {
                throw new Exception("Could not find image " + imageName);
            }
            return bitmap;
        }

        public Page GetPage(string pageName)
        {
            Page page = (Page) this.pageTable[pageName];
            if (page == null)
            {
                throw new Exception("Could not find page " + pageName);
            }
            return page;
        }

        public string GetRandomSubString(string resource, char[] delimiter)
        {
            string[] strArray = this.GetString(resource).Split(delimiter);
            return strArray[Simulator.Instance.SimState.Random.Next(strArray.GetLength(0))];
        }

        public string GetString(string Name)
        {
            if (this.resourceManagers.Count == 0)
            {
                throw new Exception("No string resource files were added to this solution.");
            }
            foreach (ResourceManager manager in this.resourceManagers)
            {
                string str = manager.GetString(Name);
                if (str != null)
                {
                    return str;
                }
            }
            return Name;
        }

        public string GetString(string Name, params object[] args)
        {
            return string.Format(this.GetString(Name), args);
        }

        public string GetStringByIndex(string Name, int index)
        {
            char[] separator = new char[] { '|' };
            return this.GetString(Name).Split(separator)[index];
        }

        public SortedList CursorTable
        {
            set
            {
                this.cursorTable = value;
            }
        }

        public SortedList ImageTable
        {
            set
            {
                this.imageTable = value;
            }
        }

        public SortedList PageTable
        {
            set
            {
                this.pageTable = value;
            }
        }
    }
}

