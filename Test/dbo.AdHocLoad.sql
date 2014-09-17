USE TestAssignment
GO

ALTER PROCEDURE dbo.AdHocLoad 
  @FeedName nvarchar(255)
 ,@FeedDate	datetime 
 ,@Message nvarchar(4000) OUTPUT
AS
-- EXEC dbo.AdHocLoad 
SET @Message = 'AdHocLoad called with: @FeedName=' + ISNULL(@FeedName,'NULL')
	+ ' , @FeedDate=' + ISNULL( CAST(@FeedDate AS nvarchar(255)), 'NULL');
	
	RETURN 10;