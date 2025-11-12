using System.Text.Json;

namespace SaborGregoNew.Extensions
{
    public static class SessionExtensions
    {
        // Método para salvar um objeto complexo na sessão
        public static void SetObjectFromJson<T>(this ISession session, string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            session.SetString(key, json);
        }

        // Método para obter um objeto complexo da sessão
        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            if (value == null) 
            {
                return default; // Retorna null (ou 0 para tipos de valor)
            }

            // 2. ⭐️ CORREÇÃO: Desserializa a string JSON e retorna o objeto.
            try
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            catch (JsonException)
            {
                // Recomendado: Tratar JSON corrompido limpando a chave
                session.Remove(key);
                return default; 
            }
        }

    }
}