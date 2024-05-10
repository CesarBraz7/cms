
using Btor_projeto.Dtos;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Btor_projeto
{
    public class UserRepository
    {
        //Injecao de dependencia para conexao com o banco
        private readonly DapperContext _context;
        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        //Tras todos os usuario do banco
        public async Task<IEnumerable<ReturnUserDto>> getUsersAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<ReturnUserDto>("SELECT * FROM Btor_User");
            }
        }

        //get user by email
        public async Task<User?> getUserByEmailAsync(string email)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Btor_User WHERE email = @Email", new { Email= email });
            }
        }
        
        public async Task<User?> getUserByLoginAsync(string login)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Btor_User WHERE login = @Login", new { Login= login });
            }
        }

        //Tras um usuario do banco por seu id
        public async Task<ReturnUserDto?> getUserbyIDAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<ReturnUserDto>("SELECT * FROM Btor_User WHERE id = @Id", new { Id = id });
            }
        }

        //Cria um novo usuario no banco
        public async Task<ReturnUserDto> addUserAsync(UserForCreationDto user)
        {

            using (var connection = _context.CreateConnection())
            {
                var query = @"INSERT INTO Btor_User (login, email, password, role) VALUES (@Login, @Email, @Password, @Role); SELECT CAST(SCOPE_IDENTITY() as int)";
                var userId = await connection.ExecuteScalarAsync<int>(query, user);

                var createdUser = new ReturnUserDto
                {
                    Id = userId,
                    Login = user.Login,
                    Email = user.Email,
                    Role = user.Role
                };

                return createdUser;
            }
        }

        //Atualiza dados já existentes de um usuario do banco
        public async Task<bool> UpdateUserAsync(int id, UserForUpdateDto user)
        {
            var query = @"UPDATE Btor_User SET login = @Login, email = @Email, password = @Password, role = @Role WHERE id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32);
            parameters.Add("login", user.Login, DbType.String);
            parameters.Add("email", user.Email, DbType.String);
            parameters.Add("password", user.Password, DbType.String);
            parameters.Add("role", user.Role, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
        }

        //Exclui um usuario do banco por seu id
        public async Task<bool> DeleteUserAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                const string query = @"DELETE FROM Btor_User WHERE id = @Id";
                var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> ResetPasswordAsync(int id, ResetPasswordDto dto)
        {
            var query = @"UPDATE Btor_User SET password = @Password WHERE id = @Id";

            if (!dto.password.Equals(dto.confirmpassword))
            {
                return false;
            }
            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32);
            parameters.Add("password", dto.password, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
        }
    }
}