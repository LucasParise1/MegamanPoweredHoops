using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private float _throwForce;

    private void Start() => Invoke(nameof(EnableCollider), 0.1f);

    public void Throw()
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        _rb.AddForce(Vector3.up * _throwForce, ForceMode.Impulse);
        _rb.AddForce(transform.forward * _throwForce, ForceMode.Impulse);
    }

    private void EnableCollider() => _collider.enabled = true;
}