﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : PlayerSpecific
{
    //public static UIManager Singleton { get; private set; } //The singleton for the UI Manager
    public static UIManager Primary => MultiplayerManager.Primary.Manager;
    [SerializeField] string defaultState = "Game"; //The starting UI State for the manager
    public AnimationCurve SmoothCurve; //The curve used to create smooth transitions
    public AnimationCurve ReadyScreenCurve; //The curve used for the ready screen transitions
    public string CurrentState { get; private set; } //The current state of the UI

    private Canvas UICanvas;

    Dictionary<string, GameObject> validStates = new Dictionary<string, GameObject>(); //A list of possible UI States

    bool started = false;

    public bool ButtonsEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        if (started)
        {
            return;
        }
        started = true;
        UICanvas = GetComponent<Canvas>();
        //MultiplayerManager.AddedPlayersUpdate += PlayerCountChanged;
        //Get all valid UI States
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            validStates.Add(child.name, child.gameObject);
        }
        //Set the current state to the default one
        SetUIState(defaultState);
        OnNewPlayerChange();
    }

    public override void OnNewPlayerChange()
    {
        Debug.Log("TEST = " + PlayerID);
        gameObject.layer = LayerMask.NameToLayer("UIPlayer" + PlayerID);
        UICanvas.worldCamera = MultiplayerManager.GetPlayerSpecifics(PlayerID).Camera.CameraComponent;
    }

    /*private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            MultiplayerManager.AddedPlayersUpdate -= PlayerCountChanged;
        }
    }*/

    public static void SetUIStateAll(string newState)
    {
        foreach (var specific in MultiplayerManager.GetAllSpecifics())
        {
            specific.Manager.SetUIState(newState);
        }
    }

    public void SetUIState(string newState)
    {
        if (!started)
        {
            defaultState = newState;
            return;
        }
        //Disable all states
        foreach (var state in validStates)
        {
            state.Value.SetActive(false);
        }
        //If the newState is valid
        if (validStates.ContainsKey(newState))
        {
            //Set it active and reset it's position
            validStates[newState].SetActive(true);
            validStates[newState].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            CurrentState = newState;
        }
        //If it is not valid
        else
        {
            //Throw an exception
            throw new System.Exception(newState + " is not a valid state for the UI");
        }
    }

    string transitionFrom; //The state to transition from
    string transitionTo; //The state to transition to
    Coroutine TransitionRoutine; //The transition routine

    //Sets the UI State with transitioning
    public void SetUIState(string newState, AnimationCurve transitionCurve = null, TransitionMode mode = TransitionMode.TopToBottom, float Speed = 2f,bool FromIsHidden = false)
    {
        if (!started)
        {
            defaultState = newState;
            return;
        }
        //If there is no transition set
        if (transitionCurve == null)
        {
            //Swap to the new state normally
            SetUIState(newState);
        }
        //If there is a transition going on right now, finish it
        FinishTransition();
        //If the newState is valid
        if (!validStates.ContainsKey(newState))
        {
            throw new System.Exception(newState + " is not a valid state");
        }
        //Set the from and to states
        transitionFrom = CurrentState;
        transitionTo = newState;
        //Star the transition routine
        TransitionRoutine = CoroutineManager.StartCoroutine(SetUIStateRoutine(transitionCurve,mode,Speed,FromIsHidden));
    }

    public static void SetUIStateAll(string newState, AnimationCurve transitionCurve = null, TransitionMode mode = TransitionMode.TopToBottom, float Speed = 2f, bool FromIsHidden = false)
    {
        foreach (var specific in MultiplayerManager.GetAllSpecifics())
        {
            specific.Manager.SetUIState(newState,transitionCurve,mode,Speed,FromIsHidden);
        }
    }

    private void FinishTransition()
    {
        //Stop the transition routine if it's running
        if (TransitionRoutine != null)
        {
            CoroutineManager.StopCoroutine(TransitionRoutine);
            TransitionRoutine = null;
        }
        if (transitionFrom != null)
        {
            //Reenable the buttons
            foreach (var button in validStates[transitionFrom].GetComponentsInChildren<Button>())
            {
                button.enabled = true;
            }
            //Hide the state
            validStates[transitionFrom].SetActive(false);
            transitionFrom = null;
        }
        if (transitionTo != null)
        {
            //Reset the state's position
            validStates[transitionTo].GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            CurrentState = transitionTo;
            transitionTo = null;
        }
    }

    private IEnumerator SetUIStateRoutine(AnimationCurve transitionType,TransitionMode mode,float Speed,bool FromIsHidden)
    {
        float T = 0f;
        RectTransform From = validStates[transitionFrom].GetComponent<RectTransform>();
        RectTransform To = validStates[transitionTo].GetComponent<RectTransform>();
        if (FromIsHidden)
        {
            From.gameObject.SetActive(false);
        }
        else
        {
            //Deactivate the buttons on the from side, to prevent potential issues
            foreach (var button in validStates[transitionFrom].GetComponentsInChildren<Button>())
            {
                button.enabled = false;
            }
            From.gameObject.SetActive(true);
        }

        To.gameObject.SetActive(true);

        while (T < 1f)
        {
            //Increment the transition timer
            T += Time.deltaTime * Speed;
            //Get the camera bounds
            (var Width, var Height) = CameraController.GetCameraBounds();
            //Interpolate the states to transition between them, based on the transition mode set
            switch (mode)
            {
                case TransitionMode.TopToBottom:
                    From.anchoredPosition = Vector2.LerpUnclamped(Vector2.zero, new Vector2(0f, -Height), transitionType.Evaluate(T));
                    To.anchoredPosition = Vector2.LerpUnclamped(new Vector2(0f, Height), Vector2.zero, transitionType.Evaluate(T));
                    break;
                case TransitionMode.BottomToTop:
                    From.anchoredPosition = Vector2.LerpUnclamped(Vector2.zero, new Vector2(0f, Height), transitionType.Evaluate(T));
                    To.anchoredPosition = Vector2.LerpUnclamped(new Vector2(0f, -Height), Vector2.zero, transitionType.Evaluate(T));
                    break;
                case TransitionMode.LeftToRight:
                    From.anchoredPosition = Vector2.LerpUnclamped(Vector2.zero, new Vector2(Width, 0f), transitionType.Evaluate(T));
                    To.anchoredPosition = Vector2.LerpUnclamped(new Vector2(-Width,0f), Vector2.zero, transitionType.Evaluate(T));
                    break;
                case TransitionMode.RightToLeft:
                    From.anchoredPosition = Vector2.LerpUnclamped(Vector2.zero, new Vector2(-Width, 0f), transitionType.Evaluate(T));
                    To.anchoredPosition = Vector2.LerpUnclamped(new Vector2(Width, 0f), Vector2.zero, transitionType.Evaluate(T));
                    break;
                default:
                    goto case TransitionMode.TopToBottom;
            }
            yield return null;
        }
        TransitionRoutine = null;
        //Finish the transition
        FinishTransition();

    }
}
