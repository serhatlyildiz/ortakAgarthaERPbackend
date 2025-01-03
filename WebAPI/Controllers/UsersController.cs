﻿using Business.Abstract;
using DataAccess.Abstract;
using Entities.DTOs;
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

        [HttpGet("delete")]
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
        public IActionResult Update(UserForUpdateDto user)
        {
            var result = _userService.UpdateForAdmin(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
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

        [HttpPost("getfilteredusers")]
        public IActionResult GetFilteredUsers([FromBody] UserFilterDto filter)
        {
            var result = _userService.GetFilteredUsers(filter);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _userService.GetByIdAdmin(id);
            if (result == null)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("restore")]
        public IActionResult Restore(int userID)
        {
            var result = _userService.Restore(userID);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}