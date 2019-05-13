using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Extensions;

public class ObstacleAvoidance : MonoBehaviour
{
    public float CurrentThreshold { get; private set; } //The current obstacle threshold
    public float DirectionalThreshold { get; private set; } //A score determining the direction the obstacles are
                                                            //A positive score indicates the obstacles are to the right
                                                            //A negative score indicates the obstacles are to the left
    //public bool ObstacleInWay => CurrentThreshold >= MaxObstacleThreshhold; //Whether there is an obstacle in the way
    public float RecommendedDirection => -180f / 15f * DirectionalThreshold; //The recommended direction to turn and how much

    public int WhiskerAmount => whiskerAmount; //The public interface for accessing the amount of whiskers
    public float WhiskerFOV => whiskerFOV; //The public interface for accessing the whisker FOV
    public float WhiskerLength => whiskerLength; //The public interface for accessing the Whisker Length
    public bool ObstacleInWay => Distances.Any(d => d < DistanceThreshold); //Returns true if there is an obstacle in the way

    [Header("Stats")]
    [Tooltip(@"How close an obstacle has to be to the enemy before the enemy reacts to it")]
    [SerializeField] private float DistanceThreshold = 4f;
    [Tooltip(@"The amount of whiskers to use for obstacle avoidance.
             The more, the more accurate the obstacle avoidance is, but the more processing power it takes")]
    [SerializeField] private int whiskerAmount = 5;
    [Tooltip("How long the whiskers will be")]
    [SerializeField] private float whiskerLength = 5f;
    [Tooltip("The Field of view the whiskers will cover")]
    [Range(0f,180f)]
    [SerializeField] private float whiskerFOV = 90f;
    //[Tooltip("The height offset of the whiskers")]
    //[SerializeField] private float HeightOffset = 0.8f;
    [Tooltip("The layers the whiskers will consider an obstacle")]
    [SerializeField] private LayerMask layers;
    [Header("Debug")]
    [Tooltip("Whether to display the whisker lines or not")]
    [SerializeField] private bool DebugWhiskers = false;

    private int whiskerAmountInternal;
    //The whiskers being used
    private List<(float Direction, float Sensitivity)> Whiskers = new List<(float, float)>();
    private List<float> Distances = new List<float>();

    private float EnemyDirection => -transform.eulerAngles.y; //Returns the direction of the enemy in degrees

    private void Start()
    {
        UpdateWhiskers();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + (DegToVector(0) * 10f),Color.black);
        if (whiskerAmount != whiskerAmountInternal)
        {
            UpdateWhiskers();
        }
        var currentDirection = EnemyDirection;
        if (DebugWhiskers)
        {
            foreach (var whisker in Whiskers)
            {
                //Debug.Log("DRAWING");
                DebugDraw.DrawFOV(new Vector3(transform.position.x,transform.position.y,transform.position.z), whiskerLength, whisker.Direction + currentDirection,0f, Color.green);
            }
            //Debug.Log("END");
        }
        CurrentThreshold = 0;
        DirectionalThreshold = 0;
        int validCollisions = 0;
        for (int i = 0; i < Whiskers.Count; i++)
        {
            var whisker = Whiskers[i];
            Physics.Raycast(transform.position, DegToVector(whisker.Direction + EnemyDirection), out var hitInfo, whiskerLength);
            //Debug.Log("HITFIRST = " + (hitInfo.collider != null));
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), DegToVector(currentDirection + whisker.Direction), out var hit, whiskerLength, layers.value) && hit.transform != null)
            {
                Debug.DrawLine(hit.point, hit.point - hit.normal,Color.magenta);
                Debug.Log("Angle = " + Vector3.SignedAngle(transform.forward, -hit.normal, Vector3.up));
                validCollisions++;
                var directionModifier = Vector3.SignedAngle(transform.forward, -hit.normal, Vector3.up).ToSignOnly();
                //Debug.Log("HIT");
                Debug.Log("SENSITIVITY = " + whisker.Sensitivity * directionModifier);
                //var score = 1 - ((hit.distance / whiskerLength) * Mathf.Abs(whisker.Sensitivity));
                Debug.Log("DISTANCE = " + hit.distance);
                //var score = (whiskerLength - hit.distance) / whiskerLength * Mathf.Abs(whisker.Sensitivity);
                var score = (1f / hit.distance) * Mathf.Abs(whisker.Sensitivity);
                Debug.Log("SCORE = " + score);
                CurrentThreshold = Mathf.Max(CurrentThreshold, score);
                //var directionalScore = 1 - ((hit.distance / whiskerLength) * whisker.Sensitivity);
                var directionalScore = (1f / hit.distance) * whisker.Sensitivity * directionModifier;
                //var directionalScore = (whiskerLength - hit.distance) / whiskerLength * whisker.Sensitivity;
                DirectionalThreshold = MostExtreme(DirectionalThreshold, directionalScore);
                //Debug.Log("DIRECTIONAL SCORE = " + directionalScore);
                Distances[i] = hit.distance;
            }
            else
            {
                Distances[i] = float.PositiveInfinity;
            }
        }
        /*if (validCollisions != 0)
        {
            DirectionalThreshold /= validCollisions;
        }*/
        CurrentThreshold = (CurrentThreshold / whiskerLength) * 100f;
        DirectionalThreshold = (DirectionalThreshold / whiskerLength) * 100f;
        //CurrentThreshold /= whiskerAmount;
    }

    public bool CanTurnInDirection(float degrees)
    {
        if (degrees < 0f)
        {
            return CanTurnLeft();
        }
        else if (degrees > 0f)
        {
            return CanTurnRight();
        }
        else
        {
            Debug.Log("END of TEST");
            return Distances[Mathf.FloorToInt(Distances.Count / 2f)] > DistanceThreshold;
        }
    }

    public bool CanTurnLeft()
    {
        int MidPoint = Mathf.FloorToInt(Distances.Count / 2f);
        for (int i = 0; i < MidPoint; i++)
        {
            if (Distances[i] < DistanceThreshold)
            {
                return false;
            }
        }
        Debug.Log("DONE LEFT");
        return true;
    }

    public bool CanTurnRight()
    {
        int MidPoint = Mathf.FloorToInt(Distances.Count / 2f);
        for (int i = Distances.Count - 1; i > MidPoint; i--)
        {
            if (Distances[i] < DistanceThreshold)
            {
                return false;
            }
        }
        Debug.Log("DONE RIGHT");
        return true;
    }

    //Returns true if there is no obstacle in the set direction
    public bool OpenInDirectionOLD(float degrees)
    {
        Debug.Log("DEGREES = " + degrees);
        if (degrees < Whiskers[0].Direction || degrees > Whiskers[Whiskers.Count - 1].Direction)
        {
            return true;
            //throw new Exception("The angle of " + degrees + " does not fit within the range of [" + Whiskers[0].Direction + " - " + Whiskers[Whiskers.Count - 1].Direction + "]");
        }
        int lowWhisker = -1;
        int highWhisker = -1;
        for (int i = 0; i < Whiskers.Count - 1; i++)
        {
            if (degrees >= Whiskers[i].Direction && degrees <= Whiskers[i + 1].Direction)
            {
                lowWhisker = i;
                highWhisker = i + 1;
                break;
            }
        }
        Debug.Log("MIN = " + lowWhisker);
        Debug.Log("MAX = " + highWhisker);
        var t = Mathf.InverseLerp(Whiskers[lowWhisker].Direction, Whiskers[highWhisker].Direction, degrees);
        Debug.Log("T = " + t);
        var finalDistance = Mathf.Lerp(Distances[lowWhisker],Distances[highWhisker],t);
        Debug.Log("FINAL DISTANCE = " + finalDistance);
        Debug.Log("DISTANCE THRESHOLD = " + DistanceThreshold);
        return float.IsNaN(finalDistance) || finalDistance > DistanceThreshold;
    }

    //Finds the value that is farthest from zero, whether positive or negative
    private static float MostExtreme(float A, float B) => Mathf.Abs(A) > Mathf.Abs(B) ? A : B;

    //Inverts the value by doing 1 - value. Also works for negative numbers
    private static float Invert(float A) => A > 0f ? 1 - A : -A - 1;

    private static float SensitivityTransform(float input) => 1 - Mathf.Abs(Mathf.Cos((input + 1) * (Mathf.PI / 2)));

    //Run when the amount of whiskers change
    void UpdateWhiskers()
    {
        whiskerAmountInternal = whiskerAmount;
        Whiskers.Clear();
        Distances.Clear();
        //float currentDirection = EnemyDirection;
        float LeftDegrees = -(whiskerFOV / 2);
        float RightDegrees = (whiskerFOV / 2);
        for (int i = 0; i < whiskerAmount; i++)
        {
            float newWhiskerDirection = Mathf.Lerp(LeftDegrees, RightDegrees, i / (float)(whiskerAmount - 1));
            //float sensitivity = Mathf.Cos((newWhiskerDirection / 90f) * Mathf.PI / 2f);
            float sensitivity = SensitivityTransform(newWhiskerDirection / 90f);
            //float sensitivity = 1;
            /*if (newWhiskerDirection > 0f)
            {
                sensitivity = -sensitivity;
            }*/
            Whiskers.Add((newWhiskerDirection,sensitivity));
            Distances.Add(float.PositiveInfinity);
        }
        Whiskers.Print();
    }
    
}
