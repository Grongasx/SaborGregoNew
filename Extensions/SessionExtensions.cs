using System.Text.Json;

namespace SaborGregoNew.Extensions
{
    public static class SessionExtensions
    {
        // Método para salvar um objeto complexo na sessão
        public static void SetObjectFromJson<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Método para obter um objeto complexo da sessão
        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            // Se a chave não existir, retorna o valor padrão (null para objetos)
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}