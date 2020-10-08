/// <reference path="../typings/jquery/jquery.d.ts" />
module Nearix {

    export class Application {
        constructor() {
            this.LoadTemplates().done(data => {
                this.Templates = data;
            });
            this.$Employeestable = $("#empl-table");
            this.$ImportCard = $("#import-card");

            if (this.$Employeestable && this.$Employeestable.length > 0) {
                this.PaginationManager = new PaginationManager(this);
                this.PaginationManager.OnButtonClick = (ev: Event) => this.OnPaginate(ev);
                this.$ExportButton = $("#export-file-btn");
                this.DoSearchInternal(this.GetSearchData(), true);
            }
            if (this.$ImportCard && this.$ImportCard.length > 0) {
                this.$UploadButton = $("#upload-file-btn");
                this.$ImportButton = $("#import-file-btn");
                this.$ImportFileInput = $('#import-file-input');
                this.$Spinner = $("#spinner-cont img");
            }
            this.BindEvents();
        }

        public Templates: Interfaces.ITemplates;
        private $UploadButton: JQuery;
        private $ImportButton: JQuery;
        private $ExportButton: JQuery;
        private $ImportFileInput: JQuery;
        private PaginationManager: PaginationManager;
        private $Employeestable: JQuery;
        private $ImportCard: JQuery;
        private PAGE_SIZE: number = 50;
        private CurrentFileName: string;
        private $Spinner: JQuery;

        //Keep it simple for the sake of this test
        private BindEvents() {
            if (this.$Employeestable && this.$Employeestable.length > 0) {
                this.$Employeestable.children('thead').first().off("click").on("click", "th[data-column-name]", (ev: Event) => this.SortTable(ev));
                this.$Employeestable.find('tr#nh-tbl-filters').find('input.nh-filter').off("keyup").on('keyup', (ev: Event) => this.DoSearch(ev));
                this.$Employeestable.find('tr#nh-tbl-filters').find('input.nh-filter[type="date"]').off("change").on('change', (ev: Event) => this.DoSearchOnClick(ev));
                $(document).off("click", '#empl-search-btn', (ev: Event) => this.DoSearchOnClick(ev)).on('click', '#empl-search-btn', (ev: Event) => this.DoSearchOnClick(ev));
                $(document).off("click", '#empl-search-input', (ev: Event) => this.DoSearch(ev)).on('keyup', '#empl-search-input', (ev: Event) => this.DoSearch(ev));
                this.$ExportButton.off().on('click', (ev: Event) => this.Export(ev));
            }
            if (this.$ImportCard && this.$ImportCard.length > 0) {
                this.$UploadButton.off().on('click', (ev: Event) => this.Upload(ev));
                this.$ImportButton.off().on('click', (ev: Event) => this.Import(ev));
                this.$ImportFileInput.change((ev: any) => {
                    var $fileElement: any = $(ev.target)[0];
                    $(ev.target).next('label').text($fileElement.files[0].name);
                });
            }
        }

        

        private OnPaginate(ev: Event) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            this.PaginationManager.Sync($(ev.currentTarget), (pageNumber: number) => this.Paginate(pageNumber));
        }

        private Paginate(pageNumber: number): Promise<boolean> {
            var search: Models.SearchModel = this.GetSearchData();
            search.Skip = (pageNumber - 1) * this.PAGE_SIZE;
            return this.DoSearchInternal(search, false);
        }

        private Upload(ev: Event) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            this.$Spinner.show();
            var $fileElement: any = $("#import-file-input")[0];
            if ($fileElement.files.length === 0) {
                alert("select a file");
                this.$Spinner.hide();
                return;
            }

            var formData = new FormData();
            formData.append("csvfile", $fileElement.files[0]);

