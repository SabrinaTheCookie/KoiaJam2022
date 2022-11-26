using UnityEngine;

public class LinkColorGradient : MonoBehaviour
{
    private Material _gradientMaterial;
    private static readonly int TopColor = Shader.PropertyToID("_ColorTop");
    private static readonly int BottomColor = Shader.PropertyToID("_ColorBottom");
    private static readonly int Origin = Shader.PropertyToID("_Origin");
    private static readonly int Spread = Shader.PropertyToID("_Spread");

    private void Awake()
    {
        _gradientMaterial = GetComponent<SpriteRenderer>().material;
    }

    public void SetTopColor(Color color)
    {
        _gradientMaterial.SetColor(TopColor, color);
    }

    public void SetBottomColor(Color color)
    {
        _gradientMaterial.SetColor(BottomColor, color);
    }

    public void SetOrigin(float value)
    {
        _gradientMaterial.SetFloat(Origin, value);
    }
    
    public void SetSpread(float value)
    {
        _gradientMaterial.SetFloat(Spread, value);
    }
}