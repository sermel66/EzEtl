
namespace EZEtl.Configuration.Misc
{
    public static class Defaults
    {
        public const int BatchSizeRows = 1000000;
        public const int OneDebugMessagePerBatchCount = 0;
        public const int DbOperationTimeoutHardCodedDefault = 1500;
        public static int DbOperationTimeout = DbOperationTimeoutHardCodedDefault;
        public const ExistingDataActionEnum ExistingDataAction = ExistingDataActionEnum.Delete;
    }
}
