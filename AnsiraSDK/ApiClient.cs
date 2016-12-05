using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Ansira.Objects;
using Newtonsoft.Json;

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

        #region API Wrappers

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
            data.Add("filter", "UUID=\"" + uuid + "\"");
            string results = CallApi("find", data);

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
            data.Add("filter", "Email=\"" + email + "\"");
            string results = CallApi("find", data);

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
            if (familyName == null && givenName == null)
            {
                throw new ArgumentNullException();
            }
            NameValueCollection data = new NameValueCollection();
            string query = null;
            if (familyName != null)
            {
                query = "FamilyName=\"" + familyName + "\"";
            }
            if (familyName != null && givenName != null)
            {
                query += " AND ";
            }
            if (givenName != null)
            {
                query += "GivenName=\"" + givenName + "\"";
            }
            data.Add("filter", query);

            string results = CallApi("find", data);

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
        /// <exception cref="System.ArgumentNullException">Thrown when User or SourceId is null</exception>
        public User CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "User must not be null");
            }
            if (user.SourceId <= 0)
            {
                throw new ArgumentNullException("user.SourceId", "SourceId must not be null");
            }
            NameValueCollection data = new NameValueCollection();
            string record = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            data.Add("record", record);
            string results = CallApi("create", data);
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
        /// Update an existing User in the Ansira API
        /// </summary>
        /// <param name="user">Ansira.Objects.User with non-null UUID</param>
        /// <returns>Ansira.Objects.User or null if error</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when User, SourceId, or UUID is null</exception>
        public User UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "User must not be null");
            }
            if (user.Uuid == null)
            {
                throw new ArgumentNullException("uuid", "User's UUID must not be null");
            }
            if (user.SourceId <= 0)
            {
                throw new ArgumentNullException("user.SourceId", "SourceId must not be null");
            }
            NameValueCollection data = new NameValueCollection();
            string record = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            data.Add("record", record);
            string results = CallApi("update", data);
            if (results != null)
            {
                ResponseV3 response = JsonConvert.DeserializeObject<ResponseV3>(results); // TEMP?
                User responseUser = JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(response.Record.Results));
                //responseUser.Uuid = user.Uuid; // TODO: question to Ansira as to why UUID is omitted
                return responseUser;
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
            DeleteUser(user.Uuid);
        }

        /// <summary>
        /// Delete an existing User from the Ansira API
        /// </summary>
        /// <param name="uuid">UUID string</param>
        /// <exception cref="System.ArgumentNullException">Thrown when UUID is null</exception>
        public void DeleteUser(string uuid)
        {
            if (uuid == null)
            {
                throw new ArgumentNullException("uuid", "UUID must not be null");
            }
            NameValueCollection data = new NameValueCollection();
            data.Add("uuid", uuid);
            string results = CallApi("delete", data);
        }

        /// <summary>
        /// Sign-out a User from the Ansira SSO
        /// </summary>
        /// <param name="user">Ansira.Objects.User with non-null ID</param>
        /// <exception cref="System.ArgumentNullException">Thrown when User is null</exception>
        public void SignOutUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "User must not be null");
            }
            SignOutUser(Convert.ToInt32(user.Id));
        }

        /// <summary>
        /// Sign-out a User from the Ansira SSO
        /// </summary>
        /// <param name="id">ID string</param>
        /// <exception cref="System.ArgumentNullException">Thrown when ID is null</exception>
        public void SignOutUser(int id)
        {
            if (id.Equals(default(int)))
            {
                throw new ArgumentNullException("id", "ID must not be null");
            }
            NameValueCollection data = new NameValueCollection();
            data.Add("userId", Convert.ToString(id));
            string results = CallApi("signout", data);
        }



        /// <summary>
        /// Get the pet types supported by the API
        /// </summary>
        /// <returns>IList of Ansira.Objects.PetType objects or null if none</returns>
        public IList<PetType> GetPetTypes()
        {
            string results = CallApi("getPetTypes", null);

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
        /// Get the pet breeds supported by the API
        /// </summary>
        /// <param name="petTypeId">Integer ID of PetType</param>
        /// <returns>IList of Ansira.Objects.Breed objects or null if none</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when Pet Type ID is null</exception>
        public IList<Breed> GetPetBreeds(int petTypeId)
        {
            if (petTypeId.Equals(default(int)))
            {
                throw new ArgumentNullException("petTypeId", "Pet Type ID must not be null");
            }
            NameValueCollection data = new NameValueCollection();
            data.Add("petType", Convert.ToString(petTypeId));
            string results = CallApi("getBreeds", data);

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


        #endregion
    }
}
