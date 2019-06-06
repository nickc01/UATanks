using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class SavedInputField : MonoBehaviour
{
    [SerializeField] protected Vector2Int InputRange;
    [SerializeField] protected int DefaultValue;
    [SerializeField] protected string SaveID;
    TMP_InputField inputField;
    SavedValue<int> save;
    bool loaded = false;

    private void Start()
    {
        Load();
    }

    private void NewValue(string str)
    {
        if (int.TryParse(str,out var result))
        {
            result = Mathf.Clamp(result, InputRange.x, InputRange.y);
            save.Value = result;
            inputField.text = result.ToString();
        }
        else
        {
            save.Value = 0;
        }
    }

    private void Load()
    {
        if (!loaded)
        {
            loaded = true;
            DefaultValue = Mathf.Clamp(DefaultValue, InputRange.x, InputRange.y);
            if (SaveID == null || SaveID == "")
            {
                SaveID = GetType().FullName;
            }
            inputField = GetComponent<TMP_InputField>();
            save = new SavedValue<int>(SaveID, DefaultValue);
            inputField.text = save.Value.ToString();
            inputField.onEndEdit.AddListener(str =>
            {
                NewValue(str);
            });
        }
    }

    public int Value
    {
        get
        {
            Load();
            return save.Value;
        }
        set
        {
            Load();
            var NewValue = Mathf.Clamp(value, InputRange.x, InputRange.y);
            save.Value = NewValue;
            inputField.text = NewValue.ToString();
        }
    }
}
