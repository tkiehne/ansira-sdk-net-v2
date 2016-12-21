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

        private string _clientId, _clientSecret;
        private ApiClient _target;

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
            this._clientId = ConfigurationManager.AppSettings["Test.Client.Id"];
            this._clientSecret = ConfigurationManager.AppSettings["Test.Client.Secret"];
            Assert.IsNotNull(this._clientId);
            Assert.IsNotNull(this._clientSecret);

            this._target = new ApiClient(_clientId, _clientSecret, true);
            Assert.IsNotNull(_target);

            string token =_target.GetAccessToken();
            Assert.IsNotNull(token);
            Assert.IsNotNull(_target.AccessToken);
        }

        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #region Miscellaneous Tests

        [TestMethod()]
        public void ProductionTest()
        {
            ApiClient target = new ApiClient(_clientId, _clientSecret, false);

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
        ///A test for User Retrieval methods
        ///</summary>
        [TestMethod()]
        public void RetrieveUserTest()
        {
            List<User> users = _target.GetUsers() as List<User>;

            Assert.IsNotNull(users, "GetUsers gets valid response");
            Assert.IsTrue(users.Count > 1, "GetUsers returns data");

            Console.WriteLine("First object: " + users[0].Id + " = " + users[0].Email);

            User user = _target.FindUserById((int)users.Last().Id);

            Assert.IsNotNull(user, "FindUserById gets valid response");
            Assert.IsTrue(user.Email == users.Last().Email, "FindUserById returns data");

            Console.WriteLine("Object: " + user.Id + " = " + user.FirstName);
        }

        /// <summary>
        ///A test for CreateUser, UpdateUser, and DeleteUser
        ///</summary>
        [TestMethod()]
        public void CreateUpdateAndDeleteUserTest()
        {
            User user = new User()
            {
                LastName = "User",
                FirstName = "Test",
                Email = Guid.NewGuid().ToString("N") + "@fosfor.us",
                Address = new Address()
                {
                    Address1 = "209 E 6th",
                    City = "Austin",
                    State = "TX",
                    PostalCode = "78701"
                },
                SourceCode = new SourceCode()
                {
                    KeyName = "PF_Subscriptions-1"
                }
            };

            User returnUser = _target.CreateUser(user);

            Assert.IsNotNull(returnUser, "CreateUser gets valid response");
            Assert.IsTrue(returnUser.Email == user.Email, "CreateUser returns updated object");
            Assert.IsNotNull(returnUser.Uuid, "CreateUser sets UUID");
            Console.WriteLine("Returned Object: UUID = " + returnUser.Uuid);

            Console.WriteLine("Object created, attempting to update");
            returnUser.FirstName = "Updated";
            User updateUser = _target.UpdateUser(returnUser);
            Assert.IsNotNull(updateUser, "UpdateUser gets valid response");
            Assert.IsNotNull(updateUser.Uuid, "UpdateUser sets UUID");
            Assert.IsTrue(updateUser.FirstName == returnUser.FirstName, "UpdateUser returns updated object");

            Console.WriteLine("Object updated, attempting to delete");
            Assert.IsTrue(_target.DeleteUser(returnUser));

            Console.WriteLine("Object deleted, attempting to retrieve");
            User deletedUser = _target.FindUserByEmail(user.Email);
            Assert.IsNull(deletedUser, "FindUserByEmail does not retrieve deleted user?");
        }

        /// <summary>
        ///A test for CreateUser with all available data
        ///</summary>
        [TestMethod()]
        public void ExtendedCreateUserTest()
        {
            User user = new User()
            {
                LastName = "User",
                MiddleName = "T",
                FirstName = "Test",
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
                },
                SourceCode = new SourceCode()
                {
                    KeyName = "PF_Subscriptions-1"
                }
            };

            user.Pets = new List<Pet>();

            user.Pets.Add(new Pet()
            {
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

            User returnUser = _target.CreateUser(user);

            Assert.IsNotNull(returnUser, "CreateUser gets valid response");
            Assert.IsTrue(returnUser.LastName == user.LastName, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.MiddleName == user.MiddleName, "CreateUser returns correct data");
            Assert.IsTrue(returnUser.FirstName == user.FirstName, "CreateUser returns correct data");
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
            _target.DeleteUser((int)returnUser.Id);
        }

        /// <summary>
        ///A test for FindUserByEmail
        ///Presumes user email tkiehne@fosforus.net already exists
        ///</summary>
        [TestMethod()]
        public void FindUserByEmailTest()
        {
            User user = _target.FindUserByEmail("tom@fosforus.com");

            Assert.IsNotNull(user, "FindUserByEmail gets valid response");
            Assert.IsNotNull(user.Uuid, "FindUserByEmail set UUID");
            Assert.IsTrue(user.Email == "tom@fosforus.com", "FindUserByEmail returns valid object");

            Console.WriteLine("Object: UUID = " + user.Uuid);
        }

        /// <summary>
        ///A test for FindUserByName
        ///Preumes user name "Tom Kiehne" already exists
        ///</summary>
        [TestMethod()]
        public void FindUserByNameTest()
        {
            User user = _target.FindUserByName("Kiehne", "Tom");

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
            User user = _target.FindUserByUuid("745eb509-b2fe-11e5-bab5-22000bcc02f5");

            Assert.IsNotNull(user, "FindUserByUuid gets valid response");
            Assert.IsNotNull(user.Uuid, "FindUserByEmail set UUID");
            Assert.IsTrue(user.Uuid == "745eb509-b2fe-11e5-bab5-22000bcc02f5", "FindUserByUuid returns valid object");

            Console.WriteLine("Object: Email = " + user.Email);
        }


        // TODO: FindOwnershipPlanByUserId, FindSourceCodeByUserId, FindLastSourceCodeByUserId

        #endregion

        #region Password Method Tests

        /// <summary>
        /// Test for User password methods - does not hit live API
        /// </summary>
        [TestMethod()]
        public void PasswordTest()
        {
            string password = "This-1-should-work";

            User user = new User()
            {
                LastName = "User",
                FirstName = "Test",
                Email = Guid.NewGuid().ToString("N") + "@fosfor.us",
                Password = password,
                SourceCode = new SourceCode()
                {
                    KeyName = "PF_Subscriptions-1"
                }
            };

            User returnUser = _target.CreateUser(user);

            Assert.IsNotNull(returnUser, "Test user created");
            
            // verify initial password
            Assert.IsTrue(_target.VerifyPassword((int)returnUser.Id, password), "VerifyPassword succeeds with initial password");

            // change password
            string password2 = "Maybe-this-1-works!";
            User updateUser = _target.ChangePassword((int)returnUser.Id, password2);
            Assert.IsNotNull(updateUser, "ChangePassword returns User object");

            // verify second password
            Assert.IsTrue(_target.VerifyPassword((int)returnUser.Id, password2), "VerifyPassword succeeds with second password");
        }

        #endregion

        #region Address Method Tests

        /// <summary>
        /// Test for User address methods - does not hit live API
        /// </summary>
        [TestMethod()]
        public void UserAddressTest()
        {
            User user = new User()
            {
                LastName = "User",
                FirstName = "Test",
                Email = "tkiehne@fosfor.us",
                SourceCode = new SourceCode()
                {
                    KeyName = "PF_Subscriptions-1"
                }
            };

            User returnUser = _target.CreateUser(user);

            Address address = new Address()
            {
                Address1 = "1600 Pennsylvania Ave NW",
                City = "Washington",
                State = "DC",
                PostalCode = "20500",
                Country = new Country() { KeyName = "US", Name = "United States" },
                PrimaryPhone = "202-456-1111",
                Latitude = 38.8976763,
                Longitude = -77.0387185
            };

            // add address
            Address returnAddress = _target.CreateUserAddress((int)returnUser.Id, address);
            Assert.IsNotNull(returnAddress, "CreateUserAddress returns valid object");
            Assert.IsTrue(returnAddress.Address1 == address.Address1, "CreateUserAddress returns correct data");
            Assert.IsTrue(returnAddress.Id > 0, "CreateUserAddress assigns ID");

            // find address
            Address findAddress = _target.FindAddressByUserId((int)returnUser.Id);
            Assert.IsNotNull(findAddress, "FindAddressByUserId returns valid object");
            Assert.IsTrue(findAddress.Address1 == returnAddress.Address1, "FindAddressByUserId retrieves correct data");

            // update address
            returnAddress.Address2 = "Apt B";
            Address updateAddress = _target.UpdateUserAddress((int)returnUser.Id, returnAddress);
            Assert.IsNotNull(updateAddress, "UpdateUserAddress returns valid object");
            Assert.IsTrue(updateAddress.Address2 == returnAddress.Address2, "UpdateUserAddress updates data");
            
            // delete address
            Assert.IsTrue(_target.DeleteUserAddress((int)returnUser.Id));

            // do not find deleted address
            Address noAddress = _target.FindAddressByUserId((int)returnUser.Id);
            Assert.IsNull(noAddress, "FindAddressByUserId does not retrieve deleted object");
        }
        
        #endregion

        #region Subscription Method Tests
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
                Email = "tkiehne@fosfor.us",
                Address = new Address()
                {
                    Address1 = "209 E 6th",
                    City = "Austin",
                    State = "TX",
                    PostalCode = "78701"
                },
                SourceCode = new SourceCode()
                {
                    KeyName = "PF_Subscriptions-1"
                }
            };

            User returnUser = _target.CreateUser(user);

            // subscribe
            Assert.IsTrue(_target.CreateUserSubscription((int)returnUser.Id, new List<string>() { "FR", "PE" }));

            List<string> subs = _target.FindSubscriptionsByUserId((int)returnUser.Id) as List<string>;
            Assert.IsTrue(subs.Contains("FR"));
            Assert.IsTrue(subs.Contains("PE"));

            // unsubscribe
            Assert.IsTrue(_target.DeleteUserSubscription((int)returnUser.Id, new List<string>() { "FR" }));

            subs = _target.FindSubscriptionsByUserId((int)returnUser.Id) as List<string>;
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
            List<Brand> brands = _target.GetBrands() as List<Brand>;

            Assert.IsNotNull(brands, "GetBrands gets valid response");
            Assert.IsTrue(brands.Count > 1, "GetBrands returns data");

            Console.WriteLine("First object: " + brands[0].Id + " = " + brands[0].Name);

            Brand brand = _target.FindBrandById(brands.Last().Id);

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
            List<SourceCode> codes = _target.GetSourceCodes() as List<SourceCode>;

            Assert.IsNotNull(codes, "GetSourceCodes gets valid response");
            Assert.IsTrue(codes.Count > 1, "GetSourceCodes returns data");

            Console.WriteLine("First object: " + codes[0].Id + " = " + codes[0].Name);

            SourceCode code = _target.FindSourceCodeById(codes.First().KeyName); // TODO: get actual code

            Assert.IsNotNull(code, "FindSourceCodeById gets valid response");
            Assert.IsTrue(code.KeyName == codes.First().KeyName, "FindSourceCodeById returns data"); // TODO: verify id type

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
            List<Breed> breeds = _target.GetBreeds() as List<Breed>;

            Assert.IsNotNull(breeds, "GetBreeds gets valid response");
            Assert.IsTrue(breeds.Count > 1, "GetBreeds returns data");

            Console.WriteLine("First object: " + breeds[0].Id + " = " + breeds[0].Name);
            
            Breed breed = _target.FindBreedById(breeds.Last().Id);

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
            List<PetType> types = _target.GetPetTypes() as List<PetType>;

            Assert.IsNotNull(types, "GetPetTypes gets valid response");
            Assert.IsTrue(types.Count > 1, "GetPetTypes returns data");

            Console.WriteLine("First object: " + types[0].Id + " = " + types[0].KeyName);

            PetType type = _target.FindPetTypeById((int)types.Last().Id);

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
            List<PetFood> foods = _target.GetPetFoods() as List<PetFood>;

            Assert.IsNotNull(foods, "GetPetFoods gets valid response");
            Assert.IsTrue(foods.Count > 1, "GetPetFoods returns data");

            Console.WriteLine("First object: " + foods[0].Id + " = " + foods[0].KeyName);

            PetFood food = _target.FindPetFoodById(foods.Last().Id);

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
            List<PetOwnershipPlan> plans = _target.GetOwnershipPlans() as List<PetOwnershipPlan>;

            Assert.IsNotNull(plans, "GetOwnershipPlans gets valid response");
            Assert.IsTrue(plans.Count > 1, "GetOwnershipPlans returns data");

            Console.WriteLine("First object: " + plans[0].Id + " = " + plans[0].KeyName);

            PetOwnershipPlan plan = _target.FindOwnershipPlanById((int)plans.Last().Id);

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
            User user = new User()
            {
                LastName = "PetUser",
                FirstName = "Test",
                Email = Guid.NewGuid().ToString("N") + "@fosfor.us",
                Address = new Address()
                {
                    Address1 = "209 E 6th",
                    City = "Austin",
                    State = "TX",
                    PostalCode = "78701"
                },
                SourceCode = new SourceCode()
                {
                    KeyName = "PF_Subscriptions-1"
                }
            };

            User returnUser = _target.CreateUser(user);

            Assert.IsNotNull(returnUser, "CreateUser gets valid response");
            Console.WriteLine("Object created, attempting to CRUD Pet");

            Pet newPet = new Pet()
            {
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

            Pet returnPet = _target.CreatePet((int)returnUser.Id, newPet);
            Assert.IsNotNull(returnPet, "CreatePet gets valid response");
            Assert.IsNotNull(returnPet.Id, "CreatePet sets Id");
            Assert.IsTrue(returnPet.Name == newPet.Name, "CreatePet returns correct data");

            returnPet.Size = "Small";

            Pet updatePet = _target.UpdatePet((int)returnUser.Id, returnPet);
            Assert.IsNotNull(updatePet, "UpdatePet gets valid response");
            Assert.IsTrue(updatePet.Id == returnPet.Id, "UpdatePet returns correct record");
            Assert.IsTrue(updatePet.Size == returnPet.Size, "UpdatePet returns correct data");

            List<Pet> pets = _target.FindPetsByUserId((int)returnUser.Id) as List<Pet>;

            Assert.IsNotNull(pets, "FindPetsByUserId gets valid response");
            Assert.IsTrue(pets.Count > 1, "FindPetsByUserId returns data");

            Console.WriteLine("First object: " + pets[0].Id + " = " + pets[0].Name);

            Pet pet = _target.FindPetById((int)returnUser.Id, (int)pets.Last().Id);

            Assert.IsNotNull(pet, "FindPetById gets valid response");
            Assert.IsTrue(pet.Id == pets.Last().Id, "FindPetById returns data");

            Console.WriteLine("Pet object: " + pet.Id + " = " + pet.Name);

            Assert.IsTrue(_target.DeletePet((int)returnUser.Id, (int)pet.Id));
        }

        #endregion

        #region I18N Method Tests

        /// <summary>
        /// Test for User I18N methods - does not hit live API
        /// </summary>
        [TestMethod()]
        public void UserI18NTest()
        {
            Country testCountry = new Country() { KeyName = "DE", Name = "Germany" };
            Language testLanguage = new Language() { KeyName = "de", Name = "Deutsch", EnglishName = "German" };
            Currency testCurrency = new Currency() { KeyName = "USD", Name = "US Dollar" };

            User user = new User()
            {
                LastName = "User",
                FirstName = "Test",
                Email = Guid.NewGuid().ToString("N") + "@fosfor.us",
                Nationality = testCountry,
                Currency = testCurrency,
                Language = testLanguage,
                SourceCode = new SourceCode()
                {
                    KeyName = "PF_Subscriptions-1"
                }
            };

            User returnUser = _target.CreateUser(user);

            Assert.IsNotNull(returnUser, "Test user created");

            // Currency
            Currency userCurrency = _target.FindCurrencyByUserId((int)returnUser.Id);
            Assert.IsTrue(userCurrency.Name == testCurrency.Name, "FindCurrencyByUserId returns correct data");

            // Language
            Language userLanguage = _target.FindLanguageByUserId((int)returnUser.Id);
            Assert.IsTrue(userLanguage.Name == testLanguage.Name, "FindLanguageByUserId returns correct data");

            // Country
            Country userCountry = _target.FindNationalityByUserId((int)returnUser.Id);
            Assert.IsTrue(userCountry.Name == testCountry.Name, "FindNationalityByUserId returns correct data");
        }

        /// <summary>
        ///A test for Country methods
        ///</summary>
        [TestMethod()]
        public void NationalityTest()
        {
            List<Country> countries = _target.GetCountries() as List<Country>;

            Assert.IsNotNull(countries, "GetCountries gets valid response");
            Assert.IsTrue(countries.Count > 1, "GetCountries returns data");

            Console.WriteLine("First object: " + countries[0].Id + " = " + countries[0].Name);

            Country country = _target.FindCountryById(countries.Last().Id);

            Assert.IsNotNull(country, "FindCountryById gets valid response");
            Assert.IsTrue(country.Id == countries.Last().Id, "FindCountryById returns data");

            Console.WriteLine("Object: " + country.Id + " = " + country.Name);
        }

        /// <summary>
        ///A test for Language methods
        ///</summary>
        [TestMethod()]
        public void LanguageTest()
        {
            List<Language> languages = _target.GetLanguages() as List<Language>;

            Assert.IsNotNull(languages, "GetLanguages gets valid response");
            Assert.IsTrue(languages.Count > 1, "GetLanguages returns data");

            Console.WriteLine("First object: " + languages[0].Id + " = " + languages[0].Name);

            Language language = _target.FindLanguageById(languages.Last().Id);

            Assert.IsNotNull(language, "FindLanguageById gets valid response");
            Assert.IsTrue(language.Id == languages.Last().Id, "FindLanguageById returns data");

            Console.WriteLine("Object: " + language.Id + " = " + language.Name);
        }

        /// <summary>
        ///A test for Currency methods
        ///</summary>
        [TestMethod()]
        public void CurrencyTest()
        {
            List<Currency> currencies = _target.GetCurrencies() as List<Currency>;

            Assert.IsNotNull(currencies, "GetCurrencies gets valid response");
            Assert.IsTrue(currencies.Count > 1, "GetCurrencies returns data");

            Console.WriteLine("First object: " + currencies[0].Id + " = " + currencies[0].Name);

            Currency currency = _target.FindCurrencyById(currencies.Last().Id);

            Assert.IsNotNull(currency, "FindCurrencyById gets valid response");
            Assert.IsTrue(currency.Id == currencies.Last().Id, "FindCurrencyById returns data");

            Console.WriteLine("Object: " + currency.Id + " = " + currency.Name);
        }


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

        #region Coupon API Tests

        /// <summary>
        ///A test for RegisterCoupon
        ///</summary>
        [TestMethod()]
        public void InsertCouponTest()
        {
            CouponUser user = new CouponUser()
            {
                LastName = "User",
                FirstName = "Test",
                Email = "tom@fosforus.com", // Guid.NewGuid().ToString("N") + "@fosfor.us",
                Address1 = "209 E 6th",
                City = "Austin",
                State = "TX",
                PostalCode = "78701",
                PetType = "DOG",
                OptIn = false
            };

            User returnUser = _target.RegisterCoupon(user, 52, "PC-AAP-SAMPLING");

            Assert.IsNotNull(returnUser, "RegisterCoupon gets valid response");
            Assert.IsTrue(returnUser.Email == user.Email, "RegisterCoupon returns updated object");
            Assert.IsNotNull(returnUser.Uuid, "RegisterCoupon sets UUID");
            Console.WriteLine("Returned Object: UUID = " + returnUser.Uuid);
        }

        #endregion
    }
}
