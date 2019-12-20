"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Name', field: 'name', maxWidth: 300 },
    { headerName: 'Code', field: 'code', maxWidth: 200 },
    { headerName: 'Full Day Rate', field: 'fullDayRate' },
    { headerName: 'Half Day Rate', field: 'halfDayRate', sortable: false, filter: false },
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
    $('#fullDayRate').val('');
    $('#halfDayRate').val('');
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
            fullDayRate: {
                required: true,
                digits: true
            },
            halfDayRate: {
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
    $('#fullDayRate').val(data.fullDayRate);
    $('#halfDayRate').val(data.halfDayRate);
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

        var record = {
            Id: $('#id').val(),
            Name: $('#name').val(),
            Code: $('#code').val(),
            FullDayRate: $('#fullDayRate').val(),
            HalfDayRate: $('#halfDayRate').val()
        };

        $.ajax({
            url: 'Sector/Save',
            method: 'POST',
            data: { sector: record },
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