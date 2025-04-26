var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#product-table-data').DataTable({
        ajax: {
            url: '/Admin/Company/GetAll'
        },
        scrollY: 500,
        columns: [
            { data: "name", width: "25%" },
            { data: "streetAddress", width: "15%" },
            { data: "city", width: "10%" },
            { data: "state", width: "15%" },
            { data: "phoneNumber" },
            {
                data: "id",
                width: "25%",
                render: function (id) {
                    return `
                    <div class="text-center">
                        <div class="w-75 btn-group " role="group">
                            <a href="/Admin/Company/Upsert/${id}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a  class="btn btn-danger mx-2" onClick="FireSweetAlert(${id})" >
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>
                    </div >`;
                }
            }
        ]
    });
}


// sweatAlert
const swal = Swal.mixin({
    customClass: {
        confirmButton: "btn btn-danger mx-2",
        cancelButton: "btn btn-light"
    },
    buttonsStyling: false
});


function FireSweetAlert(id) {
    swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete!",
        cancelButtonText: "No, cancel!",
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Admin/Company/Delete/${id}`,
                method: "DELETE",
                success: function () {
                    toastr.success("Deleted Successfully");
                    dataTable.ajax.reload();
                },
                error: fireOnError
            });
        }
    });
}

function fireOnError() {
    swalWithBootstrapButtons.fire({
        title: "Oops!",
        text: "Something went wrong.",
        icon: "error"
    });
}