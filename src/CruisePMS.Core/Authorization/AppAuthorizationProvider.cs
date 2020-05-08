using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace CruisePMS.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var masterAmenitieses = pages.CreateChildPermission(AppPermissions.Pages_MasterAmenitieses, L("MasterAmenitieses"), multiTenancySides: MultiTenancySides.Host);
            masterAmenitieses.CreateChildPermission(AppPermissions.Pages_MasterAmenitieses_Create, L("CreateNewMasterAmenities"), multiTenancySides: MultiTenancySides.Host);
            masterAmenitieses.CreateChildPermission(AppPermissions.Pages_MasterAmenitieses_Edit, L("EditMasterAmenities"), multiTenancySides: MultiTenancySides.Host);
            masterAmenitieses.CreateChildPermission(AppPermissions.Pages_MasterAmenitieses_Delete, L("DeleteMasterAmenities"), multiTenancySides: MultiTenancySides.Host);


            var cruiseContract = pages.CreateChildPermission(AppPermissions.Pages_CruiseContract, L("CruiseContract"));
            cruiseContract.CreateChildPermission(AppPermissions.Pages_CruiseContract_Create, L("CreateNewCruiseContract"));
            cruiseContract.CreateChildPermission(AppPermissions.Pages_CruiseContract_Edit, L("EditCruiseContract"));
            cruiseContract.CreateChildPermission(AppPermissions.Pages_CruiseContract_Delete, L("DeleteCruiseContract"));



            var cruiseContractCommissions = pages.CreateChildPermission(AppPermissions.Pages_CruiseContractCommissions, L("CruiseContractCommissions"));
            cruiseContractCommissions.CreateChildPermission(AppPermissions.Pages_CruiseContractCommissions_Create, L("CreateNewCruiseContractCommissions"));
            cruiseContractCommissions.CreateChildPermission(AppPermissions.Pages_CruiseContractCommissions_Edit, L("EditCruiseContractCommissions"));
            cruiseContractCommissions.CreateChildPermission(AppPermissions.Pages_CruiseContractCommissions_Delete, L("DeleteCruiseContractCommissions"));




            var clients = pages.CreateChildPermission(AppPermissions.Pages_Clients, L("Clients"));
            clients.CreateChildPermission(AppPermissions.Pages_Clients_Create, L("CreateNewClients"));
            clients.CreateChildPermission(AppPermissions.Pages_Clients_Edit, L("EditClients"));
            clients.CreateChildPermission(AppPermissions.Pages_Clients_Delete, L("DeleteClients"));

            var cruiseAmenitiesStorage = pages.CreateChildPermission(AppPermissions.Pages_CruiseAmenitiesStorage, L("CruiseAmenitiesStorage"));
            cruiseAmenitiesStorage.CreateChildPermission(AppPermissions.Pages_CruiseAmenitiesStorage_Create, L("CreateNewCruiseAmenitiesStorage"));
            cruiseAmenitiesStorage.CreateChildPermission(AppPermissions.Pages_CruiseAmenitiesStorage_Edit, L("EditCruiseAmenitiesStorage"));
            cruiseAmenitiesStorage.CreateChildPermission(AppPermissions.Pages_CruiseAmenitiesStorage_Delete, L("DeleteCruiseAmenitiesStorage"));


            var tenantTypes = pages.CreateChildPermission(AppPermissions.Pages_TenantTypes, L("TenantTypes"), multiTenancySides: MultiTenancySides.Host);
            tenantTypes.CreateChildPermission(AppPermissions.Pages_TenantTypes_Create, L("CreateNewTenantTypes"), multiTenancySides: MultiTenancySides.Host);
            tenantTypes.CreateChildPermission(AppPermissions.Pages_TenantTypes_Edit, L("EditTenantTypes"), multiTenancySides: MultiTenancySides.Host);
            tenantTypes.CreateChildPermission(AppPermissions.Pages_TenantTypes_Delete, L("DeleteTenantTypes"), multiTenancySides: MultiTenancySides.Host);

            var agePolicies = pages.CreateChildPermission(AppPermissions.Pages_AgePolicies, L("AgePolicies"));
            agePolicies.CreateChildPermission(AppPermissions.Pages_AgePolicies_Create, L("CreateNewAgePolicies"));
            agePolicies.CreateChildPermission(AppPermissions.Pages_AgePolicies_Edit, L("EditAgePolicies"));
            agePolicies.CreateChildPermission(AppPermissions.Pages_AgePolicies_Delete, L("DeleteAgePolicies"));

            var cancellationPolicy = pages.CreateChildPermission(AppPermissions.Pages_CancellationPolicy, L("CancellationPolicy"));
            cancellationPolicy.CreateChildPermission(AppPermissions.Pages_CancellationPolicy_Create, L("CreateNewCancellationPolicy"));
            cancellationPolicy.CreateChildPermission(AppPermissions.Pages_CancellationPolicy_Edit, L("EditCancellationPolicy"));
            cancellationPolicy.CreateChildPermission(AppPermissions.Pages_CancellationPolicy_Delete, L("DeleteCancellationPolicy"));


            var cruisesPerDeparture = pages.CreateChildPermission(AppPermissions.Pages_CruisesAllDeparture, L("CruisesAllDeparture"));
            cruisesPerDeparture.CreateChildPermission(AppPermissions.Pages_CruisesAllDeparture_Create, L("CreateNewCruisesAllDeparture"));
            cruisesPerDeparture.CreateChildPermission(AppPermissions.Pages_CruisesAllDeparture_Edit, L("EditCruisesAllDeparture"));
            cruisesPerDeparture.CreateChildPermission(AppPermissions.Pages_CruisesAllDeparture_Delete, L("DeleteCruisesAllDeparture"));

            var cruiseDefaultSeasons = pages.CreateChildPermission(AppPermissions.Pages_CruiseDefaultSeasons, L("CruiseDefaultSeasons"));
            cruiseDefaultSeasons.CreateChildPermission(AppPermissions.Pages_CruiseDefaultSeasons_Create, L("CreateNewCruiseDefaultSeasons"));
            cruiseDefaultSeasons.CreateChildPermission(AppPermissions.Pages_CruiseDefaultSeasons_Edit, L("EditCruiseDefaultSeasons"));
            cruiseDefaultSeasons.CreateChildPermission(AppPermissions.Pages_CruiseDefaultSeasons_Delete, L("DeleteCruiseDefaultSeasons"));



            var bedOptions = pages.CreateChildPermission(AppPermissions.Pages_BedOptions, L("BedOptions"), multiTenancySides: MultiTenancySides.Host);
            bedOptions.CreateChildPermission(AppPermissions.Pages_BedOptions_Create, L("CreateNewBedOptions"), multiTenancySides: MultiTenancySides.Host);
            bedOptions.CreateChildPermission(AppPermissions.Pages_BedOptions_Edit, L("EditBedOptions"), multiTenancySides: MultiTenancySides.Host);
            bedOptions.CreateChildPermission(AppPermissions.Pages_BedOptions_Delete, L("DeleteBedOptions"), multiTenancySides: MultiTenancySides.Host);


            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));



            var cruiseBookingStatus = pages.CreateChildPermission(AppPermissions.Pages_CruiseBookingStatus, L("CruiseBookingStatus"));
            cruiseBookingStatus.CreateChildPermission(AppPermissions.Pages_CruiseBookingStatus_Create, L("CreateNewCruiseBookingStatus"));
            cruiseBookingStatus.CreateChildPermission(AppPermissions.Pages_CruiseBookingStatus_Edit, L("EditCruiseBookingStatus"));
            cruiseBookingStatus.CreateChildPermission(AppPermissions.Pages_CruiseBookingStatus_Delete, L("DeleteCruiseBookingStatus"));



            var dynamicParameters = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters, L("DynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Create, L("CreatingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Edit, L("EditingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Delete, L("DeletingDynamicParameters"));

            var dynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue, L("DynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Create, L("CreatingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Edit, L("EditingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Delete, L("DeletingDynamicParameterValue"));

            var entityDynamicParameters = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters, L("EntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Create, L("CreatingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Edit, L("EditingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Delete, L("DeletingEntityDynamicParameters"));

            var entityDynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue, L("EntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create, L("CreatingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit, L("EditingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Delete, L("DeletingEntityDynamicParameterValue"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"));
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription") );

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, CruisePMSConsts.LocalizationSourceName);
        }
    }
}
