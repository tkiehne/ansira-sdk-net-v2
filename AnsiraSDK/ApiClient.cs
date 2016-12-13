using System;
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
        protected string apiUrl, clientId, clientSecret, accessToken;
        protected List<string> validMethods;

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

            validMethods = new List<string>() { "GET", "POST", "PUT", "PATCH", "DELETE" };

            // TODO: get access_token via OAuth & cache
        }

        #region Utility Methods

        /// <summary>
        /// Calls an API endpoint and returns results
        /// </summary>
        /// <param name="endpoint">API endpoint to call</param>
        /// <param name="data">Additional data as NameValueCollection</param>
        /// <param name="method">HTTP verb to use; defaults to GET</param>
        /// <returns>Response JSON as string or null if no results</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when API endpoint is null</exception>
        /// <exception cref="System.Net.WebException">Thrown when network operation fails</exception>
        /// <exception cref="System.ArgumentException">Thrown when HTTP method is unsupported</exception>
        protected string CallApi(string endpoint, NameValueCollection data, string method = "GET")
        {
            if (String.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException("endpoint", "API endpoint must not be null");
            }
            if (!validMethods.Contains(method))
            {
                throw new ArgumentException("method", "HTTP method is not supported");
            }

            string endpointUrl = this.apiUrl + endpoint;

            NameValueCollection message = new NameValueCollection();
            message.Add("access_token", this.accessToken);
            //message.Add("_format", "json"); // JSON by default - enable this to change format
            if (data != null)
            {
                message.Add(data);
            }

            WebClient client = new WebClient();
            byte[] output = client.UploadValues(endpointUrl, method, message);
            
            string apiResponse = Encoding.UTF8.GetString(output);

            Response response = JsonConvert.DeserializeObject<Response>(apiResponse);

            if (response.Status == 1)
            {
                /* TODO: Disabled until Ansira harmonizes their responses
                return JsonConvert.SerializeObject(response.Results);
                 */
                return apiResponse; // TEMP
            }

            return null;
        }

        /// <summary>
        /// Calls an API endpoint and returns results
        /// </summary>
        /// <param name="endpoint">API endpoint to call</param>
        /// <param name="data">Additional data as JSON string</param>
        /// <param name="method">HTTP verb to use; defaults to POST</param>
        /// <returns>Response JSON as string or null if no results</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when API endpoint is null</exception>
        /// <exception cref="System.Net.WebException">Thrown when network operation fails</exception>
        /// <exception cref="System.ArgumentException">Thrown when HTTP method is unsupported</exception>
        protected string CallApi(string endpoint, string data, string method = "POST")
        {
            if (String.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException("endpoint", "API endpoint must not be null");
            }
            if (!validMethods.Contains(method))
            {
                throw new ArgumentException("method", "HTTP method is not supported");
            }

            string endpointUrl = this.apiUrl + endpoint;

            NameValueCollection message = new NameValueCollection();
            message.Add("access_token", this.accessToken);
            //message.Add("_format", "json"); // JSON by default - enable this to change format

            WebClient client = new WebClient();
            string apiResponse = client.UploadString(endpointUrl, method, data);

            Response response = JsonConvert.DeserializeObject<Response>(apiResponse);

            if (response.Status == 1)
            {
                /* TODO: Disabled until Ansira harmonizes their responses
                return JsonConvert.SerializeObject(response.Results);
                 */
                return apiResponse; // TEMP
            }

            return null;
        }

        /// <summary>
        /// Calls an API endpoint via DELETE and returns results
        /// <para>Invokes <see cref="CallApi(System.String, System.Collections.Specialized.NameValueCollection, System.String)"/></para>
        /// </summary>
        /// <param name="endpoint">API endpoint to call</param>
        /// <param name="data">Additional POST data as NameValueCollection</param>
        /// <returns>Response JSON as string or null if no results</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when API Method is null</exception>
        /// <exception cref="System.Net.WebException">Thrown when network operation fails</exception>
        protected string CallApiDelete(string endpoint, NameValueCollection data)
        {
            return CallApi(endpoint, data, "DELETE");
        }

        /// <summary>
        /// Calls an API endpoint via POST and returns results
        /// <para>Invokes <see cref="CallApi(System.String, System.String, System.String)"/></para>
        /// </summary>
        /// <param name="endpoint">API endpoint to call</param>
        /// <param name="data">Additional POST data as string</param>
        /// <returns>Response JSON as string or null if no results</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when API Method is null</exception>
        /// <exception cref="System.Net.WebException">Thrown when network operation fails</exception>
        protected string CallApiPost(string endpoint, string data)
        {
            return CallApi(endpoint, data, "POST");
        }

        /// <summary>
        /// Calls an API endpoint via PUT and returns results
        /// <para>Invokes <see cref="CallApi(System.String, System.String, System.String)"/></para>
        /// </summary>
        /// <param name="endpoint">API endpoint to call</param>
        /// <param name="data">Additional PUT data as string</param>
        /// <returns>Response JSON as string or null if no results</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when API Method is null</exception>
        /// <exception cref="System.Net.WebException">Thrown when network operation fails</exception>
        protected string CallApiPut(string endpoint, string data)
        {
            return CallApi(endpoint, data, "PUT");
        }

        /// <summary>
        /// Calls an API endpoint via PATCH and returns results
        /// <para>Invokes <see cref="CallApi(System.String, System.String, System.String)"/></para>
        /// </summary>
        /// <param name="endpoint">API endpoint to call</param>
        /// <param name="data">Additional PATCH data as string</param>
        /// <returns>Response JSON as string or null if no results</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when API Method is null</exception>
        /// <exception cref="System.Net.WebException">Thrown when network operation fails</exception>
        protected string CallApiPatch(string endpoint, string data)
        {
            return CallApi(endpoint, data, "PATCH");
        }


        #endregion

        #region Brand Methods

        /// <summary>
        /// Get all the brands associated with Purina
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-brands</remarks>
        /// <returns>IList of Ansira.Objects.Brand objects or null if none</returns>
        public IList<Brand> GetBrands()
        {
            string results = CallApi("brands", new NameValueCollection());

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
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-brands-{id}</remarks>
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
            string results = CallApi(method, new NameValueCollection());

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
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-sourcecodes</remarks>
        /// <returns>IList of Ansira.Objects.SourceCode objects or null if none</returns>
        public IList<SourceCode> GetSourceCodes()
        {
            string results = CallApi("sourcecodes", new NameValueCollection());

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
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-sourcecodes-{id}</remarks>
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
            string results = CallApi(method, new NameValueCollection());

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
            throw new NotImplementedException();
        }
        ///api/v2/contentgroup/{id}
        //Retrieves a specific content group
        public object FindContentGroupById(int id)
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentgroups
        //GET /api/v2/contentgroups Retrieves a simple list of content groups
        public object GetContentGroups()
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentgroups/find
        //GET /api/v2/contentgroups/find Finds a simple list of content groups
        public object FindContentGroups()
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentnode/{id}
        //GET /api/v2/contentnode/{id} Retrieves a specific content
        public object FindContentNodeById(int id)
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentnode/{id}/category
        //GET /api/v2/contentnode/{id}/category Retrieves a specific content, category only
        public object FindContentNodeCategoryById(int id)
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentnode/{id}/tags
        //GET /api/v2/contentnode/{id}/tags Retrieves a specific content, tags only
        public object FindContentNodeTagsById(int id)
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentnodes/find
        //GET /api/v2/contentnodes/find Retrieves a list of contents
        public object FindContentNodes()
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentplacement/{id}
        //GET /api/v2/contentplacement/{id} Retrieves a specific content placement
        public object FindContentPlacementById(int id)
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentplacements
        //GET /api/v2/contentplacements Retrieves a list of all content placements
        public object GetContentPlacements()
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentplacements/find
        //GET /api/v2/contentplacements/find Retrieves a list of ContentPlacements
        public object FindContentPlacements()
        {
            throw new NotImplementedException();
        }
        ///api/v2/contenttaxonomy/{id}
        //GET /api/v2/contenttaxonomy/{id} Retrieves a specific content taxonomy
        public object FindContentTaxonomyById(int id)
        {
            throw new NotImplementedException();
        }
        ///api/v2/contenttaxonomys
        //GET /api/v2/contenttaxonomys Retrieves a list of all content taxonomys
        public object GetContentTaxonomys()
        {
            throw new NotImplementedException();
        }
        ///api/v2/contenttaxonomys/find
        //GET /api/v2/contenttaxonomys/find Retrieves a list of ContentTaxonomys
        public object FindContentTaxonomys()
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentzone/{id}
        //GET /api/v2/contentzone/{id} Retrieves a specific content zone
        public object FindContentZoneById(int id)
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentzones
        //GET /api/v2/contentzones Retrieves a list of all content zones
        public object GetContentZones()
        {
            throw new NotImplementedException();
        }
        ///api/v2/contentzones/find
        //GET /api/v2/contentzones/find Retrieves a list of ContentZones
        public object FindContentZones()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Event Methods

        // /api/v2/actiontypes
        // GET /api/v2/actiontypes Retrieves a list of action types available for use with our Events API method.
        public object GetActionTypes()
        {
            throw new NotImplementedException();
        }
        // /api/v2/actiontypes/{ id}
        // GET /api/v2/actiontypes/{id} Retrieves an action type
        public object FindActionTypeById(int id)
        {
            throw new NotImplementedException();
        }
        // /api/v2/eventtypes
        // GET /api/v2/eventtypes Provides a list of event types to be leveraged by the Events API method.
        public object GetEventTypes()
        {
            throw new NotImplementedException();
        }
        // /api/v2/eventtypes/{id}
        // GET /api/v2/eventtypes/{id} Retrieve an event type
        public object FindEventTypeById(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Pets Methods

        /// <summary>
        /// Get the pet breeds supported by the API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-breeds</remarks>
        /// <returns>IList of Ansira.Objects.Breed objects or null if none</returns>
        public IList<Breed> GetBreeds()
        {
            string results = CallApi("breeds", new NameValueCollection());

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
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-breeds-{id}</remarks>
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
            string results = CallApi(method, new NameValueCollection());

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

        /// <summary>
        /// Get the pet foods supported by the API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-petfoods</remarks>
        /// <returns>IList of Ansira.Objects.PetFood objects or null if none</returns>
        public IList<PetFood> GetPetFoods()
        {
            string results = CallApi("petfoods", new NameValueCollection());

            if (results != null)
            {
                ResponseV1 response = JsonConvert.DeserializeObject<ResponseV1>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<PetFood>>(JsonConvert.SerializeObject(response.Result));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find a Pet Food by its ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-petfoods-{id}</remarks>
        /// <param name="id">ID integer</param>
        /// <returns>Ansira.Objects.PetFood</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public PetFood FindPetFoodById(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("petfoods/{0}", id);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<PetFood>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the pet ownership plans supported by the API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-petownershipplans</remarks>
        /// <returns>IList of Ansira.Objects.PetOwnershipPlan objects or null if none</returns>
        public IList<PetOwnershipPlan> GetOwnershipPlans()
        {
            string results = CallApi("petownershipplans", new NameValueCollection());

            if (results != null)
            {
                ResponseV1 response = JsonConvert.DeserializeObject<ResponseV1>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<PetOwnershipPlan>>(JsonConvert.SerializeObject(response.Result));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find a Pet Ownership Plan by its ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-petownershipplans-{id}</remarks>
        /// <param name="id">ID integer</param>
        /// <returns>Ansira.Objects.PetOwnershipPlan</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public PetOwnershipPlan FindOwnershipPlanById(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("petownershipplans/{0}", id);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<PetOwnershipPlan>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the pet types supported by the API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-pettypes</remarks>
        /// <returns>IList of Ansira.Objects.PetType objects or null if none</returns>
        public IList<PetType> GetPetTypes()
        {
            string results = CallApi("pettypes", new NameValueCollection());

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
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-pettypes-{id}</remarks>
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
            string results = CallApi(method, new NameValueCollection());

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
        /// Find Pets by User ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-pets</remarks>
        /// <param name="userId">ID integer</param>
        /// <returns>IList of Ansira.Objects.Pet</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public IList<Pet> FindPetsByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/pets", userId);
            string results = CallApi(method, new NameValueCollection());

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

        /// <summary>
        /// Create a new Pet in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#post--api-v2-users-{user_id}-pets</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="pet">Ansira.Objects.Pet</param>
        /// <returns>Ansira.Objects.Pet or null if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User ID or Pet is null</exception>
        public Pet CreatePet(int userId, Pet pet)
        {
            if (pet == null)
            {
                throw new ArgumentNullException("user", "Pet must not be null");
            }
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "User ID must not be null");
            }
            // TODO: Validate Pet
            
            // pass through JSON to get correct parameters
            string record = JsonConvert.SerializeObject(pet, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            
            string method = String.Format("users/{0}/pets", userId);
            string results = CallApiPost(method, record);
            if (results != null)
            {
                ResponseV3 response = JsonConvert.DeserializeObject<ResponseV3>(results); // TEMP?
                return JsonConvert.DeserializeObject<Pet>(JsonConvert.SerializeObject(response.Record.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Delete an existing User from the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#delete--api-v2-users-{user_id}-pets-{pet_id}</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="petId">Pet ID integer</param>
        /// <returns>Boolean; True if successful, False if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User or Pet ID is null</exception>
        public bool DeletePet(int userId, int petId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "User ID must not be null");
            }
            if (String.IsNullOrEmpty(petId.ToString()))
            {
                throw new ArgumentNullException("petId", "Pet ID must not be null");
            }
            string method = String.Format("users/{0}/pets/{1}", userId, petId);
            string results = CallApiDelete(method, null);
            if (results == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Find Pet by User ID and Pet ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-pets-{pet_id}</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="petId">Pet ID integer</param>
        /// <returns>Ansira.Objects.Pet</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User or Pet ID is null</exception>
        public Pet FindPetById(int userId, int petId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "User ID must not be null");
            }
            if (String.IsNullOrEmpty(petId.ToString()))
            {
                throw new ArgumentNullException("petId", "Pet ID must not be null");
            }
            string method = String.Format("users/{0}/pets/{1}", userId, petId);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<Pet>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update a Pet in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#put--api-v2-users-{user_id}-pets-{pet_id}</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="pet">Ansira.Objects.Pet</param>
        /// <returns>Ansira.Objects.Pet or null if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User ID or Pet is null</exception>
        public Pet UpdatePet(int userId, Pet pet)
        {
            if (pet == null)
            {
                throw new ArgumentNullException("pet", "Pet must not be null");
            }
            if (String.IsNullOrEmpty(pet.Id.ToString()))
            {
                throw new ArgumentNullException("pet.Id", "Pet ID must not be null");
            }
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "User ID must not be null");
            }
            // TODO: Validate Pet
            
            string record = JsonConvert.SerializeObject(pet, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            string method = String.Format("users/{0}/pets/{1}", userId, pet.Id);
            string results = CallApiPut(method, record); // TODO: Put vs Patch
            if (results != null)
            {
                ResponseV3 response = JsonConvert.DeserializeObject<ResponseV3>(results); // TEMP?
                return JsonConvert.DeserializeObject<Pet>(JsonConvert.SerializeObject(response.Record.Results));
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Subscription Methods

        /// <summary>
        /// Delete Subscriptions for a User in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#delete--api-v2-users-{user_id}-subscriptions</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="brandCodes">List of brand codes</param>
        /// <returns>True if successful or false if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User ID is null or brandCodes is null or empty</exception>
        public bool DeleteUserSubscription(int userId, List<string> brandCodes)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            if (brandCodes == null || brandCodes.Count < 1)
            {
                throw new ArgumentNullException("brandCodes", "Must supply at least one brand code");
            }
            NameValueCollection data = new NameValueCollection();

            // assuming URL-type array and not JSON
            for (var i = 0; i < brandCodes.Count - 1; i++)
            {
                data.Add("subscriptions[" + i + "]", brandCodes[i]);
            }

            string method = String.Format("users/{0}/subscriptions", userId);
            string results = CallApiDelete(method, data);
            if (results != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Find Subscriptions by User ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-subscriptions</remarks>
        /// <param name="userId">ID integer</param>
        /// <returns>IList of Ansira.Objects.Subscription</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public IList<Subscription> FindSubscriptionsByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/subscriptions", userId);
            string results = CallApi(method, new NameValueCollection());

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

        /// <summary>
        /// Update Subscriptions for a User in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#put--api-v2-users-{user_id}-subscriptions</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="brandCodes">List of brand codes</param>
        /// <returns>True if successful or false if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User ID is null or brandCodes is null or empty</exception>
        public bool UpdateUserSubscription(int userId, List<string> brandCodes)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            if (brandCodes == null || brandCodes.Count < 1)
            {
                throw new ArgumentNullException("brandCodes", "Must supply at least one brand code");
            }

            string record = JsonConvert.SerializeObject(brandCodes, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            string method = String.Format("users/{0}/subscriptions", userId);
            string results = CallApiPut(method, record); // TODO: verify Put and Patch are the same
            if (results != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create a new Subscription for a User in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#post--api-v2-users-{user_id}-subscriptions</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="brandCodes">List of brand codes</param>
        /// <returns>True if successful or false if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User ID is null or brandCodes is null or empty</exception>
        public bool CreateUserSubscription(int userId, List<string> brandCodes)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            if(brandCodes == null || brandCodes.Count < 1)
            {
                throw new ArgumentNullException("brandCodes", "Must supply at least one brand code");
            }

            string record = JsonConvert.SerializeObject(brandCodes, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            string method = String.Format("users/{0}/subscriptions", userId);
            string results = CallApiPost(method, record);
            if (results != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Subscription for a specific User and Brand in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#delete--api-v2-users-{user_id}-subscriptions-{brand_code}</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="brandCode">Brand code</param>
        /// <returns>True if successful or false if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User ID is null or Brand Code is null or empty</exception>
        public bool DeleteUserSubscriptionByBrand(int userId, string brandCode)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            if (String.IsNullOrEmpty(brandCode))
            {
                throw new ArgumentNullException("brandCode", "Must supply a brand code");
            }
            // NameValueCollection data = new NameValueCollection(); TODO: do we need parameters?
            // data.Add("subscriptions[]", brandCode);

            string method = String.Format("users/{0}/subscriptions/{1}", userId, brandCode);
            string results = CallApiDelete(method, null);
            if (results != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Find Subscriptions by User ID and Brand
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-subscriptions-{brand_code}</remarks>
        /// <param name="userId">ID integer</param>
        /// <param name="brandId">Brand code</param>
        /// <returns>IList of Ansira.Objects.Subscription</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when Brand is null</exception>
        public IList<Subscription> FindUserSubscriptionsByBrand(int userId, string brandId)
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
            string results = CallApi(method, new NameValueCollection());

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

        /// <summary>
        /// Update a Subscription for a User and Brand in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#put--api-v2-users-{user_id}-subscriptions-{brand_code}</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="brandCode">Brand code</param>
        /// <returns>True if successful or false if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User ID is null or Brand Code is null or empty</exception>
        public bool UpdateUserSubscriptionByBrand(int userId, string brandCode)
        {
            // TODO: does this method make any sense?
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            if (String.IsNullOrEmpty(brandCode))
            {
                throw new ArgumentNullException("brandCode", "Must supply a brand code");
            }
            // NameValueCollection data = new NameValueCollection(); TODO: do we need parameters?
            // data.Add("subscriptions[]", brandCode);

            string method = String.Format("users/{0}/subscriptions/{1}", userId, brandCode);
            string results = CallApiPut(method, null); // TODO: verify if Put and Patch are the same
            if (results != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create a new Subscription for a User and Brand in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#post--api-v2-users-{user_id}-subscriptions-{brand_code}</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="brandCode">Brand code</param>
        /// <returns>True if successful or false if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User ID is null or Brand Code is null or empty</exception>
        public bool CreateUserSubscription(int userId, string brandCode)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            if (String.IsNullOrEmpty(brandCode))
            {
                throw new ArgumentNullException("brandCode", "Must supply a brand code");
            }
            // NameValueCollection data = new NameValueCollection(); TODO: do we need parameters?
            // data.Add("subscriptions[]", brandCode);

            string method = String.Format("users/{0}/subscriptions/{1}", userId, brandCode);
            string results = CallApiPost(method, null);
            if (results != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region User Methods 

        /// <summary>
        /// Get list of Ansira Users
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users</remarks>
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
            string results = CallApi("users", new NameValueCollection());

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
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{id}</remarks>
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
            string results = CallApi(method, new NameValueCollection());

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
        /// <para>See also <seealso cref="GetUsers()"/></para>
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
        /// <para>See also <seealso cref="GetUsers()"/></para>
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
        /// <para>See also <seealso cref="GetUsers()"/></para>
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
        /// <remarks>https://profiles.purina.com/service/apidoc#post--api-v2-users</remarks>
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
            // TODO: validate User

            NameValueCollection data = new NameValueCollection();
            string record = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            
            string results = CallApiPost("users", record);
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
        /// <para>Invokes <see cref="DeleteUser(System.Int32)"/></para>
        /// </summary>
        /// <param name="user">Ansira.Objects.User with non-null UUID</param>
        /// <exception cref="System.ArgumentNullException">Thrown when User is null</exception>
        public bool DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "User must not be null");
            }
            return this.DeleteUser((int)user.Id);
        }

        /// <summary>
        /// Delete an existing User from the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#delete--api-v2-users-{id}</remarks>
        /// <param name="id">ID integer</param>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public bool DeleteUser(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("users/{0}", id);
            string results = CallApiDelete(method, null);
            if(results == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Update an existing User in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#put--api-v2-users-{id}</remarks>
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
                throw new ArgumentNullException("id", "User ID must not be null");
            }
            // TODO: validate user

            NameValueCollection data = new NameValueCollection();

            string record = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            string method = String.Format("users/{0}", user.Id);
            string results = CallApiPut(method, record); // *** TODO: verify that Patch & Put are the same
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
        /// Delete an existing User's Address from the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#delete--api-v2-users-{user_id}-address</remarks>
        /// <param name="userId">ID integer for user</param>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public bool DeleteUserAddress(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/address", userId);
            string results = CallApiDelete(method, null);
            if(results == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Find Address by User ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-address</remarks>
        /// <param name="userId">ID integer</param>
        /// <returns>Ansira.Objects.Address</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public Address FindAddressByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/address", userId);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<Address>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update an Address in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#put--api-v2-users-{user_id}-address</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="address">Ansira.Objects.Address</param>
        /// <returns>Ansira.Objects.Address or null if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User ID or Address is null</exception>
        public Address UpdateUserAddress(int userId, Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address", "Address must not be null");
            }
            if (String.IsNullOrEmpty(address.Id.ToString()))
            {
                throw new ArgumentNullException("address.Id", "Address ID must not be null");
            }
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "User ID must not be null");
            }
            // TODO: Validate address

            NameValueCollection data = new NameValueCollection();
            
            string record = JsonConvert.SerializeObject(address, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            string method = String.Format("users/{0}/address/{1}", userId, address.Id);
            string results = CallApiPut(method, record); // TODO: verify if Put and Patch are identical
            if (results != null)
            {
                ResponseV3 response = JsonConvert.DeserializeObject<ResponseV3>(results); // TEMP?
                return JsonConvert.DeserializeObject<Address>(JsonConvert.SerializeObject(response.Record.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create a new Address in the Ansira API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#post--api-v2-users-{user_id}-address</remarks>
        /// <param name="userId">User ID integer</param>
        /// <param name="address">Ansira.Objects.Address</param>
        /// <returns>Ansira.Objects.Address or null if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User ID or Address is null</exception>
        public Address CreateUserAddress(int userId, Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address", "Address must not be null");
            }
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "User ID must not be null");
            }
            // TODO: Validate Address

            NameValueCollection data = new NameValueCollection();
            
            string record = JsonConvert.SerializeObject(address, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            string method = String.Format("users/{0}/address", userId);
            string results = CallApiPost(method, record);
            if (results != null)
            {
                ResponseV3 response = JsonConvert.DeserializeObject<ResponseV3>(results); // TEMP?
                return JsonConvert.DeserializeObject<Address>(JsonConvert.SerializeObject(response.Record.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find User's Currency by User ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-currency</remarks>
        /// <param name="userId">ID integer</param>
        /// <returns>Ansira.Objects.Currency</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public Currency FindCurrencyByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/currency", userId);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<Currency>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find User's Language by User ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-language</remarks>
        /// <param name="userId">ID integer</param>
        /// <returns>Ansira.Objects.Language</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public Language FindLanguageByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/language", userId);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<Language>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find User's Country by User ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-nationality</remarks>
        /// <param name="userId">ID integer</param>
        /// <returns>Ansira.Objects.Country</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public Country FindNationalityByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/nationality", userId);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<Country>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find User's Pet Ownership Plan by User ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-petownershipplan</remarks>
        /// <param name="userId">ID integer</param>
        /// <returns>Ansira.Objects.PetOwnershipPlan</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public PetOwnershipPlan FindOwnershipPlanByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/petownershipplan", userId);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<PetOwnershipPlan>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find User's Source Code by User ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-sourcecode</remarks>
        /// <param name="userId">ID integer</param>
        /// <returns>Ansira.Objects.SourceCode</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public SourceCode FindSourceCodeByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/sourcecode", userId);
            string results = CallApi(method, new NameValueCollection());

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

        /// <summary>
        /// Find User's Last Source Code by User ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-users-{user_id}-lastsourcecode</remarks>
        /// <param name="userId">ID integer</param>
        /// <returns>Ansira.Objects.SourceCode</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public SourceCode FindLastSourceCodeByUserId(int userId)
        {
            if (String.IsNullOrEmpty(userId.ToString()))
            {
                throw new ArgumentNullException("userId", "ID must not be null");
            }
            string method = String.Format("users/{0}/lastsourcecode", userId);
            string results = CallApi(method, new NameValueCollection());

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

        /// <summary>
        /// Change Ansira User password
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#post--api-v2-users-{user_id}-password-change</remarks>
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

            string record = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            string method = String.Format("users/{0}/password/change", userId);
            string results = CallApiPost(method, record);

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
        /// <remarks>https://profiles.purina.com/service/apidoc#post--api-v2-users-{user_id}-password-verify</remarks>
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
            string record = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            string method = String.Format("users/{0}/password/verify", userId);
            string results = CallApiPost(method, record);

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
        
        #endregion

        #region Internationalization Methods

        /// <summary>
        /// Get the Countries supported by the API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-countries</remarks>
        /// <returns>IList of Ansira.Objects.Country objects or null if none</returns>
        public IList<Country> GetCountries()
        {
            string results = CallApi("countries", new NameValueCollection());

            if (results != null)
            {
                ResponseV1 response = JsonConvert.DeserializeObject<ResponseV1>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<Country>>(JsonConvert.SerializeObject(response.Result));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find a Country by its ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-countries-{id}</remarks>
        /// <param name="id">ID integer</param>
        /// <returns>Ansira.Objects.Country</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public Country FindCountryById(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("countries/{0}", id);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<Country>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the Currencies supported by the API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-currencies</remarks>
        /// <returns>IList of Ansira.Objects.Currency objects or null if none</returns>
        public IList<Currency> GetCurrencies()
        {
            string results = CallApi("currencies", new NameValueCollection());

            if (results != null)
            {
                ResponseV1 response = JsonConvert.DeserializeObject<ResponseV1>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<Currency>>(JsonConvert.SerializeObject(response.Result));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find a Currency by its ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-currencies-{id}</remarks>
        /// <param name="id">ID integer</param>
        /// <returns>Ansira.Objects.Currency</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public Currency FindCurrencyById(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("currencies/{0}", id);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<Currency>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the Languages supported by the API
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-languages</remarks>
        /// <returns>IList of Ansira.Objects.Language objects or null if none</returns>
        public IList<Language> GetLanguages()
        {
            string results = CallApi("languages", new NameValueCollection());

            if (results != null)
            {
                ResponseV1 response = JsonConvert.DeserializeObject<ResponseV1>(results); // TEMP
                return JsonConvert.DeserializeObject<IList<Language>>(JsonConvert.SerializeObject(response.Result));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find a Language by its ID
        /// </summary>
        /// <remarks>https://profiles.purina.com/service/apidoc#get--api-v2-languages-{id}</remarks>
        /// <param name="id">ID integer</param>
        /// <returns>Ansira.Objects.Language</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public Language FindLanguageById(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            string method = String.Format("languages/{0}", id);
            string results = CallApi(method, new NameValueCollection());

            if (results != null)
            {
                ResponseV2 response = JsonConvert.DeserializeObject<ResponseV2>(results); // TEMP
                return JsonConvert.DeserializeObject<Language>(JsonConvert.SerializeObject(response.Results));
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
