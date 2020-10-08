
USE NearixHR;
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND type in (N'U'))
BEGIN
    CREATE TABLE Employee (
        id INT PRIMARY KEY IDENTITY (1, 1),
        employee_id INT NOT NULL,
        first_name NVARCHAR(50) NOT NULL,
        last_name NVARCHAR(50) NOT NULL,
        user_name NVARCHAR(50) NOT NULL,
        password NVARCHAR(max) NOT NULL,
        name_prefix VARCHAR(7) NOT NULL,
        middle_initial VARCHAR(2) NOT NULL,
        gender VARCHAR(5) NOT NULL,
        email NVARCHAR(50) NOT NULL,
        father_name NVARCHAR(50) NOT NULL,
        mother_name NVARCHAR(50) NOT NULL,
         mother_maiden_name NVARCHAR(50) NOT NULL,
        date_of_birth DATE NOT NULL,
        time_of_birth TIME NOT NULL,
        age_in_years FLOAT NOT NULL,
        weight_in_kgs FLOAT NOT NULL,
        date_of_joining DATETIME2 NOT NULL,
        quarter_of_joining VARCHAR(2) NOT NULL,
        half_of_joining VARCHAR(2) NOT NULL,
        year_of_joining INT NOT NULL,
        month_of_joining INT NOT NULL,
        month_name_of_joining VARCHAR(15) NOT NULL,
        short_month VARCHAR(5) NOT NULL,
        day_of_joining INT NOT NULL,
        dow_of_joining VARCHAR(15) NOT NULL,
        short_dow VARCHAR(5) NOT NULL,
        age_in_company_in_years FLOAT NOT NULL,
        salary DECIMAL NOT NULL,
        ssn NVARCHAR(50) NOT NULL,
        last_hike FLOAT NOT NULL,
        phone_number VARCHAR(20) NOT NULL,
        place_name NVARCHAR(50) NOT NULL,
        county NVARCHAR(50) NOT NULL,
        city NVARCHAR(50) NOT NULL,
        state NVARCHAR(50) NOT NULL,
        zip NVARCHAR(50) NOT NULL,
        region NVARCHAR(50) NOT NULL,

        CONSTRAINT UC_email UNIQUE([email]),
        CONSTRAINT UC_username UNIQUE([user_name]),
        CONSTRAINT UC_employeeId UNIQUE([employee_id])
    )
END

GO
CREATE NONCLUSTERED INDEX IX_first_name ON [NearixHR].[dbo].[Employee] ([first_name]) 
CREATE NONCLUSTERED INDEX IX_last_name ON [NearixHR].[dbo].[Employee] ([last_name]) 
CREATE NONCLUSTERED INDEX IX_user_name ON [NearixHR].[dbo].[Employee] ([user_name]) 
CREATE NONCLUSTERED INDEX IX_email ON [NearixHR].[dbo].[Employee] ([email]) 
CREATE NONCLUSTERED INDEX IX_employee_id ON [NearixHR].[dbo].[Employee] ([employee_id]) 
CREATE NONCLUSTERED INDEX IX_name_prefix ON [NearixHR].[dbo].[Employee] (name_prefix) 
CREATE NONCLUSTERED INDEX IX_middle_initial ON [NearixHR].[dbo].[Employee] (middle_initial) 
CREATE NONCLUSTERED INDEX IX_gender ON [NearixHR].[dbo].[Employee] (gender) 
CREATE NONCLUSTERED INDEX IX_father_name ON [NearixHR].[dbo].[Employee] (father_name) 
CREATE NONCLUSTERED INDEX IX_mother_maiden_name ON [NearixHR].[dbo].[Employee] (mother_maiden_name) 
CREATE NONCLUSTERED INDEX IX_mother_name ON [NearixHR].[dbo].[Employee] (mother_name)
CREATE NONCLUSTERED INDEX IX_date_of_birth ON [NearixHR].[dbo].[Employee] (date_of_birth) 
CREATE NONCLUSTERED INDEX IX_time_of_birth ON [NearixHR].[dbo].[Employee] (time_of_birth) 
CREATE NONCLUSTERED INDEX IX_age_in_years ON [NearixHR].[dbo].[Employee] (age_in_years) 
CREATE NONCLUSTERED INDEX IX_weight_in_kgs ON [NearixHR].[dbo].[Employee] (weight_in_kgs) 
CREATE NONCLUSTERED INDEX IX_date_of_joining ON [NearixHR].[dbo].[Employee] (date_of_joining) 
CREATE NONCLUSTERED INDEX IX_quarter_of_joining ON [NearixHR].[dbo].[Employee] (quarter_of_joining) 
CREATE NONCLUSTERED INDEX IX_half_of_joining ON [NearixHR].[dbo].[Employee] (half_of_joining) 
CREATE NONCLUSTERED INDEX IX_year_of_joining ON [NearixHR].[dbo].[Employee] (year_of_joining) 
CREATE NONCLUSTERED INDEX IX_month_of_joining ON [NearixHR].[dbo].[Employee] (month_of_joining) 
CREATE NONCLUSTERED INDEX IX_month_name_of_joining ON [NearixHR].[dbo].[Employee] (month_name_of_joining) 
CREATE NONCLUSTERED INDEX IX_short_month ON [NearixHR].[dbo].[Employee] (short_month) 
CREATE NONCLUSTERED INDEX IX_day_of_joining ON [NearixHR].[dbo].[Employee] (day_of_joining) 
CREATE NONCLUSTERED INDEX IX_dow_of_joining ON [NearixHR].[dbo].[Employee] (dow_of_joining) 
CREATE NONCLUSTERED INDEX IX_short_dow ON [NearixHR].[dbo].[Employee] (short_dow) 
CREATE NONCLUSTERED INDEX IX_age_in_company_in_years ON [NearixHR].[dbo].[Employee] (age_in_company_in_years) 
CREATE NONCLUSTERED INDEX IX_salary ON [NearixHR].[dbo].[Employee] (salary) 
CREATE NONCLUSTERED INDEX IX_ssn ON [NearixHR].[dbo].[Employee] (ssn) 
CREATE NONCLUSTERED INDEX IX_last_hike ON [NearixHR].[dbo].[Employee] (last_hike) 
CREATE NONCLUSTERED INDEX IX_phone_number ON [NearixHR].[dbo].[Employee] (phone_number) 
CREATE NONCLUSTERED INDEX IX_place_name ON [NearixHR].[dbo].[Employee] (place_name) 
CREATE NONCLUSTERED INDEX IX_county ON [NearixHR].[dbo].[Employee] (county) 
CREATE NONCLUSTERED INDEX IX_city ON [NearixHR].[dbo].[Employee] (city) 
CREATE NONCLUSTERED INDEX IX_state ON [NearixHR].[dbo].[Employee] ([state]) 
CREATE NONCLUSTERED INDEX IX_zip ON [NearixHR].[dbo].[Employee] (zip) 
CREATE NONCLUSTERED INDEX IX_region ON [NearixHR].[dbo].[Employee] (region) 

GO