using EFSamurai.DataAccess;
using Microsoft.EntityFrameworkCore.Design;
using EFSamurai.Domain;
using EFSamurai.Domain.Entities;

namespace EFSamurai.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Battle battle = new Battle()
            {
                Name = "battle between arne and Iman",
                IsBrutal = true
            };
            EFMethods.CreateBattle(battle);

        }
    }
}