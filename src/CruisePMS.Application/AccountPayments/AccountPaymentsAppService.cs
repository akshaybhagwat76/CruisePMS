using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CruisePMS.AccountPayments.Dtos;
using CruisePMS.Authorization;
using CruisePMS.Clients;
using CruisePMS.MultiTenancy;
using CruisePMS.Reservations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;

namespace CruisePMS.AccountPayments
{
    [AbpAuthorize(AppPermissions.Pages_AccountPayments)]
    public class AccountPaymentsAppService : CruisePMSAppServiceBase, IAccountPaymentsAppService
    {
        private readonly IRepository<AccountPayment> _accountPaymentsRepository;
        private readonly IRepository<Reservation, long> _reservationsRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<Client, long> _clientsRepository;
        public AccountPaymentsAppService(IRepository<Client, long> clientsRepository, IRepository<Tenant> tenantRepository, IRepository<Reservation, long> reservationsRepository, IRepository<AccountPayment> accountPaymentsRepository, IUnitOfWorkManager unitOfWork)
        {
            _accountPaymentsRepository = accountPaymentsRepository;
            _reservationsRepository = reservationsRepository;
            _unitOfWorkManager = unitOfWork;
            _tenantRepository = tenantRepository;
            _clientsRepository = clientsRepository;
        }

        public async Task<PagedResultDto<GetAccountPaymentsForViewDto>> GetAll(GetAllAccountPaymentsInput input)
        {

            var filteredAccountPayments = _accountPaymentsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CurrencyId.Contains(input.Filter) || e.BankDocumentNo.Contains(input.Filter) || e.PaymentInvoiceId.Contains(input.Filter));

            var pagedAndFilteredAccountPayments = filteredAccountPayments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var accountPayments = from o in pagedAndFilteredAccountPayments

                                  join tenant in _tenantRepository.GetAll() on o.TenantIdPayer equals tenant.Id into tenantJoined
                                  from tenant in tenantJoined.DefaultIfEmpty()

                                  join o1 in _clientsRepository.GetAll() on Convert.ToInt64(o.ClientId) equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  select new GetAccountPaymentsForViewDto()
                                  {
                                      AccountPayments = new AccountPaymentsDto
                                      {
                                          Paid = o.Paid,
                                          CurrencyId = o.CurrencyId,
                                          PaymentDate = o.PaymentDate,
                                          BankDocumentNo = o.BankDocumentNo,
                                          PaymentInvoiceId = o.PaymentInvoiceId,
                                          SupplierInvoiceId = o.SupplierInvoiceId,
                                          SystemInvoiceId = o.SystemInvoiceId,
                                          Id = o.Id,
                                          Client = s1 == null ? "" : s1.ClientFirstName + " " + s1.ClientLastName.ToString(),
                                          Agent = tenant == null ? "" : tenant.Name.ToString()
                                      }
                                  };

            var totalCount = await filteredAccountPayments.CountAsync();

            return new PagedResultDto<GetAccountPaymentsForViewDto>(
                totalCount,
                await accountPayments.ToListAsync()
            );
        }

        public async Task<GetAccountPaymentsForViewDto> GetAccountPaymentsForView(int id)
        {
            var accountPayments = await _accountPaymentsRepository.GetAsync(id);

            var output = new GetAccountPaymentsForViewDto { AccountPayments = ObjectMapper.Map<AccountPaymentsDto>(accountPayments) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AccountPayments_Edit)]
        public async Task<GetAccountPaymentsForEditOutput> GetAccountPaymentsForEdit(EntityDto input)
        {
            var accountPayments = await _accountPaymentsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAccountPaymentsForEditOutput { AccountPayments = ObjectMapper.Map<CreateOrEditAccountPaymentsDto>(accountPayments) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAccountPaymentsDto input)
        {
            if (!input.Id.HasValue)
            {
                await Create(input);
            }
            else if (input.Id == 0)
            {
                await Create(input);
            }
            else if (input.Id > 0)
            {
                await Update(input);
            }
        }


        [AbpAuthorize(AppPermissions.Pages_AccountPayments_Create)]
        protected virtual async Task Create(CreateOrEditAccountPaymentsDto input)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            var accountPayments = ObjectMapper.Map<AccountPayment>(input);
            if (AbpSession.TenantId != null)
            {
                accountPayments.TenantId = (int)AbpSession.TenantId;
            }
            await _accountPaymentsRepository.InsertAsync(accountPayments);
            if (input.ConfirmReservation)
            {
                var tblReservation = _reservationsRepository.GetAll().Where(x => x.Id == input.ReservationId).FirstOrDefault();
                tblReservation.ReservationLocked = true;
                tblReservation.ReservationStatus = 5;
                await _reservationsRepository.UpdateAsync(tblReservation);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AccountPayments_Edit)]
        protected virtual async Task Update(CreateOrEditAccountPaymentsDto input)
        {
            var accountPayments = await _accountPaymentsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, accountPayments);
        }

        [AbpAuthorize(AppPermissions.Pages_AccountPayments_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _accountPaymentsRepository.DeleteAsync(input.Id);
        }

        

    }
}
