using System;
using BrunoMikoski.AnimationSequencer;
using DG.Tweening;
using UnityEngine;

namespace UISample.UI
{
    public class ViewAnimation : BaseAnimationSequencer
    {
        [SerializeReference] 
        private AnimationStepBase[] showAnimationSteps = Array.Empty<AnimationStepBase>();
        public AnimationStepBase[] ShowAnimationSteps => showAnimationSteps;
        
        private AnimationStepBase[] hideAnimationSteps = Array.Empty<AnimationStepBase>();
        public AnimationStepBase[] HideAnimationSteps => showAnimationSteps;
        
        public override Sequence GenerateSequence()
        {
            throw new System.NotImplementedException();
        }

        public override void ResetToInitialState()
        {
            throw new System.NotImplementedException();
        }

        public override bool TryGetStepAtIndex<T>(int index, out T result)
        {
            throw new System.NotImplementedException();
        }

        public override void ReplaceTarget<T>(GameObject targetGameObject)
        {
            throw new System.NotImplementedException();
        }

        public override void ReplaceTargets(params (GameObject original, GameObject target)[] replacements)
        {
            throw new System.NotImplementedException();
        }

        public override void ReplaceTargets(GameObject originalTarget, GameObject newTarget)
        {
            throw new System.NotImplementedException();
        }
    }
}