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
    [SerializeField] private GameObject _throwPreview;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject _victoryCam;

    [Header("Visual Settings")]
    [SerializeField] private Animator _visualBall;

    private Animator _animatedCharacter;
    private bool _canPlay;
    private bool _isAttacking;
    private bool _isGrounded;
    private bool _hasBall;
    private float _currentCharge;
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
            HandleCharge();
            HandleAttack();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.GROUND))
        {
            _isGrounded = true;
            SetJumpAnimation(0);
            SetBallJump(false);
            _throwPreview.SetActive(false);
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
        _uiManager.SetGauge1MaxValue(_playerData.AttackRechargeTime);
        _uiManager.UpdatePlayer1Icon(_playerData.Icon);
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

            if (_hasBall)
            {
                SetBallJump(true);
                _throwPreview.SetActive(true);
            }
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

    private void HandleCharge()
    {
        if (_currentCharge >= _playerData.AttackRechargeTime) return;

        _currentCharge += Time.deltaTime;
        _uiManager.UpdateCharge1UI(_currentCharge);
    }

    private void HandleAttack()
    {
        if (Input.GetButtonDown("Submit") && !_isAttacking && _currentCharge >= _playerData.AttackRechargeTime)
        {
            _isAttacking = true;
            _currentCharge = 0;
            SetAttackAnimation(1);
            Projectile specialAttack = Instantiate(_playerData.AttackPrefab, transform.position, _visualTransform.rotation);
            specialAttack.SetFromPlayer();
            Invoke(nameof(CancelAttack), _playerData.AttackDuration);
        }
    }

    private void CancelAttack()
    {
        _isAttacking = false;
        SetAttackAnimation(0);
    }

    private void CancelStun()
    {
        SetCanPlay(true);
        SetStunAnimation(0);
    }
    #endregion

    #region Public Methods
    public void SetCanPlay(bool canPlay)
    {
        _canPlay = canPlay;

        if (!_canPlay) SetWalkAnimation(0);
    }

    public void SetStunned()
    {
        SetCanPlay(false);
        SetStunAnimation(1);
        Invoke(nameof(CancelStun), 3);
    }

    public void SetTie()
    {
        _victoryCam.SetActive(true);
        _uiManager.ShowVictoryScreen("Tie!");
    }

    public void SetVictory()
    {
        _victoryCam.SetActive(true);
        SetResultsAnimation(1);
        _uiManager.ShowVictoryScreen(_playerData.Name + " Wins!");
    }

    public void SetDefeat() => SetResultsAnimation(2);
    #endregion

    #region Animations
    private void SetWalkAnimation(int value) => _animatedCharacter.SetInteger(AnimationsParameters.SPEED_VALUE, value);
    private void SetJumpAnimation(int value) => _animatedCharacter.SetInteger(AnimationsParameters.JUMP_VALUE, value);
    private void SetAttackAnimation(int value) => _animatedCharacter.SetInteger(AnimationsParameters.ATTACK_VALUE, value);
    private void SetStunAnimation(int value) => _animatedCharacter.SetInteger(AnimationsParameters.HURT_VALUE, value);
    private void SetResultsAnimation(int value) => _animatedCharacter.SetInteger(AnimationsParameters.EMOTE_VALUE, value);
    private void SetBallJump(bool jump)
    {
        if (_hasBall) _visualBall.SetBool(AnimationsParameters.BALL_INAIR, jump);
    }
    #endregion
}