using System;
using System.Collections.Generic;
using InventoryLibrary;
using JSONStorageHelper;

/// <summary>
/// Represents the storage in a JSON file.
/// </summary>
public class JSONStorage
{
    private const string filepath = "../storage/inventory_manager.json";

    /// <summary>
    /// Dictionary containing all the objects
    /// </summary>
    public Dictionary<string, BaseClass> objects;

    /// <summary>
    /// Constructor.
    /// </summary>
    public JSONStorage() => objects = new Dictionary<string, BaseClass>();

    /// <summary>
    /// Returns all the objects.
    /// </summary>
    /// <returns>The dictionary containing all the objects.</returns>
    public Dictionary<string, BaseClass> All() => objects;

    /// <summary>
    /// Adds an object to the dictionary.
    /// </summary>
    /// <param name="obj">Object to add.</param>
    public void New(BaseClass obj) => objects.Add(obj.GetType().Name + "." + obj.Id, obj);

    /// <summary>
    /// Saves all the objects in the JSON file.
    /// </summary>
    public void Save() => JSONStorageUtils.Save(objects, filepath);

    /// <summary>
    /// Loads all the objects from the JSON file.
    /// </summary>
    public void Load() => JSONStorageUtils.Load(objects, filepath);
}

