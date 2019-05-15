using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Extensions;

public class ObstacleAvoidance
{
    public float RecommendedDirection //The recommended direction to take to avoid the obstacles by utilizing the whiskerScore
    {
        get
        {
            //If all the whiskers are triggering, then that most likely means that the tank is entering a corner
            if (Enabled && Distances.Average() <= WhiskerLength)
            {
                //Attempt to rotate away from the corner to prevent the tank from getting stuck in there
                return -5f;
            }
            else
            {
                //Rotate away from the obstacle
                return -6f / 15f * whiskerScore;
            }
        }
    }

    public int WhiskerAmount //The amount of whiskers to use for obstacle avoidance
    {
        get => whiskerAmountInternal;
        set
        {
            whiskerAmountInternal = value;
            UpdateWhiskers();
        }
    } 
    public float WhiskerFOV //The Field of view the whiskers will cover
    {
        get => whiskerFOVInternal;
        set
        {
            whiskerFOVInternal = value;
            UpdateWhiskers();
        }
    }
    public bool Enabled //Whether the obstacle avoidance system is enabled or not
    {
        get => updateRoutine != null;
        set
        {
            if (!value && updateRoutine != null)
            {
                CoroutineManager.StopCoroutine(updateRoutine);
                whiskerScore = 0;
            }
            else if (value && updateRoutine == null)
            {
                updateRoutine = CoroutineManager.StartCoroutine(Update());
            }
        }
    }
    public float WhiskerLength { get; set; } = 5f; //How long the whiskers will be
    public LayerMask ObstacleLayers { get; set; } //The layers the whiskers will consider an obstacle
    public bool Debug { get; set; } = false; //Whether to display the whisker lines or not
    public Transform Source { get;  set; } //The Source Tank that is utilizing the Obstacle Avoidance system

    List<(float Direction, float Sensitivity)> Whiskers = new List<(float, float)>(); //The whiskers that are being used
    List<float> Distances = new List<float>(); //The last known distance of each whisker to an obstacle

    float tankDirection => -Source.eulerAngles.y; //Returns the direction of the enemy in degrees
    float whiskerFOVInternal; //The internal variable for keeping track of the whisker fov
    int whiskerAmountInternal; //The internal variable for keeping track of the whisker amount
    float whiskerScore = 0; //A score that determines the best direction to take to move away from the obstacles
    Coroutine updateRoutine; //The Update function that is called each frame

    //Used to calculate the sensitity of each whisker
    //Visualization : https://www.desmos.com/calculator/d6tnxlp4tp
    static float SensitivityTransform(float input) => 1 - Mathf.Abs(Mathf.Cos((input + 1) * (Mathf.PI / 2)));

    //Create the obstacle avoidance algorithm for the specified source object
    public ObstacleAvoidance(Transform source = null,int whiskerAmount = 7,float whiskerLength = 5f, float whiskerFOV = 150f)
    {
        Source = source;
        WhiskerAmount = whiskerAmount;
        WhiskerLength = whiskerLength;
        WhiskerFOV = whiskerFOV;
        UpdateWhiskers();
        updateRoutine = CoroutineManager.StartCoroutine(Update());
    }

    IEnumerator Update()
    {
        while (true)
        {
            if (Source != null)
            {
                //If debug mode is turned on
                if (Debug)
                {
                    //Draw each of the whiskers in the scene view
                    foreach (var whisker in Whiskers)
                    {
                        DebugDraw.DrawFOV(new Vector3(Source.position.x, Source.position.y, Source.position.z), WhiskerLength, whisker.Direction + tankDirection, 0f, Color.green);
                    }
                }
                //Reset the overall score of the whiskers
                whiskerScore = 0;
                //Loop over all the whiskers
                for (int i = 0; i < Whiskers.Count; i++)
                {
                    //Get the current whisker
                    var whisker = Whiskers[i];
                    //Fire a raycast in the whisker's direction and check to see if it has hit an obstacle
                    if (Physics.Raycast(Source.position, DegToVector(tankDirection + whisker.Direction), out var hit, WhiskerLength, ObstacleLayers.value) && hit.transform != null)
                    {
                        //Get whether the ray came at the obstacle from the right or from the left
                        //A value of 1 is to the right, and a value of -1 is to the left
                        var directionToObstacle = Vector3.SignedAngle(Source.forward, -hit.normal, Vector3.up).Normalize();

                        //Get how perpendicular to the obstacle the ray is. 0 is perfectly parallel and 1 is perfectly perpendicular
                        var perpendicularity = Mathf.Abs(Vector3.Cross(-hit.normal, (hit.point - Source.position).normalized).magnitude);

                        //Create a final modifier with the sensitivity and the perpendicularity to judge the score of the whisker
                        //var modifier = whisker.Sensitivity * perpendicularity;

                        /*Create an overall score of the whisker, which determines it priority, based on the following statistics:
                        1 : How close the whisker is to the obstacle
                            If the whisker is very close to the obstacle, then it should have higher priority than the whiskers that are far away from an obstacle

                        2 : The Sensitivity of the whisker
                            Whiskers that are towards the front of the tank have higher priority than the ones towards the sides of the tank
                            If the front whiskers have an obstacle, then that means that the tank is heading straight towards a wall, and should dodge it immediately

                        3 : How perpendicular the whisker is to the obstacle
                            If the tank is directly perpendicular to an obstacle, then it will be harder for the tank to turn away from it

                        4 : And whether the ray came at the obstacle from the right or from the left
                            Will help determine the direction the tank should go to effectively avoid the obstacle
                        Multiply them all together to get the whisker's score
                        */
                        var currentWhiskerScore = (1f / hit.distance) * whisker.Sensitivity * perpendicularity * directionToObstacle;
                        //If this whisker has a higher absolute score then all the other ones, then use that
                        //But if it's not, then reject it
                        whiskerScore = BiggerAbsNumber(whiskerScore, currentWhiskerScore);
                        //Record the whisker's distance to the obstacle
                        Distances[i] = hit.distance;
                    }
                    //If the whisker is not hitting an obstacle
                    else
                    {
                        //Set the distance to infinity
                        Distances[i] = float.PositiveInfinity;
                    }
                }
                //Amplify the current whisker score for use with the RecommendedDirection property
                whiskerScore *= 100f;
            }
            yield return null;
        }
    }

    //Creates a set of whiskers
    //Run when the amount of whiskers change
    void UpdateWhiskers()
    {
        //Clear the whisker and distance list
        Whiskers.Clear();
        Distances.Clear();

        //Get the the most leftward direction a whisker could be
        float LeftDegrees = -(WhiskerFOV / 2);
        //Get the most rightward direction a whisker could be
        float RightDegrees = (WhiskerFOV / 2);
        //Loop over all the whiskers
        for (int i = 0; i < WhiskerAmount; i++)
        {
            //Calculate the whisker's direction
            float newWhiskerDirection = Mathf.Lerp(LeftDegrees, RightDegrees, i / (float)(WhiskerAmount - 1));
            //Calculate the whisker's sensitivity
            float sensitivity = SensitivityTransform(newWhiskerDirection / 90f);
            //Add the new whisker to the whisker list
            Whiskers.Add((newWhiskerDirection,sensitivity));
            //Set the whisker's starting distance to be infinity
            Distances.Add(float.PositiveInfinity);
        }
    }
}
