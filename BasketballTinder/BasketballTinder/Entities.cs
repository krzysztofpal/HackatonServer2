namespace BasketballTinder
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Entities : DbContext
    {
        // Your context has been configured to use a 'Entities' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'BasketballTinder.Entities' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Entities' 
        // connection string in the application configuration file.
        public Entities()
            : base("name=Entities")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

         public virtual DbSet<MyEntity> MyEntities { get; set; }
         public virtual DbSet<Koszykarz> Koszykarze { get; set; }
         public virtual DbSet<Wydarzenie> Wydarzenia { get; set; }
    }

    public class MyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Koszykarz
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Haslo { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public int Wzrost { get; set; }
    }

    public class Wydarzenie
    {
        public string Id { get; set; }
        public string Lokalizacja { get; set; }
        public DateTime Data { get; set; }
        public double? Cena { get; set; }
    }
}