IF EXISTS (SELECT * FROM sys.procedures WHERE name='sp_employee_search') 
BEGIN
   DROP PROCEDURE  dbo.sp_employee_search
END
GO
CREATE PROCEDURE  dbo.sp_employee_search
@query NVARCHAR(max)=NULL,
--Filters
@nameprefix VARCHAR(7)=NULL,
@firstname NVARCHAR(50)=NULL,
@lastname NVARCHAR(50)=NULL,
@middleinitial VARCHAR(2)=NULL,
@gender VARCHAR(5)=NULL,
@email NVARCHAR(50)=NULL,
@fathername NVARCHAR(50)=NULL,
@mothername NVARCHAR(50)=NULL,
@mothermaidenname NVARCHAR(50)=NULL,
@dateofbirth DATE=NULL,
@timeofbirth TIME=NULL,
@ageinyears INT=NULL,
@ageinyears_optr VARCHAR(10)=NULL,
@weightinkgs FLOAT=NULL,
@weightinkgs_optr VARCHAR(10)=NULL,
@dateofjoining DATE=NULL,
@quarterofjoining VARCHAR(2)=NULL,
@halfofjoining VARCHAR(2)=NULL,
@yearofjoining  INT=NULL,
@yearofjoining_optr VARCHAR(10)=NULL,
@monthofjoining INT=NULL,
@monthnameofjoining VARCHAR(15)=NULL,
@shortmonth VARCHAR(5)=NULL,
@dayofjoining INT=NULL,
@dayofjoining_optr VARCHAR(10)=NULL,
@dowofjoining VARCHAR(15)=NULL,
@shortdow VARCHAR(5)=NULL,
@ageincompanyinyears  FLOAT=NULL,
@ageincompanyinyears_optr VARCHAR(10)=NULL,
@salary DECIMAL=NULL,
@salary_optr VARCHAR(10)=NULL,
@ssn NVARCHAR(50)=NULL,
@lasthike FLOAT=NULL,
@lasthike_optr VARCHAR(10)=NULL,
@phonenumber NVARCHAR(20)=NULL,
@placename NVARCHAR(50)=NULL,
@county NVARCHAR(50)=NULL,
@city NVARCHAR(50)=NULL,
@state NVARCHAR(50)=NULL,
@zip NVARCHAR(50)=NULL,
@region NVARCHAR(50)=NULL,
@username NVARCHAR(50)=NULL,

--Filers end
@skip INT=0,
@take INT=0,
@sort_by VARCHAR(20)=NULL,
@sort_order VARCHAR(5)
AS
BEGIN
DECLARE @_sort_by VARCHAR(50);
SET @_sort_by=ISNULL(@sort_by, 'id');

DECLARE @_sort_order VARCHAR(20);
SET @_sort_order=ISNULL(@sort_order,'ASC');

DECLARE @sorting VARCHAR(100);
SET @sorting = CONCAT(' ORDER BY ', @_sort_by,' ', @_sort_order);

DECLARE @sql NVARCHAR(max);
DECLARE @has_w bit;
SET @has_w=0;
--set with below

IF @query IS NULL
BEGIN
SET @sql ='SELECT * FROM Employee';
END
ELSE
BEGIN
SET @has_w=1;
SET @sql ='
SELECT * FROM Employee WHERE 
(
last_name LIKE ''%'+@query+'%'''+'
OR first_name LIKE ''%'+@query+'%'''+'
OR last_name LIKE ''%'+@query+'%'''+'
OR email LIKE ''%'+@query+'%'''+'
OR user_name LIKE ''%'+@query+'%'''+'
OR father_name LIKE ''%'+@query+'%'''+'
OR mother_name LIKE ''%'+@query+'%'''+'
OR mother_maiden_name LIKE ''%'+@query+'%'''+'
OR month_name_of_joining LIKE ''%'+@query+'%'''+'
OR place_name LIKE ''%'+@query+'%'''+'
OR county LIKE ''%'+@query+'%'''+'
OR city LIKE ''%'+@query+'%'''+'
OR region LIKE ''%'+@query+'%''
)';
END

--Filters
IF(@nameprefix IS NOT NULL)
BEGIN
DECLARE @qpref VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref=' AND';        
    END
    ELSE 
    BEGIN
      SET @qpref=' WHERE';
      SET @has_w=1;
    END

