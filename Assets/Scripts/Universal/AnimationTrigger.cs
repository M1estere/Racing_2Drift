using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [Space(2)]

    [SerializeField] private string _triggerName;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("AI"))
        {
            _animator.SetTrigger(_triggerName);
        }
    }
}
