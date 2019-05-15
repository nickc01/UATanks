using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankColorer : MonoBehaviour
{
    [Tooltip("A modifier to add to the color of the tank. Can be used to make certain regions darker")]
    [SerializeField] Color Modifier;
    Renderer mainRenderer;
    MaterialPropertyBlock MatBlock;

    public Color Color
    {
        set
        {
            if (mainRenderer == null || MatBlock == null)
            {
                MatBlock = new MaterialPropertyBlock();
                mainRenderer = GetComponent<Renderer>();
            }
            mainRenderer.GetPropertyBlock(MatBlock);
            MatBlock.SetColor("_TankColor", value * Modifier);
            mainRenderer.SetPropertyBlock(MatBlock);
        }
    }
}
