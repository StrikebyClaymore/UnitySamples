#if DOTWEEN_ENABLED
using System;
using System.Collections;
#if UNITASK_ENABLED
using System.Threading;
using Cysharp.Threading.Tasks;
#endif
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace BrunoMikoski.AnimationSequencer
{
    public abstract class BaseAnimationSequencer : MonoBehaviour
    {
        public enum PlayType
        {
            Forward,
            Backward
        }

        public enum AutoplayType
        {
            Awake,
            OnEnable,
            Nothing
        }
        
        [SerializeField] protected UpdateType updateType = UpdateType.Normal;
        [SerializeField] protected bool timeScaleIndependent = false;
        [SerializeField] protected AutoplayType autoplayMode = AutoplayType.Awake;
        [SerializeField] 
        protected bool startPaused;
        [SerializeField] protected float playbackSpeed = 1f;
        public float PlaybackSpeed => playbackSpeed;
        [SerializeField] 
        protected PlayType playType = PlayType.Forward;
        [SerializeField] protected int loops = 0;
        [SerializeField] protected LoopType loopType = LoopType.Restart;
        [SerializeField] protected bool autoKill = true;

        [SerializeField] protected UnityEvent onStartEvent = new UnityEvent();

        public UnityEvent OnStartEvent
        {
            get => onStartEvent;
            protected set => onStartEvent = value;
        }

        [SerializeField] protected UnityEvent onFinishedEvent = new UnityEvent();

        public UnityEvent OnFinishedEvent
        {
            get => onFinishedEvent;
            protected set => onFinishedEvent = value;
        }

        [SerializeField] protected UnityEvent onProgressEvent = new UnityEvent();
        public UnityEvent OnProgressEvent => onProgressEvent;

        protected Sequence playingSequence;
        public Sequence PlayingSequence => playingSequence;
        protected PlayType playTypeInternal = PlayType.Forward;
#if UNITY_EDITOR
        protected bool requiresReset = false;
#endif

        public bool IsPlaying => playingSequence != null && playingSequence.IsActive() && playingSequence.IsPlaying();
        public bool IsPaused => playingSequence != null && playingSequence.IsActive() && !playingSequence.IsPlaying();

        [SerializeField, Range(0, 1)] protected float progress = -1;
        
        protected virtual void Awake()
        {
            progress = -1;
            if (autoplayMode != AutoplayType.Awake)
                return;

            Autoplay();
        }

        protected virtual void OnEnable()
        {
            if (autoplayMode != AutoplayType.OnEnable)
                return;

            Autoplay();
        }

        private void Autoplay()
        {
            Play();
            if (startPaused)
                playingSequence.Pause();
        }

        protected virtual void OnDisable()
        {
            if (autoplayMode != AutoplayType.OnEnable)
                return;

            if (playingSequence == null)
                return;

            ClearPlayingSequence();
            // Reset the object to its initial state so that if it is re-enabled the start values are correct for
            // regenerating the Sequence.
            ResetToInitialState();
        }

        protected virtual void OnDestroy()
        {
            ClearPlayingSequence();
        }

        public virtual void Play()
        {
            Play(null);
        }

        public virtual void Play(Action onCompleteCallback)
        {
            playTypeInternal = playType;

            ClearPlayingSequence();

            onFinishedEvent.RemoveAllListeners();

            if (onCompleteCallback != null)
                onFinishedEvent.AddListener(onCompleteCallback.Invoke);

            playingSequence = GenerateSequence();

            switch (playTypeInternal)
            {
                case PlayType.Backward:
                    playingSequence.PlayBackwards();
                    break;

                case PlayType.Forward:
                    playingSequence.PlayForward();
                    break;

                default:
                    playingSequence.Play();
                    break;
            }
        }

        public virtual void PlayForward(bool resetFirst = true, Action onCompleteCallback = null)
        {
            if (playingSequence == null)
                Play();

            playTypeInternal = PlayType.Forward;
            onFinishedEvent.RemoveAllListeners();

            if (onCompleteCallback != null)
                onFinishedEvent.AddListener(onCompleteCallback.Invoke);

            if (resetFirst)
                SetProgress(0);

            playingSequence.PlayForward();
        }

        public virtual void PlayBackwards(bool completeFirst = true, Action onCompleteCallback = null)
        {
            if (playingSequence == null)
                Play();

            playTypeInternal = PlayType.Backward;
            onFinishedEvent.RemoveAllListeners();

            if (onCompleteCallback != null)
                onFinishedEvent.AddListener(onCompleteCallback.Invoke);

            if (completeFirst)
                SetProgress(1);

            playingSequence.PlayBackwards();
        }
        
        public void PlayInstantly()
        {
            playTypeInternal = playType;
            ClearPlayingSequence();
            onFinishedEvent.RemoveAllListeners();
            playingSequence.Complete();
        }

        public virtual void SetTime(float seconds, bool andPlay = true)
        {
            if (playingSequence == null)
                Play();

            playingSequence.Goto(seconds, andPlay);
        }

        public virtual void SetProgress(float targetProgress, bool andPlay = true)
        {
            if (playingSequence == null)
                Play();
            
            targetProgress = Mathf.Clamp01(targetProgress);
            
            float duration = playingSequence.Duration();
            float finalTime = targetProgress * duration;
            SetTime(finalTime, andPlay);
        }

        public virtual void TogglePause()
        {
            if (playingSequence == null)
                return;

            playingSequence.TogglePause();
        }

        public virtual void Pause()
        {
            if (!IsPlaying)
                return;

            playingSequence.Pause();
        }

        public virtual void Resume()
        {
            if (playingSequence == null)
                return;

            playingSequence.Play();
        }


        public virtual void Complete(bool withCallbacks = true)
        {
            if (playingSequence == null)
                return;

            playingSequence.Complete(withCallbacks);
        }

        public virtual void Rewind(bool includeDelay = true)
        {
            if (playingSequence == null)
                return;

            playingSequence.Rewind(includeDelay);
        }

        public virtual void Kill(bool complete = false)
        {
            if (!IsPlaying)
                return;

            playingSequence.Kill(complete);
        }

        public virtual IEnumerator PlayEnumerator()
        {
            Play();
            yield return playingSequence.WaitForCompletion();
        }

        public abstract Sequence GenerateSequence();

        public abstract void ResetToInitialState();
        
        public void ClearPlayingSequence()
        {
            DOTween.Kill(this);
            DOTween.Kill(playingSequence);
            playingSequence = null;
        }

        public void SetAutoplayMode(AutoplayType autoplayType)
        {
            autoplayMode = autoplayType;
        }

        public void SetPlayOnAwake(bool targetPlayOnAwake)
        {
        }

        public void SetPauseOnAwake(bool targetPauseOnAwake)
        {
            startPaused = targetPauseOnAwake;
        }

        public void SetTimeScaleIndependent(bool targetTimeScaleIndependent)
        {
            timeScaleIndependent = targetTimeScaleIndependent;
        }

        public void SetPlayType(PlayType targetPlayType)
        {
            playType = targetPlayType;
        }

        public void SetUpdateType(UpdateType targetUpdateType)
        {
            updateType = targetUpdateType;
        }

        public void SetAutoKill(bool targetAutoKill)
        {
            autoKill = targetAutoKill;
        }

        public void SetLoops(int targetLoops)
        {
            loops = targetLoops;
        }

        private void Update()
        {
            if (progress == -1.0f)
                return;

            SetProgress(progress);
        }

#if UNITY_EDITOR
        // Unity Event Function called when component is added or reset.
        private void Reset()
        {
            requiresReset = true;
        }

        // Used by the CustomEditor so it knows when to reset to the defaults.
        public bool IsResetRequired()
        {
            return requiresReset;
        }

        // Called by the CustomEditor once the reset has been completed 
        public void ResetComplete()
        {
            requiresReset = false;
        }
#endif

        public abstract bool TryGetStepAtIndex<T>(int index, out T result) where T : AnimationStepBase;

        public abstract void ReplaceTarget<T>(GameObject targetGameObject) where T : GameObjectAnimationStep;

        public abstract void ReplaceTargets(params (GameObject original, GameObject target)[] replacements);

        public abstract void ReplaceTargets(GameObject originalTarget, GameObject newTarget);

#if UNITASK_ENABLED
        public async UniTask PlayAsync(CancellationToken cancellationTokenSource = default)
        {
            if (cancellationTokenSource == default)
                cancellationTokenSource = this.GetCancellationTokenOnDestroy();
            
            await PlayEnumerator().ToUniTask(PlayerLoopTiming.Update, cancellationTokenSource);
        }
#endif
    }
}
#endif