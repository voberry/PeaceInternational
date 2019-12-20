"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Name', field: 'name' },
    { headerName: 'Min PAX', field: 'minPAX' },
    { headerName: 'Max PAX', field: 'maxPAX' },
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
        url: 'Transport/Get',
        method: 'GET',
        success: (data) => {
            gridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};


//Settings for the Transport grid
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
    $('#minPAX').val('');
    $('#maxPAX').val('');
    $('#fullDayRate').val('');
    $('#halfDayRate').val('');
};

//Function specifying rules for validating the form
const transportValidation = () => {

    $('#transportForm').validate({
        rules: {
            name: {
                required: true,
                maxlength: 100,
                isOnlyWhiteSpace: true
            },
            minPAX: {
                required: true,
                digits: true
            },
            maxPAX: {
                required: true,
                digits: true
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
    $('#transportTitle').html("Edit Transport");
    $('#id').val(data.id);
    $('#name').val(data.name);
    $('#minPAX').val(data.minPAX);
    $('#maxPAX').val(data.maxPAX);
    $('#fullDayRate').val(data.fullDayRate);
    $('#halfDayRate').val(data.halfDayRate);
    $('#transportForm').validate().destroy();
    transportValidation();
    $('#transportForm').validate().resetForm();
    $('#createTransport').modal('toggle');
};

const Delete = (data) => {

    var confirm = window.confirm("Are you sure you want to delete?");

    if (confirm) {
        $.ajax({
            url: 'Transport/Delete',
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

    $('#transportForm').off('submit').on('submit', function (e) {

        e.preventDefault();

        var record = {
            Id: $('#id').val(),
            Name: $('#name').val(),
            MinPAX: $('#minPAX').val(),
            MaxPAX: $('#maxPAX').val(),
            FullDayRate: $('#fullDayRate').val(),
            HalfDayRate: $('#halfDayRate').val()
        };

        $.ajax({
            url: 'Transport/Save',
            method: 'POST',
            data: { transport: record },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });
                $('#createTransport').modal('toggle');
                setGridData();
            }
        });

    });
};

$(document).ready(function () {
    var transportGrid = document.querySelector('#transportGrid');

    new agGrid.Grid(transportGrid, gridOptions);

    setGridData();

    $('#addTransportBtn').click(function () {
        console.log('Button Pressed');
        $('#transportTitle').html("Add Transport");
        Clear();
        $('#transportForm').validate().destroy();
        transportValidation();
        $('#transportForm').validate().resetForm();
    });

    $('#btnSave').off('click').on('click', function () {
        if ($('#transportForm').valid()) {
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