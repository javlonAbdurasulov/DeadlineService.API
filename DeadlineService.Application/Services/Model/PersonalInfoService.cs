using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Application.Interfaces.Services;

namespace DeadlineService.Application.Services.Model
{
    public class PersonalInfoService : IPersonalInfoService
    {
        private readonly IPersonalInfoRepository _personalInfoRepository;
        private readonly IRedisCacheService _cache;
        private readonly IUserRepository _userRepository;
        public PersonalInfoService(IPersonalInfoRepository personalInfoRepository, IRedisCacheService cache, IUserRepository userRepository)
        {
            _userRepository= userRepository;
            _personalInfoRepository = personalInfoRepository;
            _cache = cache;
        }
        public async Task<ResponseModel<PersonalInfoGetDTO>> CreatePersonalInfoAsync(PersonalInfoCreateDTO personalInfoDTO)
        {
            var userGetById = await _userRepository.GetByIdAsync(personalInfoDTO.UserId);
            if (userGetById == null)
            {
                return new ResponseModel<PersonalInfoGetDTO>("Такого пользователя нет");
            }
            else if (userGetById.PersonalInfo!= null)
            {
                return new("User уже имеет PersonalInfo, невозможно создать!");
            }
            PersonalInfo personalInfo = new(personalInfoDTO.Email,
                personalInfoDTO.PhoneNumber, personalInfoDTO.UserId,
                personalInfoDTO.Description, personalInfoDTO.Photo);

            personalInfo = await _personalInfoRepository.CreateAsync(personalInfo);

            PersonalInfoGetDTO personalInfoGetDTO = new PersonalInfoGetDTO()
            {
                Email = personalInfo.Email,
                PhoneNumber = personalInfo.PhoneNumber,
                UserId = personalInfo.UserId,
                Description = personalInfo.Description,
                Photo = personalInfo.Photo,
                Id = personalInfo.Id
            };
            return new(personalInfoGetDTO);
        }

        public async Task<ResponseModel<IEnumerable<PersonalInfo>>> GetAllPersonalInfoAsync()
        {
            var userPersonalInfo = await _personalInfoRepository.GetAllAsync();
            return new(userPersonalInfo);
        }

        public async Task<ResponseModel<PersonalInfoGetDTO>> GetPersonalInfoByIdAsync(int Id)
        {
            PersonalInfo? personalInfo = await _personalInfoRepository.GetByIdAsync(Id);
            if (personalInfo == null)
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

        public async Task<ResponseModel<PersonalInfoGetDTO>> SetOrUpdateDescriptionAsync(int personalInfoId, string description)
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
                Description = description,
                Photo = personalInfo.Result.Photo
            };
            var personalInfoUpdated = await UpdatePersonalInfoAsync(personalInfoForUpdate);
            if (personalInfoUpdated.Result == null)
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
            var personalInfoGetById = await _personalInfoRepository.GetByIdAsync(personalInfoDTO.Id);
            if (personalInfoGetById == null)
            {
                return new ResponseModel<PersonalInfoGetDTO>("Таких данных нет");
            }
            PersonalInfo personalInfo = new()
            {
                Id = personalInfoDTO.Id,
                Email = personalInfoDTO.Email,
                PhoneNumber = personalInfoDTO.PhoneNumber,
                Description = personalInfoDTO.Description,
                Photo = personalInfoDTO.Photo,
                CreateAt = personalInfoGetById.CreateAt,
                UserId = personalInfoGetById.UserId
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
            return new ResponseModel<PersonalInfoGetDTO>(personalInfoGetDTO);
        }
    }
}
