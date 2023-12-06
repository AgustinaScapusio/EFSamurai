using EFSamurai.DataAccess;
using Microsoft.EntityFrameworkCore.Design;
using EFSamurai.Domain;
using EFSamurai.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFSamurai.App
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //List<int> samuraiId = new() { 37, 38, 39 };

            //EFMethods.LinkBattleAndSamurais(5, samuraiId);

            //Battle newBattle1 = new Battle() { IsBrutal = false, Name = "BBB" };
            //Battle newBattle2 = new Battle() { IsBrutal = false, Name = "CCC" };
            //Battle newBattle3 = new Battle() { IsBrutal = false, Name = "DDD" };
            //List<int> samurais = new List<int>() { 103, 104, 105 };
            ////EFMethods.CreateBattle(newBattle1);
            ////EFMethods.CreateBattle(newBattle2);
            ////EFMethods.CreateBattle(newBattle3);
            ////EFMethods.LinkBattleAndSamurais(37, samurais);
            ////EFMethods.LinkBattleAndSamurais(35, samurais);
            ////EFMethods.LinkBattleAndSamurais(36, samurais);
            //var i = EFMethods.CountBattlesForSamurai(103, false);
            //Console.WriteLine(i);

            //using SamuraiDbContext context = new SamuraiDbContext();
            //Quote quote = new Quote() { Text = "Live, laugh, love", SamuraiId = 138, QuoteStyle = QuoteStyle.Cheesy };
            //Quote quote1 = new Quote() { Text = "Life is like a rollercoaster of emotions", SamuraiId = 139, QuoteStyle = QuoteStyle.Lame };
            //context.Add(quote1);
            //context.Add(quote);
            //context.SaveChanges();
            //List<string> cheesy = EFMethods.StringifyQuotesOfStyleAddSamuraiName(QuoteStyle.Cheesy);
            //List<string> lame = EFMethods.StringifyQuotesOfStyleAddSamuraiName(QuoteStyle.Lame);

            //foreach (string c in cheesy)
            //{
            //    Console.WriteLine(c);
            //}
            //foreach(string c in lame)
            //{
            //    Console.WriteLine(c);
            //}
        }
    }
}