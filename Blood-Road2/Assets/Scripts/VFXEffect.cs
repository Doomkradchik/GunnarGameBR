using System;
using System.Collections;
using Characters;
using Characters.EffectSystem;
using JetBrains.Annotations;
using UnityEngine;

public class VFXEffect : MonoBehaviour
{
    [SerializeField] [CanBeNull] private BaseCharacter character;
    [SerializeField] [CanBeNull] private ParticleSystem particleSystem;
    private Transform _thisTransform;

    private void Awake()
    {
        _thisTransform = transform;
        if (particleSystem == null) particleSystem = GetComponent<ParticleSystem>();
    }

    public void SetLifeTime(float lifeTime)
    {
        StartCoroutine(Timer(lifeTime));
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        if (character != null)
            character.TakeDamage(new EffectData(Int32.MaxValue, 0, 0, 0, 0, null));
        else Destroy(gameObject);
    }

    public void SetPosition(Vector3 position)
    {
        _thisTransform.position = position;
    }

    public void SetRotation(Quaternion rotation)
    {
        _thisTransform.rotation = rotation;
    }

    public void StartPlayback()
    {
        particleSystem.Stop();
        particleSystem.Play();
    }

    public void SetCharacter(BaseCharacter character)
    {
        this.character = character;
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}