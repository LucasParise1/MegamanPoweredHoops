using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    [Header("Basic Settings")]
    [SerializeField] private CharacterData _playerData;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _visualTransform;
    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private Ball _ballPrefab;

    [Header("Visual Settings")]
    [SerializeField] private Animator _visualBall;

    private Animator _animatedCharacter;
    [SerializeField] private bool _canPlay;
    private bool _isAttacking;
    private bool _isGrounded;
    private bool _hasBall;
    #endregion

    #region Lifecycle
    private void Start()
    {
        SetUp(_playerData);
    }

    private void Update()
    {
        if (_canPlay)
        {
            Move();
            HandleJump();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.GROUND))
        {
            _isGrounded = true;
            SetJumpAnimation(0);
            SetBallJump(false);
        }

        if (collision.gameObject.CompareTag(Tags.BALL) && !_hasBall)
        {
            SetHasBall(true);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.GROUND)) _isGrounded = false;
    }
    #endregion

    #region Private Methods
    private void SetUp(CharacterData data)
    {
        _animatedCharacter = Instantiate(_playerData.AnimatedPrefab, _visualTransform);
    }

    private void Move()
    {
        if (_isAttacking)
        {
            SetWalkAnimation(0);
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        if (moveDirection != Vector3.zero)
        {
            Vector3 movement = moveDirection * _playerData.MoveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            _visualTransform.rotation = Quaternion.RotateTowards(_visualTransform.rotation, targetRotation, _playerData.RotationSpeed * Time.deltaTime);
        }

        SetWalkAnimation(moveDirection != Vector3.zero ? 1 : 0);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * _playerData.JumpForce, ForceMode.Impulse);
            SetJumpAnimation(1);
            SetBallJump(true);
        }

        if (Input.GetButtonDown("Jump") && !_isGrounded && _hasBall)
        {
            SetHasBall(false);
            Ball ballToThrow = Instantiate(_ballPrefab, _ballSpawnPoint.position, _ballSpawnPoint.rotation);
            ballToThrow.Throw();
        }
    }

    private void SetHasBall(bool value)
    {
        _hasBall = value;
        _visualBall.gameObject.SetActive(value);
    }
    #endregion

    #region Animations
    private void SetWalkAnimation(int value) => _animatedCharacter.SetInteger(AnimationsParameters.SPEED_VALUE, value);
    private void SetJumpAnimation(int value) => _animatedCharacter.SetInteger(AnimationsParameters.JUMP_VALUE, value);
    private void SetBallJump(bool jump)
    {
        if (_hasBall) _visualBall.SetBool(AnimationsParameters.BALL_INAIR, jump);
    }
    #endregion
}