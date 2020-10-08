module Nearix.Interfaces {
    export interface ITemplates {
        [index: string]: (model: any) => string;
        Employees?(model: { Employees: Models.Employee[] }): string;   
        Pagination?(model: Models.Pagination): string;
    }

    export interface IFileResult {
         Success: boolean;
         Message: string;
    }

}