IF EXISTS (SELECT * FROM sys.procedures WHERE name='sp_employee_exists') 
BEGIN
   DROP PROCEDURE  dbo.sp_employee_exists
END
GO
CREATE PROCEDURE  dbo.sp_employee_exists
@employee_id INT
AS
BEGIN
SELECT COUNT(id) FROM Employee WHERE employee_id=@employee_id
END
GO
