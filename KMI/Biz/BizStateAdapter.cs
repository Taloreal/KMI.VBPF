namespace KMI.Biz
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Runtime.CompilerServices;

    public class BizStateAdapter : SimStateAdapter
    {
        public virtual CommentLog GetComments(long entityID)
        {
            throw new NotImplementedException("GetComments not overriden in BizStateAdapter Subclass.");
        }

        public virtual ConsultantReport GetConsultantReport(long entityID)
        {
            throw new NotImplementedException("GetConsultantReport not overriden in BizStateAdapter Subclass.");
        }

        public virtual ConsultantReport[] GetGrades(long entityID)
        {
            throw new NotImplementedException("GetGrades not overriden in BizStateAdapter Subclass.");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public object[,] GetMarketShare()
        {
            int num;
            base.LogMethodCall(new object[0]);
            SimState sT = S.State;
            if (S.State.Entity.Count == 0)
            {
                return null;
            }
            int num2 = Math.Max(0, sT.CurrentWeek - GeneralLedger.WeeksOfFinancialHistory);
            float[] numArray = new float[sT.CurrentWeek - num2];
            object[,] objArray = new object[sT.Entity.Count + 1, (sT.CurrentWeek - num2) + 1];
            foreach (Entity entity in sT.Entity.Values)
            {
                num = num2;
                while (num < sT.CurrentWeek)
                {
                    numArray[num - num2] += entity.GL.AccountBalance("Revenue", num);
                    num++;
                }
            }
            int num3 = 0;
            foreach (Entity entity2 in sT.Entity.Values)
            {
                num3++;
                for (num = num2; num < sT.CurrentWeek; num++)
                {
                    if (!(numArray[num - num2] == 0f))
                    {
                        objArray[num3, (num - num2) + 1] = entity2.GL.AccountBalance("Revenue", num) / numArray[num - num2];
                    }
                    else
                    {
                        objArray[num3, (num - num2) + 1] = 0f;
                    }
                    objArray[0, (num - num2) + 1] = entity2.GL.EndingDateOfPeriod(num, GeneralLedger.Frequency.Weekly);
                }
                objArray[num3, 0] = entity2.Name + " - " + Utilities.FP((float) objArray[num3, objArray.GetUpperBound(1)]);
            }
            return objArray;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual frmTransferCash.Input getTransferCash(string playerName)
        {
            object[] args = new object[] { playerName };
            base.LogMethodCall(args);
            frmTransferCash.Input input = new frmTransferCash.Input();
            Entity[] playersEntities = S.State.GetPlayersEntities(playerName);
            input.OwnedEntities = new string[playersEntities.Length];
            input.CashBalances = new float[playersEntities.Length];
            for (int i = 0; i < playersEntities.Length; i++)
            {
                input.OwnedEntities[i] = playersEntities[i].Name;
                input.CashBalances[i] = playersEntities[i].GL.AccountBalance("Cash");
            }
            return input;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public frmVitalSigns.Input getVitalSigns(long entityID)
        {
            Entity entity = SimStateAdapter.CheckEntityExists(entityID);
            frmVitalSigns.Input input = new frmVitalSigns.Input();
            float num = 0f;
            int num2 = 0;
            foreach (Entity entity2 in S.State.Entity.Values)
            {
                if (entity2.Player == entity.Player)
                {
                    num += entity2.Journal.NumericDataSeriesLastEntry("Cumulative Profit");
                    num2++;
                }
            }
            foreach (Entity entity3 in S.State.RetiredEntity.Values)
            {
                if (entity3.Player == entity.Player)
                {
                    num += entity3.Journal.NumericDataSeriesLastEntry("Cumulative Profit");
                    num2++;
                }
            }
            input.CumProfit = num;
            input.MultipleEntities = num2 > 1;
            int num3 = Math.Min(S.State.CurrentWeek, 8);
            input.Profit = new float[num3];
            input.Sales = new float[num3];
            input.Customers = new int[num3];
            int index = 0;
            for (int i = S.State.CurrentWeek - num3; i < S.State.CurrentWeek; i++)
            {
                input.Profit[index] = entity.GL.AccountBalance("Profit", i);
                input.Sales[index] = entity.GL.AccountBalance("Revenue", i);
                input.Customers[index] = (int) entity.GL.AccountBalance("Customers", i);
                index++;
            }
            return input;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void setTransferCash(long fromEntityID, long toEntityID, float amount)
        {
            Entity entity = SimStateAdapter.CheckEntityExists(fromEntityID);
            Entity entity2 = SimStateAdapter.CheckEntityExists(toEntityID);
            if (amount > entity.GL.AccountBalance("Cash"))
            {
                object[] args = new object[] { Utilities.FC(entity.GL.AccountBalance("Cash"), S.Instance.CurrencyConversion) };
                throw new SimApplicationException(S.Resources.GetString("The amount you are trying to transfer is greater than the cash now at the store you are transferring from.  The cash balance of that store is {0}. Try transferring much less or that store will run out of cash.", args));
            }
            entity.GL.Post("Cash", -amount, "Paid-in Capital");
            entity2.GL.Post("Cash", amount, "Paid-in Capital");
            string[] textArray1 = new string[] { "Transferred ", Utilities.FC(amount, S.Instance.CurrencyConversion), " from ", entity.Name, " to ", entity2.Name, "." };
            entity.Journal.AddEntry(string.Concat(textArray1));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void TransferCashFrom(string fromEntity, float amount)
        {
            Entity entityByName = S.State.GetEntityByName(fromEntity);
            if (entityByName == null)
            {
                object[] args = new object[] { fromEntity };
                throw new SimApplicationException(S.Resources.GetString("Can't transfer cash from {0}.", args));
            }
            entityByName.GL.Post("Cash", -amount, "Paid-in Capital");
        }
    }
}

