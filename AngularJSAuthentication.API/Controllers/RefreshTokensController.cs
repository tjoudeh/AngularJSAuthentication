using System.Threading.Tasks;
using System.Web.Http;
using AngularJSAuthentication.API.Data;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController : ApiController
    {
        private IAuthRepository authRepository;

        public RefreshTokensController(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;            
        }

        //[Authorize(Users="Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(authRepository.GetAllRefreshTokens());
        }

        //[Authorize(Users = "Admin")]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await authRepository.RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                authRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
