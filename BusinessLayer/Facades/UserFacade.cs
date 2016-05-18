﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.DTO;
using DAL;
using DAL.IdentityEntities;
using Microsoft.AspNet.Identity;
using BusinessLayer.UserAccess;
using BusinessLayer;
using BusinessLayer.Facades;
using DAL.Entity;

namespace BL.Facades
{
    public class UserFacade
    {
        public void Register( string email,
                             string password,
                             string secretCode,
                             string userName,
                             string surname,
                             string firstName)
        {
            AppUserManager userManager = new AppUserManager(new AppUserStore(new AppDbContext()));
            AppUser appUser = new AppUser();
            appUser.UserName = userName;
            appUser.Email = email;
            appUser.TeacherId = null;
            appUser.StudentId = null;

            userManager.Create(appUser, password);

            AppUser ourUser = userManager.FindByEmail(appUser.Email);
            if (secretCode != null)
            {
                if (secretCode.Equals("Admin"))
                {
                    AppRoleManager roleManager = new AppRoleManager(new AppRoleStore(new AppDbContext()));
                    if (!roleManager.RoleExists("Admin"))
                        roleManager.Create(new AppRole { Name = "Admin" });

                    userManager.AddToRole(ourUser.Id, "Admin");

                    Teacher teacher = new Teacher();
                    teacher.Surname = surname;
                    teacher.FirstName = firstName;
                    Student student = new Student();
                    student.Surname = surname;
                    student.FirstName = firstName;
                    using (var context = new AppDbContext())
                    {
                        context.Database.Log = Console.WriteLine;
                        context.Student.Add(student);
                        context.Teacher.Add(teacher);
                        context.SaveChanges();
                    }
                    ourUser.TeacherId = teacher.Id;
                    ourUser.StudentId = student.Id;

                }
                else if (secretCode.Equals("Teacher"))
                {
                    AppRoleManager roleManager = new AppRoleManager(new AppRoleStore(new AppDbContext()));
                    if (!roleManager.RoleExists("Teacher"))
                    {
                        roleManager.Create(new AppRole { Name = "Teacher" });
                    }
                    userManager.AddToRole(ourUser.Id, "Teacher");
                    Teacher teacher = new Teacher();
                    teacher.Surname = surname;
                    teacher.FirstName = firstName;
                    using (var context = new AppDbContext())
                    {
                        context.Database.Log = Console.WriteLine;
                        context.Teacher.Add(teacher);
                        context.SaveChanges();
                    }
                    ourUser.TeacherId = teacher.Id;
                }
            }
            else
            {
                Student student = new Student();
                student.Surname = surname;
                student.FirstName = firstName;
                using (var context = new AppDbContext())
                {
                    context.Database.Log = Console.WriteLine;
                    context.Student.Add(student);
                    context.SaveChanges();
                }
                ourUser.StudentId = student.Id;
            }
            userManager.Update(ourUser);
        }

        public ClaimsIdentity Login(string email, string password)
        {
            var userManager = new AppUserManager(new AppUserStore(new AppDbContext()));

            var wantedUser = userManager.FindByEmail(email);

            if (wantedUser == null)
            {
                return null;
            }

            AppUser user = userManager.Find(wantedUser.UserName, password);

            if (user == null)
            {
                return null;
            }

            return userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public UserDTO GetUserById(int userId)
        {
            using (var context = new AppDbContext())
            {
                AppUserManager userManager = new AppUserManager(new AppUserStore(new AppDbContext()));
                AppUser user = userManager.FindById(userId);                
                return Mapping.Mapper.Map<UserDTO>(user);
            }
        }
    }
}