using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TheWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DemoApiController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = new HttpClient();
            var metaDataResponse = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = metaDataResponse.UserInfoEndpoint,
                Token = accessToken
            });
            if (response.IsError)
            {
                throw new Exception("Problem while fetching data from the UserInfo endpoint", response.Exception);
            }
            var refClaim = response.Claims.FirstOrDefault(c => c.Type.Equals("refid"));
            var claims = User.Claims.ToList();
            if (refClaim != null)
                claims.Add(refClaim);

            return new JsonResult(from c in claims select new { c.Type, c.Value });
        }
    }
}
