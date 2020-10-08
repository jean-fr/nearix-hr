var Nearix;
(function (Nearix) {
    class Application {
        constructor() {
            this.PAGE_SIZE = 50;
            this.LoadTemplates().done(data => {
                this.Templates = data;
            });
            this.$Employeestable = $("#empl-table");
            this.$ImportCard = $("#import-card");
            if (this.$Employeestable && this.$Employeestable.length > 0) {
                this.PaginationManager = new Nearix.PaginationManager(this);
                this.PaginationManager.OnButtonClick = (ev) => this.OnPaginate(ev);
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
        BindEvents() {
            if (this.$Employeestable && this.$Employeestable.length > 0) {
                this.$Employeestable.children('thead').first().off("click").on("click", "th[data-column-name]", (ev) => this.SortTable(ev));
                this.$Employeestable.find('tr#nh-tbl-filters').find('input.nh-filter').off("keyup").on('keyup', (ev) => this.DoSearch(ev));
                this.$Employeestable.find('tr#nh-tbl-filters').find('input.nh-filter[type="date"]').off("change").on('change', (ev) => this.DoSearchOnClick(ev));
                $(document).off("click", '#empl-search-btn', (ev) => this.DoSearchOnClick(ev)).on('click', '#empl-search-btn', (ev) => this.DoSearchOnClick(ev));
                $(document).off("click", '#empl-search-input', (ev) => this.DoSearch(ev)).on('keyup', '#empl-search-input', (ev) => this.DoSearch(ev));
                this.$ExportButton.off().on('click', (ev) => this.Export(ev));
            }
            if (this.$ImportCard && this.$ImportCard.length > 0) {
                this.$UploadButton.off().on('click', (ev) => this.Upload(ev));
                this.$ImportButton.off().on('click', (ev) => this.Import(ev));
                this.$ImportFileInput.change((ev) => {
                    var $fileElement = $(ev.target)[0];
                    $(ev.target).next('label').text($fileElement.files[0].name);
                });
            }
        }
        OnPaginate(ev) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            this.PaginationManager.Sync($(ev.currentTarget), (pageNumber) => this.Paginate(pageNumber));
        }
        Paginate(pageNumber) {
            var search = this.GetSearchData();
            search.Skip = (pageNumber - 1) * this.PAGE_SIZE;
            return this.DoSearchInternal(search, false);
        }
        Upload(ev) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            this.$Spinner.show();
            var $fileElement = $("#import-file-input")[0];
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
            }).done((result) => {
                if (result && result.Success) {
                    this.CurrentFileName = result.Message;
                    $("#upload-file-container").hide();
                    this.$ImportButton.show();
                    this.$ImportFileInput.val(null);
                }
                else {
                    alert(result.Message);
                }
                this.$Spinner.hide();
            }).fail(() => {
                this.$Spinner.hide();
                alert("Error");
            });
        }
        Import(ev) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            if (Nearix.Utils.stringIsNullOrEmpty(this.CurrentFileName)) {
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
            }).done((result) => {
                if (result.Success) {
                    this.CurrentFileName = "";
                    $("#upload-file-container").show();
                    this.$ImportButton.hide();
                    alert(`Data imported successfully | ${result.Message}`);
                }
                else {
                    alert(result.Message);
                }
                this.$ImportButton.prop('disabled', false);
                this.$Spinner.hide();
            }).fail(() => { this.$ImportButton.prop('disabled', false); this.$Spinner.hide(); });
        }
        Export(ev) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            this.$ExportButton.prop('disabled', true);
            $.ajax({
                url: '/file/export',
                data: this.GetSearchData(),
                type: 'POST',
                dataType: 'json'
            }).done((result) => {
                if (result.Success) {
                    window.location.href = `/download/exported/${result.Message}`;
                }
                else {
                    alert(result.Message);
                }
                this.$ExportButton.prop('disabled', false);
            }).fail(() => {
                alert("An error has occurred");
                this.$ExportButton.prop('disabled', false);
            });
        }
        UpdateSortTable($target, sortOrder, columnName) {
            if (Nearix.Utils.stringIsNullOrEmpty(columnName) || Nearix.Utils.stringIsNullOrEmpty(sortOrder))
                return;
            $target.attr("data-sort-order", sortOrder);
            let $tableTHead = $("#empl-table").children('thead').first();
            var $th = $tableTHead.find(`th[data-column-name="${columnName}"]`);
            if ($th.length > 0) {
                $tableTHead.find(".empl-sort").removeClass("fa-sort-asc fa-sort-desc").addClass("fa-sort");
                $th.find(".empl-sort").removeClass("fa-sort").addClass(`fa-sort-${sortOrder}`);
            }
        }
        SortTable(ev) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            var $target = $(ev.target);
            var sortOrder = $target.attr("data-sort-order");
            var columnName = $target.attr("data-column-name");
            var search = this.GetSearchData();
            if (Nearix.Utils.stringIsNullOrEmpty(sortOrder)) {
                sortOrder = 'asc';
                search.SortOrder = Nearix.Models.SortOrder.Asc;
            }
            else {
                if (sortOrder == 'asc') {
                    sortOrder = 'desc';
                    search.SortOrder = Nearix.Models.SortOrder.Desc;
                }
                else {
                    sortOrder = 'asc';
                    search.SortOrder = Nearix.Models.SortOrder.Asc;
                }
            }
            search.SortBy = columnName;
            this.UpdateSortTable($target, sortOrder, columnName);
            this.DoSearchInternal(search, false);
        }
        DoSearch(ev) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            if (ev.keyCode === 13) {
                let $target = $(ev.target);
                if ($target.attr('data-complex-col') === 'true') {
                    if (Nearix.Utils.stringIsNullOrEmpty(this.ValidateComplexFilter($target.val()))) {
                        $target.val("");
                    }
                    else {
                        this.DoSearchInternal(this.GetSearchData(), true);
                    }
                }
                else {
                    this.DoSearchInternal(this.GetSearchData(), true);
                }
            }
        }
        DoSearchOnClick(ev) {
            ev.stopImmediatePropagation();
            ev.preventDefault();
            this.DoSearchInternal(this.GetSearchData(), true);
        }
        GetSearchData() {
            var search = new Nearix.Models.SearchModel();
            search.Take = this.PAGE_SIZE;
            search.Query = $("#empl-search-input").val();
            this.$Employeestable.find('tr#nh-tbl-filters').find('input.nh-filter').each((i, e) => {
                let $e = $(e);
                if (!Nearix.Utils.stringIsNullOrEmpty($e.val())) {
                    let filter = new Nearix.Models.SearchFilter();
                    filter.FieldName = $e.attr('data-column-name');
                    let iscomplex = $e.attr("data-complex-col") === "true";
                    filter.Operator = iscomplex ? this.GetFilterOperator($e.val()) : Nearix.Models.FilterOperator.Equals;
                    filter.Value = iscomplex ? parseFloat(this.ValidateComplexFilter($e.val())) : $e.val();
                    search.Filters.push(filter);
                }
            });
            return search;
        }
        GetFilterOperator(value) {
            if (value.indexOf(">") === 0)
                return Nearix.Models.FilterOperator.Greater;
            if (value.indexOf("<") === 0)
                return Nearix.Models.FilterOperator.Less;
            return Nearix.Models.FilterOperator.Equals;
        }
        DoSearchInternal(search, newSeach) {
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
                }).done((result) => {
                    if (result.Success) {
                        this.$Employeestable.find('tbody').html(this.Templates.Employees({ Employees: result.Employees }));
                        if (newSeach) {
                            this.PaginationManager.Render(result.Pagination);
                        }
                        if (result.Employees.length > 0) {
                            this.$ExportButton.show();
                        }
                        else {
                            this.$ExportButton.hide();
                        }
                    }
                    else {
                        alert('Error on search');
                    }
                    resolve(result.Success);
                }).fail((xhr, textStatus) => {
                    console.log(`Error : ${textStatus} | ${xhr.statusText}`);
                    reject();
                });
            });
        }
        LoadTemplates() {
            var deferred = $.Deferred();
            var results = {};
            $.get("/Scripts/Template/templates.html", (data) => {
                if (!Nearix.Utils.stringIsNullOrEmpty(data)) {
                    var datas = $.parseHTML(data);
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
        ValidateComplexFilter(input) {
            if (Nearix.Utils.stringIsNullOrEmpty(input))
                return "";
            let countG = (input.match(/>/g) || []).length;
            let countL = (input.match(/</g) || []).length;
            let countE = (input.match(/=/g) || []).length;
            if (countE > 1 || countG > 1 || countL > 1)
                return "";
            if (countE === 0 && countG === 0 && countL === 0)
                return "";
            if ((input.includes('>') && input.includes('<')) ||
                (input.includes('>') && input.includes('=')) ||
                (input.includes('<') && input.includes('=')) ||
                (input.includes('>') && input.includes('<') && input.includes('=')))
                return "";
            if (input.indexOf('>') > 0 || input.indexOf('<') > 0 || input.indexOf('=') > 0)
                return "";
            let m = input.replace('>', '');
            m = m.replace('<', '');
            m = m.replace('=', '');
            m = m.replace('/\s+/g', '');
            var patt = /[A-Z]/gi;
            if (patt.test(m))
                return "";
            if (!_.all(m.split(''), s => { return /[0-9]/.test(s); }))
                return "";
            return m;
        }
    }
    Nearix.Application = Application;
})(Nearix || (Nearix = {}));
var Nearix;
(function (Nearix) {
    class PaginationManager {
        constructor(application) {
            this.application = application;
            this.$Pagination = $('.pagination-container');
        }
        BindEvents() {
            $(`a.pagination-btn`).on("click", (ev) => this.PaginateList(ev));
        }
        PaginateList(ev) {
            ev.preventDefault();
            if (this.OnButtonClick) {
                this.OnButtonClick(ev);
            }
        }
        Render(model) {
            this.$Pagination.empty();
            if (model.IsNeeded) {
                this.$Pagination.html(this.application.Templates.Pagination(model));
                this.BindEvents();
            }
        }
        UpdateState(currentPage, totalPgs) {
            if (Nearix.Utils.isNull(currentPage)) {
                var $active = $(`#emp-pagination li.active`).first().children("a.pagination-btn").first();
                var page = $active.attr("data-page");
                currentPage = page && $.isNumeric(page) ? parseInt(page) : 1;
            }
            if (totalPgs == undefined) {
                var totalPages = $(`#emp-pagination input.ptotal-pages`).first().val();
                totalPgs = !Nearix.Utils.stringIsNullOrEmpty(totalPages) && $.isNumeric(totalPages) ? parseInt(totalPages) : 0;
            }
            var prev = currentPage - 1;
            var next = currentPage + 1;
            var $prevBtn = $(`#emp-pagination li#prev-btn`);
            var $nextBtn = $(`#emp-pagination li#next-btn`);
            if (currentPage > 1) {
                $prevBtn.removeClass("disabled");
            }
            else {
                if (!$prevBtn.hasClass("disabled")) {
                    $prevBtn.addClass("disabled");
                }
            }
            if (currentPage < totalPgs) {
                $nextBtn.removeClass("disabled");
            }
            else {
                if (!$nextBtn.hasClass("disabled")) {
                    $nextBtn.addClass("disabled");
                }
            }
            if ($prevBtn.length > 0 && !$prevBtn.hasClass("disabled") && prev > 0) {
                $prevBtn.children("a.pagination-btn").first().attr("data-page", prev);
            }
            else {
                $prevBtn.children("a.pagination-btn").first().attr("data-page", "");
            }
            if ($nextBtn.length > 0 && !$nextBtn.hasClass("disabled") && next <= totalPgs) {
                $nextBtn.children("a.pagination-btn").first().attr("data-page", next);
            }
            else {
                $nextBtn.children("a.pagination-btn").first().attr("data-page", "");
            }
        }
        Update(data) {
            $(`#emp-pagination`).replaceWith(data);
            this.UpdateState();
        }
        Sync($target, handler) {
            var btnType = $target.attr("aria-label");
            var page = $target.attr("data-page");
            var pn = page && $.isNumeric(page) ? parseInt(page) : null;
            var totalPages = $(`#emp-pagination input.ptotal-pages`).first().val();
            var tPages = !Nearix.Utils.stringIsNullOrEmpty(totalPages) && $.isNumeric(totalPages) ? parseInt(totalPages) : 0;
            if (handler && pn != null) {
                handler(pn).then(() => {
                    $target.parent("li").parent("ul").children("li").removeClass("active");
                    if (btnType) {
                        $(`#emp-pagination a.pagination-btn.nbtn[data-page='${pn}']`).first().parents("li").first().addClass("active");
                    }
                    else {
                        $target.parent("li").addClass("active");
                    }
                    this.UpdateState(pn, tPages);
                });
            }
        }
    }
    Nearix.PaginationManager = PaginationManager;
})(Nearix || (Nearix = {}));
var Nearix;
(function (Nearix) {
    var Utils;
    (function (Utils) {
        function stringIsNullOrEmpty(str) {
            return _.isUndefined(str) || _.isNull(str) || $.trim(str) === '';
        }
        Utils.stringIsNullOrEmpty = stringIsNullOrEmpty;
        function isNull(str) {
            return _.isUndefined(str) || _.isNull(str);
        }
        Utils.isNull = isNull;
        function stringContains(source, subStr) {
            return source.indexOf(subStr) > -1;
        }
        Utils.stringContains = stringContains;
    })(Utils = Nearix.Utils || (Nearix.Utils = {}));
})(Nearix || (Nearix = {}));
var Nearix;
(function (Nearix) {
    var Models;
    (function (Models) {
        let SortOrder;
        (function (SortOrder) {
            SortOrder[SortOrder["Asc"] = 0] = "Asc";
            SortOrder[SortOrder["Desc"] = 1] = "Desc";
        })(SortOrder = Models.SortOrder || (Models.SortOrder = {}));
        let FilterOperator;
        (function (FilterOperator) {
            FilterOperator[FilterOperator["Equals"] = 0] = "Equals";
            FilterOperator[FilterOperator["Greater"] = 1] = "Greater";
            FilterOperator[FilterOperator["Less"] = 2] = "Less";
        })(FilterOperator = Models.FilterOperator || (Models.FilterOperator = {}));
        class Employee {
        }
        Models.Employee = Employee;
        class Pagination {
        }
        Models.Pagination = Pagination;
        class SearchModel {
            constructor() {
                this.Filters = [];
                this.Skip = 0;
                this.Take = 50;
                this.GetCount = true;
            }
        }
        Models.SearchModel = SearchModel;
        class SearchFilter {
        }
        Models.SearchFilter = SearchFilter;
        class SearchResult {
        }
        Models.SearchResult = SearchResult;
    })(Models = Nearix.Models || (Nearix.Models = {}));
})(Nearix || (Nearix = {}));
//# sourceMappingURL=nearixhr.js.map