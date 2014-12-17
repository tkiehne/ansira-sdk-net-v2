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
    private int sourceId;

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
      this.sourceId = Convert.ToInt32(ConfigurationManager.AppSettings["Test.Source.Id"]);
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
        DisplayName = "test user",
        FamilyName = "User",
        GivenName = "Test",
        SourceId = sourceId,
        Email = Guid.NewGuid().ToString("N") + "@fosfor.us",
        Address = new Address()
        {
          Address1 = "209 E 6th",
          City = "Austin",
          State = "TX",
          Zip = "78701"
        }
      };

      User returnUser = target.CreateUser(user);

      Assert.IsNotNull(returnUser, "CreateUser gets valid response");
      Assert.IsTrue(returnUser.Email == user.Email, "CreateUser returns updated object");
      Console.WriteLine("Returned Object: UUID = " + returnUser.Uuid);

      Console.WriteLine("Object created, attempting to update");
      returnUser.GivenName = "Updated";
      User updateUser = target.UpdateUser(returnUser);
      Assert.IsNotNull(updateUser, "UpdateUser gets valid response");
      Assert.IsTrue(updateUser.GivenName == returnUser.GivenName, "UpdateUser returns updated object");

      Console.WriteLine("Object updated, attempting to delete");
      target.DeleteUser(returnUser);

      Console.WriteLine("Object deleted, attempting to retrieve");
      User deletedUser = target.FindUserByEmail(user.Email);
      Assert.IsNull(deletedUser, "FindUserByEmail does not retrieve deleted user?");
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
      Assert.IsTrue(user.FamilyName == "Kiehne", "FindUserByName returns valid object");

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
        DisplayName = "test user",
        FamilyName = "User",
        GivenName = "Test",
        SourceId = sourceId,
        Email = "tkiehne@fosfor.us",
        Address = new Address()
        {
          Address1 = "209 E 6th",
          City = "Austin",
          State = "TX",
          Zip = "78701"
        }
      };

      user.Subscribe("FR", 11, this.sourceId);

      Assert.IsNotNull(user.Subscriptions);
      Assert.IsInstanceOfType(user.Subscriptions["FR"], typeof(Subscription));
      Assert.IsTrue(user.Subscriptions["FR"].BrandId == 11);
      Assert.IsTrue(user.Subscriptions["FR"].EmailStatus == "1");

      user.Unsubscribe("FR", 11, this.sourceId);

      Assert.IsNotNull(user.Subscriptions);
      Assert.IsInstanceOfType(user.Subscriptions["FR"], typeof(Subscription));
      Assert.IsTrue(user.Subscriptions["FR"].BrandId == 11);
      Assert.IsTrue(user.Subscriptions["FR"].EmailStatus == "0");
    }

    /// <summary>
    ///A test for GetAllBrands
    ///</summary>
    [TestMethod()]
    public void GetAllBrandsTest()
    {
      ApiClient target = new ApiClient(clientId, clientSecret, true);
      IList<Brand> brands = target.GetAllBrands();

      Assert.IsNotNull(brands, "GetAllBrands gets valid response");
      Assert.IsTrue(brands.Count > 1, "GetAllBrands returns data");

      Console.WriteLine("First object: " + brands[0].Id + " = " + brands[0].Name);
    }

    /// <summary>
    ///A test for GetPetBreeds
    ///</summary>
    [TestMethod()]
    public void GetPetBreedsTest()
    {
      ApiClient target = new ApiClient(clientId, clientSecret, true);
      IList<Breed> breeds = target.GetPetBreeds(1);

      Assert.IsNotNull(breeds, "GetPetBreeds gets valid response");
      Assert.IsTrue(breeds.Count > 1, "GetPetBreeds returns data");

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

      Console.WriteLine("First object: " + types[0].Id + " = " + types[0].Type);
    }

    /// <summary>
    ///A test for GetWetFoodBrands
    ///</summary>
    [TestMethod()]
    public void GetWetFoodBrandsTest()
    {
      ApiClient target = new ApiClient(clientId, clientSecret, true);
      IList<Brand> brands = target.GetWetFoodBrands(1);

      Assert.IsNotNull(brands, "GetWetFoodBrands gets valid response");
      Assert.IsTrue(brands.Count > 1, "GetWetFoodBrands returns data");

      Console.WriteLine("First object: " + brands[0].Id + " = " + brands[0].Name);
    }

    /// <summary>
    ///A test for GetDryFoodBrands
    ///</summary>
    [TestMethod()]
    public void GetDryFoodBrandsTest()
    {
      ApiClient target = new ApiClient(clientId, clientSecret, true);
      IList<Brand> brands = target.GetDryFoodBrands(1);

      Assert.IsNotNull(brands, "GetDryFoodBrands gets valid response");
      Assert.IsTrue(brands.Count > 1, "GetDryFoodBrands returns data");

      Console.WriteLine("First object: " + brands[0].Id + " = " + brands[0].Name);
    }

    /// <summary>
    ///A test for SignOutUser
    ///Presumes user ID 1551389 exists
    ///</summary>
    [TestMethod()]
    public void SignOutUserByIdTest()
    {
      ApiClient target = new ApiClient(clientId, clientSecret, true);
      target.SignOutUser(1551389);
      Assert.Inconclusive("SignOutUser does not return a value and cannot be verified.");
    }

    /// <summary>
    ///A test for SignOutUser
    ///Presumes user ID 1551389 exists
    ///</summary>
    [TestMethod()]
    public void SignOutUserByObjectTest()
    {
      ApiClient target = new ApiClient(clientId, clientSecret, true);
      User user = new User(){ Id = 1551389, DisplayName = "test"};
      target.SignOutUser(user);
      Assert.Inconclusive("SignOutUser does not return a value and cannot be verified.");
    }
  }
}
