#if DOTWEEN_ENABLED
using System;
#if UNITASK_ENABLED
using System.Threading;
using Cysharp.Threading.Tasks;
#endif
using DG.Tweening;
using UnityEngine;

namespace BrunoMikoski.AnimationSequencer
{
    [AddComponentMenu("UI/Animation Sequencer", 200)]
    public class AnimationSequencer : BaseAnimationSequencer
    {
        [SerializeReference] 
        private AnimationStepBase[] animationSteps = Array.Empty<AnimationStepBase>();
        public AnimationStepBase[] AnimationSteps => animationSteps;

        public override Sequence GenerateSequence()
        {
            Sequence sequence = DOTween.Sequence();

            // Various edge cases exists with OnStart() and OnComplete(), some of which can be solved with OnRewind(),
            // but it still leaves callbacks unfired when reversing direction after natural completion of the animation.
            // Rather than using the in-built callbacks, we simply bookend the Sequence with AppendCallback to ensure
            // a Start and Finish callback is always fired.
            sequence.AppendCallback(() =>
            {
                if (playTypeInternal == PlayType.Forward)
                {
                    onStartEvent.Invoke();
                }
                else
                {
                    onFinishedEvent.Invoke();
                }
            });

            for (int i = 0; i < animationSteps.Length; i++)
            {
                AnimationStepBase animationStepBase = animationSteps[i];
                animationStepBase.AddTweenToSequence(sequence);
            }

            sequence.SetTarget(this);
            sequence.SetAutoKill(autoKill);
            sequence.SetUpdate(updateType, timeScaleIndependent);
            sequence.OnUpdate(() => { onProgressEvent.Invoke(); });
            // See comment above regarding bookending via AppendCallback.
            sequence.AppendCallback(() =>
            {
                if (playTypeInternal == PlayType.Forward)
                {
                    onFinishedEvent.Invoke();
                }
                else
                {
                    onStartEvent.Invoke();
                }
            });

            int targetLoops = loops;

            if (!Application.isPlaying)
            {
                if (loops == -1)
                {
                    targetLoops = 10;
                    Debug.LogWarning("Infinity sequences on editor can cause issues, using 10 loops while on editor.");
                }
            }

            sequence.SetLoops(targetLoops, loopType);
            sequence.timeScale = playbackSpeed;
            return sequence;
        }

        public override void ResetToInitialState()
        {
            progress = -1.0f;
            for (int i = animationSteps.Length - 1; i >= 0; i--)
            {
                animationSteps[i].ResetToInitialState();
            }
        }
        
        public override bool TryGetStepAtIndex<T>(int index, out T result)
        {
            if (index < 0 || index > animationSteps.Length - 1)
            {
                result = null;
                return false;
            }

            result = animationSteps[index] as T;
            return result != null;
        }

        public override void ReplaceTarget<T>(GameObject targetGameObject)
        {
            for (int i = animationSteps.Length - 1; i >= 0; i--)
            {
                AnimationStepBase animationStepBase = animationSteps[i];
                if (animationStepBase == null)
                    continue;

                if (animationStepBase is not T gameObjectAnimationStep)
                    continue;

                gameObjectAnimationStep.SetTarget(targetGameObject);
            }
        }

        public override void ReplaceTargets(params (GameObject original, GameObject target)[] replacements)
        {
            for (int i = 0; i < replacements.Length; i++)
            {
                (GameObject original, GameObject target) replacement = replacements[i];
                ReplaceTargets(replacement.original, replacement.target);
            }
        }

        public override void ReplaceTargets(GameObject originalTarget, GameObject newTarget)
        {
            for (int i = animationSteps.Length - 1; i >= 0; i--)
            {
                AnimationStepBase animationStepBase = animationSteps[i];
                if (animationStepBase == null)
                    continue;
                
                if(animationStepBase is not GameObjectAnimationStep gameObjectAnimationStep)
                    continue;

                if (gameObjectAnimationStep.Target == originalTarget)
                    gameObjectAnimationStep.SetTarget(newTarget);
            }
        }
    }
}
#endif
