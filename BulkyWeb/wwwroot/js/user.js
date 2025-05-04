var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#product-table-data').DataTable({
        ajax: {
            url: '/Admin/User/GetAll'
        },
        scrollY: 500,
        columns: [
            { data: "name", width: "15%" },
            { data: "email", width: "15%" },
            { data: "phoneNumber", width: "15%" },
            { data: "company", width: "20%" },
            { data: "role" },
            {
                data: null,
                width: "25%",
                render: function (data) {
                    if (data.isLocked) {
                        return `
                            <div class="text-center">
                                <a onclick="ToggleLock('${data.id}')" class="btn btn-success text-white" style="width: 100px; cursor: pointer;">
                                    <i class="bi bi-unlock-fill"></i> Unlock
                                </a>
                                <a href="/Admin/User/RoleManagement?userId=${data.id}" class="btn btn-warning text-white" style="width: 150px; cursor: pointer;">
                                    <i class="bi bi-pencil-square"></i> Permission
                                </a>
                            </div>
                        `;
                    } else {
                        return `
                            <div class="text-center">
                                <a onclick="ToggleLock('${data.id}')" class="btn btn-danger text-white" style="width: 100px; cursor: pointer;">
                                    <i class="bi bi-lock-fill"></i> Lock
                                </a>
                                <a href="/Admin/User/RoleManagement?userId=${data.id}" class="btn btn-warning text-white" style="width: 150px; cursor: pointer;">
                                    <i class="bi bi-pencil-square"></i> Permission
                                </a>
                            </div>
                        `;
                    }
                }
            }
        ]
    });
}

function ToggleLock(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/User/ToggleLock",
        contentType: "application/json",
        data: JSON.stringify(id),
        success: function (response) {
            if (response.success) {
                dataTable.ajax.reload();
                toastr.success(response.message);
            } else {
                toastr.error(response.message);
            }
        },
        error: function (error) {
            toastr.error(error.responseText);
        }
    });
}
