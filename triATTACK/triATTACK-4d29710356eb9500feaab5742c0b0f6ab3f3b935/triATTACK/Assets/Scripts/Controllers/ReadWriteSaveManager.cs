using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ReadWriteSaveManager : Singleton<ReadWriteSaveManager>
{
    [Header("Properties")]
    [SerializeField]
    private int version = 1;
    [SerializeField]
    private string fileName = "data";
    private string path;

    [Header("Data")]
    [SerializeField]
    private List<DataBool> bools;
    [SerializeField]
    private List<DataInt> ints;
    [SerializeField]
    private List<DataFloat> floats;
    [SerializeField]
    private List<DataString> strings;

    private Data data = new Data();

    private void Awake()
    {
        path = Application.persistentDataPath + "/data/" + fileName + ".dat";

        Read();
    }

    #region Read, Write, & Wipe
    /// <summary>
    /// Writes the data to a file.
    /// </summary>
    public void Write()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if (!File.Exists(path))
            Directory.CreateDirectory(Application.persistentDataPath + "/data/");

        FileStream fileStream = File.Create(path);

        ListToArray();

        data.version = version;

        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();

        Debug.Log("Wrote save file to \"" + path + "\".");
    }

    /// <summary>
    /// Reads save data from file.
    /// Will delete save file if the current data version is higher.
    /// </summary>
    public void Read()
    {
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(path, FileMode.Open);

            Data savedData = (Data)binaryFormatter.Deserialize(fileStream);

            if (version > savedData.version)
            {
                fileStream.Close();

                Debug.Log("Save data version (" + savedData.version + ") does not match current data version (" + version + "). Wiping save data...");

                Wipe();
            }
            else
            {
                data = savedData;

                Debug.Log("Loaded save file at \"" + path + "\".");

                ArrayToList();

                fileStream.Close();
            }
        }
        else
        {
            Debug.Log("Could not find a save file at \"" + path + "\", creating one...");

            Write();
        }
    }

    /// <summary>
    /// Resets the current save file's values.
    /// </summary>
    public void Wipe()
    {
        data = new Data();

        ArrayToList();

        Debug.Log("Wiped save data... Saving...");

        Write();

        Read();
    }
    #endregion

    #region Array & List Conversion
    /// <summary>
    /// Converts the arrays in the data class to the lists the game edits.
    /// </summary>
    public void ArrayToList()
    {
        bools.Clear();
        for (int i = 0; i < data.bools.Length; i++)
        {
            bools.Add(data.bools[i]);
        }

        ints.Clear();
        for (int i = 0; i < data.ints.Length; i++)
        {
            ints.Add(data.ints[i]);
        }

        floats.Clear();
        for (int i = 0; i < data.floats.Length; i++)
        {
            floats.Add(data.floats[i]);
        }

        strings.Clear();
        for (int i = 0; i < data.strings.Length; i++)
        {
            strings.Add(data.strings[i]);
        }

        Debug.Log("Updated list values.");
    }

    /// <summary>
    /// Converts the lists the game edits to the arrays in the data class.
    /// </summary>
    public void ListToArray()
    {
        data.bools = new DataBool[bools.Count];
        for (int i = 0; i < bools.Count; i++)
        {
            data.bools[i] = bools[i];
        }

        data.ints = new DataInt[ints.Count];
        for (int i = 0; i < ints.Count; i++)
        {
            data.ints[i] = ints[i];
        }

        data.floats = new DataFloat[floats.Count];
        for (int i = 0; i < floats.Count; i++)
        {
            data.floats[i] = floats[i];
        }

        data.strings = new DataString[strings.Count];
        for (int i = 0; i < strings.Count; i++)
        {
            data.strings[i] = strings[i];
        }

        Debug.Log("Updated data array values.");
    }
    #endregion

    #region Get Data
    /// <summary>
    /// Searches for a saved data value.
    /// </summary>
    /// <param name="name">The name of the value stored in the data.</param>
    /// <param name="fallback">What value should be returned if there is no value found with that name?</param>
    /// <param name="saveFallback">Should Read Write Save Manager save the fallback value if no other value is found?</param>
    public bool GetData(string name, bool fallback, bool saveFallback = false)
    {
        foreach (DataBool dataBool in bools)
        {
            if (dataBool.name == name)
            {
                return dataBool.value;
            }
        }

        if (saveFallback)
        {
            SetData(name, fallback, true);
        }

        return fallback;
    }

    /// <summary>
    /// Searches for a saved data value.
    /// </summary>
    /// <param name="name">The name of the value stored in the data.</param>
    /// <param name="fallback">What value should be returned if there is no value found with that name?</param>
    /// <param name="saveFallback">Should Read Write Save Manager save the fallback value if no other value is found?</param>
    public int GetData(string name, int fallback, bool saveFallback = false)
    {
        foreach (DataInt dataInt in ints)
        {
            if (dataInt.name == name)
            {
                return dataInt.value;
            }
        }

        if (saveFallback)
        {
            SetData(name, fallback, true);
        }

        return fallback;
    }

    /// <summary>
    /// Searches for a saved data value.
    /// </summary>
    /// <param name="name">The name of the value stored in the data.</param>
    /// <param name="fallback">What value should be returned if there is no value found with that name?</param>
    /// <param name="saveFallback">Should Read Write Save Manager save the fallback value if no other value is found?</param>
    public float GetData(string name, float fallback, bool saveFallback = false)
    {
        foreach (DataFloat dataFloat in floats)
        {
            if (dataFloat.name == name)
            {
                return dataFloat.value;
            }
        }

        if (saveFallback)
        {
            SetData(name, fallback, true);
        }

        return fallback;
    }

    /// <summary>
    /// Searches for a saved data value.
    /// </summary>
    /// <param name="name">The name of the value stored in the data.</param>
    /// <param name="fallback">What value should be returned if there is no value found with that name?</param>
    /// <param name="saveFallback">Should Read Write Save Manager save the fallback value if no other value is found?</param>
    public string GetData(string name, string fallback, bool saveFallback = false)
    {
        foreach (DataString dataString in strings)
        {
            if (dataString.name == name)
            {
                return dataString.value;
            }
        }

        if (saveFallback)
        {
            SetData(name, fallback, true);
        }

        return fallback;
    }
    #endregion

    #region Set Data
    /// <summary>
    /// Saves an data value.
    /// </summary>
    /// <param name="name">The name of the value you are trying to save.</param>
    /// <param name="value">What value should be saved under this name?</param>
    /// <param name="save">Should Read Write Save Manager save the value?</param>
    public void SetData(string name, bool value, bool save = false)
    {
        foreach (DataBool dataBool in bools)
        {
            if (dataBool.name == name)
            {
                dataBool.value = value;

                Debug.Log("Set bool data value \"" + name + "\" to \"" + value + "\".");

                if (save)
                {
                    Write();
                }

                return;
            }
        }

        bools.Add(new DataBool(name, value));

        Debug.Log("Added bool data value \"" + name + "\" with a value of \"" + value + "\".");

        if (save)
        {
            Write();
        }

        return;
    }

    /// <summary>
    /// Saves an data value.
    /// </summary>
    /// <param name="name">The name of the value you are trying to save.</param>
    /// <param name="value">What value should be saved under this name?</param>
    /// <param name="save">Should Read Write Save Manager save the value?</param>
    public void SetData(string name, int value, bool save = false)
    {
        foreach (DataInt dataInt in ints)
        {
            if (dataInt.name == name)
            {
                dataInt.value = value;

                Debug.Log("Set int data value \"" + name + "\" to \"" + value + "\".");

                if (save)
                {
                    Write();
                }

                return;
            }
        }

        ints.Add(new DataInt(name, value));

        Debug.Log("Added int data value \"" + name + "\" with a value of \"" + value + "\".");

        if (save)
        {
            Write();
        }

        return;
    }

    /// <summary>
    /// Saves an data value.
    /// </summary>
    /// <param name="name">The name of the value you are trying to save.</param>
    /// <param name="value">What value should be saved under this name?</param>
    /// <param name="save">Should Read Write Save Manager save the value?</param>
    public void SetData(string name, float value, bool save = false)
    {
        foreach (DataFloat dataFloat in floats)
        {
            if (dataFloat.name == name)
            {
                dataFloat.value = value;

                Debug.Log("Set float data value \"" + name + "\" to \"" + value + "\".");

                if (save)
                {
                    Write();
                }

                return;
            }
        }

        floats.Add(new DataFloat(name, value));

        Debug.Log("Added float data value \"" + name + "\" with a value of \"" + value + "\".");

        if (save)
        {
            Write();
        }

        return;
    }

    /// <summary>
    /// Saves an data value.
    /// </summary>
    /// <param name="name">The name of the value you are trying to save.</param>
    /// <param name="value">What value should be saved under this name?</param>
    /// <param name="save">Should Read Write Save Manager save the value?</param>
    public void SetData(string name, string value, bool save = false)
    {
        foreach (DataString dataString in strings)
        {
            if (dataString.name == name)
            {
                dataString.value = value;

                Debug.Log("Set string data value \"" + name + "\" to \"" + value + "\".");

                if (save)
                {
                    Write();
                }

                return;
            }
        }

        strings.Add(new DataString(name, value));

        Debug.Log("Added string data value \"" + name + "\" with a value of \"" + value + "\".");

        if (save)
        {
            Write();
        }

        return;
    }
    #endregion
}