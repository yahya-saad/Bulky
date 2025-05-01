var dataTable;

$(document).ready(function () {
    var url = window.location.search.toLowerCase();
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    } else if (url.includes("completed")) {
        loadDataTable("completed");
    } else if (url.includes("pending")) {
        loadDataTable("pending");
    } else if (url.includes("approved")) {
        loadDataTable("approved");
    } else {
        loadDataTable();
    }
});


function loadDataTable(status = "all") {
    dataTable = $('#product-table-data').DataTable({
        ajax: {
            url: `/Admin/Order/GetAll?status=${status}`
        },
        scrollY: 500,
        columns: [
            { data: "id", width: "10%" },
            { data: "name", width: "20%" },
            { data: "phoneNumber", width: "15%" },
            { data: "applicationUser.email", width: "15%" },
            { data: "orderStatus", width: "10%" },
            { data: "orderTotal", width: "10%" },
            {
                data: "id",
                width: "10%",
                render: function (id) {
                    return `
                    <div class="text-center">
                        <div class="w-75 btn-group " role="group">
                            <a href="/Admin/Order/Details?orderId=${id}" class="btn btn-primary mx-2">
                                <i class="bi bi-eye-fill"></i>
                            </a>
                        </div>
                    </div >`;
                }
            }
        ]
    });
}
