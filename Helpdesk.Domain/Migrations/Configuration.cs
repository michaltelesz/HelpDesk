namespace Helpdesk.Domain.Migrations
{
    using Helpdesk.Domain.Entities.Computers;
    using Helpdesk.Domain.Entities.Requests;
    using Helpdesk.Domain.Entities.Users;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Helpdesk.Domain.Concrete.EFDbContext>
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
            var statuses = new List<Status>
            {
                new Status { Description = "Nowe" },
                new Status { Description = "W trakcie" },
                new Status { Description = "Zako�czone" }
            };
            statuses.ForEach(s => context.Statuses.AddOrUpdate(x => x.Description, s));
            context.SaveChanges();

            var customers = new List<Customer> { new Customer { Name = "Poczta Polska", Address = "Cysters�w 21" },
                new Customer { Name = "PC Medyk", Address = "Wielicka 76" },
                new Customer { Name = "Google", Address = "Rynek G��wny 7" },
                new Customer { Name = "Philips", Address = "Aleje Jerozolimskie 195B" }
            };
            customers.ForEach(c => context.Customers.AddOrUpdate(x => x.Name, c));
            context.SaveChanges();

            var categories = new List<ComponentTypeCategory>
            {
                new ComponentTypeCategory { Name = "G��wne", Priority = 1 },
                new ComponentTypeCategory { Name = "Dyski", Priority = 2 },
                new ComponentTypeCategory { Name = "Karty rozszerze�", Priority = 3 },
                new ComponentTypeCategory { Name = "Wy�wietlanie", Priority = 4 },
                new ComponentTypeCategory { Name = "Peryferia", Priority = 5 }
            };
            categories.ForEach(c => context.TypeCategories.AddOrUpdate(x => x.Name, c));
            context.SaveChanges();

            var types = new List<ComponentType> {
                new ComponentType { Name = "Karta graficzna", CategoryID = categories.Single(c => c.Name == "Karty rozszerze�").ID },
                new ComponentType { Name = "HDD", CategoryID =categories.Single(c => c.Name == "Dyski").ID },
                new ComponentType { Name = "SSD", CategoryID = categories.Single(c => c.Name == "Dyski").ID },
                new ComponentType { Name = "Monitor", CategoryID = categories.Single(c => c.Name == "Wy�wietlanie").ID },
                new ComponentType { Name = "Klawiatura", CategoryID = categories.Single(c => c.Name == "Peryferia").ID },
                new ComponentType { Name = "P�yta g��wna", CategoryID = categories.Single(c => c.Name == "G��wne").ID },
                new ComponentType { Name = "Procesor", CategoryID = categories.Single(c => c.Name == "G��wne").ID },
                new ComponentType { Name = "Mysz", CategoryID = categories.Single(c => c.Name == "Peryferia").ID }
            };
            types.ForEach(t => context.ComponentTypes.AddOrUpdate(x => x.Name, t));
            context.SaveChanges();

            var computers = new List<Computer> {
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
                    SerialNo = "rf�-av",
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

            var components = new List<Component>
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
                    ComputerID = computers.Single(c => c.SerialNo == "rf�-av").ID
                },
                new Component {
                    Name = "Logitech HeyKey",
                    SerialNo ="lg113",
                    TypeID = types.Single(c => c.Name == "Klawiatura").ID,
                    ComputerID = computers.Single(c => c.SerialNo == "rf�-av").ID
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

            var requests = new List<Request>
            {
                new Request
                {
                    ReadableID = "2016/aaa/001",
                    Description = "Stare zg�oszenie",
                    ComputerID = context.Computers.Single(c => c.SerialNo == "ph111").ID,
                    ReceivedDate = DateTime.Parse("2016-09-15 19:30:41.7752486"),
                    ResolvedDate = DateTime.Parse("2016-09-16 09:17:41.5248612"),
                    StatusID = statuses.Single(s => s.Description == "Zako�czone").ID
                },
                new Request
                {
                    ReadableID = "2016/aaa/002",
                    Description = "Drugie zg�oszenie",
                    ComputerID = context.Computers.Single(c => c.SerialNo == "p0001").ID,
                    ReceivedDate = DateTime.Parse("2016-12-31 23:59:59.2134585"),
                    StatusID = statuses.Single(s => s.Description == "W trakcie").ID
                },
                new Request
                {
                    ReadableID = "2017/aab/001",
                    Description = "Drogi Marsza�ku, Wysoka Izbo. PKB ro�nie. R�norakie i miejsce szkolenia kadr poci�ga za najwa�niejszy punkt naszych dzia�a� obierzemy praktyk�, nie zapewni i� zakup nowego sprz�tu zmusza nas do tej decyzji sk�oni� mnie fakt, �e rozszerzenie naszej dzia�alno�ci rozszerza nam efekt systemu powszechnego uczestnictwa. Tak samo istotne jest �e, zawi�zanie koalicji ukazuje nam horyzonty postaw uczestnik�w wobec zada� programowych zmusza nas do przeanalizowania form dzia�alno�ci zabezpiecza udzia� szerokiej grupie w okre�laniu istniej�cych kryteri�w koliduje z tym, wykorzystanie unijnych dotacji przedstawia interpretuj�c� pr�b� sprawdzenia systemu powszechnego uczestnictwa. Jak ju� zapewne zd��y� zauwa�y� i� zako�czenie tego projektu spe�nia wa�ne zadanie w okre�laniu dalszych kierunk�w post�powego wychowania. Z drugiej strony, wykorzystanie unijnych dotacji powoduje docenianie wag systemu obs�ugi umo�liwia w tym zakresie zabezpiecza udzia� szerokiej grupie w kszta�towaniu odpowiednich warunk�w administracyjno-finansowych. Nie m�wili prawdy. W ten spos�b doko�czenie aktualnych projekt�w wymaga sprecyzowania i realizacji istniej�cych kryteri�w wymaga sprecyzowania i miejsce szkolenia kadry odpowiadaj�cego potrzebom. W praktyce zakres i unowocze�niania systemu powszechnego uczestnictwa. Jednak�e, zmiana istniej�cych kryteri�w koliduje z powodu obecnej sytuacji. Wy�sze za�o�enie ideowe, a tak�e rozpocz�cie powszechnej akcji kszta�towania podstaw powoduje docenianie wag.",
                    ComputerID = context.Computers.Single(c => c.SerialNo == "rf�-av").ID,
                    ReceivedDate = DateTime.Now,
                    StatusID = statuses.Single(s => s.Description == "Nowe").ID
                },
                new Request
                {
                    ReadableID = "tag1",
                    Description = "Chcia�bym zg�osi� uszkodzenie monitora. Po ok. 5 minutach u�ytkowania, na ekranie pojawia si� migotanie. Model mojego sprz�tu to: 123abc. Prosz� o szybk� pomoc.",
                    ComputerID = context.Computers.Single(c => c.SerialNo == "p0001").ID,
                    ReceivedDate = DateTime.Now,
                    StatusID = statuses.Single(s => s.Description == "Nowe").ID
                },
                new Request
                {
                    ReadableID = "tag2",
                    Description = "W dniu wczorajszym m�j komputer przesta� odtwarza� p�yty CD. Po w�o�eniu p�yty do czytnika, wydaje on dziwne d�wi�ki skrobania i trzeszczenia. Z g�ry dzi�kuj� za rozwi�zanie problemu.",
                    ComputerID = context.Computers.Single(c => c.SerialNo == "p0001").ID,
                    ReceivedDate = DateTime.Now,
                    StatusID = statuses.Single(s => s.Description == "Nowe").ID
                },
                new Request
                {
                    ReadableID = "tag3",
                    Description = "Klient zg�osi� uszkodzenie laptopa objawiaj�ce si� nie reagowaniem na przycisk zasilania. Kontrolka baterii reaguje na pod��czenie zasilacza. Wst�pne ogl�dziny sugeruj� uszkodzenie p�yty g��wnej.",
                    ComputerID = context.Computers.Single(c => c.SerialNo == "p0001").ID,
                    ReceivedDate = DateTime.Now,
                    StatusID = statuses.Single(s => s.Description == "Nowe").ID
                },
                new Request
                {
                    ReadableID = "tag4",
                    Description = "Zg�oszenie klienta zawiera�o informacje o p�kni�tej matrycy ekranu w laptopie. Zosta�y udzielone informacje o orientacyjnych kosztach naprawy. Klient ma dostarczy� sprz�t w ci�gu najbli�szego tygodnia.",
                    ComputerID = context.Computers.Single(c => c.SerialNo == "p0001").ID,
                    ReceivedDate = DateTime.Now,
                    StatusID = statuses.Single(s => s.Description == "Nowe").ID
                }
            };
            requests.ForEach(r => context.Requests.AddOrUpdate(x => x.ReadableID, r));
            context.SaveChanges();
        }
    }
}
