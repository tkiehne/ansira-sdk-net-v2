﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Ansira.Objects;
using Newtonsoft.Json;
using System.Linq;

namespace Ansira
{
    /// <summary>
    /// API Client for Ansira CRM / SSO API version 2
    /// ref: https://profiles.purina.com/service/apidoc
    /// </summary>
    public class ApiClient
    {
        public string uatUrl = "https://uat-purinareg.ansiradigital.com/api/v2/";
        public string prodUrl = "https://profiles.purina.com/api/v2/";
        protected string apiUrl, clientId, clientSecret;

        /// <summary>
        /// Ansira API Client
        /// </summary>
        /// <param name="clientId">Assigned Client ID</param>
        /// <param name="clientSecret">Assigned Client Secret</param>
        /// <param name="uat">Use UAT endpoint (true) or production (false / false)</param>
        /// <exception cref="System.ArgumentNullException">Thrown when clientId or clientSecret is null</exception>
        public ApiClient(string clientId, string clientSecret, bool uat = false)
        {
            if (clientId == null)
            {
                throw new ArgumentNullException("clientId", "Client ID must not be null");
            }
            if (clientSecret == null)
            {
                throw new ArgumentNullException("clientSecret", "Client Secret must not be null");
            }
            if (uat == true)
            {
                this.apiUrl = uatUrl;
            }
            else
            {
                this.apiUrl = prodUrl;
            }
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        #region Utility Methods

        /// <summary>
        /// Calls an API endpoint and returns results
        /// </summary>
        /// <param name="method">API method to call</param>
        /// <param name="data">Additional POST data as NameValueCollection</param>
        /// <returns>Response JSON as string or null if no results</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when API Method is null</exception>
        /// <exception cref="System.Net.WebException">Thrown when network operation fails</exception>
        /// *** TODO: need to alter to handle different HTTP Verbs
        protected string CallApi(string method, NameValueCollection data)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method", "API Method must not be null");
            }
            string endpoint = this.apiUrl + method;

            NameValueCollection message = new NameValueCollection();
            message.Add("client_id", this.clientId);
            message.Add("client_sec", this.clientSecret);
            if (data != null)
            {
                message.Add(data);
            }

            WebClient client = new WebClient();
            byte[] output = client.UploadValues(endpoint, "POST", message);

            string apiResponse = Encoding.UTF8.GetString(output);

            Response response = JsonConvert.DeserializeObject<Response>(apiResponse);

            if (response.Status == 1)
            {
                /* TODO: Disabled until Ansira harmonizes their responses; although we may have an edge case that precludes this
                return JsonConvert.SerializeObject(response.Results);
                 */
                return apiResponse; // TEMP
            }

            return null;
        }

        #endregion

        #region Brand Methods

