namespace Btor_projeto.Dtos
{
    //Classe Dto para atualizar um usuario.
    public class UserForUpdateDto
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }    
    }
}
