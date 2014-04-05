ALTER PROCEDURE dbo.AdHocLoad 
  @FeedName nvarchar(255)
 ,@FeedDate	datetime 
AS
-- EXEC dbo.AdHocLoad 
DECLARE @message nvarchar(4000) = 'AdHocLoad called with: @FeedName=' + ISNULL(@FeedName,'NULL')
	+ ' , @FeedDate=' + ISNULL( CAST(@FeedDate AS nvarchar(255)), 'NULL');
	
RAISERROR(@message,11,1);