SET @sql+= @qpref + ' name_prefix LIKE ''%'+@nameprefix+'%''';
END

IF(@firstname IS NOT NULL)
BEGIN
DECLARE @qpref1 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref1=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref1=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref1 + ' first_name LIKE ''%'+@firstname+'%''';
END

IF(@lastname IS NOT NULL)
BEGIN
DECLARE @qpref2 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref2=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref2=' WHERE';
      SET @has_w=1;
    END
SET @sql+= @qpref2+' last_name LIKE ''%'+@lastname+'%''';
END

IF(@middleinitial IS NOT NULL)
BEGIN
DECLARE @qpref3 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref3=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref3=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref3+ ' middle_initial ='+ @middleinitial;
END

IF(@gender IS NOT NULL)
BEGIN
DECLARE @qpref4 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref4=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref4=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref4+ ' gender ='+ @gender;
END

IF(@email IS NOT NULL)
BEGIN
DECLARE @qpref5 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref5=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref5=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref5+ ' email LIKE ''%'+@email+'%''';
END

IF(@fathername IS NOT NULL)
BEGIN
DECLARE @qpref6 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref6=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref6=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref6+ ' father_name LIKE ''%'+@fathername+'%''';
END

IF(@mothername IS NOT NULL)
BEGIN
DECLARE @qpref7 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref7=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref7=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref7+ ' mother_name LIKE ''%'+@mothername+'%''';
END

IF(@mothermaidenname IS NOT NULL)
BEGIN
DECLARE @qpref8 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref8=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref8=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref8+ ' mother_maiden_name LIKE ''%'+@mothermaidenname+'%''';
END

IF(@quarterofjoining IS NOT NULL)
BEGIN
DECLARE @qpref9 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref9=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref9=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref9+ ' quarter_of_joining ='+@quarterofjoining;
END

IF(@halfofjoining IS NOT NULL)
BEGIN
DECLARE @qpref10 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref10=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref10=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref10+ ' half_of_joining ='+@halfofjoining;
END

IF(@shortmonth IS NOT NULL)
BEGIN
DECLARE @qpref11 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref11=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref11=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref11+ ' short_month LIKE ''%'+@shortmonth+'%''';
END

IF(@dowofjoining IS NOT NULL)
BEGIN
DECLARE @qpref12 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref12=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref12=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref12+ ' dow_of_joining LIKE ''%'+@dowofjoining+'%''';
END

IF(@shortdow IS NOT NULL)
BEGIN
DECLARE @qpref13 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref13=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref13=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref13+ ' short_dow LIKE ''%'+@shortdow+'%''';
END

IF(@ssn IS NOT NULL)
BEGIN
DECLARE @qpref14 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref14=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref14=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref14+ ' ssn LIKE ''%'+@ssn+'%''';
END

IF(@phonenumber IS NOT NULL)
BEGIN
DECLARE @qpref15 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref15=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref15=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref15+ ' phone_number LIKE ''%'+@phonenumber+'%''';
END

IF(@placename IS NOT NULL)
BEGIN
DECLARE @qpref16 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref16=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref16=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref16+ ' place_name LIKE ''%'+@placename+'%''';
END

IF(@county IS NOT NULL)
BEGIN
DECLARE @qpref17 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref17=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref17=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref17+ ' county LIKE ''%'+@county+'%''';
END

IF(@city IS NOT NULL)
BEGIN
DECLARE @qpref18 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref18=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref18=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref18+ ' city LIKE ''%'+@city+'%''';
END

IF(@state IS NOT NULL)
BEGIN
DECLARE @qpref19 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref19=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref19=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref19+ ' state LIKE ''%'+@state+'%''';
END

IF(@zip IS NOT NULL)
BEGIN
DECLARE @qpref20 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref20=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref20=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref20+ ' zip LIKE ''%'+@zip+'%''';
END

IF(@region IS NOT NULL)
BEGIN
DECLARE @qpref21 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref21=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref21=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref21+ ' region LIKE ''%'+@region+'%''';
END

