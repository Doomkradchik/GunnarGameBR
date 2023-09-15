using System;
using Better.Attributes.Runtime.Select;
using Characters;
using UnityEngine;

namespace MapSystem.Structs
{
    [Serializable]
    public struct View
    {
        [field: SerializeField] public int ID { get; private set; }

        [field: SerializeReference]
        [field: SelectImplementation(typeof(BaseState))]
        public BaseState State { get; private set; }

        [field: SerializeField] public AnimationClip Animation { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public VFXEffect Effect { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public AudioParameters AudioParameters { get; private set; }
        [field: SerializeField] public bool UseAnimationLine { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float AnimationLine { get; private set; }

        public View(View view)
        {
            Animation = view.Animation;
            Effect = view.Effect;
            State = view.State;
            ID = view.ID;
            AudioParameters = view.AudioParameters;
            AudioClip = view.AudioClip;
            UseAnimationLine = view.UseAnimationLine;
            AnimationLine = view.AnimationLine;
            Speed = view.Speed;
        }

        public View(AnimationClip animation, VFXEffect effect, BaseState state, int id, AudioClip audioClip, AudioParameters audioParameters,
            bool useAnimationLine, float line, float speed)
        {
            Animation = animation;
            Effect = effect;
            State = state;
            ID = id;
            AudioParameters = audioParameters;
            AudioClip = audioClip;
            UseAnimationLine = useAnimationLine;
            AnimationLine = line;
            Speed = speed;
        }
    }
}

[Serializable]
public struct AudioParameters
{
    public bool loop;
    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 256)]
    public int priority;
}