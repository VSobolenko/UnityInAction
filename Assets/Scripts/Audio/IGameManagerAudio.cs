namespace Audio
{
    public interface IGameManagerAudio
    {
        ManageStatus Status { get; }

        void Setup(NetworkService service);
    }
}