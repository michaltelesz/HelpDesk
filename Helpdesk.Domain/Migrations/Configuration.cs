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
            context.Statuses.AddOrUpdate(
                s => s.Description,
                new Status { Description = "Nowe" },
                new Status { Description = "W trakcie" },
                new Status { Description = "Zakoñczone" }
                );

            context.Customers.AddOrUpdate(
                c => c.Name,
                new Customer { Name = "Poczta Polska", Address = "Cystersów 21" },
                new Customer { Name = "PC Medyk", Address = "Wielicka 76" },
                new Customer { Name = "Google", Address = "Rynek G³ówny 7" },
                new Customer { Name = "Philips", Address = "Aleje Jerozolimskie 195B" }
                );

            context.TypeCategories.AddOrUpdate(
                c => c.Name,
                new ComponentTypeCategory { Name = "G³ówne", Priority = 1 },
                new ComponentTypeCategory { Name = "Dyski", Priority = 2 },
                new ComponentTypeCategory { Name = "Karty rozszerzeñ", Priority = 3 },
                new ComponentTypeCategory { Name = "Wyœwietlanie", Priority = 4 },
                new ComponentTypeCategory { Name = "Peryferia", Priority = 5 }
                );

            context.SaveChanges();

            context.ComponentTypes.AddOrUpdate(
                c => c.Name,
                new ComponentType { Name = "Karta graficzna", Category = context.TypeCategories.FirstOrDefault(c => c.Name.Equals("Karty rozszerzeñ")) },
                new ComponentType { Name = "HDD", Category = context.TypeCategories.FirstOrDefault(c => c.Name.Equals("Dyski")) },
                new ComponentType { Name = "SSD", Category = context.TypeCategories.FirstOrDefault(c => c.Name.Equals("Dyski")) },
                new ComponentType { Name = "Monitor", Category = context.TypeCategories.FirstOrDefault(c => c.Name.Equals("Wyœwietlanie")) },
                new ComponentType { Name = "Klawiatura", Category = context.TypeCategories.FirstOrDefault(c => c.Name.Equals("Peryferia")) },
                new ComponentType { Name = "P³yta g³ówna", Category = context.TypeCategories.FirstOrDefault(c => c.Name.Equals("G³ówne")) },
                new ComponentType { Name = "Procesor", Category = context.TypeCategories.FirstOrDefault(c => c.Name.Equals("G³ówne")) },
                new ComponentType { Name = "Mysz", Category = context.TypeCategories.FirstOrDefault(c => c.Name.Equals("Peryferia")) }
                );

            context.SaveChanges();

            context.Computers.AddOrUpdate(
                c => c.SerialNo,
                new Computer
                {
                    Name = "Pocztowy 1",
                    SerialNo = "p0001",
                    Temporary = false,
                    Owner = context.Customers.FirstOrDefault(c => c.Name.Equals("Poczta Polska")),
                    Components = new List<Component>
                    {
                        new Component { Name = "Core 2 Duo", SerialNo ="C2D xxx1", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Procesor"))},
                        new Component { Name = "Logitech", SerialNo ="lg111", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Klawiatura"))},
                        new Component { Name = "LG 17c", SerialNo ="17c001", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Monitor"))}
                    }
                },
                new Computer
                {
                    Name = "Pocztowy 2",
                    SerialNo = "p0002",
                    Temporary = false,
                    Owner = context.Customers.FirstOrDefault(c => c.Name.Equals("Poczta Polska")),
                    Components = new List<Component>
                    {
                        new Component { Name = "Core 2 Duo", SerialNo ="C2D xxx2", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Procesor"))},
                        new Component { Name = "Logitech HeyKey", SerialNo ="lg112", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Klawiatura"))},
                        new Component { Name = "LG 17c", SerialNo ="17c002", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Monitor"))}
                    }
                },
                new Computer
                {
                    Name = "Serwisowy",
                    SerialNo = "mx102",
                    Temporary = false,
                    Owner = context.Customers.FirstOrDefault(c => c.Name.Equals("PC Medyk")),
                    Components = new List<Component>
                    {
                        new Component { Name = "AMD Athlon 1600+", SerialNo ="At 1600+ 1", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Procesor"))},
                        new Component { Name = "Logitech Mouse", SerialNo ="ml74444", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Mysz"))},
                        new Component { Name = "NoName 15 ph", SerialNo ="nn1", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Monitor"))},
                        new Component { Name = "Dell IBA", SerialNo ="A02", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Klawiatura"))}
                    }
                },
                new Computer
                {
                    Name = "Multimedialny",
                    SerialNo = "ph111",
                    Temporary = false,
                    Owner = context.Customers.FirstOrDefault(c => c.Name.Equals("Philips")),
                    Components = new List<Component>
                    {
                        new Component { Name = "GeForce GT9600", SerialNo ="GT9600-123", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Karta graficzna"))},
                        new Component { Name = "Seagate 256G", SerialNo ="SG MG256A001", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("SSD"))}
                    }
                },
                new Computer
                {
                    Name = "Audio/Video",
                    SerialNo = "rf³-av",
                    Temporary = false,
                    Owner = context.Customers.FirstOrDefault(c => c.Name.Equals("Philips")),
                    Components = new List<Component>
                    {
                        new Component { Name = "Seagate 500G", SerialNo ="SG MG500A003", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("SSD"))}
                    }
                },
                new Computer
                {
                    Name = "Big Data 2",
                    SerialNo = "g-bd-2",
                    Temporary = false,
                    Owner = context.Customers.FirstOrDefault(c => c.Name.Equals("Google")),
                    Components = new List<Component>
                    {
                        new Component { Name = "WD Hiper Mega 10TB", SerialNo ="WD 10T1500100900", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("HDD"))},
                        new Component { Name = "WD Hiper Mega 10TB", SerialNo ="WD 10T1500100901", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("HDD"))}
                    }
                },
                new Computer
                {
                    Name = "Big Data 1",
                    SerialNo = "g-bd-1",
                    Temporary = false,
                    Owner = context.Customers.FirstOrDefault(c => c.Name.Equals("Google")),
                    Components = new List<Component>
                    {
                        new Component { Name = "Intel Xeon E5-2699 v4", SerialNo ="X2699-aa00bb1", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("Procesor"))},
                        new Component { Name = "WD Hiper Mega 10TB", SerialNo ="WD 10T1500100902", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("HDD"))},
                        new Component { Name = "WD Hiper Mega 10TB", SerialNo ="WD 10T1500100903", Type = context.ComponentTypes.FirstOrDefault(c => c.Name.Equals("HDD"))}
                    }
                }
                );

            context.SaveChanges();
        }
    }
}
