public interface IAnimationHandler
{
    void PlayAnimationState(string animationStateName, int layer = 0);
    void PauseCurrentAnimationState();
    void ResumeCurrentAnimationState();
    float GetCurrentAnimationDuration();
}