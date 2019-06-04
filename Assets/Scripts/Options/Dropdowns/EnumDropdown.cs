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
    TMP_Dropdown dropdown;
    string[] enumNames;
    Array enumValues;
    Type EnumType;
    Type UnderlyingType;
    bool loaded = false;

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
        EnumType = typeof(E);
        dropdown = GetComponent<TMP_Dropdown>();
        enumNames = EnumType.GetEnumNames();
        enumValues = EnumType.GetEnumValues();
        UnderlyingType = EnumType.GetEnumUnderlyingType();
        dropdown.ClearOptions();
        dropdown.onValueChanged.AddListener(newVal => 
        {
            OnDropdownUpdate((E)enumValues.GetValue(dropdown.value));
        });
        List<string> newOptions = new List<string>();
        for (int i = 0; i < enumNames.Length; i++)
        {
            newOptions.Add(enumNames[i].Clean());
        }
        dropdown.AddOptions(newOptions);
    }

    protected virtual void OnDropdownUpdate(E newValue)
    {
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
            dropdown.value = index;
        }
    }
}

public abstract class SavedEnumDropdown<E> : EnumDropDown<E> where E : Enum
{
    public string SaveID = typeof(E).FullName;
    bool isLoaded = false;
    SavedValue<E> saved;

    protected override void Load()
    {
        base.Load();
        saved = new SavedValue<E>(SaveID);
        isLoaded = true;
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
            return base.Value;
        }
        set
        {
            if (!isLoaded)
            {
                Load();
            }
            saved.Value = value;
            base.Value = value;
        }
    }
}
