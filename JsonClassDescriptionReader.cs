using System.IO;

namespace GenerateCode
{
    public class JsonClassDescriptionReader
    {
        private string fileName;

        public JsonClassDescriptionReader(string fileName)
        {
            this.fileName = fileName;
        }

        public string Read()
        {
            var file = new StreamReader(fileName);
            var jsonClassDecscription = file.ReadToEnd();
            file.Close();
            return jsonClassDecscription;
        }
    }
}
