using CruisePMS.CruiseMasterAmenities;
using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CruisePMS.Authorization.Delegation;
using CruisePMS.Authorization.Roles;
using CruisePMS.Authorization.Users;
using CruisePMS.Chat;
using CruisePMS.Editions;
using CruisePMS.Friendships;
using CruisePMS.MultiTenancy;
using CruisePMS.MultiTenancy.Accounting;
using CruisePMS.MultiTenancy.Payments;
using CruisePMS.Storage;
using CruisePMS.CruiseTenantTypes;
using CruisePMS.CruiseItineraries;
using CruisePMS.CruiseTenantTypesPermissions;
using CruisePMS.Cruises;
using CruisePMS.CruiseServiceGroups;
using CruisePMS.CruiseServices;
using CruisePMS.CruiseServiceUnits;
using CruisePMS.CruiseShipCategories;
using CruisePMS.CruiseShips;
using CruisePMS.CruiseThemes;
using CruisePMS.CruiseDefaultSeasons;
using CruisePMS.AccountPayments;
using CruisePMS.Clients;
using CruisePMS.Reservations;
using CruisePMS.AgePolicies;
using CruisePMS.BedOptions;
using CruisePMS.CancellationPolicies;
using CruisePMS.Localization;
using CruisePMS.ReservationsClients;
using CruisePMS.AmenityStorages;
using CruisePMS.BookingStatuses;
using CruisePMS.CruiseBookingStatuses;
using CruisePMS.Contracts;
using CruisePMS.Common;
using CruisePMS.ContractCommissions;
using CruisePMS.MainServices;
using CruisePMS.CruiseCabinAmenitiesGroups;
using CruisePMS.CruiseCabinAmenities;
using CruisePMS.CruiseDepartures;

namespace CruisePMS.EntityFrameworkCore
{
    public class CruisePMSDbContext : AbpZeroDbContext<Tenant, Role, User, CruisePMSDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<MasterAmenities> MasterAmenitieses { get; set; }
        public virtual DbSet<AccountPayment> AccountPayments { get; set; }
        public virtual DbSet<AgePolicy> AgePolicies { get; set; }
        public virtual DbSet<BedOption> BedOptions { get; set; }
        public virtual DbSet<ip2location_country_multilingual> ip2location_country_multilingual { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<ReadSettings> ReadSettings { get; set; }
        public virtual DbSet<ContractCommission> ContractCommissions { get; set; }
        public virtual DbSet<MainService> MainServices { get; set; }
        public virtual DbSet<CruiseCabinAmenity> CruiseCabinAmenities { get; set; }
        public virtual DbSet<CruiseCabinAmenitiesGroup> CruiseCabinAmenitiesGroups { get; set; }
        public virtual DbSet<AmenityStorage> AmenityStorages { get; set; }
        public virtual DbSet<ReservationsClient> ReservationsClients { get; set; }
        public virtual DbSet<TenantTypes> TenantTypes { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<BookingStatus> BookingStatuses { get; set; }
        public virtual DbSet<CruiseBookingStatus> CruiseBookingStatuses { get; set; }
        public virtual DbSet<CruiseDeparture> CruiseDepartures { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<CruiseItinerary> CruiseItineraries { get; set; }
        public virtual DbSet<CancellationPolicy> CancellationPolicies { get; set; }

        public virtual DbSet<CruiseDefaultSeason> CruiseDefaultSeasons { get; set; }
        public virtual DbSet<TenantTypesPermissions> TenantTypesPermissions { get; set; }
        public virtual DbSet<Cruise> Cruises { get; set; }
        public virtual DbSet<CruiseServiceGroup> CruiseServiceGroups { get; set; }
        public virtual DbSet<CruiseService> CruiseServices { get; set; }
        public virtual DbSet<CruiseServiceUnit> CruiseServiceUnits { get; set; }
        public virtual DbSet<CruiseShipCategory> CruiseShipCategories { get; set; }
        public virtual DbSet<CruiseShip> CruiseShips { get; set; }
        public virtual DbSet<CruiseTheme> CruiseThemes { get; set; }


        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public CruisePMSDbContext(DbContextOptions<CruisePMSDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
