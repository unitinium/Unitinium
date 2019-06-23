namespace Unitinium
{
    public interface ILuaRuntimeExecution
    {
        int Id { get; }
        RuntimeExecutionState State { get; }
        object Result { get; }
    }

    public enum RuntimeExecutionState
    {
        Execution,
        Success
    }

    public interface IUnitiniumLuaRuntime
    {
        object Execute(string script, bool isCoroutine = false);
        ILuaRuntimeExecution GetExecution(int id);
        void UpdateCoroutines();
    }
}