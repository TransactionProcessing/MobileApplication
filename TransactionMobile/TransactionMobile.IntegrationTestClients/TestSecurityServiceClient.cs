namespace TransactionMobile.IntegrationTestClients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using SecurityService.Client;
    using SecurityService.DataTransferObjects;
    using SecurityService.DataTransferObjects.Requests;
    using SecurityService.DataTransferObjects.Responses;

    public class TestSecurityServiceClient : ISecurityServiceClient
    {
        private List<(UserDetails userDetails, String password)> Users = new List<(UserDetails userDetails, String password)>();

        public void CreateUserDetails(String userName, Dictionary<String, String> claims, String password = "123456")
        {
            (UserDetails userDetails, String password) user = this.Users.SingleOrDefault(u => u.userDetails.EmailAddress == userName && u.password == password);

            if (user.userDetails == null)
            {
                this.Users.Add((new UserDetails
                                {
                                    Claims = claims,
                                    EmailAddress = userName,
                                    PhoneNumber = String.Empty,
                                    Roles = new List<String>(),
                                    UserId = Guid.NewGuid(),
                                    UserName = userName,

                                }, password));
                Console.WriteLine($"User {userName} added");
            }
        }
        public TestSecurityServiceClient()
        {
        }

        public async Task<TokenResponse> GetToken(String username,
                                                  String password,
                                                  String clientId,
                                                  String clientSecret,
                                                  CancellationToken cancellationToken)
        {
            (UserDetails userDetails, String password) user = this.Users.SingleOrDefault(u => u.userDetails.EmailAddress == username && u.password == password);
            if (user.userDetails == null)
            {
                // TODO: thrown an error
                Console.WriteLine($"User {username} NOT found");
            }
            Console.WriteLine($"User {username} found");

            String json = JsonConvert.SerializeObject(user);

            user.userDetails.Claims.TryGetValue("estateId", out String estateId);
            user.userDetails.Claims.TryGetValue("merchantId", out String merchantId);
            
            return TokenResponse.Create($"{estateId}|{merchantId}", null, 0, DateTimeOffset.Now);
        }

        #region Not Implemented
        public async Task<CreateApiResourceResponse> CreateApiResource(CreateApiResourceRequest createApiResourceRequest,
                                                                       CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateApiScopeResponse> CreateApiScope(CreateApiScopeRequest createApiScopeRequest,
                                                                 CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateClientResponse> CreateClient(CreateClientRequest createClientRequest,
                                                             CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateIdentityResourceResponse> CreateIdentityResource(CreateIdentityResourceRequest createIdentityResourceRequest,
                                                                                 CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateRoleResponse> CreateRole(CreateRoleRequest createRoleRequest,
                                                         CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest createUserRequest,
                                                         CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<ApiResourceDetails> GetApiResource(String apiResourceName,
                                                             CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<ApiScopeDetails> GetApiScope(String apiScopeName,
                                                       CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<List<ApiResourceDetails>> GetApiResources(CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<List<ApiScopeDetails>> GetApiScopes(CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<ClientDetails> GetClient(String clientId,
                                                   CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<List<ClientDetails>> GetClients(CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<IdentityResourceDetails> GetIdentityResource(String identityResourceName,
                                                                       CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<List<IdentityResourceDetails>> GetIdentityResources(CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<RoleDetails> GetRole(Guid roleId,
                                               CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<List<RoleDetails>> GetRoles(CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<TokenResponse> GetToken(String clientId,
                                                  String clientSecret,
                                                  CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<TokenResponse> GetToken(String clientId,
                                                  String clientSecret,
                                                  String refreshToken,
                                                  CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<UserDetails> GetUser(Guid userId,
                                               CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<List<UserDetails>> GetUsers(String userName,
                                                      CancellationToken cancellationToken)
        {
            return null;
        }
        #endregion
    }
}