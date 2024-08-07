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
        public PersonalInfoService(IPersonalInfoRepository personalInfoRepository, IUserService userService)
        {
            _personalInfoRepository = personalInfoRepository;
            _userService = userService;
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
            personalInfo.Result.Description = description;
            PersonalInfoUpdateDTO personalInfoForUpdate = new(){
                Id = personalInfo.Result.Id,
                Email = personalInfo.Result.Email,
                PhoneNumber = personalInfo.Result.PhoneNumber,
                Description = personalInfo.Result.Description, 
                Photo = personalInfo.Result.Photo
            };
            var personalInfoUpdated = await UpdatePersonalInfoAsync(personalInfoForUpdate);
            
            return new(new PersonalInfoGetDTO { });
        }

        public Task<ResponseModel<PersonalInfoGetDTO>> SetOrUpdatePhotoAsync(int personalInfoId, byte[] photo)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<PersonalInfoGetDTO>> UpdatePersonalInfoAsync(PersonalInfoUpdateDTO personalInfoDTO)
        {

            throw new NotImplementedException();
        }
    }
}
