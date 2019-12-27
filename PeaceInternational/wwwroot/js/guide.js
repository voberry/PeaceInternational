"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Name', field: 'name' },
    { headerName: 'Full Day Rate', field: 'fullDayRate' },
    { headerName: 'Half Day Rate', field: 'halfDayRate' },
    { headerName: 'Overnight', field: 'overNight' },
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
        url: 'Guide/Get',
        method: 'GET',
        success: (data) => {
            gridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};


//Settings for the Guide grid
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
    $('#fullDayRate').val('');
    $('#halfDayRate').val('');
    $('#overnight').val('');
};

//Function specifying rules for validating the form
const guideValidation = () => {

    $('#guideForm').validate({
        rules: {
            name: {
                required: true,
                maxlength: 100,
                isOnlyWhiteSpace: true
            },
            fullDayRate: {
                required: true,
                digits: true
            },
            halfDayRate: {
                required: true,
                digits: true
            },
            overnight: {
                required: true,
                digits: true
            }
        }
    });
};

const Edit = (data) => {

    Clear();
    $('#guideTitle').html("Edit Guide");
    $('#id').val(data.id);
    $('#name').val(data.name);
    $('#fullDayRate').val(data.fullDayRate);
    $('#halfDayRate').val(data.halfDayRate);
    $('#overnight').val(data.overnight);
    $('#guideForm').validate().destroy();
    guideValidation();
    $('#guideForm').validate().resetForm();
    $('#createGuide').modal('toggle');
};

const Delete = (data) => {

    var confirm = window.confirm("Are you sure you want to delete?");

    if (confirm) {
        $.ajax({
            url: 'Guide/Delete',
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

    $('#guideForm').off('submit').on('submit', function (e) {

        e.preventDefault();

        var record = {
            Id: $('#id').val(),
            Name: $('#name').val(),
            FullDayRate: $('#fullDayRate').val(),
            HalfDayRate: $('#halfDayRate').val(),
            Overnight: $('#overnight').val()
        };

        $.ajax({
            url: 'Guide/Save',
            method: 'POST',
            data: { guide: record },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });
                $('#createGuide').modal('toggle');
                setGridData();
            }
        });

    });
};

$(document).ready(function () {
    var guideGrid = document.querySelector('#guideGrid');

    new agGrid.Grid(guideGrid, gridOptions);

    setGridData();

    $('#addGuideBtn').click(function () {
        console.log('Button Pressed');
        $('#guideTitle').html("Add Guide");
        Clear();
        $('#guideForm').validate().destroy();
        guideValidation();
        $('#guideForm').validate().resetForm();
    });

    $('#btnSave').off('click').on('click', function () {
        if ($('#guideForm').valid()) {
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