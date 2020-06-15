using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JulioCesarDecrypt
{
    public class Message
    {
        public int numero_casas { get; set; }
        public string token { get; set; }
        public string cifrado { get; set; }
        public string decifrado { get; set; }
        public string resumo_criptografico { get; set; }

        public async Task<Message> Deserialize()
        {
            using (FileStream fs = File.OpenRead("answer.json"))
            {                
                return await JsonSerializer.DeserializeAsync<Message>(fs);
            }
        }       

        public void UpdateJson()
        {
            var jsonSerialized = JsonSerializer.Serialize(this);
            File.WriteAllText("answer.json", jsonSerialized);
        }

        public void Decrypt()
        {
            char[] buffer = this.cifrado.ToCharArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                if (char.IsLetter(buffer[i]))
                {        
                    char letter = buffer[i];
                    letter = (char)(letter - this.numero_casas);

                    if (letter > 'z')
                    {
                        letter = (char)(letter - 26);
                    }
                    else if (letter < 'a')
                    {
                        letter = (char)(letter + 26);
                    }

                    buffer[i] = letter;
                }   
            }

             this.decifrado = new string(buffer);
        }

        public void EncryptSHA1()
        {
            var sha1 = new SHA1Managed();
            var plaintextBytes = Encoding.UTF8.GetBytes(this.decifrado);
            var hashBytes = sha1.ComputeHash(plaintextBytes);

            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                sb.AppendFormat("{0:x2}", hashByte);
            }

            this.resumo_criptografico = sb.ToString();
        }
    }
}