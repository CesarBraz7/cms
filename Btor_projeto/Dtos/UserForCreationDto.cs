namespace Btor_projeto.Dtos
{
    //Classe dto para criar um user.
    public class UserForCreationDto
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        //Verifica se o login tem no minimo 5 caracteres
        public bool isLoginValid()
        {
            return Login.Length > 4;
        }

        //Verifica se o email é valido
        public bool isEmailValid()
        {
            try
            {
                //Usa a classe MailAddress para validar o email passado
                var email = new System.Net.Mail.MailAddress(Email);
                return email.Address == Email;
            }
            catch
            {
                return false;
            }
        }

        //Verifica se a senha é valida
        public bool isPasswordValid()
        {
            return Password.Length > 5;
        }
    }
}
