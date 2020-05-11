using System;
using System.Collections.Generic;
using System.Text;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.IO;
using System.Threading.Tasks;
using CruisePMS.CruisePhotos.Dtos;
using CruisePMS.Authorization;
using CruisePMS.CruiseItineraryDetails.Dtos;
using Abp.Application.Services.Dto;
using CruisePMS.CruiseMasterAmenities;
using Abp.Domain.Repositories;
using CruisePMS.Common.Dto;

namespace CruisePMS.CruisePhotos
{
    [AbpAuthorize(AppPermissions.Pages_CruisePhotos)]
    public class CruisePhotosAppService : CruisePMSAppServiceBase, ICruisePhotosAppService
    {
        private readonly IRepository<CruisePhoto, long> _cruisePhotosRepository;
        private readonly IRepository<MasterAmenities, int> _lookup_cruiseMasterAmenitiesRepository;


        public CruisePhotosAppService(IRepository<CruisePhoto, long> cruisePhotosRepository, IRepository<MasterAmenities, int> lookup_cruiseMasterAmenitiesRepository)
        {
            _cruisePhotosRepository = cruisePhotosRepository;
            _lookup_cruiseMasterAmenitiesRepository = lookup_cruiseMasterAmenitiesRepository;

        }

        public async Task<PagedResultDto<GetCruisePhotosForViewDto>> GetAll(GetAllCruisePhotosInput input)
        {

            var filteredCruisePhotos = _cruisePhotosRepository.GetAll()
                        .Include(e => e.PhotoNameFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PhotoSource.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseMasterAmenitiesDisplayNameFilter), e => e.PhotoNameFk != null && e.PhotoNameFk.DisplayName.ToLower() == input.CruiseMasterAmenitiesDisplayNameFilter.ToLower().Trim())
                        .Where(x => x.PhotoSource == input.SourceName && x.PhotoSourceId == input.SourceId);

            var pagedAndFilteredCruisePhotos = filteredCruisePhotos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cruisePhotos = from o in pagedAndFilteredCruisePhotos
                               join o1 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.PhotoNameId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               select new GetCruisePhotosForViewDto()
                               {
                                   CruisePhotos = new CruisePhotosDto
                                   {
                                       PhotoSource = o.PhotoSource,
                                       Photo1 = "data:image/png;base64," + Convert.ToBase64String(CreateThumbnail(o.Photo5, 150)),
                                       Id = o.Id
                                   },
                                   CruiseMasterAmenitiesDisplayName = s1 == null ? "" : s1.DisplayName.ToString()
                               };

            var totalCount = await filteredCruisePhotos.CountAsync();

            return new PagedResultDto<GetCruisePhotosForViewDto>(
                totalCount,
                await cruisePhotos.ToListAsync()
            );
        }

        public async Task<GetCruisePhotosForViewDto> GetCruisePhotosForView(long id)
        {
            var cruisePhotos = await _cruisePhotosRepository.GetAsync(id);

            var output = new GetCruisePhotosForViewDto { CruisePhotos = ObjectMapper.Map<CruisePhotosDto>(cruisePhotos) };

            if (output.CruisePhotos.PhotoNameId != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CruisePhotos.PhotoNameId);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePhotos_Edit)]
        public async Task<GetCruisePhotosForEditOutput> GetCruisePhotosForEdit(EntityDto<long> input)
        {
            var cruisePhotos = await _cruisePhotosRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCruisePhotosForEditOutput { CruisePhotos = ObjectMapper.Map<CreateOrEditCruisePhotosDto>(cruisePhotos) };

            if (output.CruisePhotos.PhotoNameId != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CruisePhotos.PhotoNameId);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCruisePhotosDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CruisePhotos_Create)]
        private async Task Create(CreateOrEditCruisePhotosDto input)
        {
            var cruisePhotos = ObjectMapper.Map<CruisePhoto>(input);


            if (AbpSession.TenantId != null)
            {
                cruisePhotos.TenantId = (int?)AbpSession.TenantId;
            }


            await _cruisePhotosRepository.InsertAsync(cruisePhotos);
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePhotos_Edit)]
        private async Task Update(CreateOrEditCruisePhotosDto input)
        {
            var cruisePhotos = await _cruisePhotosRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, cruisePhotos);
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePhotos_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _cruisePhotosRepository.DeleteAsync(input.Id);
        }



        [AbpAuthorize(AppPermissions.Pages_CruisePhotos)]
        public async Task<PagedResultDto<CruisePhotosCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseMasterAmenitiesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName.ToString().Contains(input.Filter)
               ).Where(x => x.ParentId == 83);

            var totalCount = await query.CountAsync();

            var cruiseMasterAmenitiesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruisePhotosCruiseMasterAmenitiesLookupTableDto>();
            foreach (var cruiseMasterAmenities in cruiseMasterAmenitiesList)
            {
                lookupTableDtoList.Add(new CruisePhotosCruiseMasterAmenitiesLookupTableDto
                {
                    Id = cruiseMasterAmenities.Id,
                    DisplayName = cruiseMasterAmenities.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<CruisePhotosCruiseMasterAmenitiesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }



        [AbpAuthorize(AppPermissions.Pages_CruisePhotos_Create)]
        public async Task SaveCruisePhotos(List<SaveCruisePhotos> collectionList)
        {

            foreach (var item in collectionList)
            {
                var imageParts = item.Filbase64.Split(',').ToList<string>();

                byte[] imageBytes = Convert.FromBase64String(imageParts[1]);
                CruisePhoto cruisePhotos = new CruisePhoto();
                if (AbpSession.TenantId != null)
                {
                    cruisePhotos.TenantId = (int?)AbpSession.TenantId;
                }
                cruisePhotos.Photo1 = CreateThumbnail(imageBytes, 192);
                cruisePhotos.Photo2 = CreateThumbnail(imageBytes, 368);
                cruisePhotos.Photo3 = CreateThumbnail(imageBytes, 896);
                cruisePhotos.Photo4 = CreateThumbnail(imageBytes, 1072);
                cruisePhotos.Photo5 = CreateThumbnail(imageBytes, 1920);
                //cruisePhotos.Photo1 = imageBytes ConvertFileWidthHeight(192, 108, imageBytes);
                //cruisePhotos.Photo2 = imageBytes ConvertFileWidthHeight(368, 207, imageBytes);
                //cruisePhotos.Photo3 = imageBytes ConvertFileWidthHeight(896, 504, imageBytes);
                //cruisePhotos.Photo4 = imageBytes ConvertFileWidthHeight(1072, 603, imageBytes);
                //cruisePhotos.Photo5 = imageBytes ConvertFileWidthHeight(1920, 1080, imageBytes);
                cruisePhotos.PhotoNameId = item.PhotoNameId;
                cruisePhotos.PhotoSource = item.PhotoSource;
                cruisePhotos.PhotoSourceId = item.PhotoSourceId;
                await _cruisePhotosRepository.InsertAsync(cruisePhotos);
            }
        }

        // (RESIZE an image in a byte[] variable.)  
        public static byte[] CreateThumbnail(byte[] PassedImage, int LargestSide)
        {
            byte[] ReturnedThumbnail;

            using (MemoryStream StartMemoryStream = new MemoryStream(),
                                NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }

        // Resize a Bitmap  
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        }





    }
}
