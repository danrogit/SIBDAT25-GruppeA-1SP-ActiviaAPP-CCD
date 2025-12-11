using System.Collections.ObjectModel;

namespace ActiviaAPP.Classes
{
    //////Kodet af alle
    public class ActivityStore
    {
        //Static liste over alle aktiviteter - kan bruges af både Admin og User
        public static ObservableCollection<ActivityClass> activities = new ObservableCollection<ActivityClass>();
    }
}
