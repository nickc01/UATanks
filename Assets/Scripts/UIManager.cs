using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Curves
{
    public static AnimationCurve Smooth => UIManager.Singleton.SmoothCurve;
    public static AnimationCurve ReadyCurve => UIManager.Singleton.ReadyScreenCurve;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton { get; private set; } //The singleton for the UI Manager
    [SerializeField] string defaultState = "Game"; //The starting UI State for the manager
    public AnimationCurve SmoothCurve;
    public AnimationCurve ReadyScreenCurve;
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

    static string transitionFrom;
    static string transitionTo;
    static Coroutine TransitionRoutine;

    public static void SetUIState(string newState, AnimationCurve transitionCurve = null, TransitionMode mode = TransitionMode.TopToBottom, float Speed = 2f,bool FromIsHidden = false)
    {
        if (transitionCurve == null)
        {
            SetUIState(newState);
        }
        FinishTransition();
        //If the newState is valid
        if (!validStates.ContainsKey(newState))
        {
            throw new System.Exception(newState + " is not a valid state");
        }
        transitionFrom = CurrentState;
        transitionTo = newState;
        TransitionRoutine = CoroutineManager.StartCoroutine(SetUIStateRoutine(transitionCurve,mode,Speed,FromIsHidden));
    }

    private static void FinishTransition()
    {
        if (TransitionRoutine != null)
        {
            CoroutineManager.StopCoroutine(TransitionRoutine);
            TransitionRoutine = null;
        }
        if (transitionFrom != null)
        {
            foreach (var button in validStates[transitionFrom].GetComponentsInChildren<Button>())
            {
                button.enabled = true;
            }
            validStates[transitionFrom].SetActive(false);
            transitionFrom = null;
        }
        if (transitionTo != null)
        {
            validStates[transitionTo].GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            CurrentState = transitionTo;
            transitionTo = null;
        }
    }

    private static IEnumerator SetUIStateRoutine(AnimationCurve transitionType,TransitionMode mode,float Speed,bool FromIsHidden)
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
            foreach (var button in validStates[transitionFrom].GetComponentsInChildren<Button>())
            {
                button.enabled = false;
            }
            From.gameObject.SetActive(true);
        }

        To.gameObject.SetActive(true);

        while (T < 1f)
        {
            T += Time.deltaTime * Speed;
            (var Width, var Height) = CameraController.GetCameraBounds();
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
        FinishTransition();

    }

    public static bool InvisibilityActivated = false;

    public static float EnemyDeltaTime()
    {
        if (InvisibilityActivated == true)
        {
            //Slow the enemies by half the speed
            return Time.deltaTime / 2f;
        }
        else
        {
            //Go at regular speed
            return Time.deltaTime;
        }
    }

}
