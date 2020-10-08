/// <reference path="../typings/underscore/underscore.d.ts" />

module Nearix.Utils {
    
    export function stringIsNullOrEmpty(str) {
        return _.isUndefined(str) || _.isNull(str) || $.trim(str) === '';
    }
    export function isNull(str:any) {
        return _.isUndefined(str) || _.isNull(str);
    }
    export function stringContains(source: string, subStr: string) {
        return source.indexOf(subStr) > -1;
    }
}