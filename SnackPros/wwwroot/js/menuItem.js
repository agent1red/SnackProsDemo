﻿var dataTable;


$(document).ready(function () {
    loadList();
    
});

function loadList() {
    dataTable = $('#DT-load').DataTable({

        "ajax": {
                "url": "/api/MenuItem",
               "type": "GET",
          "datatype" : "json"
        },

        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "price", "width": "5%" },
            { "data": "category.name", "width": "15%" },
            { "data": "snackType.name", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return ` <div class="text-center">
                                <a href="/Admin/MenuItem/upsert?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                    <i class="far fa-edit"></i> Edit
                                </a>
                                <a class="btn btn-danger text-white" style="cursor:pointer; width:100px;" onclick=Delete('/api/MenuItem/'+${data})>
                                    <i class="far fa-trash-alt"></i> Delete
                                </a>
                             </div>
                           
                           `;
                }, "width" : "20%"
            }
        ],
        "language": {
            "emptyTable" : "no data found."
        },
        "width": "100%",
        "order": [[0,"asc"]]
    });
}


function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}