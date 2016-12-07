using System;
using System.Collections.Generic;
using System.Configuration;
using Ansira;
using Ansira.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnsiraSDKTests
{


    /// <summary>
    ///This is a test class for ApiClientTest and is intended
    ///to contain all ApiClientTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ApiClientTest
    {


        private TestContext testContextInstance;

        private string clientId, clientSecret;
        private SourceCode sourceCode;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            this.clientId = ConfigurationManager.AppSettings["Test.Client.Id"];
            this.clientSecret = ConfigurationManager.AppSettings["Test.Client.Secret"];
            this.sourceCode = new SourceCode() { Id = 1, KeyName = "TS", Name = "Test" };
        }

        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ApiClient Constructor
        ///</summary>
        [TestMethod()]
        public void ApiClientConstructorTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateUser, UpdateUser, and DeleteUser
        ///</summary>
        [TestMethod()]
        public void CreateUpdateAndDeleteUserTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            User user = new User()
            {
                LastName = "User",
                FirstName = "Test",
                SourceCode = sourceCode,
                Email = Guid.NewGuid().ToString("N") + "@fosfor.us",
                Address = new Address()
                {
                    Address1 = "209 E 6th",
                    City = "Austin",
                    State = "TX",
                    PostalCode = "78701"
                }
            };
            //user.Subscribe("FR", 11, sourceId); // TODO: breaks on update

            User returnUser = target.CreateUser(user);

            Assert.IsNotNull(returnUser, "CreateUser gets valid response");
            Assert.IsTrue(returnUser.Email == user.Email, "CreateUser returns updated object");
            Assert.IsNotNull(returnUser.Uuid, "CreateUser sets UUID");
            Console.WriteLine("Returned Object: UUID = " + returnUser.Uuid);

            Console.WriteLine("Object created, attempting to update");
            returnUser.FirstName = "Updated";
            User updateUser = target.UpdateUser(returnUser);
            Assert.IsNotNull(updateUser, "UpdateUser gets valid response");
            Assert.IsNotNull(updateUser.Uuid, "UpdateUser sets UUID");
            Assert.IsTrue(updateUser.FirstName == returnUser.LastName, "UpdateUser returns updated object");

            Console.WriteLine("Object updated, attempting to delete");
            target.DeleteUser(returnUser);

            Console.WriteLine("Object deleted, attempting to retrieve");
            User deletedUser = target.FindUserByEmail(user.Email);
            Assert.IsNull(deletedUser, "FindUserByEmail does not retrieve deleted user?");
        }

        /// <summary>
        ///A test for CreateUser with all available data
        ///</summary>
        [TestMethod()]
        public void ExtendedCreateUserTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            User user = new User()
            {
                LastName = "User",
                MiddleName = "T",
                FirstName = "Test",
                SourceCode = sourceCode,
                Email = Guid.NewGuid().ToString("N") + "@fosfor.us",
                //EmailVerified = "0", // API conflict - docs say [1|0], live API says date?
                //AddressVerified = "1", // Not working
                // Password should be handled via OAuth or similar, not via API
                //PetOwnershipPlans = "none",
                Address = new Address()
                {
                    Address1 = "209 E 6th",
                    Address2 = "Ste 200",
                    City = "Austin",
                    State = "TX",
                    PostalCode = "78701",
                    PrimaryPhone = "123-123-5432",
                    AltPhone = "123-543-1234",
                    Country = new Country() { Name = "United States", KeyName = "US" }
                }
            };
            //user.Subscribe("FR", 11, sourceId);
            //user.Subscribe("PU", 18, sourceId);
            //user.Subscribe("PE", 1, sourceId);

            ///user.Pets = new List<Pet>();

            /*user.Pets.Add(new Pet()
            {
                SourceId = sourceId,
                Name = "Tester",
                PetTypeId = 1,
                BreedId = 8,
                DryFoodId = 4,
                WetFoodId = 4,
                DateOfBirth = DateTime.Now.Date.AddYears(-3).AddMonths(-12),
                DateOfAdoption = DateTime.Now.Date.AddYears(-3).AddMonths(-1)
            }
            );*/

            User returnUser = target.CreateUser(user);

            Assert.IsNotNull(returnUser, "CreateUser gets valid response");
            Assert.IsTrue(returnUser.LastName == user.LastName, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.MiddleName == user.MiddleName, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.FirstName == user.FirstName, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.SourceCode.Id == sourceCode.Id, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.Email == user.Email, "CreateUser returns correct data");
            //Assert.IsTrue(returnUser.PetOwnershipPlans == user.PetOwnershipPlans, "CreateUser returns correct data");

            Assert.IsNotNull(returnUser.Address);
            Assert.IsTrue(returnUser.Address.Address1 == user.Address.Address1, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.Address2 == user.Address.Address2, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.City == user.Address.City, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.State == user.Address.State, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.PostalCode == user.Address.PostalCode, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.PrimaryPhone == "1231235432", "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.AltPhone == "1235431234", "CreateUser returns correct Address data");

            /*Assert.IsNotNull(returnUser.Pets);
            Assert.IsNotNull(returnUser.Pets[0]);
            Assert.IsTrue(returnUser.Pets[0].SourceId == sourceId, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].Name == user.Pets[0].Name, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].PetTypeId == user.Pets[0].PetTypeId, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].BreedId == user.Pets[0].BreedId, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].DryFoodId == user.Pets[0].DryFoodId, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].WetFoodId == user.Pets[0].WetFoodId, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].DateOfBirth == user.Pets[0].DateOfBirth, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].DateOfAdoption == user.Pets[0].DateOfAdoption, "CreateUser returns correct Pets data");*/


            Assert.IsNotNull(returnUser.Uuid, "CreateUser sets UUID");
            Assert.IsNotNull(returnUser.Id, "CreateUser sets Id");
            Assert.IsNotNull(returnUser.Address.Id, "CreateUser sets Address Id");
            //Assert.IsNotNull(returnUser.Pets[0].Id, "CreateUser sets Pet Id");
            //Assert.IsNotNull(returnUser.Pets[0].UserId, "CreateUser sets Pet UserId");
            Console.WriteLine("Returned Object: UUID = " + returnUser.Uuid);

            Console.WriteLine("Object updated, attempting to delete");
            target.DeleteUser(returnUser);
        }

        /// <summary>
        ///A test for FindUserByEmail
        ///Presumes user email tkiehne@fosforus.net already exists
        ///</summary>
        [TestMethod()]
        public void FindUserByEmailTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            User user = target.FindUserByEmail("tkiehne@fosforus.net");

            Assert.IsNotNull(user, "FindUserByEmail gets valid response");
            Assert.IsNotNull(user.Uuid, "FindUserByEmail set UUID");
            Assert.IsTrue(user.Email == "tkiehne@fosforus.net", "FindUserByEmail returns valid object");

            Console.WriteLine("Object: UUID = " + user.Uuid);
        }

        /// <summary>
        ///A test for FindUserByName
        ///Preumes user name "Tom Kiehne" already exists
        ///</summary>
        [TestMethod()]
        public void FindUserByNameTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            User user = target.FindUserByName("Kiehne", "Tom");

            Assert.IsNotNull(user, "FindUserByName gets valid response");
            Assert.IsNotNull(user.Uuid, "FindUserByEmail set UUID");
            Assert.IsTrue(user.LastName == "Kiehne", "FindUserByName returns valid object");

            Console.WriteLine("Object: Email = " + user.Email);
        }

        /// <summary>
        ///A test for FindUserByUuid
        ///Presumes user UUID f3804062-8248-11e4-8559-22000a8b39f0 already exists
        ///</summary>
        [TestMethod()]
        public void FindUserByUuidTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            User user = target.FindUserByUuid("f3804062-8248-11e4-8559-22000a8b39f0");

            Assert.IsNotNull(user, "FindUserByUuid gets valid response");
            Assert.IsNotNull(user.Uuid, "FindUserByEmail set UUID");
            Assert.IsTrue(user.Uuid == "f3804062-8248-11e4-8559-22000a8b39f0", "FindUserByUuid returns valid object");

            Console.WriteLine("Object: Email = " + user.Email);
        }

        /// <summary>
        /// Test for User subscription methods - does not hit live API
        /// </summary>
        [TestMethod()]
        public void SubscribeUserTest()
        {
            User user = new User()
            {
                LastName = "User",
                FirstName = "Test",
                SourceCode = sourceCode,
                Email = "tkiehne@fosfor.us",
                Address = new Address()
                {
                    Address1 = "209 E 6th",
                    City = "Austin",
                    State = "TX",
                    PostalCode = "78701"
                }
            };

            //user.Subscribe("FR", 11, this.sourceId);
            //user.Subscribe("PE", 1, this.sourceId);

            /*Assert.IsNotNull(user.Subscriptions);
            Assert.IsInstanceOfType(user.Subscriptions["FR"], typeof(Subscription));
            Assert.IsTrue(user.Subscriptions["FR"].BrandId == 11);
            Assert.IsTrue(user.Subscriptions["FR"].EmailStatus == "1");

            user.Unsubscribe("FR", 11, this.sourceId);

            Assert.IsNotNull(user.Subscriptions);
            Assert.IsInstanceOfType(user.Subscriptions["FR"], typeof(Subscription));
            Assert.IsTrue(user.Subscriptions["FR"].BrandId == 11);
            Assert.IsTrue(user.Subscriptions["FR"].EmailStatus == "0"); */
        }

        /// <summary>
        ///A test for GetAllBrands
        ///</summary>
        [TestMethod()]
        public void GetAllBrandsTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            IList<Brand> brands = target.GetBrands();

            Assert.IsNotNull(brands, "GetBrands gets valid response");
            Assert.IsTrue(brands.Count > 1, "GetBrands returns data");

            Console.WriteLine("First object: " + brands[0].Id + " = " + brands[0].Name);
        }

        /// <summary>
        ///A test for GetPetBreeds
        ///</summary>
        [TestMethod()]
        public void GetPetBreedsTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            IList<Breed> breeds = target.GetBreeds();

            Assert.IsNotNull(breeds, "GetBreeds gets valid response");
            Assert.IsTrue(breeds.Count > 1, "GetBreeds returns data");

            Console.WriteLine("First object: " + breeds[0].Id + " = " + breeds[0].Name);
        }

        /// <summary>
        ///A test for GetPetTypes
        ///</summary>
        [TestMethod()]
        public void GetPetTypesTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            IList<PetType> types = target.GetPetTypes();

            Assert.IsNotNull(types, "GetPetTypes gets valid response");
            Assert.IsTrue(types.Count > 1, "GetPetTypes returns data");

            Console.WriteLine("First object: " + types[0].Id + " = " + types[0].KeyName);
        }

        [TestMethod()]
        public void ProductionTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, false);

            User prodUser = target.FindUserByEmail("kiehnet@netscape.net");
            Assert.IsNotNull(prodUser, "FindUserByEmail does not retrieve deleted user");
            
            //prodUser.SourceId = sourceId;
            User updateUser = target.UpdateUser(prodUser);
            Assert.IsNotNull(updateUser, "UpdateUser gets valid response");
            Assert.IsNotNull(updateUser.Uuid, "UpdateUser sets UUID");
            Assert.IsTrue(updateUser.FirstName == prodUser.FirstName, "UpdateUser returns updated object");

        }
    }
}
