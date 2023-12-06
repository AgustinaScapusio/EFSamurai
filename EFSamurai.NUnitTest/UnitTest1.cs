using EFSamurai.Domain;
using EFSamurai.DataAccess;
using EFSamurai.App;
using EFSamurai.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace EFSamurai.NUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            EFMethods.ClearDatabase();
        }

        //[OneTimeSetUp] // Marks a method to be called ONCE, at start of a test-run.
        //public void OneTimeSetup()
        //{
        //    EFMethods.RebuildDatabase();
        //}
        [Test]
        public void ReadSamuraiNames_TwoSamurais_NamesAreCorrect()
        {
            // Arrange
            EFMethods.CreateSamurai("Zelda");
            EFMethods.CreateSamurai("Link");
            // Act
            List<string> result = EFMethods.ReadSamuraiNames();
            // Assert
            CollectionAssert.AreEqual(new List<string>() { "Zelda", "Link" }, result);
        }

        [Test]
        public void CreateSamurais_ThreeSamurais_NamesAreCorrect()
        {
            //Arrange
            List<Samurai> samurais = new List<Samurai>() {
                new Samurai(){Name="Agus"},
                new Samurai() {Name="Arman" },
                new Samurai(){Name="Arne"} };

            //Act
            EFMethods.CreateSamurais(samurais);
            List<string> expected = new() { "Agus", "Arman", "Arne" };
            List<string> actual = EFMethods.ReadSamuraiNames();

            //Assert
            CollectionAssert.AreEqual(actual, expected);
        }
        [Test]
        public void ReadSamurai_TwoSamurais_ObjectsAreCorrect()
        {
            //Arrange
            Samurai actualSamurai = new Samurai()
            {
                Name = "Zelda",
                HairStyle = HairStyle.Oicho,
                Quotes = new List<Quote> { new() { Text = "Live, laugh, love" } }
            };
            EFMethods.CreateSamurai(actualSamurai);
            Samurai expected = new Samurai()
            {
                Name = "Zelda",
                HairStyle = HairStyle.Oicho,
                Quotes = new List<Quote> { new() { Text = "Live, laugh, love" } }
            };
            //Act
            Samurai actual = EFMethods.ReadSamurai(actualSamurai.Id)!;

            //Assert
            Assert.That(actual!.Name, Is.EqualTo(expected.Name));
            CollectionAssert.AreEqual(actual.HairStyle.ToString(), expected.HairStyle.ToString());
        }

        [Test]
        public void Delete_Samurai_RowsAffected()
        {
            //Arrange
            int expectCount = 0;
            using SamuraiDbContext context = new SamuraiDbContext();
            Samurai actualSamurai = new Samurai()
            {
                Name = "Zelda",
                HairStyle = HairStyle.Oicho,
                Quotes = new List<Quote> { new() { Text = "Live, laugh, love" } }
            };
            EFMethods.CreateSamurai(actualSamurai);
            //Act
            EFMethods.DeleteSamurai(actualSamurai);
            int actual = context.Samurai.Count();
            //Assert
            Assert.That(actual, Is.EqualTo(expectCount));
        }

        [Test]
        public void Delete_Samurai()
        {
            //Arrange
            Samurai actualSamurai = new Samurai()
            {
                Name = "Zelda",
                HairStyle = HairStyle.Oicho,
                Quotes = new List<Quote> { new() { Text = "Live, laugh, love" } }
            };
            EFMethods.CreateSamurai(actualSamurai);
            //Act
            //Det finnes Samurai med id=10
            bool actualActual = EFMethods.DeleteSamurai(actualSamurai.Id);
            bool expectedExpected = true;
            //Assert
            Assert.That(actualActual, Is.EqualTo(expectedExpected));
        }

        [Test]
        public void UpdateSamuraiSetSecretIdentityRealName_ReturnBool()
        {
            //Arrange
            Samurai actualSamurai = new Samurai()
            {
                Name = "Zelda",
                HairStyle = HairStyle.Oicho,
                Quotes = new List<Quote> { new() { Text = "Live, laugh, love" } }
            };
            EFMethods.CreateSamurai(actualSamurai);
            //Act
            bool actual = EFMethods.UpdateSamuraiSetSecretIdentityRealName(12, "Agus");
            bool expected = false;
            bool actualActual = EFMethods.UpdateSamuraiSetSecretIdentityRealName(actualSamurai.Id, "Agus Scapusio");
            bool expectedExpected = true;

            //Assert
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(actualActual, Is.EqualTo(expectedExpected));
        }
        [Test]
        public void CreateBattle_TwoBattle_ObjectsAreCorrect()
        {   //Arrange
            Battle expectedBattle = new Battle()
            {   Id=12,
                Name = "Battle of Winterfell",
                IsBrutal = true,
                Description = "between humans and whitewalkers"
            };
            Battle actualBattle = new Battle()
            {   
                Name = "Battle of Winterfell",
                IsBrutal = true,
                Description = "between humans and whitewalkers"
            };
            //Act
            int id= EFMethods.CreateBattle(actualBattle);
            using SamuraiDbContext db= new SamuraiDbContext();
            string s=db.Battle.Select(b=> b.Name).First();
           
            //Assert
            Assert.That(expectedBattle.Id, Is.EqualTo(id));
            Assert.That(s, Is.EqualTo("Battle of Winterfell"));
        }

        [Test]
        public void LinkBattleToSamurai_GetIdOfSamurai_CompareWithBattle()
        {   //Arrange
            using SamuraiDbContext db = new SamuraiDbContext();
            List<Samurai> samurais = new()
            {
                new Samurai(){Name="Toyota",HairStyle= HairStyle.Oicho},
                new Samurai(){Name="Akeyu",HairStyle= HairStyle.Oicho},
                new Samurai(){Name="Ars",HairStyle=HairStyle.Chonmage}
            };
            EFMethods.CreateSamurais(samurais);
          
            List<int> samuraiIds = samurais.Select(s => s.Id).ToList();

            Battle battle = new Battle() { 
                Name = "Battle of Winterfell",
                Description="Between humans and whitewalkers",
                IsBrutal=true };
            var id=EFMethods.CreateBattle(battle);
            //Act
            
            EFMethods.LinkBattleAndSamurais(id, samuraiIds);
            List<int> actualIds= db.SamuraiBattle.Select(sb=>sb.SamuraiId).ToList();
            List<string> actualNames = db.SamuraiBattle.Include(sb => sb.Samurai).Select(s => s.Samurai!.Name).ToList();
            List<string> expectedNames = new() { "Toyota", "Akeyu", "Ars" };
            List<int> expectedIds = samuraiIds;
            //Assert
            CollectionAssert.AreEqual(expectedNames, actualNames);
            CollectionAssert.AreEqual(expectedIds, actualIds);
        }

        [Test]
        public void CountBattlesForSamurai_CompareTwoInt_ReturnBool()
        {   //Arrange
            Battle newBattle1 = new Battle() { IsBrutal = false, Name = "BBB" };
            Battle newBattle2 = new Battle() { IsBrutal = false, Name = "CCC" };
            Battle newBattle3 = new Battle() { IsBrutal = false, Name = "DDD" };
            EFMethods.CreateBattle(newBattle1);
            EFMethods.CreateBattle(newBattle2);
            EFMethods.CreateBattle(newBattle3);
            Samurai sam = new Samurai() { Name = "Agus", HairStyle = HairStyle.Chonmage };
            EFMethods.CreateSamurai(sam);
            List<int> samurais = new List<int>() { sam.Id };
            
            EFMethods.LinkBattleAndSamurais(newBattle1.Id, samurais);
            EFMethods.LinkBattleAndSamurais(newBattle2.Id, samurais);
            EFMethods.LinkBattleAndSamurais(newBattle3.Id, samurais);
            //Act
            int actual = EFMethods.CountBattlesForSamurai(sam.Id, false);
            int expected = 3;
            //Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ReadSamuraisOrderById_CompareTwoList_ReturnBool()
        {//Arrange
            List<Samurai> samurais = new() {
            new Samurai()
            {
                Name = "Zelda",
                HairStyle = HairStyle.Oicho,
                Quotes = new List<Quote> { new() { Text = "Live, laugh, love" } },
                SecretIdentity= new SecretIdentity() { RealName="Peach"}
            },
            new Samurai()
            {
                Name = "Link",
                HairStyle = HairStyle.Oicho,
                Quotes = new List<Quote> { new() { Text = "It´s me, Mario!" } },
                SecretIdentity = new SecretIdentity() { RealName = "Mario" }
            } };
            EFMethods.CreateSamurais(samurais);
            //Act
            List<Samurai> actual= EFMethods.ReadSamuraisById();
            var expected=samurais.OrderBy(s=>s.Id).ToList();    
            //Assert
            CollectionAssert.AreEqual(actual.ToString(), expected.ToString());
        }

        [Test]
        public void ReadQuotesOfStyle()
        {   //Arrange
            using SamuraiDbContext db = new();
            Samurai samurai = new() { Name = "Link" };
            Quote q = new Quote() { Text = "Live, laugh, love", SamuraiId = 151 , QuoteStyle = QuoteStyle.Cheesy };
            EFMethods.CreateSamurai(samurai);
            db.Add(q);
            db.SaveChanges();
            //Act
            List<string> actual = EFMethods.StringifyQuotesOfStyleAddSamuraiName(QuoteStyle.Cheesy);
            List<string> expected = new() { "'Live, laugh, love' is a cheesy quote by Link" };
            //Assert 
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateSamuraiWithRelatedData()
        {   //Arrange
            using SamuraiDbContext db= new();
            Samurai samurai = new()
            {   Name = "Mario",
                HairStyle = HairStyle.Western,
                Quotes = new List<Quote> { new() { Text = "Live, laugh, love" } },
                SecretIdentity = new() { RealName = "Link" },
            };
            Battle battle = new Battle() 
            {   Name="Battle of Winterfell",
                Description="When Mario killed whitewalkers in Winterfell with Luigi",
            };
            Quote quote = new Quote() { Text= "Live, laugh ,love" };
            SecretIdentity secretIdentity = new() { RealName = "Link" };
            EFMethods.CreateBattle(battle);
            
            //Act
            EFMethods.CreateSamuraiWithRelatedData(samurai,secretIdentity,quote, battle);
            Samurai actual=db.Samurai
                .Where(s=>s.Name =="Mario").Single();
            //Assert
            Assert.That(actual.HairStyle,Is.EqualTo(samurai.HairStyle));
            Assert.That(actual.Quotes, Is.EqualTo(samurai.Quotes));
            Assert.That(actual.SecretIdentity, Is.EqualTo(samurai.SecretIdentity));
        }

        [Test]
        public void StringifyBattles_CompareTwoBattles_ReturnBool()
        {   //Arrange
            Battle actual = new Battle()
            {
                IsBrutal = true,
                Name = "AAAA",
                StartDate= new DateTime(1995, 02, 16),
                EndDate= new DateTime(1999, 07, 12)
            };
            EFMethods.CreateBattle(actual);
            //Act
            List<string> actual1 = EFMethods.StringifyBattles(new DateTime(1995, 02, 16), new DateTime(1999, 07, 12), true);
          
            var expected = "AAAA is  a brutal battle within the period 16/02/1995 00:00:00 to 12/07/1999 00:00:00";
            //Assert
            Assert.That(expected, Is.EqualTo(actual1[0]));
        }

        [Test]
        public void StringifySamuraiNamesAddAliases()
        {   //Arrangue
            List<Samurai> samurais = new() {
            new Samurai()
            {
                Name = "Zelda",
                HairStyle = HairStyle.Oicho,
                Quotes = new List<Quote> { new() { Text = "Live, laugh, love" } },
                SecretIdentity= new SecretIdentity() { RealName="Peach"}
            },
            new Samurai()
            {
                Name = "Link",
                HairStyle = HairStyle.Oicho,
                Quotes = new List<Quote> { new() { Text = "It´s me, Mario!" } },
                SecretIdentity = new SecretIdentity() { RealName = "Mario" }
            } };
            EFMethods.CreateSamurais(samurais);
            //Act
            List<string>? actual = EFMethods.StringifySamuraiNamesAddAliases().ToList();
            List<string> expected = new() { "Zelda alias Peach", "Link alias Mario" };
            //Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}