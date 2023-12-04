using EFSamurai.Domain;
using EFSamurai.DataAccess;
using EFSamurai.App;
using EFSamurai.Domain.Entities;


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
            { Id=5,
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
            //Assert
            //Assert.That(expectedBattle.Name, Is.EqualTo(actualBattle.Name));
            //Assert.That(expectedBattle.IsBrutal,Is.EqualTo(actualBattle.IsBrutal));
            //Assert.That(expectedBattle.Description,Is.EqualTo(actualBattle.Description));
            Assert.That(expectedBattle.Id, Is.EqualTo(id));

        }
    }
}