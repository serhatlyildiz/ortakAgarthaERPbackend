﻿using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IUserDal _userDal;

        public UsersController(IUserService userService, IUserDal userDal)
        {
            _userService = userService;
            _userDal = userDal;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            Thread.Sleep(1000);

            var result = _userService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(int userID)
        {
            var result = _userService.Delete(userID);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(Users user)
        {
            var result = _userService.Update(user);  // Service çağrısı
            if (result.Success)
            {
                return Ok(result);  // Başarılı güncelleme
            }
            return BadRequest(result);  // Hata durumu
        }

        [HttpGet("getallwithroles")]
        public IActionResult getAllWithRoles()
        {
            var result = _userDal.GetAllWithRoles();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}