namespace Helpdesk.Domain.Migrations
{
    using Helpdesk.Domain.Concrete;
    using Helpdesk.Domain.Entities;
    using Helpdesk.Domain.Entities.Computers;
    using Helpdesk.Domain.Entities.Requests;
    using Helpdesk.Domain.Entities.Users;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Helpdesk.Domain.Concrete.EFDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            RoleStore<AppRole> roleStore = new RoleStore<AppRole>(context);
            RoleManager<AppRole> roleMgr = new RoleManager<AppRole>(roleStore);

            AppRole roleAdmin = roleMgr.FindByName("Admin");
            if (roleAdmin == null)
            {
                roleAdmin = new AppRole()
                {
                    Name = "Admin"
                };

                roleMgr.Create(roleAdmin);
            }

            AppRole roleConsultant = roleMgr.FindByName("Consultant");
            if (roleConsultant == null)
            {
                roleConsultant = new AppRole()
                {
                    Name = "Consultant"
                };

                roleMgr.Create(roleConsultant);
            }


            UserStore<AppUser> userStore = new UserStore<AppUser>(context);
            UserManager<AppUser> userMgr = new UserManager<AppUser>(userStore);

            AppUser userAdmin = userMgr.FindByName("Admin");

            if (userAdmin == null)
            {
                userAdmin = new AppUser()
                {
                    UserName = "Admin",
                    Email = "admin@example.com"
                };

                userMgr.Create(userAdmin, "password");
            }

            if (!userMgr.IsInRole(userAdmin.Id, "Admin"))
            {
                userMgr.AddToRole(userAdmin.Id, "Admin");
            }

            AppUser userPoczta = userMgr.FindByName("Poczta");

            if (userPoczta == null)
            {
                userPoczta = new AppUser
                {
                    UserName = "Poczta",
                    Email = "poczta@example.com"
                };

                userMgr.Create(userPoczta, "password");
            }

            AppUser userMedyk = userMgr.FindByName("Medyk");

            if (userMedyk == null)
            {
                userMedyk = new AppUser
                {
                    UserName = "Medyk",
                    Email = "medyk@example.com"
                };

                userMgr.Create(userMedyk, "password");
            }

            AppUser userGoogle = userMgr.FindByName("Google");

            if (userGoogle == null)
            {
                userGoogle = new AppUser
                {
                    UserName = "Google",
                    Email = "google@example.com"
                };

                userMgr.Create(userGoogle, "password");
            }
            AppUser userPhilips = userMgr.FindByName("Philips");

            if (userPhilips == null)
            {
                userPhilips = new AppUser
                {
                    UserName = "Philips",
                    Email = "philips@example.com"
                };

                userMgr.Create(userPhilips, "password");
            }

            AppUser userPascal = userMgr.FindByName("Pascal");

            if (userPascal == null)
            {
                userPascal = new AppUser
                {
                    UserName = "Pascal",
                    Email = "pascal@example.com"
                };

                userMgr.Create(userPascal, "password");
            }

            if (!userMgr.IsInRole(userPascal.Id, "Consultant"))
            {
                userMgr.AddToRole(userPascal.Id, "Consultant");
            }

            List<Status> statuses = new List<Status>
            {
                new Status {ID = 1, Description = "Nowe", Level="success" },
                new Status {ID = 2, Description = "W trakcie", Level="info" },
                new Status {ID = 3, Description = "Zakoñczone", Level="danger" },
                new Status {ID = 4, Description = "Oczekuj¹ce", Level="warning" },
            };
            statuses.ForEach(s => context.Statuses.AddOrUpdate(x => x.Description, s));
            context.SaveChanges();

            List<Customer> customers = new List<Customer> {
                new Customer { Name = "Poczta Polska", Address = "Cystersów 21", PhoneNo = "12 410 60 65", UserID = userPoczta.Id },
                new Customer { Name = "PC Medyk", Address = "Wielicka 76", PhoneNo = "690 903 003", UserID = userMedyk.Id },
                new Customer { Name = "Google", Address = "Rynek G³ówny 7", UserID = userGoogle.Id },
                new Customer { Name = "Philips", Address = "Aleje Jerozolimskie 195B", PhoneNo = "22 571 00 00" , UserID = userPhilips.Id }
            };
            customers.ForEach(c => context.Customers.AddOrUpdate(x => x.Name, c));
            context.SaveChanges();

            List<Consultant> consultants = new List<Consultant> {
                new Consultant { Name = "Pascal", UserID = userPascal.Id }
            };
            consultants.ForEach(c => context.Consultants.AddOrUpdate(x => x.Name, c));
            context.SaveChanges();

