using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to give the tank a color when the play button is pressed
public class TankColorer : MonoBehaviour
{
    [Tooltip("A modifier to add to the color of the tank. Can be used to make certain regions darker")]
    [SerializeField] Color Modifier = Color.white; //Used to modify the set color. Can be used to darken the input color
    Renderer mainRenderer; //The renderer that renders the tank
    MaterialPropertyBlock MatBlock; //Used to set the color of the tank

    public Color Color //Set the color of the tank
    {
        set
        {
            //Initialize the mainRenderer and the MatBlock if not set
            if (mainRenderer == null || MatBlock == null)
            {
                MatBlock = new MaterialPropertyBlock();
                mainRenderer = GetComponent<Renderer>();
            }
            //Set the tank color
            mainRenderer.GetPropertyBlock(MatBlock);
            MatBlock.SetColor("_TankColor", value * Modifier);
            mainRenderer.SetPropertyBlock(MatBlock);
        }
    }
}
