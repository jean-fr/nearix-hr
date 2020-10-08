module Nearix {
    export class PaginationManager {
        constructor(private application: Application) {
            this.$Pagination = $('.pagination-container');
        }

        public OnButtonClick: (ev: Event) => void;
        private $Pagination: JQuery;

        private BindEvents() {
            $(`a.pagination-btn`).on("click", (ev) => this.PaginateList(ev));
        }

        private PaginateList(ev: Event) {
            ev.preventDefault();
            if (this.OnButtonClick) {
                this.OnButtonClick(ev);
            }
        }

        public Render(model: Models.Pagination) {
            this.$Pagination.empty();
            if (model.IsNeeded) {
                this.$Pagination.html(this.application.Templates.Pagination(model));
                this.BindEvents();
            }
        }

        private UpdateState(currentPage?: number, totalPgs?: number) {
            if (Utils.isNull(currentPage)) {
                var $active = $(`#emp-pagination li.active`).first().children("a.pagination-btn").first();
                var page = $active.attr("data-page");
                currentPage = page && $.isNumeric(page) ? parseInt(page) : 1;
            }

            if (totalPgs == undefined) {
                var totalPages = $(`#emp-pagination input.ptotal-pages`).first().val();
                totalPgs = !Utils.stringIsNullOrEmpty(totalPages) && $.isNumeric(totalPages) ? parseInt(totalPages) : 0;
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
            } else {
                $prevBtn.children("a.pagination-btn").first().attr("data-page", "");
            }

            if ($nextBtn.length > 0 && !$nextBtn.hasClass("disabled") && next <= totalPgs) {
                $nextBtn.children("a.pagination-btn").first().attr("data-page", next);
            } else {
                $nextBtn.children("a.pagination-btn").first().attr("data-page", "");
            }
        }

        public Update(data: string) {
            $(`#emp-pagination`).replaceWith(data);
            this.UpdateState();
        }

        public Sync($target: JQuery, handler: (pageNumber: number) => Promise<boolean>) {
            var btnType = $target.attr("aria-label");
            var page = $target.attr("data-page");
            var pn = page && $.isNumeric(page) ? parseInt(page) : null;
            var totalPages = $(`#emp-pagination input.ptotal-pages`).first().val();
            var tPages = !Utils.stringIsNullOrEmpty(totalPages) && $.isNumeric(totalPages) ? parseInt(totalPages) : 0;
            if (handler && pn != null) {
                handler(pn).then(() => {
                    $target.parent("li").parent("ul").children("li").removeClass("active");
                    if (btnType) {
                        $(`#emp-pagination a.pagination-btn.nbtn[data-page='${pn}']`).first().parents("li").first().addClass("active");

                    } else {
                        //paginage by clicking numbers
                        $target.parent("li").addClass("active");
                    }
                    this.UpdateState(pn, tPages);
                });
            }
        }


    }
}