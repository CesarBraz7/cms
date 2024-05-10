using System.Security.Claims;
using Btor_projeto.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Bcpg;

namespace Btor_projeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcountController : ControllerBase
    {
        //Cria uma instancia de UserRepo e de TokenRepo
        private readonly UserRepository _userRepository;
        private readonly TokenService _tokenService;

        public AcountController(UserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        //Recebe o post do login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<Token>> Login(LoginDto request)
        {
            try
            {
                var user = await _userRepository.getUserByLoginAsync(request.login);
                if (user == null)
                {
                    return BadRequest("Usuário não cadastrado!");
                }
                if (request.password == user.Password)
                {
                    var token = _tokenService.GenerateToken(user);
                    return Ok(token);
                }
                return BadRequest("Senha ou login inválido");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // recebe um token a partir do usuário
        
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> ForgotPassword(FPDto model)
        {
            try
            {
                var user = await _userRepository.getUserByEmailAsync(model.email);
                if (user == null)
                {
                    return BadRequest("Se o usuário existe, um email de redefinição foi enviado");
                }

                var token = _tokenService.GenerateFpToken(user);
                var mailService = new MailService();
                //Oque deve ser enviado por email.
                var body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text =
                        $"<h2> Use o link abaixo para alterar a senha: </h2>" +
                        $"<br>" +
                        $"<h2>http://localhost:3000/recuperacao-senha/{token}</h2>"
                };
                
                mailService.SendEmail(user.Email, "Redefinir senha", body.ToString());
                
                return Ok("Se o usuário existe, um email de redefinição foi enviado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("authenticated")]
        [Authorize]
        public async Task<ActionResult> Authenticated()
        {
            return Ok();
        }

        [HttpPost("update-password")]
        [Authorize]
        public async Task<ActionResult> UpdatePassword(ResetPasswordDto model)
        {
            try
            {
                if (model.password != model.confirmpassword)
                {
                    return BadRequest("As senhas não coincidem!");
                }

                var identity = User.Identity as ClaimsIdentity;
                if (identity == null)
                {
                    return Unauthorized("O token não existe ou expirou.");
                }

                var userId = identity.FindFirst("user_id").Value;

                if (userId == null)
                    return StatusCode(500);

                var result = await _userRepository.ResetPasswordAsync(int.Parse(userId), model);

                if (!result)
                    return StatusCode(500);

                return Ok("A senha foi alterada com sucesso");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
