using ApplicationRequestIt.Models;
using ApplicationRequestIt.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ApplicationRequestIt.Models;

namespace ApplicationRequestIt.Data
{
    public class DbInitializer
    {
        private static object serviceProvider;

        public static async Task Initialize(ApplicationDbContext context, IServiceProvider serviceProvider)
        {

            await context.Database.EnsureCreatedAsync();

            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { SD.AdminEndUser, SD.BehandelaarEndUser, SD.CustomerEndUser };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //rollen maken
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //hier 3 initiele users aanmaken
            if (UserManager.Users.Count() == 0)
            {
                var admin = new ApplicationUser
                {
                    UserName = "Admin@admin.be",
                    Email = "Admin@admin.be",
                    isAdmin = true,
                    Voornaam = "Seppe",
                    Achternaam = "Berghmans",
                };
                var Behandelaar = new ApplicationUser
                {
                    UserName = "Behandelaar@behandelaar.be",
                    Email = "Behandelaar@behandelaar.be",
                    isBehandelaar = true,
                    Voornaam = "Felix",
                    Achternaam = "bergs",

                };
                var user = new ApplicationUser
                {
                    UserName = "User@user.be",
                    Email = "User@user.be",
                    Voornaam = "pieter",
                    Achternaam = "kardinaals",
                    Klant = "torfs"
                };
                var user2 = new ApplicationUser
                {
                    UserName = "koen@devoslemmens.be",
                    Email = "koen@devoslemmens.be",
                    Voornaam = "Koen",
                    Achternaam = "Lemmens",
                    Klant = "De vos lemmens"
                };
                //paswoorden voor de users
                string adminPWD = "Admin!234";
                string behandelaarPWD = "Behandelaar!234";
                string userPWD = "User!234";
                string koenpsw = "Koen!234";

                var _admin = await UserManager.FindByEmailAsync("Admin@admin.be");
                var _behandelaar = await UserManager.FindByEmailAsync("Behandelaar@behandelaar.be");
                var _user = await UserManager.FindByEmailAsync("User@user.be");
                var _koen = await UserManager.FindByEmailAsync("koen@devoslemmens.be");


                if (_admin == null)
                {
                    var createPowerUser = await UserManager.CreateAsync(admin, adminPWD);
                    if (createPowerUser.Succeeded)
                    {
                        //aan rol toevoegen
                        await UserManager.AddToRoleAsync(admin, SD.AdminEndUser);
                    }
                }
                if (_behandelaar == null)
                {
                    var createPowerUser = await UserManager.CreateAsync(Behandelaar, behandelaarPWD);
                    if (createPowerUser.Succeeded)
                    {
                        //aan rol toevoegen
                        await UserManager.AddToRoleAsync(Behandelaar, SD.BehandelaarEndUser);
                    }
                }
                if (_user == null)
                {
                    var createPowerUser = await UserManager.CreateAsync(user, userPWD);
                    if (createPowerUser.Succeeded)
                    {
                        //aan rol toevoegen
                        await UserManager.AddToRoleAsync(user, SD.CustomerEndUser);
                    }
                }
                if (_koen == null)
                {
                    var createPowerUser = await UserManager.CreateAsync(user2, koenpsw);
                    if (createPowerUser.Succeeded)
                    {
                        //aan rol toevoegen
                        await UserManager.AddToRoleAsync(user2, SD.CustomerEndUser);

                    }

                }
            }


            if (!context.Statussen.Any())
            {
                var statussen = new Status[]
             {
                new Status { Naam = "Verstuurd", Omschrijving = "Status is verstuurd en wacht op ontvangst" },
                new Status { Naam = "Ontvangen", Omschrijving = "Aanvraag is onvangen en wordt z.s.m toegewezen" },
                new Status { Naam = "Toegewezen", Omschrijving = "Aanvraag is toegewezen aan een behandelaar" },
                new Status { Naam = "In Behandeling", Omschrijving = "Aanvraag is in behandeling" },
                new Status { Naam = "Wacht op goedkeuring", Omschrijving = "Aanvraag is klaar en wacht op goedkeuring klant" },
                new Status { Naam = "Klaar", Omschrijving = "Aanvraag is klaar" },
                new Status { Naam = "Afgesloten", Omschrijving = "Aanvraag afgehandeld" }
             };
                foreach (Status s in statussen)
                {
                    context.Statussen.Add(s);
                }
            }

            //if (!context.Aanvragen.Any())
            //{

            //    var koen = UserManager.Users.Where(s => s.Email.Equals("koen@devoslemmens.be"));

            //    var aanvragen = new Aanvraag[]
            //    {

            //    //new Aanvraag { Aanmaakdatum = new DateTime(2018, 05, 16), BehandelaarApplicationUser = await UserManager.FindByEmailAsync("behandelaar@behandelaar.be"), Omschrijving ="graag hadden we content om onze wintercollectie te tonen", StartDatum = new DateTime(2018,10,10), StatusId = 1, Titel = "Wintercollectie", UserId =  };
            //    //new Aanvraag { Aanmaakdatum = new DateTime(2018, 05, 16), BehandelaarApplicationUser = await UserManager.FindByEmailAsync("behandelaar@behandelaar.be"), Omschrijving = "graag hadden we content om onze wintercollectie te tonen", StartDatum = new DateTime(2018, 10, 10), StatusId = 1, Titel = "Wintercollectie", UserId =  };
            //    //new Aanvraag { Aanmaakdatum = new DateTime(2018, 05, 16), BehandelaarApplicationUser = await UserManager.FindByEmailAsync("behandelaar@behandelaar.be"), Omschrijving = "graag hadden we content om onze wintercollectie te tonen", StartDatum = new DateTime(2018, 10, 10), StatusId = 1, Titel = "Wintercollectie", UserId =  };
            //    //new Aanvraag { Aanmaakdatum = new DateTime(2018, 05, 16), BehandelaarApplicationUser = await UserManager.FindByEmailAsync("behandelaar@behandelaar.be"), Omschrijving = "graag hadden we content om onze wintercollectie te tonen", StartDatum = new DateTime(2018, 10, 10), StatusId = 1, Titel = "Wintercollectie", UserId =  };
            //    //new Aanvraag { Aanmaakdatum = new DateTime(2018, 05, 16), BehandelaarApplicationUser = await UserManager.FindByEmailAsync("behandelaar@behandelaar.be"), Omschrijving = "graag hadden we content om onze wintercollectie te tonen", StartDatum = new DateTime(2018, 10, 10), StatusId = 1, Titel = "Wintercollectie", UserId =  };
            //    //new Aanvraag { Aanmaakdatum = new DateTime(2018, 05, 16), BehandelaarApplicationUser = await UserManager.FindByEmailAsync("behandelaar@behandelaar.be"), Omschrijving = "graag hadden we content om onze wintercollectie te tonen", StartDatum = new DateTime(2018, 10, 10), StatusId = 1, Titel = "Wintercollectie", UserId =  };


            //    {

            //    };
            //    foreach (Aanvraag a in aanvragen)
            //    {
            //        context.Aanvragen.Add(a);
            //    }
            //}

            if (!context.Berichten.Any())
            {

            }

            await context.SaveChangesAsync();


        }
    }
}
