using System.Security.Claims;

namespace SaborGregoNew.Extensions
{
    public static class User
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            try
            {
                if (principal == null) // verifica se está realmente logado
                {
                    throw new UnauthorizedAccessException("Usuário não autenticado.");
                }

                var userIdString = principal.FindFirstValue(ClaimTypes.NameIdentifier); //encontrar o claim

                if (string.IsNullOrWhiteSpace(userIdString)) // caso não encontre o claim
                {
                    throw new KeyNotFoundException("ID do usuário (Claim) não encontrado no token.");
                }

                if (int.TryParse(userIdString, out int userId)) // tenta transformar a string do claim em um inteiro
                {
                    Console.WriteLine($"ID do usuário encontrado: {userId}");
                    return userId; // Caso dê certo, retorna o ID do usuário
                }
                else
                {
                    throw new InvalidCastException($"O valor do ID do usuário ('{userIdString}') não é um número inteiro válido."); // Caso de errado, lança um erro
                }
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is KeyNotFoundException || ex is InvalidCastException)
            {
                // Captura e relança as exceções específicas para tratamento a montante
                throw;
            }
            catch (Exception ex)
            {
                // Captura qualquer outra exceção inesperada e a reempacota
                throw new ApplicationException("Ocorreu um erro inesperado ao tentar obter o ID do usuário.", ex);
            }
        }
    }
}
