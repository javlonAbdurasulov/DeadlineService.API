using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Services
{
    public class PersonalInfoService : IPersonalInfoService
    {
        private readonly IPersonalInfoRepository _personalInfoRepository;
        private readonly IUserService _userService;
        private readonly IRedisCacheService _cache;
        public PersonalInfoService(IPersonalInfoRepository personalInfoRepository, IUserService userService, IRedisCacheService cache)
        {
            _personalInfoRepository = personalInfoRepository;
            _userService = userService;
            _cache = cache;
        }
        public async Task<ResponseModel<PersonalInfoGetDTO>> CreatePersonalInfoAsync(PersonalInfoCreateDTO personalInfoDTO)
        {
            var userGetById = await _userService.GetUserByIdAsync(personalInfoDTO.UserId);
            if(userGetById.Result == null)
            {
                return new(userGetById.Error);
            }else if (userGetById.Result.PersonalInfoId!=null)
            {
                return new("User уже имеет PersonalInfo, невозможно создать!");
            }
            PersonalInfo personalInfo = new(personalInfoDTO.Email,
                personalInfoDTO.PhoneNumber, personalInfoDTO.UserId,
                personalInfoDTO.Description, personalInfoDTO.Photo);
            
            personalInfo = await _personalInfoRepository.CreateAsync(personalInfo);

            PersonalInfoGetDTO personalInfoGetDTO = new PersonalInfoGetDTO()
            {
                CreateAt = personalInfo.CreateAt,
                Email = personalInfo.Email,
                PhoneNumber = personalInfo.PhoneNumber,
                UserId = personalInfo.UserId,
                Description = personalInfo.Description,
                Photo = personalInfo.Photo,
                Id = personalInfo.Id
            };
            return new(personalInfoGetDTO);
        }

        public async Task<ResponseModel<PersonalInfoGetDTO>> GetPersonalInfoByIdAsync(int Id)
        {
            PersonalInfo? personalInfo = await _personalInfoRepository.GetById(Id);
            if(personalInfo == null)
            {
                return new("PersonalInfo с таким Id не найден!");
            }
            PersonalInfoGetDTO personalInfoGetDTO = new()
            {
                Id = personalInfo.Id,
                CreateAt = personalInfo.CreateAt,
                Email = personalInfo.Email,
                PhoneNumber = personalInfo.PhoneNumber,
                UserId = personalInfo.UserId,
                Description = personalInfo.Description,
                Photo = personalInfo.Photo
            };
            return new(personalInfoGetDTO);
        }

        public async Task<ResponseModel<PersonalInfoGetDTO>> SetOrUpdateDescriptionAsync(int personalInfoId,string description)
        {
            var personalInfo = await GetPersonalInfoByIdAsync(personalInfoId);
            if(personalInfo.Result == null)
            {
                return new(personalInfo.Error);
            }
            PersonalInfoUpdateDTO personalInfoForUpdate = new(){
                Id = personalInfo.Result.Id,
                Email = personalInfo.Result.Email,
                PhoneNumber = personalInfo.Result.PhoneNumber,
                Description = description, 
                Photo = personalInfo.Result.Photo
            };
            var personalInfoUpdated = await UpdatePersonalInfoAsync(personalInfoForUpdate);
            if(personalInfoUpdated.Result == null)
            {
                return new(personalInfoUpdated.Error);
            }
            return new(personalInfoUpdated.Result);
        }

        public async Task<ResponseModel<PersonalInfoGetDTO>> SetOrUpdatePhotoAsync(int personalInfoId, byte[] photo)
        {
            var personalInfo = await GetPersonalInfoByIdAsync(personalInfoId);
            if (personalInfo.Result == null)
            {
                return new(personalInfo.Error);
            }
            PersonalInfoUpdateDTO personalInfoForUpdate = new()
            {
                Id = personalInfo.Result.Id,
                Email = personalInfo.Result.Email,
                PhoneNumber = personalInfo.Result.PhoneNumber,
                Description = personalInfo.Result.Description,
                Photo = photo
            };
            var personalInfoUpdated = await UpdatePersonalInfoAsync(personalInfoForUpdate);
            if (personalInfoUpdated.Result == null)
            {
                return new(personalInfoUpdated.Error);
            }
            return new(personalInfoUpdated.Result);
        }

        public async Task<ResponseModel<PersonalInfoGetDTO>> UpdatePersonalInfoAsync(PersonalInfoUpdateDTO personalInfoDTO)
        {
            var personalInfoGetById = await GetPersonalInfoByIdAsync(personalInfoDTO.Id);
            if(personalInfoGetById.Result == null)
            {
                return new(personalInfoGetById.Error);
            }
            PersonalInfo personalInfo = new()
            {
                Id = personalInfoDTO.Id,
                Email = personalInfoDTO.Email,
                PhoneNumber = personalInfoDTO.PhoneNumber,
                Description = personalInfoDTO.Description,
                Photo = personalInfoDTO.Photo,
                CreateAt = personalInfoGetById.Result.CreateAt,
                UserId = personalInfoGetById.Result.UserId
            };
            personalInfo = await _personalInfoRepository.UpdateAsync(personalInfo);
            PersonalInfoGetDTO personalInfoGetDTO = new()
            {
                Id = personalInfo.Id,
                CreateAt = personalInfo.CreateAt,
                Email = personalInfo.Email,
                PhoneNumber = personalInfo.PhoneNumber,
                UserId = personalInfo.UserId,
                Description = personalInfo.Description,
                Photo = personalInfo.Photo
            };
            return new(personalInfoGetDTO);
        }
    }
}
