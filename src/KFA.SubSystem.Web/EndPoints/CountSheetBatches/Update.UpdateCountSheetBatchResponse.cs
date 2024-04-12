namespace KFA.SubSystem.Web.EndPoints.CountSheetBatches;

public class UpdateCountSheetBatchResponse
{
  public UpdateCountSheetBatchResponse(CountSheetBatchRecord countSheetBatch)
  {
    CountSheetBatch = countSheetBatch;
  }

  public CountSheetBatchRecord CountSheetBatch { get; set; }
}
