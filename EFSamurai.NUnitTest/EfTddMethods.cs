using System;
using System.Collections.Generic;
using System.Linq;
using EFSamurai.DataAccess.Migrations;
using EFSamurai.Domain;
using EFSamurai.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace EFSamurai.DataAccess
{
    public class EfTddMethods
    {
        #region Helper methods

        public static void RebuildDatabase()
        {
            using SamuraiDbContext db = new();

            db.Database.EnsureDeleted();
            db.Database.Migrate();
            //db.Database.EnsureCreated();
        }

        public static void ResetIdentityStartingValue(string tableName, int startingValue = 1)
        {

            using (SamuraiDbContext db = new SamuraiDbContext())
            {
                db.Database.ExecuteSqlRaw("IF EXISTS(SELECT * FROM sys.identity_columns " +
                    "WHERE OBJECT_NAME(OBJECT_ID) = @tableName AND last_value IS NOT NULL) " +
                    "DBCC CHECKIDENT(@tableName, RESEED, @startingValueMinusOne);",
                    new SqlParameter("tableName", tableName),
                    new SqlParameter("startingValueMinusOne", startingValue - 1));
            }
        }
        public static void ClearAllData()
        {
            using SamuraiDbContext db = new();

            db.RemoveRange(db.Samurai);
            db.RemoveRange(db.Battle);

            ResetIdentityStartingValue("Samurai");
            ResetIdentityStartingValue("SecretIdentity");
            ResetIdentityStartingValue("Quote");
            ResetIdentityStartingValue("Battle");
            ResetIdentityStartingValue("BattleLog");
            ResetIdentityStartingValue("BattleEvent");

            db.SaveChanges();
        }

        #endregion Helper methods

        public static int CreateSamurai(string name, HairStyle hairStyle)
        {
            using SamuraiDbContext db = new SamuraiDbContext();
            Samurai samurai = new Samurai() { Name = name, HairStyle = hairStyle };
            db.Add(samurai);
            db.SaveChanges();
            return samurai.Id;
        }
        public static void CreateSamurai(Samurai samurai)
        {
            using SamuraiDbContext db = new SamuraiDbContext();

            db.Add(samurai);
            db.SaveChanges();
        }


        public static List<string> ReadAlphabeticallyAllSamuraiNamesWithSpecificHairstyle(HairStyle hairstyle)
        {
            using SamuraiDbContext db = new SamuraiDbContext();
            List<string> names = db.Samurai.Where(h => h.HairStyle == hairstyle)
                                           .OrderBy(s => s.Name)
                                           .Select(p => p.Name)
                                           .ToList();
            return names;
        }
        public static List<Quote> ReadAllQuotesWithSpecificQuoteStyle(QuoteStyle quoteStyle)
        {
            using SamuraiDbContext db = new SamuraiDbContext();
            List<Quote> quotes = db.Quote.Include(s => s.Samurai)
                                        .Where(q => q.QuoteStyle == quoteStyle).ToList();
            return quotes;
        }

        public static int CreateBattle(string name, bool isBrutal, string description, DateTime startDate, DateTime EndDate)
        {
            using SamuraiDbContext db = new SamuraiDbContext();
            Battle battle = new Battle() { 
                Name = name, 
                IsBrutal = isBrutal, 
                Description = description, 
                StartDate = startDate, 
                EndDate = EndDate };
            db.Add(battle);
            db.SaveChanges();
            return battle.Id;
        }
        public static Battle? ReadOneBattle(int battleId)
        {
            using SamuraiDbContext db = new SamuraiDbContext();
            Battle? battle = db.Battle.Where(b => b.Id == battleId).SingleOrDefault();
            return battle;
        }

        public static void CreateOrUpdateSecretIdentitySetRealName(int samuraiId, string realName)
        {
            using SamuraiDbContext db = new SamuraiDbContext();
            bool doesExist = db.SecretIdentity.Where(s => s.SamuraiID == samuraiId).Any();
            if (!doesExist)
            {
                SecretIdentity secretI = new() { SamuraiID = samuraiId, RealName = realName };
                db.Add(secretI);
                db.SaveChanges();
            }
            else
            {
                SecretIdentity sI = db.SecretIdentity.Where(s => s.SamuraiID == samuraiId).Single()!;
                sI.RealName = realName;
                db.Update(sI);
                db.SaveChanges();
                
            }
        }
       
       public static SecretIdentity? ReadSecretIdentityOfSpecificSamurai(int samuraiId)
        {
            using SamuraiDbContext db = new SamuraiDbContext();

            SecretIdentity sI=db.SecretIdentity.Include(s=>s.Samurai).Where(s=>s.SamuraiID==samuraiId).SingleOrDefault()!;
            return sI;
        }
    }
}