            List<ComponentTypeCategory> categories = new List<ComponentTypeCategory>
            {
                new ComponentTypeCategory { Name = "G³ówne", Priority = 1 },
                new ComponentTypeCategory { Name = "Dyski", Priority = 2 },
                new ComponentTypeCategory { Name = "Karty rozszerzeñ", Priority = 3 },
                new ComponentTypeCategory { Name = "Wyœwietlanie", Priority = 4 },
                new ComponentTypeCategory { Name = "Peryferia", Priority = 5 }
            };
            categories.ForEach(c => context.TypeCategories.AddOrUpdate(x => x.Name, c));
            context.SaveChanges();

            List<ComponentType> types = new List<ComponentType> {
                new ComponentType { Name = "Karta graficzna", CategoryID = categories.Single(c => c.Name == "Karty rozszerzeñ").ID },
                new ComponentType { Name = "HDD", CategoryID =categories.Single(c => c.Name == "Dyski").ID },
                new ComponentType { Name = "SSD", CategoryID = categories.Single(c => c.Name == "Dyski").ID },
                new ComponentType { Name = "Monitor", CategoryID = categories.Single(c => c.Name == "Wyœwietlanie").ID },
                new ComponentType { Name = "Klawiatura", CategoryID = categories.Single(c => c.Name == "Peryferia").ID },
                new ComponentType { Name = "P³yta g³ówna", CategoryID = categories.Single(c => c.Name == "G³ówne").ID },
                new ComponentType { Name = "Procesor", CategoryID = categories.Single(c => c.Name == "G³ówne").ID },
                new ComponentType { Name = "Mysz", CategoryID = categories.Single(c => c.Name == "Peryferia").ID }
            };
            types.ForEach(t => context.ComponentTypes.AddOrUpdate(x => x.Name, t));
            context.SaveChanges();

            List<Computer> computers = new List<Computer> {
                new Computer
                {
                    Name = "Pocztowy 1",
                    SerialNo = "p0001",
                    Temporary = false,
                    OwnerID = context.Customers.Single(c => c.Name == "Poczta Polska").ID
                },
                new Computer
                {
                    Name = "Pocztowy 2",
                    SerialNo = "p0002",
                    Temporary = false,
                    OwnerID = context.Customers.Single(c => c.Name == "Poczta Polska").ID
                },
                new Computer
                {
                    Name = "Serwisowy",
                    SerialNo = "mx102",
                    Temporary = false,
                    OwnerID = context.Customers.Single(c => c.Name == "PC Medyk").ID
                },
                new Computer
                {
                    Name = "Multimedialny",
                    SerialNo = "ph111",
                    Temporary = false,
                    OwnerID = context.Customers.Single(c => c.Name == "Philips").ID
                },
                new Computer
                {
                    Name = "Audio/Video",
                    SerialNo = "rf³-av",
                    Temporary = false,
                    OwnerID = context.Customers.Single(c => c.Name == "Philips").ID
                },
                new Computer
                {
                    Name = "Big Data 2",
                    SerialNo = "g-bd-2",
                    Temporary = false,
                    OwnerID = context.Customers.Single(c => c.Name == "Google").ID
                },
                new Computer
                {
                    Name = "Big Data 1",
                    SerialNo = "g-bd-1",
                    Temporary = false,
                    OwnerID = context.Customers.Single(c => c.Name == "Google").ID
                }
            };
            computers.ForEach(c => context.Computers.AddOrUpdate(x => x.SerialNo, c));
            context.SaveChanges();

            List<Component> components = new List<Component>
            {
                new Component {
                    Name = "Core 2 Duo",
                    SerialNo = "C2D xxx1",
                    TypeID = types.Single(c => c.Name == "Procesor").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "p0001").ID
                },

