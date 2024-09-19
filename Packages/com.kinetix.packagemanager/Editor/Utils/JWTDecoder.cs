using System;
using Newtonsoft.Json;
using System.Text;

namespace UnityEditor.VSAttribution.Kinetix
{
public class JWTDecoder
{
    public class JWTPayload
    {
        public string sub { get; set; }
        public string[] cognito_groups { get; set; }
        public string iss { get; set; }
        // Add other necessary fields based on your JWT structure
    }

    public JWTPayload Decode(string token)
    {
        string[] parts = token.Split('.');
        if (parts.Length != 3)
        {
            throw new InvalidOperationException("JWT does not have 3 parts!");
        }

        string payload = parts[1];
        byte[] payloadBytes = Convert.FromBase64String(Base64UrlDecode(payload));
        string payloadJson = Encoding.UTF8.GetString(payloadBytes);

        return JsonConvert.DeserializeObject<JWTPayload>(payloadJson);
    }

    private string Base64UrlDecode(string payload)
    {
        string padded = payload.Length % 4 == 0
            ? payload
            : payload + "====".Substring(payload.Length % 4);

        string base64 = padded.Replace("_", "/")
                              .Replace("-", "+");
        return base64;
    }
}
}
