using System;
using System.Buffers.Text;
using System.Linq;
using System.Threading.Tasks;
using BlackWhiteBlog.DbModels;
using BlackWhiteBlog.Helpers.Permissions;
using BlackWhiteBlog.Services;
using BlackWhiteBlog.TransferObjects;
using BlackWhiteBlog.TransferObjects.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlackWhiteBlog.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        //todo: put метод для редактирования пользователя(смена имени, смена прав)
        
        private readonly IUserService _userService;
        private readonly BlogDbContext _ctx;
        public UserController(BlogDbContext ctx, IUserService userService)
        {
            _ctx = ctx;
            _userService = userService;
        }
        
        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> Get([FromBody]UserRequestDto value)
        {
            var privMessage = await CheckPriv(value);
            if (!string.IsNullOrEmpty(privMessage))
                return BadRequest(privMessage);
            
            var users = await _ctx.Users
                .Select(u => new {u.UserId, u.UserName})
                .ToListAsync();

            var result = users.Select(x => new UserCardDto()
            {
                UserId = x.UserId,
                UserName = x.UserName
            }).ToList();

            if (result.Any())
                return Ok(result);
            return NotFound();
        }

        // GET: api/Users
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id, [FromBody] UserRequestDto value)
        {
            var privMessage = await CheckPriv(value);
            if (!string.IsNullOrEmpty(privMessage))
                return BadRequest(privMessage);

            var userInfo = await _ctx.Users
                .Include(u => u.Author)
                .FirstOrDefaultAsync(u => u.UserId == id);
            if (userInfo == null)
                return NotFound();
            
            var result = new UserDetailDto()
             {
                 AuthorId = userInfo.AuthorId ?? -1,
                 AuthorName = userInfo.Author.AuthorName,
                 AuthorPicLink = userInfo.Author.AuthorPicLink,
                 Privs = userInfo.Privs,
                 UserId = userInfo.UserId,
                 UserName = userInfo.UserName
             };
            
            return Ok(result);
        }
        
        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterUserDto value)
        {
            //Регистрация пользователя и его авторизация сразу же
            if (value is null)
                return BadRequest("User is undefined");
            
            var userDto = await _userService.Register(value);
            if (userDto == null)
                return BadRequest("Can't register User");
            return Ok(userDto);
        }

        // PUT: api/Users/Login/5
        [HttpPut("{id}", Name = "Login")]
        public async Task<IActionResult> Login(int id, [FromBody] LoginDto value)
        {
            if (value is null)
                return BadRequest("Не указан имеющийся пользователь для авторизации");

            try
            {
                var result = await _userService.Login(value);
                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private async Task<string> CheckPriv(UserRequestDto value)
        {
            if (value is null)
                return "Невозможно определить права на просмотр пользователей";

            var currentUser = await _ctx.Users
                .FirstOrDefaultAsync(u => u.UserId == value.UserId);
            if (currentUser.Privs < (int) UserPermission.ManageUsers)
                return "Недостаточно прав для редактирования пользователей";

            return null;
        }
        
    }
}