        /// <summary>
        /// Get all the brands associated with Purina
        /// </summary>
        /// <returns>IList of Ansira.Objects.Brand objects or null if none</returns>
        public IList<Brand> GetBrands()
        {
            string results = CallApi("brands", null);

            if (results != null)
            {
                ResponseV1 response = JsonConvert.DeserializeObject<ResponseV1>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<Brand>>(JsonConvert.SerializeObject(response.Result));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find a Brand by its ID
        /// </summary>
        /// <param name="id">ID integer</param>
        /// <returns>Ansira.Objects.Brand</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public Brand FindBrandById(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("brands/{0}", id);
            string results = CallApi(method, null);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<Brand>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Get all the brands associated with Purina
        /// </summary>
        /// <returns>IList of Ansira.Objects.SourceCode objects or null if none</returns>
        public IList<SourceCode> GetSourceCodes()
        {
            string results = CallApi("sourcecodes", null);

            if (results != null)
            {
                ResponseV1 response = JsonConvert.DeserializeObject<ResponseV1>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<SourceCode>>(JsonConvert.SerializeObject(response.Result));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find a SourceCode by its ID
        /// </summary>
        /// <param name="id">ID string</param>
        /// <returns>Ansira.Objects.SourceCode</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public SourceCode FindSourceCodeById(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("sourcecodes/{0}", id);
            string results = CallApi(method, null);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<SourceCode>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Content Methods

        ///api/v2/contentgroup/find
        //GET /api/v2/contentgroup/find Finds a specific content group
        public object FindContentGroup()
        {
            return new NotImplementedException();
        }
        ///api/v2/contentgroup/{id}
        //Retrieves a specific content group
        public object FindContentGroupById(int id)
        {
            return new NotImplementedException();
        }
        ///api/v2/contentgroups
        //GET /api/v2/contentgroups Retrieves a simple list of content groups
        public object GetContentGroups()
        {
            return new NotImplementedException();
        }
        ///api/v2/contentgroups/find
        //GET /api/v2/contentgroups/find Finds a simple list of content groups
        public object FindContentGroups()
        {
            return new NotImplementedException();
        }
        ///api/v2/contentnode/{id}
        //GET /api/v2/contentnode/{id} Retrieves a specific content
        public object FindContentNodeById(int id)
        {
            return new NotImplementedException();
        }
        ///api/v2/contentnode/{id}/category
        //GET /api/v2/contentnode/{id}/category Retrieves a specific content, category only
        public object FindContentNodeCategoryById(int id)
        {
            return new NotImplementedException();
        }
        ///api/v2/contentnode/{id}/tags
        //GET /api/v2/contentnode/{id}/tags Retrieves a specific content, tags only
        public object FindContentNodeTagsById(int id)
        {
            return new NotImplementedException();
        }
        ///api/v2/contentnodes/find
        //GET /api/v2/contentnodes/find Retrieves a list of contents
        public object FindContentNodes()
        {
            return new NotImplementedException();
        }
        ///api/v2/contentplacement/{id}
        //GET /api/v2/contentplacement/{id} Retrieves a specific content placement
        public object FindContentPlacementById(int id)
        {
            return new NotImplementedException();
        }
        ///api/v2/contentplacements
        //GET /api/v2/contentplacements Retrieves a list of all content placements
        public object GetContentPlacements()
        {
            return new NotImplementedException();
        }
        ///api/v2/contentplacements/find
        //GET /api/v2/contentplacements/find Retrieves a list of ContentPlacements
        public object FindContentPlacements()
        {
            return new NotImplementedException();
        }
        ///api/v2/contenttaxonomy/{id}
        //GET /api/v2/contenttaxonomy/{id} Retrieves a specific content taxonomy
        public object FindContentTaxonomyById(int id)
        {
            return new NotImplementedException();
        }
        ///api/v2/contenttaxonomys
        //GET /api/v2/contenttaxonomys Retrieves a list of all content taxonomys
        public object GetContentTaxonomys()
        {
            return new NotImplementedException();
        }
        ///api/v2/contenttaxonomys/find
        //GET /api/v2/contenttaxonomys/find Retrieves a list of ContentTaxonomys
        public object FindContentTaxonomys()
        {
            return new NotImplementedException();
        }
        ///api/v2/contentzone/{id}
        //GET /api/v2/contentzone/{id} Retrieves a specific content zone
        public object FindContentZoneById(int id)
        {
            return new NotImplementedException();
        }
        ///api/v2/contentzones
        //GET /api/v2/contentzones Retrieves a list of all content zones
        public object GetContentZones()
        {
            return new NotImplementedException();
        }
        ///api/v2/contentzones/find
        //GET /api/v2/contentzones/find Retrieves a list of ContentZones
        public object FindContentZones()
        {
            return new NotImplementedException();
        }

        #endregion

        #region Event Methods

        // /api/v2/actiontypes
        // GET /api/v2/actiontypes Retrieves a list of action types available for use with our Events API method.
        public object GetActionTypes()
        {
            return new NotImplementedException();
        }
        // /api/v2/actiontypes/{ id}
        // GET /api/v2/actiontypes/{id} Retrieves an action type
        public object FindActionTypeById(int id)
        {
            return new NotImplementedException();
        }
        // /api/v2/eventtypes
        // GET /api/v2/eventtypes Provides a list of event types to be leveraged by the Events API method.
        public object GetEventTypes()
        {
            return new NotImplementedException();
        }
        // /api/v2/eventtypes/{id}
        // GET /api/v2/eventtypes/{id} Retrieve an event type
        public object FindEventTypeById(int id)
        {
            return new NotImplementedException();
        }

        #endregion

        #region Pets Methods

        /// <summary>
        /// Get the pet breeds supported by the API
        /// </summary>
        /// <returns>IList of Ansira.Objects.Breed objects or null if none</returns>
        public IList<Breed> GetBreeds()
        {
            string results = CallApi("breeds", null);

            if (results != null)
            {
                ResponseV1 response = JsonConvert.DeserializeObject<ResponseV1>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<Breed>>(JsonConvert.SerializeObject(response.Result));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find a Breed by its ID
        /// </summary>
        /// <param name="id">ID integer</param>
        /// <returns>Ansira.Objects.Breed</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public Breed FindBreedById(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("breeds/{0}", id);
            string results = CallApi(method, null);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<Breed>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        // /api/v2/petfoods
        // GET /api/v2/petfoods Provides a standardized list of pet food brands.
        public object GetPetFoods()
        {
            return new NotImplementedException();
        }
        // /api/v2/petfoods/{id}
        // GET /api/v2/petfoods/{id} Retrieve a pet food
        public object FindPetFoodById(int id)
        {
            return new NotImplementedException();
        }
        // /api/v2/petownershipplans
        // GET /api/v2/petownershipplans Retrieve list of pet ownership plans
        public object GetOwnershipPlans()
        {
            return new NotImplementedException();
        }
        // /api/v2/petownershipplans/{id}
        // GET /api/v2/petownershipplans/{id} Retrieve a pet type
        public object FindOwnershipPlanById(int id)
        {
            return new NotImplementedException();
        }

        /// <summary>
        /// Get the pet types supported by the API
        /// </summary>
        /// <returns>IList of Ansira.Objects.PetType objects or null if none</returns>
        public IList<PetType> GetPetTypes()
        {
            string results = CallApi("pettypes", null);

            if (results != null)
            {
                ResponseV1 response = JsonConvert.DeserializeObject<ResponseV1>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<PetType>>(JsonConvert.SerializeObject(response.Result));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find a PetType by its ID
        /// </summary>
        /// <param name="id">ID integer</param>
        /// <returns>Ansira.Objects.PetType</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public PetType FindPetTypeById(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("pettypes/{0}", id);
            string results = CallApi(method, null);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<PetType>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Pets by User ID
        /// </summary>
        /// <param name="userId">ID integer</param>
        /// <returns>IList of Ansira.Objects.Pet</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public IList<Pet> GetPetsByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/pets", userId);
            string results = CallApi(method, null);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<Pet>>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        // POST /api/v2/users/{user_id}/pets Creates/updates user's pet
        public object UpdatePet(int userId)
        {
            return new NotImplementedException();
        }
        public object CreatePet(int userId)
        {
            return this.UpdatePet(userId);
        }

        #endregion

        #region Subscription Methods

        // /api/v2/users/{user_id}/subscriptions
        // DELETE /api/v2/users/{user_id}/subscriptions Unsubscribe user
        public object DeleteUserSubscription(int userId)
        {
            return new NotImplementedException();
        }

        /// <summary>
        /// Get Subscriptions by User ID
        /// </summary>
        /// <param name="userId">ID integer</param>
        /// <returns>IList of Ansira.Objects.Subscription</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public IList<Subscription> GetSubscriptionsByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/subscriptions", userId);
            string results = CallApi(method, null);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<Subscription>>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        // PATCH /api/v2/users/{user_id}/subscriptions Updates a user subscription
        // POST /api/v2/users/{user_id}/subscriptions Create user subscription
        // PUT /api/v2/users/{user_id}/subscriptions Updates user subscription
        public object UpdateUserSubscription(int userId)
        {
            return new NotImplementedException();
        }
        public object CreateUserSubscription(int userId)
        {
            return this.UpdateUserSubscription(userId);
        }

        // /api/v2/users/{user_id}/subscriptions/{brand_code}
        // DELETE /api/v2/users/{user_id}/subscriptions/{brand_code} Unsubscribe user
        public object DeleteUserSubscriptionByBrand(int userId, string brandId)
        {
            return new NotImplementedException();
        }

        /// <summary>
        /// Get Subscriptions by User ID and Brand
        /// </summary>
        /// <param name="userId">ID integer</param>
        /// <param name="brandId">Brand code</param>
        /// <returns>IList of Ansira.Objects.Subscription</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when Brand is null</exception>
        public IList<Subscription> GetUserSubscriptionsByBrand(int userId, string brandId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            if (String.IsNullOrEmpty(brandId))
            {
                throw new ArgumentNullException("brandId", "ID must not be null");
            }
            string method = String.Format("users/{0}/subscriptions/{1}", userId, brandId);
            string results = CallApi(method, null);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<Subscription>>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        // PATCH /api/v2/users/{user_id}/subscriptions/{brand_code} Updates a user subscription
        // POST /api/v2/users/{user_id}/subscriptions/{brand_code} Create user subscription
        // PUT /api/v2/users/{user_id}/subscriptions/{brand_code} Updates user subscription
        public object UpdateUserSubscriptionByBrand(int userId, string brandId)
        {
            return new NotImplementedException();
        }
        public object CreateUserSubscriptionByBrand(int userId, string brandId)
        {
            return this.UpdateUserSubscriptionByBrand(userId, brandId);
        }

        #endregion

        #region User Methods 

        /// <summary>
        /// Get list of Ansira Users
        /// </summary>
        /// <returns>IList of Ansira.Objects.User</returns>
        public IList<User> GetUsers()
        {
            // *** TODO: add filters
            /*if (uuid == null)
            {
                throw new ArgumentNullException("uuid", "UUID must not be null");
            }
            NameValueCollection data = new NameValueCollection();
            data.Add("uuid", uuid);
            */
            string results = CallApi("users", null);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<User>>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find a User by its ID
        /// </summary>
        /// <param name="id">ID integer</param>
        /// <returns>Ansira.Objects.User</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public User FindUserById(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("users/{0}", id);
            string results = CallApi(method, null);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find an Ansira User by its UUID
        /// </summary>
        /// <param name="uuid">UUID string</param>
        /// <returns>Ansira.Objects.User</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when UUID is null</exception>
        public User FindUserByUuid(string uuid)
        {
            if (uuid == null)
            {
                throw new ArgumentNullException("uuid", "UUID must not be null");
            }
            NameValueCollection data = new NameValueCollection();
            data.Add("uuid", uuid);
            string results = CallApi("users", data);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find an Ansira User by its Email
        /// </summary>
        /// <param name="email">A valid email address</param>
        /// <returns>Ansira.Objects.User</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when email is null</exception>
        public User FindUserByEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email", "Email must not be null");
            }
            // TODO: validate email?

            NameValueCollection data = new NameValueCollection();
            data.Add("email", email);
            string results = CallApi("users", data);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find an Ansira User by its First and Last name
        /// </summary>
        /// <param name="familyName">Last name</param>
        /// <param name="givenName">First name</param>
        /// <returns>Ansira.Objects.User</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when both given and family names are null</exception>
        public User FindUserByName(string familyName, string givenName)
        {
            if (String.IsNullOrEmpty(familyName) && String.IsNullOrEmpty(givenName))
            {
                throw new ArgumentNullException();
            }
            NameValueCollection data = new NameValueCollection();
            if (familyName != null)
            {
                data.Add("lastName", familyName);
            }
            if (givenName != null)
            {
                data.Add("firstName", givenName);
            }

            string results = CallApi("users", data);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(response.Results)); // TODO: this may be a list instead of a single object?
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create a new User in the Ansira API
        /// </summary>
        /// <param name="user">Ansira.Objects.User</param>
        /// <returns>Ansira.Objects.User or null if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User or Email is null</exception>
        public User CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "User must not be null");
            }
            if (String.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException("user.Email", "Email must not be null");
            }
            NameValueCollection data = new NameValueCollection();

            // pass through JSON to get correct parameters
            string record = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var tmp = new Object();
            JsonConvert.PopulateObject(record, tmp);

            tmp.GetType().GetProperties().ToList().ForEach(pi => data.Add(pi.Name, (pi.GetValue(tmp, null) ?? "").ToString()));

            string results = CallApi("users", data);
            if (results != null)
            {
                ResponseV3 response = JsonConvert.DeserializeObject<ResponseV3>(results); // TEMP?
                return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(response.Record.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Delete an existing User from the Ansira API
        /// </summary>
        /// <param name="user">Ansira.Objects.User with non-null UUID</param>
        /// <exception cref="System.ArgumentNullException">Thrown when User is null</exception>
        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "User must not be null");
            }
            this.DeleteUser((int)user.Id);
        }

        /// <summary>
        /// Delete an existing User from the Ansira API
        /// </summary>
        /// <param name="id">ID integer</param>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public void DeleteUser(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("users/{0}", id); // *** TODO: Delete verb
            string results = CallApi(method, null);
        }

        /// <summary>
        /// Update an existing User in the Ansira API
        /// </summary>
        /// <param name="user">Ansira.Objects.User with non-null ID</param>
        /// <returns>Ansira.Objects.User or null if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User or ID is null</exception>
        public User UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "User must not be null");
            }
            if (String.IsNullOrEmpty(user.Id.ToString()))
            {
                throw new ArgumentNullException("id", "User's ID must not be null");
            }
            NameValueCollection data = new NameValueCollection();

            // pass through JSON to get correct parameters
            string record = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var tmp = new Object();
            JsonConvert.PopulateObject(record, tmp);

            tmp.GetType().GetProperties().ToList().ForEach(pi => data.Add(pi.Name, (pi.GetValue(tmp, null) ?? "").ToString()));

            string results = CallApi("users", data); // *** TODO: Put/Patch verbs
            if (results != null)
            {
                ResponseV3 response = JsonConvert.DeserializeObject<ResponseV3>(results); // TEMP?
                return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(response.Record.Results));
            }
            else
            {
                return null;
            }
        }

        // /api/v2/users/{user_id}/address
        // DELETE /api/v2/users/{user_id}/address Deletes a user's address
        public object DeleteUserAddress(int userId)
        {
            return new NotImplementedException();
        }

        /// <summary>
        /// Get Address by User ID
        /// </summary>
        /// <param name="userId">ID integer</param>
        /// <returns>IList of Ansira.Objects.Address</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public IList<Address> FindAddressByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/address", userId);
            string results = CallApi(method, null);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<Address>>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }
        // PATCH /api/v2/users/{user_id}/address Updates a user's address
        // POST /api/v2/users/{user_id}/address Creates/updates a user's address
        // PUT /api/v2/users/{user_id}/address Updates a user's address
        public object UpdateUserAddress(int userId)
        {
            return new NotImplementedException();
        }
        public object CreateUserAddress(int userId)
        {
            return this.UpdateUserSubscription(userId);
        }

        // /api/v2/users/{user_id}/currency
        // GET /api/v2/users/{user_id}/currency Get user currency
        public object FindCurrencyByUserId(int userId)
        {
            return new NotImplementedException();
        }
        // /api/v2/users/{user_id}/language
        // GET /api/v2/users/{user_id}/language Get user language
        public object FindLanguageByUserId(int userId)
        {
            return new NotImplementedException();
        }
        // /api/v2/users/{user_id}/lastsourcecode
        // GET /api/v2/users/{user_id}/lastsourcecode Get user last source code
        public object FindLastSourceCodeByUserId(int userId)
        {
            return new NotImplementedException();
        }
        // /api/v2/users/{user_id}/nationality
        // GET /api/v2/users/{user_id}/nationality Get user locale
        public object FindNationalityByUserId(int userId)
        {
            return new NotImplementedException();
        }

        /// <summary>
        /// Change Ansira User password
        /// </summary>
        /// <param name="userId">ID integer</param>
        /// <param name="password">Password string</param>
        /// <returns>Ansira.Objects.User</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when userId or password are null</exception>
        public User ChangePassword(int userId, string password)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "Email must not be null");
            }
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password", "Password must not be null");
            }

            NameValueCollection data = new NameValueCollection();
            data.Add("password", password);
            string method = String.Format("users/{0}/password/change", userId);
            string results = CallApi(method, data);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Verify Ansira User password for authentication
        /// </summary>
        /// <param name="userId">ID integer</param>
        /// <param name="password">Password string</param>
        /// <returns>Boolean</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when userId or password are null</exception>
        public bool VerifyPassword(int userId, string password)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "Email must not be null");
            }
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password", "Password must not be null");
            }

            NameValueCollection data = new NameValueCollection();
            data.Add("password", password);
            string method = String.Format("users/{0}/password/verify", userId);
            string results = CallApi(method, data);

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return response.Status == 1 ? true : false; // TODO: what does this method return?
            }
            else
            {
                return false;
            }
        }

        // /api/v2/users/{user_id}/petownershipplan
        // GET /api/v2/users/{user_id}/petownershipplan Get user's pet ownership plan
        public object FindOwnershipPlanByUserId(int userId)
        {
            return new NotImplementedException();
        }
        // /api/v2/users/{user_id}/sourcecode
        // GET /api/v2/users/{user_id}/sourcecode Get user source code
        public object FindSourceCodeByUserId(int userId)
        {
            return new NotImplementedException();
        }

        #endregion

        #region Internationalization Methods

        // /api/v2/countries
        // GET /api/v2/countries Provides a standardized list of countries.
        public object GetCountries()
        {
            return new NotImplementedException();
        }
        // /api/v2/countries/{id}
        // GET /api/v2/countries/{id} Retrieve a country by ID
        public object FindCountryById(int id)
        {
            return new NotImplementedException();
        }
        // /api/v2/currencies
        // GET /api/v2/currencies Provides a standardized list currencies.
        public object GetCurrencies()
        {
            return new NotImplementedException();
        }
        // /api/v2/currencies/{ id}
        // GET /api/v2/currencies/{id} Retrieves a currency.
        public object FindCurrencyById(int id)
        {
            return new NotImplementedException();
        }
        // /api/v2/languages
        // GET /api/v2/languages Provides a standardized list of languages.
        public object GetLanguages()
        {
            return new NotImplementedException();
        }
        // /api/v2/languages/{id}
        // GET /api/v2/languages/{id} Retrieve a language
        public object FindLanguageById(int id)
        {
            return new NotImplementedException();
        }

        #endregion
    }
}
