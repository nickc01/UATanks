using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float AnimationSpeed = 0.4f;
    public List<Sprite> ExplosionSprites;
    new SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(PlayExplosion());
    }

    private IEnumerator PlayExplosion()
    {
        Audio.Play(GameManager.Game.ExplosionSounds.RandomElement(),Audio.SoundEffects,transform);
        foreach (var sprite in ExplosionSprites)
        {
            renderer.sprite = sprite;
            yield return WaitForSecondsGame.Wait(AnimationSpeed / ExplosionSprites.Count);
        }
        Destroy(gameObject);
    }

    //Plays an explosion at the set position
    public static void Spawn(Vector3 position,float Scale)
    {
        GameObject.Instantiate(GameManager.Game.ExplosionPrefab, position, GameManager.Game.ExplosionPrefab.transform.rotation).transform.localScale = new Vector3(Scale, Scale, Scale);
    }
}
