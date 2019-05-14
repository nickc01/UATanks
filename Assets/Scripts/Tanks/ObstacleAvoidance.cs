using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Extensions;

public class ObstacleAvoidance : MonoBehaviour
{
    public float DirectionalThreshold { get; private set; } //A score that determines the best direction to take to move away from the obstacles
    public float RecommendedDirection //The recommended direction to take to avoid the obstacles
    {
        get
        {
            //If all the whiskers are triggering, then that most likely means that the tank is entering a corner
            if (Distances.Average() <= whiskerLength)
            {
                //Attempt to rotate away from the corner to prevent the tank from getting stuck in there
                return -5f;
            }
            else
            {
                //Rotate away from the obstacle
                return -6f / 15f * DirectionalThreshold;
            }
        }
    }

    public int WhiskerAmount => whiskerAmount; //The public interface for accessing the amount of whiskers
    public float WhiskerFOV => whiskerFOV; //The public interface for accessing the whisker FOV
    public float WhiskerLength => whiskerLength; //The public interface for accessing the Whisker Length

    [Header("Stats")]
    [Tooltip("The amount of whiskers to use for obstacle avoidance")]
    [SerializeField] private int whiskerAmount = 5;
    [Tooltip("How long the whiskers will be")]
    [SerializeField] private float whiskerLength = 5f;
    [Tooltip("The Field of view the whiskers will cover")]
    [Range(0f,180f)]
    [SerializeField] private float whiskerFOV = 90f;
    [Tooltip("The layers the whiskers will consider an obstacle")]
    [SerializeField] private LayerMask layers;
    [Header("Debug")]
    [Tooltip("Whether to display the whisker lines or not")]
    [SerializeField] private bool DebugWhiskers = false;

    private int whiskerAmountInternal;
    private List<(float Direction, float Sensitivity)> Whiskers = new List<(float, float)>(); //The whiskers that are being used
    private List<float> Distances = new List<float>(); //The last known distance of each whisker to an obstacle

    private float EnemyDirection => -transform.eulerAngles.y; //Returns the direction of the enemy in degrees

    private void Start()
    {
        //Create the whiskers
        UpdateWhiskers();
    }

    private void Update()
    {
        if (whiskerAmount != whiskerAmountInternal)
        {
            UpdateWhiskers();
        }
        var currentDirection = EnemyDirection;
        if (DebugWhiskers)
        {
            foreach (var whisker in Whiskers)
            {
                DebugDraw.DrawFOV(new Vector3(transform.position.x,transform.position.y,transform.position.z), whiskerLength, whisker.Direction + currentDirection,0f, Color.green);
            }
        }
        DirectionalThreshold = 0;
        int validCollisions = 0;
        for (int i = 0; i < Whiskers.Count; i++)
        {
            var whisker = Whiskers[i];
            if (Physics.Raycast(transform.position, DegToVector(currentDirection + whisker.Direction), out var hit, whiskerLength, layers.value) && hit.transform != null)
            {
                validCollisions++;
                var directionModifier = Vector3.SignedAngle(transform.forward, -hit.normal, Vector3.up).ToSignOnly();

                //Get how perpendicular to the wall the ray is. 0 is perfectly parallel and 1 is perfectly perpendicular
                var perpendicularity = Mathf.Abs(Vector3.Cross(-hit.normal,(hit.point - transform.position).normalized).magnitude);

                //Create a final modifier with the sensitivity and the perpendicularity to judge the score of the whisker
                var modifier = whisker.Sensitivity * perpendicularity;

                var directionalScore = (1f / hit.distance) * modifier * directionModifier;
                DirectionalThreshold = MostExtreme(DirectionalThreshold, directionalScore);
                Distances[i] = hit.distance;
            }
            else
            {
                Distances[i] = float.PositiveInfinity;
            }
        }
        DirectionalThreshold = (DirectionalThreshold / whiskerLength) * 100f;
    }

    //Finds the value that is farthest from zero, whether positive or negative
    private static float MostExtreme(float A, float B) => Mathf.Abs(A) > Mathf.Abs(B) ? A : B;

    //Inverts the value by doing 1 - value. Also works for negative numbers
    private static float Invert(float A) => A > 0f ? 1 - A : -A - 1;

    private static float SensitivityTransform(float input) => 1 - Mathf.Abs(Mathf.Cos((input + 1) * (Mathf.PI / 2)));

    //Creates a set of whiskers
    //Run when the amount of whiskers change
    void UpdateWhiskers()
    {
        whiskerAmountInternal = whiskerAmount;
        //Clear the whisker and distance list
        Whiskers.Clear();
        Distances.Clear();
        float LeftDegrees = -(whiskerFOV / 2);
        float RightDegrees = (whiskerFOV / 2);
        for (int i = 0; i < whiskerAmount; i++)
        {
            float newWhiskerDirection = Mathf.Lerp(LeftDegrees, RightDegrees, i / (float)(whiskerAmount - 1));
            float sensitivity = SensitivityTransform(newWhiskerDirection / 90f);
            Whiskers.Add((newWhiskerDirection,sensitivity));
            Distances.Add(float.PositiveInfinity);
        }
        //Whiskers.Print();
    }
    
}
