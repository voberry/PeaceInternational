"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Name', field: 'name', maxWidth: 500 },
    { headerName: 'Code', field: 'code', maxWidth: 200 },  
    {
        headerName: 'Edit', maxWidth: 200, sortable: false, filter: false,
        cellRenderer: function () {
            return '<i class="btn fas fa-edit" id="editButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            Edit(params.data);

        }
    },
    {
        headerName: 'Delete', maxWidth: 200, sortable: false, filter: false,
        cellRenderer: function () {
            return '<i class="btn fas fa-trash" id="trashButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            Delete(params.data);

        }
    }
];

//Function to set the data for the grid
const setGridData = () => {

    $.ajax({
        url: 'Sector/Get',
        method: 'GET',
        success: (data) => {
            gridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};


//Settings for the Sector grid
let gridOptions = {
    columnDefs: columnDefs,
    rowHeight: 40,
    defaultColDef: {
        sortable: true,
        filter: true
    },
    paginationAutoPageSize: true,
    pagination: true,
    accentedSort: true,
    onGridSizeChanged: (params) => {
        params.api.sizeColumnsToFit();
    }
};

//Function to clear form
const Clear = () => {
    removeBorderClass();
    $("#id").val('');
    $('#name').val('');
    $('#code').val('');   
    $('#t1fdcost').val('');   
    $('#t1hdcost').val('');
    $('#t2fdcost').val('');
    $('#t2hdcost').val('');
    $('#t3fdcost').val('');
    $('#t3hdcost').val('');
    $('#t4fdcost').val('');
    $('#t4hdcost').val('');
    $('#t5fdcost').val('');
    $('#t5hdcost').val('');
    $('#t6fdcost').val('');
    $('#t6hdcost').val('');   
};

//Function specifying rules for validating the form
const sectorValidation = () => {

    $('#sectorForm').validate({
        rules: {
            name: {
                required: true,
                maxlength: 100,
                isOnlyWhiteSpace: true
            },
            code: {
                required: true,
                maxlength: 15
            },
            t1fdcost: {
                required: true,
                digits: true
            },
            t1hdcost: {
                required: true,
                digits: true
            },
            t2fdcost: {
                required: true,
                digits: true
            },
            t2hdcost: {
                required: true,
                digits: true
            },
            t3fdcost: {
                required: true,
                digits: true
            },
            t3hdcost: {
                required: true,
                digits: true
            },
            t4fdcost: {
                required: true,
                digits: true
            },
            t4hdcost: {
                required: true,
                digits: true
            },
            t5fdcost: {
                required: true,
                digits: true
            },
            t5hdcost: {
                required: true,
                digits: true
            },
            t6fdcost: {
                required: true,
                digits: true
            },
            t6hdcost: {
                required: true,
                digits: true
            }
        }
    });
};

const Edit = (data) => {

    Clear();
    $('#sectorTitle').html("Edit Sector");
    $('#id').val(data.id);
    $('#name').val(data.name);
    $('#code').val(data.code);   
    $('#t1fdcost').val(data.sectorTransport[0].fullDayCost);   
    $('#t1hdcost').val(data.sectorTransport[0].halfDayCost);   
    $('#t2fdcost').val(data.sectorTransport[1].fullDayCost);
    $('#t2hdcost').val(data.sectorTransport[1].halfDayCost);   
    $('#t3fdcost').val(data.sectorTransport[2].fullDayCost);
    $('#t3hdcost').val(data.sectorTransport[2].halfDayCost);   
    $('#t4fdcost').val(data.sectorTransport[3].fullDayCost);
    $('#t4hdcost').val(data.sectorTransport[3].halfDayCost);   
    $('#t5fdcost').val(data.sectorTransport[4].fullDayCost);
    $('#t5hdcost').val(data.sectorTransport[4].halfDayCost);   
    $('#t6fdcost').val(data.sectorTransport[5].fullDayCost);
    $('#t6hdcost').val(data.sectorTransport[5].halfDayCost);   
    $('#sectorForm').validate().destroy();
    sectorValidation();
    $('#sectorForm').validate().resetForm();
    $('#createSector').modal('toggle');
};

const Delete = (data) => {

    var confirm = window.confirm("Are you sure you want to delete?");

    if (confirm) {
        $.ajax({
            url: 'Sector/Delete',
            method: 'POST',
            data: { id: data.id },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });

                setGridData();
            }
        });
    }
};


const Save = () => {

    $('#sectorForm').off('submit').on('submit', function (e) {

        e.preventDefault();

        var sector = {
            Id: $('#id').val(),
            Name: $('#name').val(),
            Code: $('#code').val()           
        };

        var sectorTransport = [
            {
                TransportId: 1,
                HalfDayCost: $('#t1hdcost').val(), 
                FullDayCost: $('#t1fdcost').val() 
                
            },
            {
                TransportId: 2,
                HalfDayCost: $('#t2hdcost').val(),
                FullDayCost: $('#t2fdcost').val()

            },
            {
                TransportId: 3,
                HalfDayCost: $('#t3hdcost').val(),
                FullDayCost: $('#t3fdcost').val()

            },
            {
                TransportId: 4,
                HalfDayCost: $('#t4hdcost').val(),
                FullDayCost: $('#t4fdcost').val()

            },
            {
                TransportId: 5,
                HalfDayCost: $('#t5hdcost').val(),
                FullDayCost: $('#t5fdcost').val()

            },
            {
                TransportId: 6,
                HalfDayCost: $('#t6hdcost').val(),
                FullDayCost: $('#t6fdcost').val()

            }
        ];

        var record = { sector, sectorTransport };

        $.ajax({
            url: 'Sector/Save',
            method: 'POST',
            data: { sectorDTO: record },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });
                $('#createSector').modal('toggle');
                setGridData();
            }
        });

    });
};

$(document).ready(function () {
    var sectorGrid = document.querySelector('#sectorGrid');

    new agGrid.Grid(sectorGrid, gridOptions);

    setGridData();

    $('#addSectorBtn').click(function () {
        console.log('Button Pressed');
        $('#sectorTitle').html("Add Sector");
        Clear();
        $('#sectorForm').validate().destroy();
        sectorValidation();
        $('#sectorForm').validate().resetForm();
    });

    $('#btnSave').off('click').on('click', function () {
        if ($('#sectorForm').valid()) {
            Save();
        }
    });

    $('#searchField').on('keyup', function () {
        var filter;
        filter = {
            name: { type: 'contains', filter: $('#searchField').val() }
        };
        gridOptions.api.setFilterModel(filter);
        gridOptions.api.onFilterChanged();
    });
});