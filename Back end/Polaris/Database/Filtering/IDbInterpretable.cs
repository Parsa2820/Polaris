namespace Database.Filtering
{
    public interface IDbInterpretable<TQueryContainer>
    {
        TQueryContainer Interpret();
    }
}