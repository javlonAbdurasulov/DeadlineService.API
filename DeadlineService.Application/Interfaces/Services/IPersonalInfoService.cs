using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Services
{
    public interface IPersonalInfoService
    {
        public Task<ResponseModel<PersonalInfoGetDTO>> GetPersonalInfoByIdAsync(int Id);
        public Task<ResponseModel<PersonalInfoGetDTO>> CreatePersonalInfoAsync(PersonalInfoCreateDTO personalInfoDTO);
        public Task<ResponseModel<PersonalInfoGetDTO>> UpdatePersonalInfoAsync(PersonalInfoUpdateDTO personalInfoDTO);
        public Task<ResponseModel<PersonalInfoGetDTO>> SetOrUpdatePhotoAsync(int personalInfoId, byte[] photo);
        public Task<ResponseModel<PersonalInfoGetDTO>> SetOrUpdateDescriptionAsync(int personalInfoId, string description);
    }
}
