namespace BonyadCode.Logger.AspNetCore;

public enum ELogType : byte
{
    Default,

    Startup,
    StartupException,

    TraceLog,
    TraceLogException,
    TraceLogFailure,

    Exception,
    ExceptionDatabase,
    ExceptionDataTamper,
    ExceptionFailure,

    Failure,
}