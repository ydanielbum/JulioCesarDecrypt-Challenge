using System;
using System.IO;

namespace JulioCesarDecrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            CodenationRequests _codeReq = new CodenationRequests();
            Message _message = new Message();
           
            string response = _codeReq.Get().Result;
            File.WriteAllText("answer.json", response);

            _message = _message.Deserialize().Result;
            _message.Decrypt();
            _message.EncryptSHA1();
            _message.UpdateJson();

            _codeReq.Post(_message);
            Console.ReadLine();
        }
    }
}
