using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CarUpdate _carUpdate;
    [Space(2)]

    [SerializeField] private RectTransform _staminaBarFill;
    [Space(2)]

    [SerializeField] private Image _staminaImage;
    [Space(2)]

    [Header("Change Bar Colour")]
    [SerializeField] private Color _defaultColor;
    [Space(2)]

    [SerializeField] private Color _middleColor;
    [Space(2)]

    [SerializeField] private Color _lowColor;

    private float lerpSpeed;

    private void Update()
    {
        float stamina = _carUpdate.ReturnStamina();
        lerpSpeed = 6f * Time.unscaledDeltaTime;

        SetStaminaAmount(stamina);

        if (stamina > .65f)
        {
            _staminaImage.color = Color.Lerp(_staminaImage.color, _defaultColor, lerpSpeed / 2);
        } else if (stamina <= .65f && stamina > .35f)
        {
            _staminaImage.color = Color.Lerp(_staminaImage.color, _middleColor, lerpSpeed / 2);
        } else if (stamina <= .35f && stamina > 0)
        {
            _staminaImage.color = Color.Lerp(_staminaImage.color, _lowColor, lerpSpeed / 2);
        } 
    }

    private void SetStaminaAmount(float amount)
    {
        if (amount < 0) amount = 0;
        
        float scale = _staminaBarFill.localScale.x;

        if (scale > amount)
            scale = amount;
        else if (scale < amount)
            scale = Mathf.Lerp(amount, scale, lerpSpeed);

        _staminaBarFill.localScale = new Vector3(scale, 1, 1);
    }
}
