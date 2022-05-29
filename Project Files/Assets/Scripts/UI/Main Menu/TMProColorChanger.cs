using UnityEngine;

public class TMProColorChanger : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Gradient _topGradient;
    [Space(2)]
    
    [SerializeField] private Gradient _bottomGradient;
    [Space(5)]
    
    [Header("Texts")]

    [SerializeField] private TMPro.TMP_Text _text;
    [Space(5)] 
    [SerializeField] private float _strobeDuration;
    
    private void Update()
    {
        float t = Mathf.PingPong(Time.time / _strobeDuration, 1f);
        
        Color tempTopColor = _topGradient.Evaluate(t);
        Color tempBottomColor = _bottomGradient.Evaluate(t);
        
        _text.colorGradient = new TMPro.VertexGradient(tempTopColor, tempTopColor, tempBottomColor, tempBottomColor);
    }
}