                new Component {
                    Name = "Logitech",
                    SerialNo = "lg111",
                    TypeID = types.Single(c => c.Name == "Klawiatura").ID,
                    ComputerID = computers.Single(c=> c.SerialNo == "p0001").ID
                },
                new Component {
                    Name = "LG 17c",
                    SerialNo = "17c001",
                    TypeID = types.Single(c => c.Name == "Monitor").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "p0001").ID
                },

                new Component {
                    Name = "Core 2 Duo",
                    SerialNo ="C2D xxx2",
                    TypeID = types.Single(c => c.Name == "Procesor").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "p0002").ID
                },
                new Component {
                    Name = "Logitech HeyKey",
                    SerialNo ="lg112",
                    TypeID = types.Single(c => c.Name == "Klawiatura").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "p0002").ID
                },
                new Component {
                    Name = "LG 17c",
                    SerialNo ="17c002",
                    TypeID = types.Single(c => c.Name == "Monitor").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "p0002").ID
                },

                new Component {
                    Name = "AMD Athlon 1600+",
                    SerialNo ="At 1600+ 1",
                    TypeID = types.Single(c => c.Name == "Procesor").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "mx102").ID
                },
                new Component {
                    Name = "Logitech Mouse",
                    SerialNo ="ml74444",
                    TypeID = types.Single(c => c.Name == "Mysz").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "mx102").ID},
                new Component {
                    Name = "NoName 15 ph",
                    SerialNo ="nn1",
                    TypeID = types.Single(c => c.Name == "Monitor").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "mx102").ID
                },
                new Component {
                    Name = "Seagate 500GB",
                    SerialNo ="SG 500G63154225",
                    TypeID = types.Single(c => c.Name == "HDD").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "mx102").ID
                },
                new Component {
                    Name = "Samsung SpinPoint 500GB",
                    SerialNo ="Sam 500G4561003",
                    TypeID = types.Single(c => c.Name == "HDD").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "mx102").ID
                },
                new Component {
                    Name = "Dell IBA",
                    SerialNo ="A02",
                    TypeID = types.Single(c => c.Name == "Klawiatura").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "mx102").ID
                },

                new Component {
                    Name = "GeForce GT9600",
                    SerialNo ="GT9600-123",
                    TypeID = types.Single(c => c.Name == "Karta graficzna").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "ph111").ID
                },
                new Component {
                    Name = "Seagate 256G",
                    SerialNo ="SG MG256A001",
                    TypeID = types.Single(c => c.Name == "SSD").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "ph111").ID
                },

                new Component {
                    Name = "Seagate 500G",
                    SerialNo ="SG MG500A003",
                    TypeID = types.Single(c => c.Name == "SSD").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "rf³-av").ID
                },
                new Component {
                    Name = "Logitech HeyKey",
                    SerialNo ="lg113",
                    TypeID = types.Single(c => c.Name == "Klawiatura").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "rf³-av").ID
                },

                new Component {
                    Name = "WD Hiper Mega 10TB",
                    SerialNo ="WD 10T1500100900",
                    TypeID = types.Single(c => c.Name == "HDD").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "g-bd-2").ID
                },
                new Component {
                    Name = "WD Hiper Mega 10TB",
                    SerialNo ="WD 10T1500100901",
                    TypeID = types.Single(c => c.Name == "HDD").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "g-bd-2").ID
                },

                new Component {
                    Name = "Intel Xeon E5-2699 v4",
                    SerialNo ="X2699-aa00bb1",
                    TypeID = types.Single(c => c.Name == "Procesor").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "g-bd-1").ID
                },
                new Component {
                    Name = "WD Hiper Mega 10TB",
                    SerialNo ="WD 10T1500100902",
                    TypeID = types.Single(c => c.Name == "HDD").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "g-bd-1").ID
                },
                new Component {
                    Name = "WD Hiper Mega 10TB",
                    SerialNo ="WD 10T1500100903",
                    TypeID = types.Single(c => c.Name == "HDD").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "g-bd-1").ID
                }
            };
            components.ForEach(c => context.Components.AddOrUpdate(x => x.SerialNo, c));
            context.SaveChanges();

            List<Request> requests = new List<Request>
            {
                //new Request
                //{
                //    ReadableID = "2016/aaa/001",
                //    Description = "Stare zg³oszenie",
                //    ComputerID = context.Computers.Single(c => c.SerialNo == "ph111").ID,
                //    ReceivedDate = DateTime.Parse("2016-09-15 19:30:41.7752486"),
                //    ResolvedDate = DateTime.Parse("2016-09-16 09:17:41.5248612"),
                //    StatusID = 3,
                //    CreatedByID = userPascal.Id,
                //    ClosedByID = consultants[0].ID
                //},
                //new Request
                //{
                //    ReadableID = "2016/aaa/002",
                //    Description = "Drugie zg³oszenie",
                //    ComputerID = context.Computers.Single(c => c.SerialNo == "p0001").ID,
                //    ReceivedDate = DateTime.Parse("2016-12-31 23:59:59.2134585"),
                //    StatusID = 2
                //},
                //new Request
                //{
                //    ReadableID = "2017/aab/001",
                //    Description = "Drogi Marsza³ku, Wysoka Izbo. PKB roœnie. Ró¿norakie i miejsce szkolenia kadr poci¹ga za najwa¿niejszy punkt naszych dzia³añ obierzemy praktykê, nie zapewni i¿ zakup nowego sprzêtu zmusza nas do tej decyzji sk³oni³ mnie fakt, ¿e rozszerzenie naszej dzia³alnoœci rozszerza nam efekt systemu powszechnego uczestnictwa. Tak samo istotne jest ¿e, zawi¹zanie koalicji ukazuje nam horyzonty postaw uczestników wobec zadañ programowych zmusza nas do przeanalizowania form dzia³alnoœci zabezpiecza udzia³ szerokiej grupie w okreœlaniu istniej¹cych kryteriów koliduje z tym, wykorzystanie unijnych dotacji przedstawia interpretuj¹c¹ próbê sprawdzenia systemu powszechnego uczestnictwa. Jak ju¿ zapewne zd¹¿y³ zauwa¿yæ i¿ zakoñczenie tego projektu spe³nia wa¿ne zadanie w okreœlaniu dalszych kierunków postêpowego wychowania. Z drugiej strony, wykorzystanie unijnych dotacji powoduje docenianie wag systemu obs³ugi umo¿liwia w tym zakresie zabezpiecza udzia³ szerokiej grupie w kszta³towaniu odpowiednich warunków administracyjno-finansowych. Nie mówili prawdy. W ten sposób dokoñczenie aktualnych projektów wymaga sprecyzowania i realizacji istniej¹cych kryteriów wymaga sprecyzowania i miejsce szkolenia kadry odpowiadaj¹cego potrzebom. W praktyce zakres i unowoczeœniania systemu powszechnego uczestnictwa. Jednak¿e, zmiana istniej¹cych kryteriów koliduje z powodu obecnej sytuacji. Wy¿sze za³o¿enie ideowe, a tak¿e rozpoczêcie powszechnej akcji kszta³towania podstaw powoduje docenianie wag.",
                //    ComputerID = context.Computers.Single(c => c.SerialNo == "rf³-av").ID,
                //    ReceivedDate = DateTime.Parse("2017-11-23 13:41:52.12841"),
                //    StatusID = 4
                //},
                //new Request
                //{
                //    ReadableID = "tag1",
                //    Description = "Chcia³bym zg³osiæ uszkodzenie monitora. Po ok. 5 minutach u¿ytkowania, na ekranie pojawia siê migotanie. Model mojego sprzêtu to: 123abc. Proszê o szybk¹ pomoc.",
                //    ComputerID = context.Computers.Single(c => c.SerialNo == "p0001").ID,
                //    ReceivedDate = DateTime.Parse("2017-01-23 20:41:52.12841"),
                //    StatusID = 1,
                //    CreatedByID = userKorpo.Id
                //},
                //new Request
                //{
                //    ReadableID = "tag2",
                //    Description = "W dniu wczorajszym mój komputer przesta³ odtwarzaæ p³yty CD. Po w³o¿eniu p³yty do czytnika, wydaje on dziwne dŸwiêki skrobania i trzeszczenia. Z góry dziêkujê za rozwi¹zanie problemu.",
                //    ComputerID = context.Computers.Single(c => c.SerialNo == "p0001").ID,
                //    ReceivedDate = DateTime.Parse("2017-12-24 12:34:56.7890"),
                //    StatusID = 2,
                //    CreatedByID = userPoczta.Id
                //},
                //new Request
                //{
                //    ReadableID = "tag3",
                //    Description = "Klient zg³osi³ uszkodzenie laptopa objawiaj¹ce siê nie reagowaniem na przycisk zasilania. Kontrolka baterii reaguje na pod³¹czenie zasilacza. Wstêpne oglêdziny sugeruj¹ uszkodzenie p³yty g³ównej.",
                //    ComputerID = context.Computers.Single(c => c.SerialNo == "p0001").ID,
                //    ReceivedDate = DateTime.Parse("2017-03-21 09:01:23.456789"),
                //    StatusID = 2,
                //    CreatedByID = userPascal.Id,
                //    AssignedToID = consultants[0].ID
                //},
                //new Request
                //{
                //    ReadableID = "tag4",
                //    Description = "Zg³oszenie klienta zawiera³o informacje o pêkniêtej matrycy ekranu w laptopie. Zosta³y udzielone informacje o orientacyjnych kosztach naprawy. Klient ma dostarczyæ sprzêt w ci¹gu najbli¿szego tygodnia.",
                //    ComputerID = context.Computers.Single(c => c.SerialNo == "p0001").ID,
                //    ReceivedDate = DateTime.Parse("2017-01-20 20:10:13.7321468"),
                //    StatusID = 1
                //}
            };
            requests.ForEach(r => context.Requests.AddOrUpdate(x => x.ReadableID, r));
            context.SaveChanges();
        }
    }
}
