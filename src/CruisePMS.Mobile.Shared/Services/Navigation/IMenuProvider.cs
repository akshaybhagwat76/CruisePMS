using System.Collections.Generic;
using MvvmHelpers;
using CruisePMS.Models.NavigationMenu;

namespace CruisePMS.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}