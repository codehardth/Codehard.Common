namespace Codehard.Common.DomainModel;

[Serializable]
public class DomainLogicException : Exception
{
    public DomainLogicException()
        : base()
    {
    }

    public DomainLogicException(string message)
        : base(message)
    {
    }

    public DomainLogicException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}