namespace ContentAddicts.SharedTestUtils.Interfaces;

public interface IBuilderBase<out T> where T : class
{
    void Reset();
    T Build();
}
