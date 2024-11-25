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
                // Kullanıcının rollerine göre işlem yapıyoruz
                var result = from operationClaim in context.OperationClaims
                             where user.Roles.Contains(operationClaim.Id)  // Kullanıcının rollerine göre filtreleme
                             select new OperationClaim
                             {
                                 Id = operationClaim.Id,
                                 Name = operationClaim.Name
                             };

                return result.ToList();
            }
        }

        public List<UserWithRolesDto> GetAllWithRoles()
        {
            using (var context = new NorthwindContext())
            {
                var result = from user in context.Users
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
                                 Roles = user.Roles != null && user.Roles.Any()  // Eğer kullanıcının rolleri varsa
                                     ? context.OperationClaims
                                           .Where(claim => user.Roles.Contains(claim.Id))  // Kullanıcının rollerine göre filtreleme
                                           .Select(claim => claim.Name)
                                           .ToList()
                                     : new List<string> { "Müşteri" } // Rolü olmayan kullanıcıya "Müşteri" ata
                             };

                return result.ToList();
            }
        }

    }
}

