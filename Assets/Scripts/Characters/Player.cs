using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    [Header("Basic Settings")]
    [SerializeField] private CharacterData _playerData;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _visualTransform;

    private Animator _animatedCharacter;
    [SerializeField] private bool _canPlay;
    private bool _isAttacking;
    #endregion

    #region Lifecycle
    private void Start()
    {
        SetUp(_playerData);
    }

    private void Update()
    {
        Move();
    }
    #endregion

    #region Private Methods
    private void SetUp(CharacterData data)
    {
        _animatedCharacter = data.AnimatedPrefab;
    }

    private void Move()
    {
        if (_canPlay)
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

            //SetWalkAnimation(moveDirection != Vector3.zero ? 1 : 0);
        }
    }
    #endregion

    #region Animations
    private void SetWalkAnimation(int value) => _animatedCharacter.SetInteger(AnimationsParameters.SPEED_VALUE, value);
    #endregion
}