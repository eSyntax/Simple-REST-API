using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_API.Enums;
using Simple_API.Models;

namespace Simple_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Jwt authorization is required
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ToDoListController : ControllerBase
    {
        private readonly AppDBContext _context;

        private UserInfo _user;

        //JWT access token used to check if same user is logged
        private string _accessToken;

        public ToDoListController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/ToDoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoList>>> GetToDoList()
        {
            //Decrypt user info from JWT token
            await JwtUserInfo();

            IQueryable<ToDoList> result;

            //Show full list if user is admin
            if (_user.Privileges == UserPrivileges.Admin)
                result = _context.ToDoList.Include(x => x.UserDetails).Where(
                    c => 
                        c.UserId == c.UserDetails.UserId);
            else
                result = _context.ToDoList.Include(x => x.UserDetails).Where(
                    c => 
                        c.UserId == _user.UserId &&
                        c.UserId == c.UserDetails.UserId);


            //Select used to integrate NotMapped LoggedUserDetails
            return await result.Select(x => new ToDoList
            {
                ToDoId = x.ToDoId,
                Description = x.Description,
                TaskCompleted = x.TaskCompleted,
                LoggedUserDetails = _user,
                UserId = x.UserId,
                UserDetails = x.UserDetails
            }).ToListAsync();
        }

        // GET: api/ToDoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoList>> GetToDoList(int id)
        {
            //Decrypt user info from JWT token
            await JwtUserInfo();

            IQueryable<ToDoList> result;

            //Show full list if user is admin
            if (_user.Privileges == UserPrivileges.Admin)
                result = _context.ToDoList.Include(x => x.UserDetails).Where(
                    c =>
                        c.ToDoId == id);
            else
                result = _context.ToDoList.Include(x => x.UserDetails).Where(
                    c =>
                        c.UserId == _user.UserId &&
                        c.UserId == c.UserDetails.UserId &&
                        c.ToDoId == id);
            
            if (result == null)
            {
                return NotFound();
            }

            //Select used to integrate NotMapped LoggedUserDetails
            return await result.Select(x => new ToDoList
            {
                ToDoId = x.ToDoId,
                Description = x.Description,
                TaskCompleted = x.TaskCompleted,
                LoggedUserDetails = _user,
                UserId = x.UserId,
                UserDetails = x.UserDetails
            }).SingleOrDefaultAsync();
        }

        // PUT: api/ToDoList/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoList(int id, ToDoList toDoList)
        {

            if (id != toDoList.ToDoId)
                return BadRequest();

            //Decrypt user info from JWT token
            await JwtUserInfo();


            //Update ToDoList item only if item id related to logged user id
            if (_user.Privileges == UserPrivileges.User)
            {

                //Check if ToDo item exist with input ToDoId and logged UserId
                var result = await _context.ToDoList.Include(x => x.UserDetails)
                    .Where(x => x.UserId == _user.UserId && x.ToDoId == id).ToListAsync();

                //If ToDo items not found that means user wants to modify item which not exist or don't have permissions
                if (result.Count == 0)
                    return NotFound("Item not exist or you don't have permissions.");

                toDoList.UserId = _user.UserId;
            }
            else
            {
                //If admin not set UserId then set logged UserId otherwise set specified UserId
                toDoList.UserId = toDoList.UserId == 0 ? _user.UserId : toDoList.UserId;
            }

            //Used to clear tracker for new request
            _context.ChangeTracker.Clear();

            _context.Entry(toDoList).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoListExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/ToDoList
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ToDoList>> PostToDoList(ToDoList toDoList)
        {
            //Decrypt user info from JWT token
            await JwtUserInfo();

            if (_user.Privileges == UserPrivileges.User || 
                (_user.Privileges == UserPrivileges.Admin && toDoList.UserId == 0))
            {
                toDoList.UserId = _user.UserId;
            }

            _context.ToDoList.Add(toDoList);
            await _context.SaveChangesAsync();

            toDoList.LoggedUserDetails = _user;

            return CreatedAtAction("GetToDoList", new { id = toDoList.ToDoId }, toDoList);
        }

        // DELETE: api/ToDoList/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ToDoList>> DeleteToDoList(int id)
        {

            //Decrypt user info from JWT token
            await JwtUserInfo();

            //Update ToDoList item only if item id related to logged user id
            if (_user.Privileges == UserPrivileges.User)
            {
                //Check if ToDo item exist with input ToDoId and logged UserId
                var result = await _context.ToDoList.Include(x => x.UserDetails)
                    .Where(x => x.UserId == _user.UserId && x.ToDoId == id).ToListAsync();

                //If ToDo items not found that means user wants to modify item which not exist or don't have permissions
                if (result.Count == 0)
                    return NotFound("Item not exist or you don't have permissions.");

            }

            var toDoList = await _context.ToDoList.FindAsync(id);

            if (toDoList == null)
                return NotFound();

            _context.ToDoList.Remove(toDoList);
            await _context.SaveChangesAsync();

            toDoList.LoggedUserDetails = _user;

            return toDoList;
        }

        private bool ToDoListExists(int id)
        {
            return _context.ToDoList.Any(e => e.ToDoId == id);
        }

        //Decrypt UserInfo data from JWT token
        private async Task JwtUserInfo()
        {
            var tempToken = await HttpContext.GetTokenAsync("access_token");

            if (_user == null || tempToken != _accessToken)
            {
                _accessToken = tempToken;

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(_accessToken).Claims;

                _user = new UserInfo
                {
                    UserId = int.Parse(token.First(claim => claim.Type == "UserId").Value),
                    Email = token.First(claim => claim.Type == "Email").Value,
                    Password = token.First(claim => claim.Type == "Password").Value,
                    //Convert string to enum
                    Privileges = (UserPrivileges)Enum.Parse(typeof(UserPrivileges), token.First(claim => claim.Type == "Privileges").Value)
                };
            }
        }
    }
}
