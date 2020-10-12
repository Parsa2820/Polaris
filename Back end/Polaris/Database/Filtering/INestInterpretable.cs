using Nest;

namespace Database.Filtering
{
    public interface INestInterpretable
    {
        QueryContainer Interpret();
    }
}