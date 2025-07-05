DECLARE @ExecSQL AS NVARCHAR (MAX) = '';
SELECT @ExecSQL = @ExecSQL + 
       'DROP TABLE ' + QUOTENAME(S.name) + '.' + QUOTENAME(T.name) + '; ' 
FROM sys.tables T
JOIN sys.schemas S ON S.schema_id = T.schema_id
EXEC (@ExecSQL)