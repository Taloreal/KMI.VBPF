namespace KMI.Utility
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    public class ControlArray : CollectionBase
    {
        protected Form hostForm;

        public ControlArray(Form form)
        {
            this.hostForm = form;
        }

        public void AddExistingControl(Control control)
        {
            base.List.Add(control);
        }

        public void AddNewControl(Control control)
        {
            base.List.Add(control);
            this.hostForm.Controls.Add(control);
        }

        public int IndexOf(object control)
        {
            if (!base.List.Contains(control) || (this.Count == 0))
            {
                return -1;
            }
            return base.List.IndexOf(control);
        }

        public void Remove(int index, bool fromForm)
        {
            base.List.RemoveAt(index);
            if (fromForm)
            {
                this.hostForm.Controls.Remove(this[index]);
            }
        }

        public void Remove(object control, bool fromForm)
        {
            base.List.Remove(control);
            if (fromForm)
            {
                this.hostForm.Controls.Remove((Control) control);
            }
        }

        public int Count
        {
            get
            {
                return base.List.Count;
            }
        }

        public Control this[int index]
        {
            get
            {
                return (Control) base.List[index];
            }
        }
    }
}

