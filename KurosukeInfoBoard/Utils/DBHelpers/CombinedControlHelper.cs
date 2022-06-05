using KurosukeInfoBoard.Models.SQL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Utils.DBHelpers
{
    public class CombinedControlHelper
    {
        public async Task Init()
        {
            using (var context = new CombinedControlContext())
            {
                try
                {
                    await context.Database.EnsureCreatedAsync();
                }
                catch (Exception ex)
                {
                    DebugHelper.Debugger.WriteErrorLog("Ingnoring DB Migration error.", ex);
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.EnsureCreatedAsync();
                }
            }
        }

        public ObservableCollection<CombinedControlEntity> GetCombinedControls()
        {
            using (var context = new CombinedControlContext())
            {
                return new ObservableCollection<CombinedControlEntity>(context.CombinedControls);
            }
        }

        /// <summary>
        /// Add or Update Combined Control setting item
        /// </summary>
        /// <param name="combinedControl">Combined Control item newly created or retrieved from DB and updated.</param>
        /// <returns></returns>
        public async Task AddUpdateCombinedControl(CombinedControlEntity combinedControl)
        {
            using (var context = new CombinedControlContext())
            {
                var match = from item in context.CombinedControls
                            where item.Id == combinedControl.Id
                            select item;
                if (match.Any())
                {
                    context.Update(combinedControl);
                }
                else
                {
                    context.Add(combinedControl);
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveCombindControl(CombinedControlEntity combinedControl)
        {
            using (var context = new CombinedControlContext())
            {
                context.Remove(combinedControl);
                await context.SaveChangesAsync();
            }
        }
    }
}
