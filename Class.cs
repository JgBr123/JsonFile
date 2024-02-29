using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JgBr123.JsonFile
{
    /// <summary>
    /// Class for converting objects into JSON and writing it to files.
    /// </summary>
    public class JsonFileManager
    {
        /// <summary>
        /// Converts an object to JSON and then writes it to a file.
        /// </summary>
        /// <param name="path">Path of where the file will be created.</param>
        /// <param name="obj">Object that will be saved to the file.</param>
        /// <exception cref="IOException">Is thrown if something goes wrong during the process of writing the file.</exception>
        public static void Write(string path, object obj)
        {
            try
            {
                //Settings to include all fields into json
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new IncludeAllFieldsContractResolver()
                };

                //Serializes object into json and saves it
                string json = JsonConvert.SerializeObject(obj, settings);
                File.WriteAllText(path, json);
            }
            catch { throw new IOException($"File \"{path}\" is not valid or cannot be created at the moment."); }
        }
        /// <summary>
        /// Reads the JSON from a file and converts it to an object.
        /// </summary>
        /// <param name="path">Path of where the file is located.</param>
        /// <exception cref="IOException">Is thrown if something goes wrong during the process of reading the file.</exception>
        public static T? Read<T>(string path)
        {
            try
            {
                //Reads json file and deserializes it
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch { throw new IOException($"File \"{path}\" does not exist or is not accessible at the moment."); }
        }
    }
    //Internal classes and methods
    class IncludeAllFieldsContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            // Force serialization for fields (public or private)
            if (property.PropertyType!.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Func<>))
            {
                property.ShouldSerialize = instance => true;
            }

            return property;
        }
    }
}