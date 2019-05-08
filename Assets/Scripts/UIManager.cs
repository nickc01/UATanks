using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager Singleton; //The singleton for the UI Manager
    [SerializeField] string defaultState = "Game"; //The starting UI State for the manager
    public static string CurrentState { get; private set; } //The current state of the UI

    static Dictionary<string, GameObject> validStates = new Dictionary<string, GameObject>(); //A list of possible UI States

    // Start is called before the first frame update
    void Start()
    {
        //Set the singleton
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //Get all valid UI States
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            validStates.Add(child.name, child.gameObject);
        }
        //Set the current state to the default one
        SetUIState(defaultState);
    }

    public static void SetUIState(string newState)
    {
        //Disable all states
        foreach (var state in validStates)
        {
            state.Value.SetActive(false);
        }
        //If the newState is valid
        if (validStates.ContainsKey(newState))
        {
            //Set it active
            validStates[newState].SetActive(true);
            CurrentState = newState;
        }
        //If it is not valid
        else
        {
            //Throw an exception
            throw new System.Exception(newState + " is not a valid state for the UI");
        }
    }
}
