using Btor_projeto.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Btor_projeto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase

    {
        //Cria uma instancia de UserRepo
        private readonly UserRepository _userRepository;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //Recebe o Get todos os usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnUserDto>>> GetUsers()
        {
            try
            {
                var users = await _userRepository.getUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Recebe o get por id do usuario
        [HttpGet("{Id}")]
        public async Task<ActionResult<ReturnUserDto>> getUserbyId(int Id)
        {
            try
            {
                var user = await _userRepository.getUserbyIDAsync(Id);
                if (user == null)
                {
                    return NotFound("Usuário não encontrado.");
                }
                return Ok(user);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Recebe o post com os dados para criar um novo Usuario
        [HttpPost]
        public async Task<ActionResult<ReturnUserDto>> addUser(UserForCreationDto user)
        {
            try
            {
                if (!user.isLoginValid())
                    return BadRequest("Login inválido!");

                if (!user.isEmailValid())
                    return BadRequest("Email inválido!");
                
                if (user.Role != "user" && user.Role != "admin")
                    return BadRequest("A role fornecida não é válida. Deve ser 'user' ou 'admin'.");
                
                var newuser = await _userRepository.addUserAsync(user);
                return Ok(newuser);
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Recebe uma requisição put para atualizar um usuário existente
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateUser(int id, UserForUpdateDto user)
        {
            try
            {
                var dbUser = await _userRepository.getUserbyIDAsync(id);
                if (dbUser == null)
                    return NotFound();


                var result = await _userRepository.UpdateUserAsync(id, user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Recebe uma requisição delete para excluir um usuario existente por ser id.
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            try 
            { 
                var result = await _userRepository.DeleteUserAsync(id);

                if(!result){
                    return NotFound("Usuário não encontrado.");
                }

                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
