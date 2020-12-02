using UnityEngine;
using Newtonsoft.Json;

public class SavedValue<valType>
{
    string SaveID; //The save ID for playerprefs
    bool loaded = false; //Whether the saved value has been loaded
    valType valueInternal; //The internal storage value
    valType DefaultValue; //The default value

    public SavedValue(string saveID, valType defaultValue)
    {
        //Set the saveID and default value
        SaveID = saveID;
        DefaultValue = defaultValue;
    }

    private valType Load()
    {
        loaded = true;
        //Load the saved value based on the type of valType
        if (!PlayerPrefs.HasKey(SaveID))
        {
            return DefaultValue;
        }
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
            var str = PlayerPrefs.GetString(SaveID);
            if (str == null || str == "")
            {
                return default;
            }
            else
            {
                return JsonConvert.DeserializeObject<valType>(PlayerPrefs.GetString(SaveID));
            }
        }
    }

    private void Save(valType value)
    {
        //Save the value based on the type of valType
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
            //Gets the internal value
            return valueInternal;
        }
        set
        {
            //Sets the internal value
            valueInternal = value;
            Save(value);
        }
    }

}