            $.ajax({
                url: '/file/upload',
                method: 'POST',
                data: formData,
                processData: false,
                contentType: false,
            }).done((result: Interfaces.IFileResult) => {
                if (result && result.Success) {
                    this.CurrentFileName = result.Message;
                    $("#upload-file-container").hide();
                    this.$ImportButton.show();
                    this.$ImportFileInput.val(null);
                } else {
                    alert(result.Message); //keep it simple for the sake of this test
                }
                this.$Spinner.hide();
            }).fail(() => {
                this.$Spinner.hide();
                alert("Error");
            });
        }

        private Import(ev: Event) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            if (Utils.stringIsNullOrEmpty(this.CurrentFileName)) {
                alert("No file to import");
                return;
            }
            this.$ImportButton.prop('disabled', true);
            this.$Spinner.show();
            $.ajax({
                url: '/file/import',
                data: { fileName: this.CurrentFileName },
                type: 'POST',
                dataType: 'json'
            }).done((result: Interfaces.IFileResult) => {
                if (result.Success) {
                    this.CurrentFileName = "";
                    $("#upload-file-container").show();
                    this.$ImportButton.hide();
                    //display notif with alert, keep it simple for the sake of this test
                    alert(`Data imported successfully | ${result.Message}`);
                } else {
                    //display error, keep it simple for the sake of this test
                    alert(result.Message);
                }
                this.$ImportButton.prop('disabled', false);
                this.$Spinner.hide();
            }).fail(() => { this.$ImportButton.prop('disabled', false); this.$Spinner.hide(); });
        }

        private Export(ev: Event) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            this.$ExportButton.prop('disabled', true);
            $.ajax({
                url: '/file/export',
                data: this.GetSearchData(),
                type: 'POST',
                dataType: 'json'
            }).done((result: Interfaces.IFileResult) => {
                if (result.Success) {
                    window.location.href = `/download/exported/${result.Message}`;

                } else {
                    //display error, keep it simple for the sake of this test
                    alert(result.Message);
                }
                this.$ExportButton.prop('disabled', false);
                //spinner off
            }).fail(() => {
                alert("An error has occurred");
                this.$ExportButton.prop('disabled', false);
            });
        }

        private UpdateSortTable($target: JQuery, sortOrder: string, columnName: string) {
            if (Utils.stringIsNullOrEmpty(columnName) || Utils.stringIsNullOrEmpty(sortOrder)) return;

            $target.attr("data-sort-order", sortOrder);

            let $tableTHead = $("#empl-table").children('thead').first();
            var $th = $tableTHead.find(`th[data-column-name="${columnName}"]`);
            if ($th.length > 0) {
                $tableTHead.find(".empl-sort").removeClass("fa-sort-asc fa-sort-desc").addClass("fa-sort");
                $th.find(".empl-sort").removeClass("fa-sort").addClass(`fa-sort-${sortOrder}`);
            }
        }

        private SortTable(ev: Event) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            var $target = $(ev.target);

            var sortOrder = $target.attr("data-sort-order");
            var columnName = $target.attr("data-column-name");

            var search = this.GetSearchData();

            if (Utils.stringIsNullOrEmpty(sortOrder)) {
                sortOrder = 'asc';
                search.SortOrder = Models.SortOrder.Asc;
            } else {
                if (sortOrder == 'asc') {
                    sortOrder = 'desc';
                    search.SortOrder = Models.SortOrder.Desc;
                } else {
                    sortOrder = 'asc';
                    search.SortOrder = Models.SortOrder.Asc;
                }
            }

            search.SortBy = columnName;
            this.UpdateSortTable($target, sortOrder, columnName);
            this.DoSearchInternal(search, false);
        }

        private DoSearch(ev: any) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            if (ev.keyCode === 13) {
                let $target = $(ev.target);
                if ($target.attr('data-complex-col') === 'true') {
                    if (Utils.stringIsNullOrEmpty(this.ValidateComplexFilter($target.val()))) {
                        $target.val("");
                    }
                    else {
                        this.DoSearchInternal(this.GetSearchData(), true);
                    }               
                } else {
                    this.DoSearchInternal(this.GetSearchData(), true);
                }

            }
        }

        private DoSearchOnClick(ev: Event) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            this.DoSearchInternal(this.GetSearchData(), true);
        }

        private GetSearchData(): Models.SearchModel {
            var search: Models.SearchModel = new Models.SearchModel();
            search.Take = this.PAGE_SIZE;
            search.Query = $("#empl-search-input").val();

            /*filters*/
            this.$Employeestable.find('tr#nh-tbl-filters').find('input.nh-filter').each((i: number, e: HTMLElement) => {
                let $e = $(e);
                if (!Utils.stringIsNullOrEmpty($e.val())) {
                    let filter: Models.SearchFilter = new Models.SearchFilter();
                    filter.FieldName = $e.attr('data-column-name');
                    let iscomplex = $e.attr("data-complex-col") === "true";
                    filter.Operator = iscomplex ? this.GetFilterOperator($e.val()) : Models.FilterOperator.Equals;
                    filter.Value = iscomplex ? parseFloat(this.ValidateComplexFilter($e.val())) : $e.val();
                    search.Filters.push(filter);
                }

            });
            return search;
        }
        private GetFilterOperator(value: string): Models.FilterOperator {
            if (value.indexOf(">") === 0) return Models.FilterOperator.Greater;
            if (value.indexOf("<") === 0) return Models.FilterOperator.Less;
            return Models.FilterOperator.Equals;
        }

        private DoSearchInternal(search: Models.SearchModel, newSeach: boolean): Promise<boolean> {
            return new Promise((resolve, reject) => {
                if (!search) {
                    resolve(true);
                    return;
                }
                $.ajax({
                    url: '/employee/search',
                    data: search,
                    type: 'POST',
                    dataType: 'json'
                }).done((result: Models.SearchResult) => {
                    if (result.Success) {
                        this.$Employeestable.find('tbody').html(this.Templates.Employees({ Employees: result.Employees }));
                        if (newSeach) {
                            this.PaginationManager.Render(result.Pagination);
                        }
                        if (result.Employees.length > 0) {
                            this.$ExportButton.show();
                        } else {
                            this.$ExportButton.hide();
                        }
                    } else {
                        //display error, keep it simple for the sake of this test
                        alert('Error on search');
                    }
                    resolve(result.Success);
                }).fail((xhr: JQueryXHR, textStatus: string) => {
                    console.log(`Error : ${textStatus} | ${xhr.statusText}`);
                    reject();
                });
            })
        }

        private LoadTemplates(): JQueryPromise<Interfaces.ITemplates> {
            var deferred = $.Deferred<Interfaces.ITemplates>();
            var results: Interfaces.ITemplates = {};
            $.get("/Scripts/Template/templates.html", (data: string) => {
                if (!Utils.stringIsNullOrEmpty(data)) {
                    var datas: HTMLScriptElement[] = $.parseHTML(data);
                    if (datas.length > 0) {
                        for (var i = 0; i < datas.length; i++) {
                            var id = datas[i].id;
                            if (id) {
                                try {
                                    results[id] = _.template(datas[i].innerHTML);
                                }
                                catch (e) {
                                    console.error(`TemplateName: ${id} | Error : ${e.stack}`);
                                }
                            }
                        }
                    }
                    deferred.resolve(results);
                }
            }, 'html');

            return deferred.promise();
        }

        /**
         Validate complex filter input. e.g. n < or n> or n= (n is number)
         */
        private ValidateComplexFilter(input: string): string {
            if (Utils.stringIsNullOrEmpty(input)) return "";
            let countG = (input.match(/>/g) || []).length;
            let countL = (input.match(/</g) || []).length;
            let countE = (input.match(/=/g) || []).length;
            if (countE > 1 || countG > 1 || countL > 1) return ""; //only one
            if (countE === 0 && countG === 0 && countL === 0) return "";//must contain at least one of them
            if ((input.includes('>') && input.includes('<')) ||
                (input.includes('>') && input.includes('=')) ||
                (input.includes('<') && input.includes('=')) ||
                (input.includes('>') && input.includes('<') && input.includes('='))
            ) return ""; //only one
            if (input.indexOf('>') > 0 || input.indexOf('<') > 0 || input.indexOf('=') > 0) return "";//must start with

            let m = input.replace('>', '');
            m = m.replace('<', '');
            m = m.replace('=', '');

            /**check alphabet, must be only numeric here */
            m = m.replace('/\s+/g', '');
            var patt = /[A-Z]/gi;
            if (patt.test(m)) return "";

            if (!_.all(m.split(''), s => { return /[0-9]/.test(s); })) return "";
            return m;
        }
    }
}