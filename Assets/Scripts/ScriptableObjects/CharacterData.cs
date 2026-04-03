using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header ("Basic Data")]
    public Sprite Icon;
    public string Name;
    public Animator AnimatedPrefab;
    public Projectile AttackPrefab;

    [Header("Stats")]
    public float MoveSpeed;
    public float RotationSpeed;
    public float JumpForce;
    public float AttackRechargeTime;
    public float AttackDuration;
}