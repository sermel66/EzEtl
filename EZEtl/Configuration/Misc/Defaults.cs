
namespace EZEtl.Configuration.Misc
{
    public static class Defaults
    {
        public const int BatchSizeRows = 1000000;
        public const int OneDebugMessagePerBatchCount = 0;
        public const int DbOperationTimeout = 1500;
        public const ExistingDataActionEnum ExistingDataAction = ExistingDataActionEnum.Delete;
    }
}
