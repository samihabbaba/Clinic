using System.Collections.Generic;
using API.Clinic.API.DataTransferObject;
using Clinic.API.Dtos.UserListDto;

namespace Clinic.API.Dtos
{
    public class SystemUserPaging
    {
        public IEnumerable<SystemUserViewDto> SystemUsers { get; set; }
        public PagingDto PagingInfo { get; set; }
    }
}