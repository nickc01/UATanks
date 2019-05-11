using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{
    public float CurrentThreshold { get; private set; } //The current obstacle threshold
    public float DirectionalThreshold { get; private set; } //A score determining the direction the obstacles are
                                                            //A positive score indicates the obstacles are to the right
                                                            //A negative score indicates the obstacles are to the left
    public bool ObstacleInWay => CurrentThreshold >= MaxObstacleThreshhold; //Whether there is an obstacle in the way

    public int WhiskerAmount => whiskerAmount; //The public interface for accessing the amount of whiskers
    public float WhiskerFOV => whiskerFOV; //The public interface for accessing the whisker FOV
    public float WhiskerLength => whiskerLength; //The public interface for accessing the Whisker Length

    [Header("Stats")]
    [Tooltip(@"A percentage of how close to an obstacle the enemy has to be before it can react.
               The higher the number, the closer the enemy can get to an obstacle")]
    [SerializeField] private float MaxObstacleThreshhold = 0.6f;
    [Tooltip(@"The amount of whiskers to use for obstacle avoidance.
             The more, the more accurate the obstacle avoidance is, but the more processing power it takes")]
    [SerializeField] private int whiskerAmount = 5;
    [Tooltip("How long the whiskers will be")]
    [SerializeField] private float whiskerLength = 5f;
    [Tooltip("The Field of view the whiskers will cover")]
    [Range(0f,180f)]
    [SerializeField] private float whiskerFOV = 90f;
    [Tooltip("The height offset of the whiskers")]
    [SerializeField] private float HeightOffset = 0.8f;
    [Tooltip("The layers the whiskers will consider an obstacle")]
    [SerializeField] private LayerMask layers;
    [Header("Debug")]
    [Tooltip("Whether to display the whisker lines or not")]
    [SerializeField] private bool DebugWhiskers = false;

    private int whiskerAmountInternal;
    //The whiskers being used
    private List<(float Direction, float Sensitivity)> Whiskers = new List<(float, float)>();

    private float EnemyDirection => -transform.eulerAngles.y + 90f; //Returns the direction of the enemy in degrees

    private void Start()
    {
        UpdateWhiskers();
    }

    //Converts a degrees rotation to a unit vector
    private Vector3 DegToVector(float degrees)
    {
        return new Vector3(Mathf.Cos(degrees * Mathf.Deg2Rad),0f,Mathf.Sin(degrees * Mathf.Deg2Rad));
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
                Debug.Log("DRAWING");
                DebugDraw.DrawFOV(new Vector3(transform.position.x,transform.position.y + HeightOffset,transform.position.z), whiskerLength, whisker.Direction + currentDirection,0f, Color.green);
            }
            Debug.Log("END");
        }
        CurrentThreshold = 0;
        DirectionalThreshold = 0;
        foreach (var whisker in Whiskers)
        {
            Physics.Raycast(transform.position, DegToVector(whisker.Direction + EnemyDirection), out var hitInfo, whiskerLength);
            Debug.Log("HITFIRST = " + (hitInfo.collider != null));
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + HeightOffset, transform.position.z), DegToVector(currentDirection + whisker.Direction), out var hit, whiskerLength, layers.value) && hit.transform != null)
            {
                Debug.Log("HIT");
                var score = 1 - ((hit.distance / whiskerLength) * Mathf.Abs(whisker.Sensitivity));
                CurrentThreshold += score;
                var directionalScore = 1 - ((hit.distance / whiskerLength) * whisker.Sensitivity);
                DirectionalThreshold += directionalScore;
            }
        }
        Debug.Log("THRESHOLD = " + CurrentThreshold);
        Debug.Log("DIRECTIONAL THRESHOLD  = " + DirectionalThreshold);
    }

    //Run when the amount of whiskers change
    void UpdateWhiskers()
    {
        whiskerAmountInternal = whiskerAmount;
        Whiskers.Clear();
        //float currentDirection = EnemyDirection;
        float LeftDegrees = -(whiskerFOV / 2);
        float RightDegrees = (whiskerFOV / 2);
        for (int i = 0; i < whiskerAmount; i++)
        {
            float newWhiskerDirection = Mathf.Lerp(LeftDegrees, RightDegrees, i / (float)(whiskerAmount - 1));
            float sensitivity = Mathf.Cos(newWhiskerDirection / 90f);
            Whiskers.Add((newWhiskerDirection,sensitivity));
        }
    }
    
}
