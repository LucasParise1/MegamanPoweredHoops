using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _moveSpeed;

    private bool _isMoving;

    private void Update()
    {
        if (_isMoving) transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isMoving) _isMoving = false;
    }

    public void Throw()
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _isMoving = true;
    }
}