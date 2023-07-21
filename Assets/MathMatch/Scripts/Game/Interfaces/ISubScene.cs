namespace MathMatch.Game.Interfaces
{
    public interface ISubScene
    {
        bool IsActive { get; }
        void SetActive(bool value);
    }
}
