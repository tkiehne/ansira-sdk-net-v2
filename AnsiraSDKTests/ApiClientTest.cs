using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Ansira;
using Ansira.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnsiraSDKTests
{
    /// <summary>
    ///This is a test class for Ansira Api ClientTest
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

        #region Miscellaneous Tests
        /// <summary>
        ///A test for ApiClient Constructor
        ///</summary>
        [TestMethod()]
        public void ApiClientConstructorTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            Assert.IsNotNull(target);
        }

        [TestMethod()]
        public void ProductionTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, false);

            User prodUser = target.FindUserByEmail("kiehnet@netscape.net");
            Assert.IsNotNull(prodUser, "FindUserByEmail does not retrieve deleted user");

            prodUser.FirstName = Guid.NewGuid().ToString("N");
            User updateUser = target.UpdateUser(prodUser);
            Assert.IsNotNull(updateUser, "UpdateUser gets valid response");
            Assert.IsNotNull(updateUser.Uuid, "UpdateUser sets UUID");
            Assert.IsTrue(updateUser.FirstName == prodUser.FirstName, "UpdateUser returns updated object");

        }

        #endregion

        #region User Method Tests
        
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
            Assert.IsTrue(updateUser.FirstName == returnUser.FirstName, "UpdateUser returns updated object");

            Console.WriteLine("Object updated, attempting to delete");
            Assert.IsTrue(target.DeleteUser(returnUser));

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
                // Password should be handled via OAuth or similar, not via API
                DogCount = 1,
                CatCount = 0,
                TotalPets = 1,
                //PetOwnershipPlan
                //Language,
                //Nationality,
                //Currency,
                Address = new Address()
                {
                    Address1 = "209 E 6th",
                    Address2 = "Ste 200",
                    City = "Austin",
                    State = "TX",
                    PostalCode = "78701",
                    PrimaryPhone = "123-123-5432",
                    AltPhone = "123-543-1234",
                    Fax = "123-123-8765",
                    Country = new Country() { Name = "United States", KeyName = "US" },
                    Latitude = 30.267298,
                    Longitude = -97.7433287
                }
            };

            user.Pets = new List<Pet>();

            user.Pets.Add(new Pet()
            {
                SourceCode = sourceCode,
                Name = "Tester",
                ImageUrl = "https://www.purina.com/media/284062/Akitas_2913.jpg/560/0/center/middle",
                Size = "Large",
                Color = "Brown",
                IsSterile = false,
                Gender = "female",
                DateOfBirth = DateTime.Now.Date.AddMonths(-11),
                AgeInMonths = 11,
                DateOfAdoption = DateTime.Now.Date.AddMonths(-1),
                AcquisitionMethod = "test",
                DiscoveryMethod = "test",
                DiscoveryMethodDetail = "This is not a real pet, just a test"
                //Species, PrimaryBreed, SecondaryBreed, FoodPrefDry, FoodPrefWet
            }
            );

            User returnUser = target.CreateUser(user);

            Assert.IsNotNull(returnUser, "CreateUser gets valid response");
            Assert.IsTrue(returnUser.LastName == user.LastName, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.MiddleName == user.MiddleName, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.FirstName == user.FirstName, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.SourceCode.Id == sourceCode.Id, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.Email == user.Email, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.DogCount == user.DogCount, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.CatCount == user.CatCount, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.TotalPets == user.TotalPets, "CreateUser returns correct data");
            //Assert.IsTrue(returnUser.PetOwnershipPlan.KeyName == user.PetOwnershipPlan.KeyName, "CreateUser returns correct data");
            //Assert.IsTrue(returnUser.Language.KeyName == user.Language.KeyName, "CreateUser returns correct data");
            //Assert.IsTrue(returnUser.Nationality.KeyName == user.Nationality.KeyName, "CreateUser returns correct data");
            //Assert.IsTrue(returnUser.Currency.KeyName == user.Currency.KeyName, "CreateUser returns correct data");

            Assert.IsNotNull(returnUser.Address);
            Assert.IsTrue(returnUser.Address.Address1 == user.Address.Address1, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.Address2 == user.Address.Address2, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.City == user.Address.City, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.State == user.Address.State, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.PostalCode == user.Address.PostalCode, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.PrimaryPhone == "1231235432", "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.AltPhone == "1235431234", "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.Fax == "1231238765", "CreateUser returns correct Address data");
            //Assert.IsTrue(returnUser.Address.Country == user.Address.Country, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.Latitude == user.Address.Latitude, "CreateUser returns correct Address data");
            Assert.IsTrue(returnUser.Address.Longitude == user.Address.Longitude, "CreateUser returns correct Address data");

            Assert.IsNotNull(returnUser.Pets);
            Assert.IsNotNull(returnUser.Pets[0]);
            Assert.IsTrue(returnUser.Pets[0].SourceCode.KeyName == sourceCode.KeyName, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].Name == user.Pets[0].Name, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].ImageUrl == user.Pets[0].ImageUrl, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].Size == user.Pets[0].Size, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].Color == user.Pets[0].Color, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].IsSterile == user.Pets[0].IsSterile, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].Gender == user.Pets[0].Gender, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].AgeInMonths == user.Pets[0].AgeInMonths, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].AcquisitionMethod == user.Pets[0].AcquisitionMethod, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].DiscoveryMethod == user.Pets[0].DiscoveryMethod, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].DiscoveryMethodDetail == user.Pets[0].DiscoveryMethodDetail, "CreateUser returns correct Pets data");
            //Assert.IsTrue(returnUser.Pets[0].Species.KeyName == user.Pets[0].Species.KeyName, "CreateUser returns correct Pets data");
            //Assert.IsTrue(returnUser.Pets[0].PrimaryBreed.KeyName == user.Pets[0].PrimaryBreed.KeyName, "CreateUser returns correct Pets data");
            //Assert.IsTrue(returnUser.Pets[0].SecondaryBreed.KeyName == user.Pets[0].SecondaryBreed.KeyName, "CreateUser returns correct Pets data");
            //Assert.IsTrue(returnUser.Pets[0].FoodPrefDry.KeyName == user.Pets[0].FoodPrefDry.KeyName, "CreateUser returns correct Pets data");
            //Assert.IsTrue(returnUser.Pets[0].FoodPrefWet.KeyName == user.Pets[0].FoodPrefWet.KeyName, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].DateOfBirth == user.Pets[0].DateOfBirth, "CreateUser returns correct Pets data");
            Assert.IsTrue(returnUser.Pets[0].DateOfAdoption == user.Pets[0].DateOfAdoption, "CreateUser returns correct Pets data");


            Assert.IsNotNull(returnUser.Uuid, "CreateUser sets UUID");
            Assert.IsNotNull(returnUser.Id, "CreateUser sets Id");
            Assert.IsNotNull(returnUser.Address.Id, "CreateUser sets Address Id");
            Assert.IsNotNull(returnUser.Pets[0].Id, "CreateUser sets Pet Id");
            Assert.IsNotNull(returnUser.Pets[0].UserId, "CreateUser sets Pet UserId");
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

        #endregion

        #region Address Method Tests

        #endregion

        #region Subscription Method Tests
        /// <summary>
        /// Test for User subscription methods - does not hit live API
        /// </summary>
        [TestMethod()]
        public void SubscribeUserTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);

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

            User returnUser = target.CreateUser(user);

            // subscribe
            Assert.IsTrue(target.CreateUserSubscription((int)returnUser.Id, new List<string>() { "FR", "PE" }));

            List<string> subs = target.FindSubscriptionsByUserId((int)returnUser.Id) as List<string>;
            Assert.IsTrue(subs.Contains("FR"));
            Assert.IsTrue(subs.Contains("PE"));

            // unsubscribe
            Assert.IsTrue(target.DeleteUserSubscription((int)returnUser.Id, new List<string>() { "FR" }));

            subs = target.FindSubscriptionsByUserId((int)returnUser.Id) as List<string>;
            Assert.IsFalse(subs.Contains("FR"));
            Assert.IsTrue(subs.Contains("PE"));

            // Update (PUT / PATCH)? Do these replace or just add?
        }

        #endregion

        #region Brand Method Tests

        /// <summary>
        ///A test for Brand methods
        ///</summary>
        [TestMethod()]
        public void BrandsTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            List<Brand> brands = target.GetBrands() as List<Brand>;

            Assert.IsNotNull(brands, "GetBrands gets valid response");
            Assert.IsTrue(brands.Count > 1, "GetBrands returns data");

            Console.WriteLine("First object: " + brands[0].Id + " = " + brands[0].Name);

            Brand brand = target.FindBrandById(brands.Last().Id);

            Assert.IsNotNull(brand, "FindBrandById gets valid response");
            Assert.IsTrue(brand.Id == brands.Last().Id, "FindBrandById returns data");

            Console.WriteLine("Object: " + brand.Id + " = " + brand.Name);
        }

        /// <summary>
        ///A test for SourceCode methods
        ///</summary>
        [TestMethod()]
        public void SourceCodesTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            List<SourceCode> codes = target.GetSourceCodes() as List<SourceCode>;

            Assert.IsNotNull(codes, "GetSourceCodes gets valid response");
            Assert.IsTrue(codes.Count > 1, "GetSourceCodes returns data");

            Console.WriteLine("First object: " + codes[0].Id + " = " + codes[0].Name);

            SourceCode code = target.FindSourceCodeById(codes.Last().KeyName); // TODO: get actual code

            Assert.IsNotNull(code, "FindSourceCodeById gets valid response");
            Assert.IsTrue(code.Id == codes.Last().Id, "FindSourceCodeById returns data"); // TODO: verify id type

            Console.WriteLine("Object: " + code.Id + " = " + code.Name);
        }

        #endregion

        #region Breed & Pet Type Method Tests

        /// <summary>
        ///A test for Breed methods
        ///</summary>
        [TestMethod()]
        public void PetBreedsTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            List<Breed> breeds = target.GetBreeds() as List<Breed>;

            Assert.IsNotNull(breeds, "GetBreeds gets valid response");
            Assert.IsTrue(breeds.Count > 1, "GetBreeds returns data");

            Console.WriteLine("First object: " + breeds[0].Id + " = " + breeds[0].Name);
            
            Breed breed = target.FindBreedById(breeds.Last().Id);

            Assert.IsNotNull(breed, "FindBreedById gets valid response");
            Assert.IsTrue(breed.Id == breeds.Last().Id, "FindBreedById returns data");

            Console.WriteLine("Breed object: " + breed.Id + " = " + breed.Name);
        }

        /// <summary>
        ///A test for PetType methods
        ///</summary>
        [TestMethod()]
        public void PetTypesTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            List<PetType> types = target.GetPetTypes() as List<PetType>;

            Assert.IsNotNull(types, "GetPetTypes gets valid response");
            Assert.IsTrue(types.Count > 1, "GetPetTypes returns data");

            Console.WriteLine("First object: " + types[0].Id + " = " + types[0].KeyName);

            PetType type = target.FindPetTypeById(types.Last().Id);

            Assert.IsNotNull(type, "FindPetTypeById gets valid response");
            Assert.IsTrue(type.Id == types.Last().Id, "FindPetTypeById returns data");

            Console.WriteLine("Type object: " + type.Id + " = " + type.KeyName);
        }

        #endregion

        #region Food Method Tests
        /// <summary>
        ///A test for PetFood methods
        ///</summary>
        [TestMethod()]
        public void PetFoodsTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            List<PetFood> foods = target.GetPetFoods() as List<PetFood>;

            Assert.IsNotNull(foods, "GetPetFoods gets valid response");
            Assert.IsTrue(foods.Count > 1, "GetPetFoods returns data");

            Console.WriteLine("First object: " + foods[0].Id + " = " + foods[0].KeyName);

            PetFood food = target.FindPetFoodById(foods.Last().Id);

            Assert.IsNotNull(food, "FindPetFoodById gets valid response");
            Assert.IsTrue(food.Id == foods.Last().Id, "FindPetFoodById returns data");

            Console.WriteLine("Food object: " + food.Id + " = " + food.KeyName);
        }
        #endregion

        #region Pet Method Tests

        /// <summary>
        ///A test for PetOwnership methods
        ///</summary>
        [TestMethod()]
        public void PetOwnershipPlanTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            List<PetOwnershipPlan> plans = target.GetOwnershipPlans() as List<PetOwnershipPlan>;

            Assert.IsNotNull(plans, "GetOwnershipPlans gets valid response");
            Assert.IsTrue(plans.Count > 1, "GetOwnershipPlans returns data");

            Console.WriteLine("First object: " + plans[0].Id + " = " + plans[0].KeyName);

            PetOwnershipPlan plan = target.FindOwnershipPlanById(plans.Last().Id);

            Assert.IsNotNull(plan, "FindOwnershipPlanById gets valid response");
            Assert.IsTrue(plan.Id == plans.Last().Id, "FindOwnershipPlanById returns data");

            Console.WriteLine("Plan object: " + plan.Id + " = " + plan.KeyName);
        }

        /// <summary>
        ///A test for Pet methods
        ///</summary>
        [TestMethod()]
        public void CreateRetrieveUpdateAndDeletePetTest()
        {
            ApiClient target = new ApiClient(clientId, clientSecret, true);
            User user = new User()
            {
                LastName = "PetUser",
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

            User returnUser = target.CreateUser(user);

            Assert.IsNotNull(returnUser, "CreateUser gets valid response");
            Console.WriteLine("Object created, attempting to CRUD Pet");

            Pet newPet = new Pet()
            {
                SourceCode = sourceCode,
                Name = "Tester",
                ImageUrl = "https://www.purina.com/media/284062/Akitas_2913.jpg/560/0/center/middle",
                Size = "Large",
                Color = "Brown",
                IsSterile = false,
                Gender = "female",
                DateOfBirth = DateTime.Now.Date.AddMonths(-11),
                AgeInMonths = 11,
                DateOfAdoption = DateTime.Now.Date.AddMonths(-1),
                AcquisitionMethod = "test",
                DiscoveryMethod = "test",
                DiscoveryMethodDetail = "This is not a real pet, just a test"
                //Species, PrimaryBreed, SecondaryBreed, FoodPrefDry, FoodPrefWet
            };

            Pet returnPet = target.CreatePet((int)returnUser.Id, newPet);
            Assert.IsNotNull(returnPet, "CreatePet gets valid response");
            Assert.IsNotNull(returnPet.Id, "CreatePet sets Id");
            Assert.IsTrue(returnPet.Name == newPet.Name, "CreatePet returns correct data");

            returnPet.Size = "Small";

            Pet updatePet = target.UpdatePet((int)returnUser.Id, (int)returnPet.Id, returnPet);
            Assert.IsNotNull(updatePet, "UpdatePet gets valid response");
            Assert.IsTrue(updatePet.Id == returnPet.Id, "UpdatePet returns correct record");
            Assert.IsTrue(updatePet.Size == returnPet.Size, "UpdatePet returns correct data");

            List<Pet> pets = target.FindPetsByUserId((int)returnUser.Id) as List<Pet>;

            Assert.IsNotNull(pets, "FindPetsByUserId gets valid response");
            Assert.IsTrue(pets.Count > 1, "FindPetsByUserId returns data");

            Console.WriteLine("First object: " + pets[0].Id + " = " + pets[0].Name);

            Pet pet = target.FindPetById((int)returnUser.Id, (int)pets.Last().Id);

            Assert.IsNotNull(pet, "FindPetById gets valid response");
            Assert.IsTrue(pet.Id == pets.Last().Id, "FindPetById returns data");

            Console.WriteLine("Pet object: " + pet.Id + " = " + pet.Name);

            Assert.IsTrue(target.DeletePet((int)returnUser.Id, (int)pet.Id));
        }

        #endregion

        #region I18N Method Tests

        #endregion

        #region Content Method Tests

        [TestMethod()]
        public void ContentGroupTest()
        {
            Assert.Inconclusive("Test not implemented");
        }

        [TestMethod()]
        public void ContentNodeTest()
        {
            Assert.Inconclusive("Test not implemented");
        }

        [TestMethod()]
        public void ContentPlacementTest()
        {
            Assert.Inconclusive("Test not implemented");
        }

        [TestMethod()]
        public void ContentTaxonomyTest()
        {
            Assert.Inconclusive("Test not implemented");
        }

        [TestMethod()]
        public void ContentZoneTest()
        {
            Assert.Inconclusive("Test not implemented");
        }
        #endregion

        #region Event Method Tests

        [TestMethod()]
        public void ActionTypeTest()
        {
            Assert.Inconclusive("Test not implemented");
        }

        [TestMethod()]
        public void EventTypeTest()
        {
            Assert.Inconclusive("Test not implemented");
        }

        #endregion
    }
}
