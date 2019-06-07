using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Collections;

public class SavedInputField : MonoBehaviour
{
    [SerializeField] protected Vector2Int InputRange; //The min and max value that the input field value must be contained within
    [SerializeField] protected int DefaultValue; //The default value if there is no saved value yet
    [SerializeField] protected string SaveID; //The save id for playerprefs
    TMP_InputField inputField; //The input field component
    SavedValue<int> save; //The saved value
    bool loaded = false; //Whether this object is loaded or not

    private void Start()
    {
        Load();
    }

    private void OnEnable()
    {
        Load();
        StartCoroutine(RefreshRoutine());
    }

    //Used to update the text area to prevent odd text placement at startup
    IEnumerator RefreshRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        inputField.ForceLabelUpdate();
    }

    //Called when the input field is updated
    private void NewValue(string str)
    {
        //If the value inputted is a valid number
        if (int.TryParse(str,out var result))
        {
            //Updated the saved value
            result = Mathf.Clamp(result, InputRange.x, InputRange.y);
            save.Value = result;
            inputField.text = result.ToString();
        }
        else
        {
            //Set it to zero
            save.Value = 0;
            inputField.text = 0.ToString();
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
            //Get the input field component
            inputField = GetComponent<TMP_InputField>();
            //Create the saved value object
            save = new SavedValue<int>(SaveID, DefaultValue);
            //Retrive the currently saved value
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
            //Get the saved value
            return save.Value;
        }
        set
        {
            Load();
            //Set the saved value
            var NewValue = Mathf.Clamp(value, InputRange.x, InputRange.y);
            save.Value = NewValue;
            inputField.text = NewValue.ToString();
        }
    }
}
