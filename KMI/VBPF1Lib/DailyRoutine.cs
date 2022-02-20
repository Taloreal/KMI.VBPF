namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Collections;

    [Serializable]
    public class DailyRoutine
    {
        public SortedList Tasks = new SortedList();

        public bool CheckConflicts(Task task)
        {
            foreach (Task task2 in this.Tasks.Values)
            {
                if (task2.ID != task.ID)
                {
                    for (int i = task.StartPeriod; i < (task.StartPeriod + task.Duration); i++)
                    {
                        for (int j = task2.StartPeriod; j < (task2.StartPeriod + task2.Duration); j++)
                        {
                            if ((i % 0x30) == (j % 0x30))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            foreach (Task task3 in this.Tasks.Values)
            {
                if ((task.Building != task3.Building) && ((task.StartPeriod == task3.EndPeriod) || (task.EndPeriod == task3.StartPeriod)))
                {
                    return false;
                }
            }
            return true;
        }

        public void CheckHoursConflict(Task task)
        {
            foreach (Task task2 in this.Tasks.Values)
            {
                if (task2.ID != task.ID)
                {
                    for (int i = task.StartPeriod; i < (task.StartPeriod + task.Duration); i++)
                    {
                        for (int j = task2.StartPeriod; j < (task2.StartPeriod + task2.Duration); j++)
                        {
                            if ((i % 0x30) == (j % 0x30))
                            {
                                throw new SimApplicationException(A.Resources.GetString("Those hours conflict with other activities in your daily routine. Please adjust other activities to make room."));
                            }
                        }
                    }
                }
            }
            foreach (Task task3 in this.Tasks.Values)
            {
                if ((task.Building != task3.Building) && ((task.StartPeriod == task3.EndPeriod) || (task.EndPeriod == task3.StartPeriod)))
                {
                    throw new SimApplicationException(A.Resources.GetString("You must allow at least a half hour between activities at different locations. Please adjust your schedule."));
                }
            }
        }

        public Task GetCurrentTask()
        {
            Task task = null;
            foreach (Task task2 in this.Tasks.Values)
            {
                if (task2.StartPeriod < task2.EndPeriod)
                {
                    if (((task2.StartPeriod <= A.State.Period) && (A.State.Period < task2.EndPeriod)) && (task2.DayLastStarted != A.State.Day))
                    {
                        task = task2;
                        task.DayLastStarted = A.State.Day;
                    }
                }
                else if ((task2.StartPeriod <= A.State.Period) && (task2.DayLastStarted != A.State.Day))
                {
                    task = task2;
                    task.DayLastStarted = A.State.Day;
                }
                else if ((A.State.Period < task2.EndPeriod) && (task2.DayLastStarted != A.State.Now.AddDays(-1.0).Day))
                {
                    task = task2;
                    task.DayLastStarted = A.State.Now.AddDays(-1.0).Day;
                }
            }
            return task;
        }

        public DailyRoutine MakeCopy()
        {
            return new DailyRoutine { Tasks = (SortedList) this.Tasks.Clone() };
        }

        public Task PriorTask(Task task)
        {
            int num = this.Tasks.IndexOfValue(task);
            if (num == 0)
            {
                return (Task) this.Tasks.GetByIndex(this.Tasks.Count - 1);
            }
            return (Task) this.Tasks.GetByIndex(num - 1);
        }
    }
}

