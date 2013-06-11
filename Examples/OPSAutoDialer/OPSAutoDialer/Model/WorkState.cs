namespace OPSAutoDialer.Model
{
    public enum WorkState
    {
        Init,
        InProgress,
        Success,
        HungUpByRemote,
        CallError,
        FileError,
        NotFound,
        Busy,
        RingingTimeExpired,
        Cancelled,
        WaitForValidDate
    }
}
