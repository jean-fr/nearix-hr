module Nearix.Models {
    export enum SortOrder {
        Asc = 0,
        Desc = 1
    }
    export enum FilterOperator {
        Equals = 0,
        Greater = 1,
        Less = 2
    }
    export class Employee {
        public EmployeeId: number;
        public FirstName: string;
        public LastName: string;
        public UserName: string;
        public Password: string;
        public NamePrefix: string;
        public MiddleInitial: string;
        public Gender: string;
        public Email: string;
        public FatherName: string;
        public MotherName: string;
        public MotherMaidenName: string;
        public DateOfBirth: Date;
        public TimeOfBirth: string;
        public AgeInYears: number;
        public WeightInKgs: number;
        public DateOfJoining: Date;
        public QuarterOfJoining: string;
        public HalfOfJoining: string;
        public YearOfJoining: number;
        public MonthOfJoining: number;
        public MonthNameOfJoining: string;
        public ShortMonth: string;
        public DayOfJoining: number;
        public DowOfJoining: string;
        public ShortDow: string;
        public AgeInCompanyInYears: number;
        public Salary: number;
        public LastHike: number;
        public Ssn: string;
        public PhoneNumber: string;
        public PlaceName: string;
        public County: string;
        public City: string;
        public State: string;
        public Zip: string;
        public Region: string;

    }

    export class Pagination {
        public TotalCount: number;
        public CurrentPage: number;
        public PageNumber: number;
        public IsNeeded: boolean;
        public TotalPages: number;
    }

    export class SearchModel {
        constructor() {
            this.Filters = [];
            this.Skip = 0;
            this.Take = 50;/*default*/
            this.GetCount = true;/*since skip=0*/
        }
        public Query: string;
        public Skip: number;
        public Take: number;
        public SortOrder: SortOrder;
        public SortBy: string;
        public GetCount: boolean;
        public Filters: SearchFilter[];
    }

    export class SearchFilter {
        public FieldName: string;
        public Operator: FilterOperator;
        public Value: string;
    }

    export class SearchResult {
        public Employees: Employee[];
        public Success: boolean;
        public Pagination: Pagination;
    }

    
}