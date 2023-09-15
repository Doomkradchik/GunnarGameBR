using Characters;
using Characters.Player;
using Characters.Player.States;
using Dreamteck.Splines;
using Interaction;
using JetBrains.Annotations;
using MapSystem;
using UnityEngine;
using Attack = Characters.Player.States.Attack;

public class TransitionAndStatesData
{
    public Animator Animator { get; }
    public AudioSource AudioSource { get; }
    public GetCurrentPoint GetCurrentPoint { get; }

    public Transform Transform { get; }

    public GameObject Model { get; }

    public RunToPointData RunToPointData { get; }
    public Money MoneyPrefab { get; }

    public int MoneyCountAfterDeath { get; private set; } = 0;

    public Placeholder MapStates { get; }
    public int ID { get; }
    public VFXTransforms VFXTransforms { get; }
    public AnimatorOverrideController AnimatorOverrideController { get; }
    public CharacterController CharacterController { get; }
    public SplineFollower SplineFollower { get; }
    public SplineProjector SplineProjector { get; }

    [CanBeNull] public GetIsAttack GetIsAttack { get; }

    [CanBeNull] public CharacterData CharacterData { get; }

    [CanBeNull] public GetRecoil GetRecoil { get; }
    public Enable Enable { get; }
    public Attack Attack { get; private set; }
    public Die Die { get; private set; }

    public HasCharacter HasCharacter { get; }


    public TransitionAndStatesData(Animator animator, AudioSource audioSource, GetCurrentPoint getCurrentPoint, Transform transform, GameObject model,
        RunToPointData runToPointData, [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] CharacterData characterData,[CanBeNull] GetRecoil getRecoil, [CanBeNull] Enable enable,
        Placeholder mapStates, int id, HasCharacter hasCharacter, AnimatorOverrideController animatorOverrideController,VFXTransforms vfxTransforms,
        [CanBeNull] SplineFollower splineFollower=null,[CanBeNull] SplineProjector splineProjector = null, [CanBeNull] Money moneyPrefab=null)
    {
        Animator = animator;
        AudioSource = audioSource;
        GetCurrentPoint = getCurrentPoint;
        Transform = transform;
        RunToPointData = runToPointData;
        GetIsAttack = getIsAttack;
        CharacterData = characterData;
        MapStates = mapStates;
        Attack = null;
        Die = null;
        HasCharacter = hasCharacter;
        CharacterController = runToPointData.CharacterController;
        AnimatorOverrideController = animatorOverrideController;
        VFXTransforms = vfxTransforms;  
        SplineFollower = splineFollower;
        MoneyPrefab = moneyPrefab;
        ID = id;
        GetRecoil = getRecoil;
        Enable = enable;
        SplineProjector = splineProjector;
        Model = model;
    }

    public TransitionAndStatesData OverrideMoneyCount(int amount)
    {
        MoneyCountAfterDeath = amount;
        return this;
    }

    public void CreateAttack(Attack attack)
    {
        Attack = attack;
    }

    public void CreateDie(Die die)
    {
        Die = die;
    }
}