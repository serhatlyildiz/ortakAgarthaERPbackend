using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<Users, NorthwindContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(Users user)
        {
            using (var context = new NorthwindContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();
            }
        }

        public List<UserWithRolesDto> GetAllWithRoles()
        {
            using (var context = new NorthwindContext())
            {
                var result = from user in context.Users
                             join userClaim in context.UserOperationClaims
                             on user.Id equals userClaim.UserId into userClaimsGroup
                             from userClaim in userClaimsGroup.DefaultIfEmpty() // Left join
                             join operationClaim in context.OperationClaims
                             on userClaim.OperationClaimId equals operationClaim.Id into operationClaimsGroup
                             from operationClaim in operationClaimsGroup.DefaultIfEmpty() // Left join
                             select new UserWithRolesDto
                             {
                                 Id = user.Id,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Email = user.Email,
                                 City = user.City,
                                 District = user.District,
                                 Adress = user.Adress,
                                 Status = user.Status,
                                 Cinsiyet = user.Cinsiyet,
                                 Roles = userClaim != null
                                     ? context.OperationClaims
                                           .Where(claim => claim.Id == userClaim.OperationClaimId)
                                           .Select(claim => claim.Name)
                                           .ToList()
                                     : new List<string> { "Müşteri" } // Rolü olmayan kullanıcıya "Müşteri" ata
                             };

                return result.ToList();
            }
        }

    }
}

