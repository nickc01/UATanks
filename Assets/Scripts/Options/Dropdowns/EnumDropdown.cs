using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using TMPro;

public abstract class EnumDropDown<E> : MonoBehaviour where E : Enum
{
    TMP_Dropdown dropdown; //The dropdown component
    string[] enumNames; //The list of all the names in the enum
    Array enumValues; //The list of all the values in the enum
    Type EnumType; //The enum's type
    bool loaded = false; //Whether this object has been loaded or not

    protected void Start()
    {
        if (!loaded)
        {
            Load();
        }
    }

    protected virtual void Load()
    {
        loaded = true;
        //Get the base stats and components
        EnumType = typeof(E);
        dropdown = GetComponent<TMP_Dropdown>();
        enumNames = EnumType.GetEnumNames();
        enumValues = EnumType.GetEnumValues();
        //Clear the existing options from the list
        dropdown.ClearOptions();
        //Add a function that is called when the dropdown value is updated
        dropdown.onValueChanged.AddListener(newVal => 
        {
            OnDropdownUpdate((E)enumValues.GetValue(dropdown.value));
        });
        //Create a list of new options
        List<string> newOptions = new List<string>();
        for (int i = 0; i < enumNames.Length; i++)
        {
            newOptions.Add(enumNames[i].Clean());
        }
        dropdown.AddOptions(newOptions);
    }

    //If the dropdown is updated
    protected virtual void OnDropdownUpdate(E newValue)
    {
        //Updates the stored value
        Value = newValue;
    }

    public virtual E Value
    {
        get
        {
            if (!loaded)
            {
                Load();
            }
            //Get the current value of the dropdown
            return (E)enumValues.GetValue(dropdown.value);
        }
        set
        {
            if (!loaded)
            {
                Load();
            }
            int index = 0;
            for (int i = 0; i < enumValues.Length; i++)
            {
                if (enumValues.GetValue(i).Equals(value))
                {
                    index = i;
                    break;
                }
            }
            //Set the current value of the dropdown
            dropdown.value = index;
        }
    }
}

public abstract class SavedEnumDropdown<E> : EnumDropDown<E> where E : Enum
{
    public abstract E DefaultValue { get; } //The default value of the saved value if there is no saved value stored yet
    public string SaveID = typeof(E).FullName; //The save ID used for playerPrefs
    bool isLoaded = false; //Whether this object is loaded or not
    SavedValue<E> saved; //The saved value

    protected override void Load()
    {
        base.Load();
        //Create the saved value
        saved = new SavedValue<E>(SaveID,DefaultValue);
        isLoaded = true;
        //Retrieve the currently saved value
        base.Value = saved.Value;
    }

    public override E Value
    {
        get
        {
            if (!isLoaded)
            {
                Load();
            }
            //Get the saved value
            return base.Value;
        }
        set
        {
            if (!isLoaded)
            {
                Load();
            }
            //Set the saved value
            saved.Value = value;
            base.Value = value;
        }
    }
}
