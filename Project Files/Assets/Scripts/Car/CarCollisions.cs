using UnityEngine;

[RequireComponent(typeof(CarUpdate))]
public class CarCollisions : MonoBehaviour
{
    [SerializeField] private GameObject _baffTakenEffect;

    private CarUpdate _carUpdate;

    private void Awake()
    {
        _carUpdate = GetComponent<CarUpdate>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Baff"))
        {
            GameObject temp = Instantiate(_baffTakenEffect, col.transform.position, _baffTakenEffect.transform.rotation);

            _carUpdate.RegainStamina();
            
            Destroy(temp, 2);
            Destroy(col.gameObject);
        }
    }
}
