using Newtonsoft.Json;

namespace GenerateCode
{
    public class JsonClassDescriptionParser
    {
        private string jsonClassDescription;

        public JsonClassDescriptionParser(string jsonClassDescription)
        {
            this.jsonClassDescription = jsonClassDescription;
        }

        public ClassDescription Parse()
        {
            return JsonConvert.DeserializeObject<ClassDescription>(jsonClassDescription);
        }
    }
}
