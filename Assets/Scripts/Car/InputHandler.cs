using UnityEngine;

[RequireComponent(typeof(CarController))]
public class InputHandler : MonoBehaviour
{
    public bool UIInput;

    private CarController _carController;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private Vector2 input = Vector2.zero;

    private void Awake()
    {
        _carController = GetComponent<CarController>();
    }

    private void Update()
    {
        if (UIInput)
        {

        } else 
        {
            input = Vector2.zero;

            input.x = Input.GetAxis(HORIZONTAL);
            input.y = Input.GetAxis(VERTICAL);
        }

        _carController.SetInput(input);
    }

    public void SetInput(Vector2 inputNew)
    {
        input = inputNew;
    }
}
