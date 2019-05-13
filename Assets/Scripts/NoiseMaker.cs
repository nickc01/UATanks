using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    public float NoiseLevel = 0f; //The amount of noise this object is emitting
    [SerializeField] bool Debug = false;

    private void Update()
    {
        if (Debug)
        {
            DebugDraw.DrawCircle2D(transform.position, NoiseLevel, 100, Color.red, (a, r) => {
                Random.InitState((int)(a * 100f * Time.deltaTime));
                return (a, Random.Range(r - 0.2f, r + 0.2f));
            });
        }
    }
}
