CREATE FUNCTION [dbo].[Function1]
(
	@param1 int
)
RETURNS INTEGER
AS
BEGIN   
DECLARE @qty INT
SELECT @qty = 0
SELECT 
    @qty = MAX(SIF_KALK) 
FROM Kalkulacija 
WHERE SIF_KALK = @param1
RETURN 0
END;