IF(@username IS NOT NULL)
BEGIN
DECLARE @qpref22 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref22=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref22=' WHERE';
      SET @has_w=1;
    END
SET @sql+=@qpref22+ ' user_name LIKE ''%'+@username+'%''';
END

--Numeric filter
IF(@yearofjoining IS NOT NULL)
BEGIN
DECLARE @opr VARCHAR(10);
SET @opr=CASE WHEN @yearofjoining_optr = 'greater' THEN '>'
              WHEN @yearofjoining_optr = 'less' THEN '<'
              ELSE '='
              END;

DECLARE @qpref23 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref23=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref23=' WHERE';
      SET @has_w=1;
    END

SET @sql += CONCAT(@qpref23,' year_of_joining ', @opr, @yearofjoining) ;
END

IF(@salary IS NOT NULL)
BEGIN
DECLARE @opr1 VARCHAR(10);
SET @opr1=CASE WHEN @salary_optr = 'greater' THEN '>'
              WHEN @salary_optr = 'less' THEN '<'
              ELSE '='
              END;

DECLARE @qpref24 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref24=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref24=' WHERE';
      SET @has_w=1;
    END
SET @sql += CONCAT(@qpref24,' salary ', @opr1,@salary) ;
END

IF(@ageinyears IS NOT NULL)
BEGIN
DECLARE @opr2 VARCHAR(10);
SET @opr2=CASE WHEN @ageinyears_optr = 'greater' THEN '>'
              WHEN @ageinyears_optr = 'less' THEN '<'
              ELSE '='
              END;

DECLARE @qpref25 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref25=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref25=' WHERE';
      SET @has_w=1;
    END
SET @sql += CONCAT(@qpref25,' age_in_years ', @opr2, @ageinyears) ;
END	 

IF(@weightinkgs IS NOT NULL)
BEGIN
DECLARE @opr3 VARCHAR(10);
SET @opr3=CASE WHEN @weightinkgs_optr = 'greater' THEN '>'
              WHEN @weightinkgs_optr = 'less' THEN '<'
              ELSE '='
              END;
DECLARE @qpref26 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref26=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref26=' WHERE';
      SET @has_w=1;
    END
SET @sql += CONCAT(@qpref26,' weight_in_kgs ', @opr3, @weightinkgs) ;
END	 

IF(@dayofjoining IS NOT NULL)
BEGIN
DECLARE @opr4 VARCHAR(10);
SET @opr4=CASE WHEN @dayofjoining_optr = 'greater' THEN '>'
              WHEN @dayofjoining_optr = 'less' THEN '<'
              ELSE '='
              END;

DECLARE @qpref27 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref27=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref27=' WHERE';
      SET @has_w=1;
    END

SET @sql += CONCAT(@qpref27,' day_of_joining ', @opr4, @dayofjoining) ;
END	 

IF(@ageincompanyinyears IS NOT NULL)
BEGIN
DECLARE @opr5 VARCHAR(10);
SET @opr5=CASE WHEN @ageincompanyinyears_optr = 'greater' THEN '>'
              WHEN @ageincompanyinyears_optr = 'less' THEN '<'
              ELSE '='
              END;

DECLARE @qpref28 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref28=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref28=' WHERE';
      SET @has_w=1;
    END

SET @sql += CONCAT(@qpref28,' age_in_company_in_years ', @opr5, @ageincompanyinyears) ;
END	 

IF(@lasthike IS NOT NULL)
BEGIN
DECLARE @opr6 VARCHAR(10);
SET @opr6=CASE WHEN @lasthike_optr = 'greater' THEN '>'
              WHEN @lasthike_optr = 'less' THEN '<'
              ELSE '='
              END;

DECLARE @qpref29 VARCHAR(10);
IF (@has_w=1)
    BEGIN
        SET @qpref29=' AND';
    END
    ELSE 
    BEGIN
      SET @qpref29=' WHERE';
      SET @has_w=1;
    END

SET @sql += CONCAT(@qpref29,' last_hike ', @opr6, @lasthike) ;
END	 

SET @sql +=CONCAT(@sorting,' OFFSET ',@skip, ' ROWS FETCH NEXT ', @take , ' ROWS ONLY');

EXEC sp_executesql @sql;

END
GO


