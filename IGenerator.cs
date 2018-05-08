namespace CodeGenerator
{
    public interface IGenerator<out TOut, in TIn>
    {
        TOut Generate(TIn description);
    }
}
