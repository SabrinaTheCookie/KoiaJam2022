using UnityEngine;
using Random = UnityEngine.Random;

public class LikeBlip : MonoBehaviour
{

    [Range(0,1)]
    public float blipChance;

    public float duration;

    public Vector3 velocity;

    public AnimationCurve speedCurve;
    public AnimationCurve scaleCurve;

    private float _timeOfUnalive;

    private void Start()
    {
        _timeOfUnalive = Time.time + duration;
        
        //Only spawn blipChance percentage of the time
        if(Random.Range(0f,1f) > blipChance) Destroy(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        //Is it time to unalive?
        if (Time.time > _timeOfUnalive) Destroy(gameObject);

        //Move
        //Get time of life in 0-1
        float t = (_timeOfUnalive - Time.time) / duration;
        //Move, modified by speed curve.
        transform.position += Time.deltaTime * speedCurve.Evaluate(t) * velocity;
        
        
        //TODO this is going wayyyy tooo fast!?!? Why!?!?
        //Reduce scale
        Vector3 newScale = transform.localScale;
        newScale = scaleCurve.Evaluate(t) * Vector3.one;
        //transform.localScale = newScale;
    }
}
