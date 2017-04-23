using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MistwoodTales.Game.Server.Context;
using MistwoodTales.Game.Server.Data.Models;

namespace MistwoodTales.Game.Server
{
    class Program
    {
        // Used Commands:
        // > Add-Migration -Name "InitialMigration" -Context "ModelDbContext"
        // > Update-Database [-Context "ModelDbContext"  -Migration "InitialMigration"]
        // > Remove-Migration 

        // EF connects to DB at 'localhost' with username 'mistwoodtales' and password 'mistwoodtales'

        static void Main(string[] args)
        {
            string postgresConnectionString =
                "User ID=mistwoodtales;Password=mistwoodtales;Host=localhost;Port=5432;Database=mistwoodtales;Pooling=true;";
            using (var db = new ModelDbContext(postgresConnectionString))
            {
                db.Database.Migrate();
                db.SeedInitialDataIfEmpty();
                
                Console.WriteLine($"Accounts: {db.Accounts.Count()}; Chars: {db.Characters.Count()}");
                //db.Wipe();
            }
            Console.ReadKey();
        }
    }
}