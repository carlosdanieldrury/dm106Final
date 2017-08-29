namespace dm106CarlosDrury.Migrations
{
    using dm106CarlosDrury.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<dm106CarlosDrury.Models.dm106CarlosDruryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(dm106CarlosDrury.Models.dm106CarlosDruryContext context)
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


            context.Products.AddOrUpdate(p => p.Id,
                new Product
                {
                    Id = 1,
                    nome = "produto 1",
                    codigo = "COD1",
                    descricao = "descrição produto 1",
                    cor = "vermelho",
                    modelo = "modelo 1",
                    preco = 10,
                    peso = 10.1M,
                    altura = 1.5M,
                    comprimento = 1.4M,
                    diametro = 0.5M,
                    Url = "www.teste.com"
                },
                new Product
                {
                    Id = 2,
                    nome = "produto 2",
                    codigo = "COD2",
                    descricao = "descrição produto 2",
                    cor = "azul",
                    modelo = "modelo 2",
                    preco = 5,
                    peso = 9.1M,
                    altura = 0.5M,
                    comprimento = 0.4M,
                    diametro = 0.7M,
                    Url = "www.teste.com"
                },
                new Product
                {
                    Id = 3,
                    nome = "produto 3",
                    codigo = "COD3",
                    descricao = "descrição produto 3",
                    cor = "preto",
                    modelo = "modelo 3",
                    preco = 20,
                    peso = 7.1M,
                    altura = 2M,
                    comprimento = 1.0M,
                    diametro = 0.3M,
                    Url = "www.teste.com"
                }
                );


        }
    }
}
