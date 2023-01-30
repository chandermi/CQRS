using Solution.Infrastructure;


namespace Solution.Dipatcher
{

    public interface ICommandDispatcher
    {
        void Dispatch<T>(T command) where T : ICommandDisp;
    }
}
