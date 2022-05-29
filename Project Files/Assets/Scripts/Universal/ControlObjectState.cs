using UnityEngine;

public class ControlObjectState : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToControl;
    [Space(2)]

    [SerializeField] private bool _newState = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("AI"))
        {
            foreach (GameObject obj in _objectsToControl)
            {
                obj.SetActive(_newState);
            }
        }
    }   
}