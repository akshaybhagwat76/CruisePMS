using Xamarin.Forms.Internals;

namespace CruisePMS.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}