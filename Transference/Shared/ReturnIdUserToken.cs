using Newtonsoft.Json;
using System.Text;

namespace Transference.Shared
{
    public static class ReturnIdUserToken
    {
        public static string GetUserIdFromToken(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException(DiccionaryMessage.TokenInvalid);

            var payload = parts[1];

            // Decodificar el payload
            switch (payload.Length % 4)
            {
                case 2:
                    payload += "==";
                    break;
                case 3:
                    payload += "=";
                    break;
            }

            var jsonBytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(jsonBytes);

            // Deserializar el JSON al objeto
            var payloadData = JsonConvert.DeserializeObject<JwtPayload>(json);
            return payloadData.UserId; // Asegúrate de que esta propiedad coincida con el nombre en tu token
        }
    }

    // Clase que representa el payload
    public class JwtPayload
    {
        public string UserId { get; set; }
    }
}

