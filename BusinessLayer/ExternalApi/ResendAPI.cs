using Resend;

namespace BusinessLayer.ExternalApi
{
    public static class ResendApi
    {
        private static IResend _resend;

        public static void Initialize(IResend resend)
        {
            _resend = resend ?? throw new ArgumentNullException(nameof(resend));
        }

        public static async Task<bool> SendCredentialsAsync(string toEmail, string username, string password)
        {
            try
            {
                var emailMessage = new EmailMessage
                {
                    From = "",
                    Subject = "Credenciales de Acceso",
                };

                emailMessage.To.Add(toEmail);
                emailMessage.HtmlBody = $@"
                    <p>Hola,</p>
                    <p>Se te ha registrado exitosamente en el sistema. Tus credenciales de acceso son las siguientes:</p>
                    <ul>
                        <li><strong>Usuario:</strong> {username}</li>
                        <li><strong>Contraseña:</strong> {password}</li>
                    </ul>
                    <p>Por favor, cambia tu contraseña al iniciar sesión.</p>";

                var response = await _resend.EmailSendAsync(emailMessage);

                return response.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                return false;
            }
        }
    }
}