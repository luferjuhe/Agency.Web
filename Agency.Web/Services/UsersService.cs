using System;
using Agency.Data;
using Agency.Data.Entities;
using Agency.Web.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Agency.Web.Services
{
    public interface IUsersService
    {
        Task<List<UserViewModel>> GetUsersAsync();
        Task<UserViewModel> GetUserById(int userId);
        Task<User> GetUserByEmail(string email);
        Task AddUserAsync(UserViewModel vm);
        Task EditUserAsync(UserViewModel vm);
        Task DeleteUserAsync(int userId);
    }

    public class UsersService : IUsersService
    {
        private readonly DbContextAgency ctx;
        private readonly IMapper mapper;

        public UsersService(DbContextAgency ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        public async Task<List<UserViewModel>> GetUsersAsync() =>
             mapper.Map<List<UserViewModel>>(await ctx.Users.Where(u => u.InactiveDate == null).ToListAsync());

        public async Task AddUserAsync(UserViewModel vm)
        {
            User user = new User
            {
                Name = vm.Name,
                CreationDate = DateTime.Now,
                Email = vm.Email,
                Password = vm.Password,
                CreationUserId = 1
            };

            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();
        }

        public async Task EditUserAsync(UserViewModel vm)
        {
            User user = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == vm.UserId);
            user.Email = vm.Email;
            user.Name = vm.Name;

            await ctx.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            User user = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            user.InactiveDate = DateTime.Now;
            await ctx.SaveChangesAsync();
        }

        public async Task<UserViewModel> GetUserById(int userId) =>
            mapper.Map<UserViewModel>(await ctx.Users.FirstOrDefaultAsync(u => u.UserId == userId));

        public async Task<User> GetUserByEmail(string email) =>
            await ctx.Users.FirstOrDefaultAsync(u => u.Email == email);

    }
}

