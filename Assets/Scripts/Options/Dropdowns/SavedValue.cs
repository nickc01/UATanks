using UnityEngine;
using Newtonsoft.Json;

public class SavedValue<valType>
{
    string SaveID;
    bool loaded = false;
    valType valueInternal;

    public SavedValue(string saveID)
    {
        SaveID = saveID;
    }

    private valType Load()
    {
        loaded = true;
        if (valueInternal is int)
        {
            return (valType)(object)PlayerPrefs.GetInt(SaveID);
        }
        else if (valueInternal is float)
        {
            return (valType)(object)PlayerPrefs.GetFloat(SaveID);
        }
        else if (valueInternal is string)
        {
            return (valType)(object)PlayerPrefs.GetString(SaveID);
        }
        else
        {
            return JsonConvert.DeserializeObject<valType>(PlayerPrefs.GetString(SaveID));
        }
    }

    private void Save(valType value)
    {
        if (value is int intValue)
        {
            PlayerPrefs.SetInt(SaveID,intValue);
        }
        else if (value is float floatValue)
        {
            PlayerPrefs.SetFloat(SaveID, floatValue);
        }
        else if (value is string stringValue)
        {
            PlayerPrefs.SetString(SaveID, stringValue);
        }
        else
        {
            Debug.Log("SAVE VALUE = " + JsonConvert.SerializeObject(value));
            PlayerPrefs.SetString(SaveID, JsonConvert.SerializeObject(value));
        }
    }

    public valType Value
    {
        get
        {
            if (!loaded)
            {
                valueInternal = Load();
            }
            return valueInternal;
        }
        set
        {
            valueInternal = value;
            Save(value);
        }
    }

}
