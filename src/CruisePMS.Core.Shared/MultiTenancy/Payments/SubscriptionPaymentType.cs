namespace CruisePMS.MultiTenancy.Payments
{
    public enum SubscriptionPaymentType
    {
        Manual = 0,
        RecurringAutomatic = 1,
        RecurringManual = 2
    }


    public enum TenantType
    {
        Cruise = 0,
        Ship = 1,
        Travel = 2
    }
}