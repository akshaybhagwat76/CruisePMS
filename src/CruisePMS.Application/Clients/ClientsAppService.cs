using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using CruisePMS.Authorization;
using CruisePMS.Clients.Dtos;
using CruisePMS.Common.Dto;
using CruisePMS.Configuration;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.Localization;
using CruisePMS.ReservationsClients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruisePMS.Clients
{
    [AbpAuthorize(AppPermissions.Pages_Clients)]
    public class ClientsAppService : CruisePMSAppServiceBase, IClientsAppService
    {
        private readonly IRepository<Client, long> _clientsRepository;
        private readonly IRepository<MasterAmenities, int> _lookup_cruiseMasterAmenitiesRepository;
        private readonly IRepository<ip2location_country_multilingual> _countrysRepository;
        private readonly IRepository<ReservationsClient, long> _reservationsClientsRepository;

        public ClientsAppService(IRepository<ReservationsClient, long> reservationsClientsRepository, IRepository<Client, long> clientsRepository, IRepository<MasterAmenities, int> lookup_cruiseMasterAmenitiesRepository, IRepository<ip2location_country_multilingual> countryRepository)
        {
            _reservationsClientsRepository = reservationsClientsRepository;
            _clientsRepository = clientsRepository;
            _lookup_cruiseMasterAmenitiesRepository = lookup_cruiseMasterAmenitiesRepository;
            _countrysRepository = countryRepository;
        }

        public async Task<PagedResultDto<GetClientsForViewDto>> GetAll(GetAllClientsInput input)
        {
            var filteredClients = _clientsRepository.GetAll();
            string defaultCurrentLanguage = await SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier());
            if (string.IsNullOrWhiteSpace(defaultCurrentLanguage))
            { defaultCurrentLanguage = "EN"; }


            var pagedAndFilteredClients = filteredClients
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var clients = from o in pagedAndFilteredClients
                          join o1 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.ClientGender equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty().Where(x => x.Lang.ToUpper() == defaultCurrentLanguage.ToUpper())

                          join o2 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.ClientDocumentID equals o2.Id into j2
                          from s2 in j2.DefaultIfEmpty().Where(x => x.Lang.ToUpper() == defaultCurrentLanguage.ToUpper())

                              //join o3 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.ClientDocumentNo equals o3.Id into j3
                              //from s3 in j3.DefaultIfEmpty()

                          select new GetClientsForViewDto()
                          {
                              Clients = new ClientsDto
                              {
                                  ReservationHolderID = o.ReservationHolderID,
                                  ClientFirstName = o.ClientFirstName,
                                  ClientLastName = o.ClientLastName,
                                  ClientDOB = o.ClientDOB,
                                  WeddingAniversary = o.WeddingAniversary,
                                  ClientCountry = o.ClientCountry,
                                  ClientGoogleAddress = o.ClientGoogleAddress,
                                  ClientEmail = o.ClientEmail,
                                  ContactCellPhone = o.ContactCellPhone,
                                  Issued = o.Issued,
                                  Expiration = o.Expiration,
                                  CountryResidence = o.CountryResidence,
                                  MedicalIssues = o.MedicalIssues,
                                  FoodIssues = o.FoodIssues,
                                  ClientPassword = o.ClientPassword,
                                  IsClientLoginWith = o.IsClientLoginWith,
                                  LoginFailAttempt = o.LoginFailAttempt,
                                  IsLocked = o.IsLocked,
                                  ProfilePictureId = o.ProfilePictureId,
                                  Id = o.Id
                              },
                              CruiseMasterAmenitiesDisplayName = s1 == null ? "" : s1.DisplayName.ToString(),
                              CruiseMasterAmenitiesDisplayName2 = s2 == null ? "" : s2.DisplayName.ToString(),
                              // CruiseMasterAmenitiesDisplayName3 = s3 == null ? "" : s3.DisplayName.ToString()
                          };

            var totalCount = await filteredClients.CountAsync();

            return new PagedResultDto<GetClientsForViewDto>(
                totalCount,
                await clients.ToListAsync()
            );
        }

        public async Task<GetClientsForViewDto> GetClientsForView(int id)
        {
            var clients = await _clientsRepository.GetAsync(id);

            var output = new GetClientsForViewDto { Clients = ObjectMapper.Map<ClientsDto>(clients) };

            if (output.Clients.ClientGender != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.Clients.ClientGender);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            if (output.Clients.ClientDocumentID != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.Clients.ClientDocumentID);
                output.CruiseMasterAmenitiesDisplayName2 = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            if (output.Clients.ClientDocumentNo != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.Clients.ClientDocumentNo);
                output.CruiseMasterAmenitiesDisplayName3 = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Clients_Edit)]
        public async Task<GetClientsForEditOutput> GetClientsForEdit(EntityDto input)
        {
            var clients = await _clientsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetClientsForEditOutput { Clients = ObjectMapper.Map<CreateOrEditClientsDto>(clients) };

            if (output.Clients.ClientGender != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.Clients.ClientGender);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            if (output.Clients.ClientDocumentID != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.Clients.ClientDocumentID);
                output.CruiseMasterAmenitiesDisplayName2 = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditClientsDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Clients_Create)]
        private async Task Create(CreateOrEditClientsDto input)
        {
            var clients = ObjectMapper.Map<Client>(input);


            if (AbpSession.TenantId != null)
            {
                clients.TenantId = (int?)AbpSession.TenantId;
            }


            await _clientsRepository.InsertAsync(clients);
        }

        [AbpAuthorize(AppPermissions.Pages_Clients_Edit)]
        private async Task Update(CreateOrEditClientsDto input)
        {
            var clients = await _clientsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, clients);
        }

        [AbpAuthorize(AppPermissions.Pages_Clients_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _clientsRepository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<ClientsCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input)
        {
            string defaultCurrentLanguage = await SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier());
            if (string.IsNullOrWhiteSpace(defaultCurrentLanguage))
            { defaultCurrentLanguage = "EN"; }

            var query = _lookup_cruiseMasterAmenitiesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName.ToString().Contains(input.Filter)
               ).Where(x => x.ParentId == input.Parentid && x.Lang.ToUpper() == defaultCurrentLanguage.ToUpper());

            var totalCount = await query.CountAsync();

            var cruiseMasterAmenitiesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ClientsCruiseMasterAmenitiesLookupTableDto>();
            foreach (var cruiseMasterAmenities in cruiseMasterAmenitiesList)
            {
                lookupTableDtoList.Add(new ClientsCruiseMasterAmenitiesLookupTableDto
                {
                    Id = cruiseMasterAmenities.Id,
                    DisplayName = cruiseMasterAmenities.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<ClientsCruiseMasterAmenitiesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        [AbpAuthorize(AppPermissions.Pages_Clients)]
        public async Task<PagedResultDto<ClientsLookupTableDto>> GetAllClientsForLookupTable(GetAllForLookupTableInput input)
        {
            string defaultLanguage = "en";
            string currentLang = await SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier());
            if (!string.IsNullOrWhiteSpace(currentLang))
            {
                defaultLanguage = currentLang;
            }


            var query = _clientsRepository.GetAll().Join(_countrysRepository.GetAll(), cu => cu.ClientCountry,
                            p => p.country_alpha2_code, (cu, p) => new { cu, p })
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                            e => e.cu.ClientFirstName.ToString().Contains(input.Filter)
                            || e.cu.ClientLastName.ToString().Contains(input.Filter) || e.cu.ClientEmail.ToString().Contains(input.Filter)
                            || e.p.country_name.ToString().Contains(input.Filter)
                            || (e.cu.ClientFirstName.ToString() + " " + e.cu.ClientLastName.ToString()).Contains(input.Filter)
                            ).Where(x => x.p.lang.ToUpper() == defaultLanguage.ToUpper())
                            .Select(x => new ClientsDetails
                            {
                                ClientEmail = x.cu.ClientEmail,
                                CountryName = x.p.country_name,
                                ClientName = x.cu.ClientFirstName + " " + x.cu.ClientLastName,
                                DOB = x.cu.ClientDOB,
                                Age = DateTime.Now.Year - x.cu.ClientDOB.Year,
                                Id = x.cu.Id,

                            });

            query = query.Where(x => x.Age > 18);


            var totalCount = await query.CountAsync();

            var clientList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ClientsLookupTableDto>();
            foreach (var client in clientList)
            {
                lookupTableDtoList.Add(new ClientsLookupTableDto
                {
                    Id = client.Id,
                    DisplayName = client.ClientName,
                    Email = client.ClientEmail,
                    Country = client.CountryName
                });
            }

            return new PagedResultDto<ClientsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        [AbpAuthorize(AppPermissions.Pages_Clients)]
        public async Task<PagedResultDto<ClientsLookupTableDto>> GetAllPassengerForLookupTable(GetAllForLookupTableInput input)
        {
            string defaultLanguage = "en";
            string currentLang = await SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier());
            if (!string.IsNullOrWhiteSpace(currentLang))
            {
                defaultLanguage = currentLang;
            }


            var query = _clientsRepository.GetAll().Join(_countrysRepository.GetAll(), cu => cu.ClientCountry,
                            p => p.country_alpha2_code, (cu, p) => new { cu, p })
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                            e => e.cu.ClientFirstName.ToString().Contains(input.Filter)
                            || e.cu.ClientLastName.ToString().Contains(input.Filter) || e.cu.ClientEmail.ToString().Contains(input.Filter)
                            || e.p.country_name.ToString().Contains(input.Filter)
                            || (e.cu.ClientFirstName.ToString() + " " + e.cu.ClientLastName.ToString()).Contains(input.Filter)
                            ).Where(x => x.p.lang.ToUpper() == defaultLanguage.ToUpper())
                            .Select(x => new ClientsDetails
                            {
                                ClientEmail = x.cu.ClientEmail,
                                CountryName = x.p.country_name,
                                ClientName = x.cu.ClientFirstName + " " + x.cu.ClientLastName,
                                DOB = x.cu.ClientDOB,
                                Id = x.cu.Id,

                            });

            var clientListTemp = query.ToList();

            var reservationClients = _reservationsClientsRepository.GetAll().Where(x => x.ReservationId == input.ReservationId && x.CabinIdentificator == input.CabinIdentificator);
            foreach (var client in reservationClients)
            {

                var itemIndex = clientListTemp.FindIndex(x => x.Id == client.ClientId);
                if (itemIndex >= 0)
                {
                    var item = clientListTemp.ElementAt(itemIndex);
                    item.IsSelectDisable = true;
                    if (client.ReservationHolder)
                    {
                        item.IsReservationHolder = true;
                    }
                }
            }
            query = clientListTemp.AsQueryable();

            var totalCount = query.Count();

            var clientList = query
                .PageBy(input)
                .ToList();

            var lookupTableDtoList = new List<ClientsLookupTableDto>();
            foreach (var client in clientList)
            {
                lookupTableDtoList.Add(new ClientsLookupTableDto
                {
                    Id = client.Id,
                    DisplayName = client.ClientName,
                    Email = client.ClientEmail,
                    Country = client.CountryName,
                    IsSelectDisable = client.IsSelectDisable,
                    IsReservationHolder = client.IsReservationHolder
                });
            }

            return new PagedResultDto<ClientsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
