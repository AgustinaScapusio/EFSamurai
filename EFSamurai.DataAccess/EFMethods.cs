using EFSamurai.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.DataAccess
{
    public static class EFMethods
    {
        public static int CreateSamurai(Samurai samurai)
        {
            using SamuraiDbContext db = new();
            db.Samurai.Add(samurai);
            db.SaveChanges();

            // Id got a new value (is no longer 0) when we did SaveChanges(), above.
            return samurai.Id;
        }

        public static int CreateSamurai(string name)
        {
            // NOTE: We don't want to make two bodies of the same method if we don't
            // have to. So in this (the more restrictive) version, we just reshape the
            // in-parameter and call the other version.
            return CreateSamurai(new Samurai() { Name = name });
        }

        public static void RebuildDatabase()
        {
            using SamuraiDbContext db = new();

            // Deletes the entire database:
            db.Database.EnsureDeleted();

            // Recreates the DB tables, based on the Migrations folder data.
            db.Database.Migrate();
            // NOTE: EnsureCreated(), below, also recreates the DB tables. But
            // it doesn't play well with migrations. Only use it if you
            // don't plan to build further on DB with migrations later.
            //db.Database.EnsureCreated();
        }
        public static List<string> ReadSamuraiNames()
        {
            using SamuraiDbContext db = new();
            List<string> names = db.Samurai.Select(p => p.Name).ToList();
            return names;

        }

        public static void ClearDatabase()
        {
            using SamuraiDbContext db = new();
            db.Samurai.ExecuteDelete();
            db.Battle.ExecuteDelete();
        }

        public static void CreateSamurais(List<Samurai> samuraiList)
        {
            using SamuraiDbContext db = new();
            db.AddRange(samuraiList);
            db.SaveChanges();
        }

        public static Samurai? ReadSamurai(int id)
        {
            using SamuraiDbContext db = new();
            Samurai? samurai = db.Samurai
                             .Include(s => s.SecretIdentity)
                             .Include(s => s.Quotes)
                             .Include(s => s.SamuraiBattles)!
                             .ThenInclude(sb => sb.Battle)
                             .ThenInclude(b => b!.BattleLog)
                             .ThenInclude(bl => bl!.BattleEvents)
                             .SingleOrDefault(p => p.Id == id);
            return samurai;
        }
        public static void DeleteSamurai(Samurai samurai)
        {
            using SamuraiDbContext db = new();
            db.Samurai.Remove(samurai);
            db.SaveChanges();
        }
        public static bool DeleteSamurai(int id)
        {
            using SamuraiDbContext db = new();
            int before = db.Samurai.Count();
            Samurai samurai = db.Samurai.Single(s => s.Id == id);
            DeleteSamurai(samurai);
            if (before != db.Samurai.Count())
            {
                return true;
            }
            else
                return false;
        }
        public static bool UpdateSamuraiSetSecretIdentityRealName(int samuraiId, string realName)
        {
            using SamuraiDbContext db = new();
            if (db.Samurai.Any(s => s.Id == samuraiId))
            {
                db.SecretIdentity.Add(new SecretIdentity { RealName = realName, SamuraiID = samuraiId });
                db.SaveChanges();
                return true;
            }
            
                return false;
        }
        public static int CreateBattle(Battle battle)
        {
            using SamuraiDbContext db = new();

            Battle newBattle = new()
            {
                Name = battle.Name,
                Description = battle.Description,
                IsBrutal = battle.IsBrutal,
                StartDate = battle.StartDate,
                EndDate = battle.EndDate,
                BattleLog = new BattleLog
                {
                    BattleEvents = new List<BattleEvent>
            {
                new BattleEvent
                {
  
                }
            }
                }
            };

            db.Add(newBattle);
            db.SaveChanges();
            return newBattle.Id;
        }

        //public static Quote? ReadQuote(int id)
        //{
        //    using SamuraiDbContext db = new();
        //    Quote? test = db.Quote
        //                        .Include(q => q.Samurai)
        //                        .ThenInclude(s => s.SecretIdentity)
        //                        .Include(q => q.Samurai)
        //                        .ThenInclude(s => s.SamuraiBattles)
        //                        .ThenInclude(sb => sb.Battle)
        //                        .ThenInclude(b => b.BattleLog)
        //                        .ThenInclude(bl => bl.BattleEvents).SingleOrDefault(s=>s.Id==id);
        //    return test;
        //}
    }
}

