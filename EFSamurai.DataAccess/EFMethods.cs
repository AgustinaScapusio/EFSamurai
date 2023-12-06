using EFSamurai.Domain;
using EFSamurai.Domain.Entities;
using Microsoft.Data.SqlClient;
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
                        new BattleEvent { }
                    }
                }
            };

            db.Battle.Add(newBattle);
            db.SaveChanges();
            return newBattle.Id;
        }


        public static void LinkBattleAndSamurais(int battleId, List<int> samuraiIds)
        {
            using SamuraiDbContext db = new();
            foreach (int samuraiId in samuraiIds)
            {
                db.SamuraiBattle.Add(new() { BattleId = battleId, SamuraiId = samuraiId });
            }
            db.SaveChanges();

        }
        public static int CountBattlesForSamurai(int samuraiId, bool? isBrutal = null)
        {
            using SamuraiDbContext db = new();
            if (isBrutal != null)
            {
                return db.SamuraiBattle.Include(sb => sb.Battle)
                                              .Where(s => s.SamuraiId == samuraiId && s.Battle!.IsBrutal == isBrutal)
                                              .Select(s => s.Battle).Count();

            }
            else
            {
                return db.SamuraiBattle.Include(sb => sb.Battle)
                                            .Where(s => s.SamuraiId == samuraiId)
                                            .Select(s => s.Battle).Count();
            }
        }

        public static List<Samurai> ReadSamuraisById()
        {
            using SamuraiDbContext db = new();
            List<Samurai> samurais = db.Samurai.OrderBy(s => s.Id).ToList();
            return samurais;
        }
        public static List<Quote> ReadQuotesOfStyle(QuoteStyle quoteStyle)
        {
            using SamuraiDbContext db = new();
            List<Quote> quotes = db.Quote.Where(q => q.QuoteStyle == quoteStyle).ToList();
            return quotes;

        }
        public static void CreateSamuraiWithRelatedData(Samurai samurai, SecretIdentity sI, Quote quote, Battle battle)
        {
            using SamuraiDbContext db = new();
            List<int> samuraiIds = new() { samurai.Id };
            CreateSamurai(samurai);
            UpdateSamuraiSetSecretIdentityRealName(sI.Id, sI.RealName!);
            CreateBattle(battle);
            LinkBattleAndSamurais(battle.Id, samuraiIds);
            db.Add(quote);
            db.SaveChanges();

        }
        public static List<string> StringifyQuotesOfStyleAddSamuraiName(QuoteStyle quoteStyle)
        {
            string s = string.Empty;
            using SamuraiDbContext db = new();
            List<string> quotes = db.Quote//.Include(q => q.Samurai)
                                          .Where(q => q.QuoteStyle == quoteStyle && q.SamuraiId == q.Samurai!.Id)
                                          .Select(s => $"'{s.Text}' is a {s.QuoteStyle.ToString().ToLower()} quote by {s.Samurai!.Name ?? string.Empty}").ToList();


            return quotes;
        }

        public static List<string> StringifyBattles(DateTime from, DateTime to, bool? isBrutal)
        {
            using SamuraiDbContext db = new();
            string s = string.Empty;

            if (isBrutal != null)
            {

                List<string>? battles = db.Battle.Where(b => b.IsBrutal == isBrutal && b.StartDate <= from && b.EndDate >= to)
                    .Select(b => $"{b.Name} is {((bool)isBrutal ? "" : "not")} a brutal battle within the period {from} to {to}")
                                        .ToList();
                return battles;
            }
            else
            {
                List<string>? battles = db.Battle.Where(b => b.StartDate <= from && b.EndDate >= to)
                    .Select(b => $"{b.Name} is {((bool)isBrutal! ? "" : "not")} a brutal battle within the period {from} to {to}")
                                            .ToList();
                return battles;

            }
        }

        public static ICollection<string> StringifySamuraiNamesAddAliases()
        {
            using SamuraiDbContext db = new ();
            List<string> names = db.Samurai.Include(s => s.SecretIdentity)
                                           .Where(s => s.Id == s.SecretIdentity!.SamuraiID)
                                           .Select(s => $"{s.Name} alias {s.SecretIdentity!.RealName ?? "NoRealName"}").ToList();
            return names;
        }

        public static List<string> StringifyBattlesWithLog(DateTime from, DateTime to, bool isBrutal)
        {
            using SamuraiDbContext db = new();

            List<Battle> battles = db.Battle.Include(b => b.BattleLog)
                                          .ThenInclude(b => b!.BattleEvents)
                                          .Where(b => b.IsBrutal == isBrutal && b.StartDate <= from && b.EndDate >= to).ToList();
            
            List<string> text = new();
            foreach (Battle b in battles)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("----------------------------------------------------------");
                sb.AppendLine($"Name of the battle:            {b.Name}");
                sb.AppendLine($"Log name            {b.BattleLog?.Name ?? "Unknown"}");
                foreach (BattleEvent battle in b.BattleLog?.BattleEvents ?? new List<BattleEvent> { new() { Sumary = "Unknown" } })
                    sb.AppendLine(battle.Sumary);

            text.Add(sb.ToString());
            sb.Clear();
            }
            return text;
        }
    }
}

