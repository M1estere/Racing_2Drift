using UnityEngine;

[RequireComponent(typeof(CarController))]
public class InputHandler : MonoBehaviour
{

    private CarController _carController;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private void Awake()
    {
        _carController = GetComponent<CarController>();
    }

    private void Update()
    {
        Vector2 input = Vector2.zero;

        input.x = Input.GetAxis(HORIZONTAL);
        input.y = Input.GetAxis(VERTICAL);

        _carController.SetInput(input);
    }
}
