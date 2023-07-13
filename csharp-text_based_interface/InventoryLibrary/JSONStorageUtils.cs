using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using InventoryLibrary;

namespace JSONStorageHelper
{
    /// <summary>
    /// Utils to save and load from the JSON file
    /// </summary>
    public static class JSONStorageUtils
    {
        private static readonly Dictionary<string, Type> TypeMappings = new Dictionary<string, Type>
        {
            { "Item", typeof(Item) },
            { "User", typeof(User) },
            { "Inventory", typeof(Inventory) }
        };

        /// <summary>
        /// Saves all the objects in the JSON file.
        /// </summary>
        /// <param name="objects">All the objects to save</param>
        /// <param name="filePath">Path to the JSON file</param>
        public static void Save(Dictionary<string, BaseClass> objects, string filePath)
        {
            if (!File.Exists(filePath))
                File.Create(filePath).Close();

            File.WriteAllText(filePath, JsonSerializer.Serialize(objects.ToDictionary(kvp => kvp.Key, kvp => MapToJsonObject(kvp.Value)), new JsonSerializerOptions { WriteIndented = true }));
        }

        /// <summary>
        /// Loads all the objects from the JSON file.
        /// </summary>
        /// <param name="objects">Dictionary where the objects are put</param>
        /// <param name="filePath">Path to the JSON file</param>
        public static void Load(Dictionary<string, BaseClass> objects, string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var jsonObject in JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(File.ReadAllText(filePath)))
            {
                if (GetObjectTypeFromKey(jsonObject.Key) == null)
                    continue;

                if (JsonSerializer.Deserialize(jsonObject.Value.GetRawText(), GetObjectTypeFromKey(jsonObject.Key)) is BaseClass baseObject)
                    objects[jsonObject.Key] = baseObject;
            }
        }

        private static object MapToJsonObject(BaseClass obj)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();

            foreach (var property in JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(obj, obj.GetType(), new JsonSerializerOptions { WriteIndented = true })))
                jsonObject[property.Key] = property.Value;

            return jsonObject;
        }

        private static Type GetObjectTypeFromKey(string key)
        {
            string[] keySplit = key.Split('.');

            if (keySplit.Length >= 2)
            {
                if (TypeMappings.ContainsKey(keySplit[0]))
                    return TypeMappings[keySplit[0]];
            }
            return null;
        }
    }
}
