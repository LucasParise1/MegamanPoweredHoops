using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject EndParticles;
    [SerializeField] private bool _fromPlayer;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;

    private bool _colided;

    void Start() => Invoke(nameof(DestroyProjectile), _lifeTime);

    void Update() => transform.Translate(Vector3.forward * _speed * Time.deltaTime);

    private void OnTriggerEnter(Collider other)
    {
        if (_fromPlayer && other.CompareTag(Tags.ENEMY) && !_colided)
        {
            _colided = true;
            //get bot component and set stun
            DestroyProjectile();
        }

        if (!_fromPlayer && other.CompareTag(Tags.PLAYER) && !_colided)
        {
            _colided = true;
            other.GetComponent<Player>().SetStunned();
            DestroyProjectile();
        }

        if (other.CompareTag(Tags.OBSTACLE) && !_colided)
        {
            _colided = true;
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        Instantiate(EndParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void SetFromPlayer() => _fromPlayer = true;
}
