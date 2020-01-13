"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'File Code No.', field: 'fileCodeNo', maxWidth: 100 },
    { headerName: 'Tour Name', field: 'tourName' },
    { headerName: 'Country', field: 'country' },
    {
        headerName: 'Arrival Date', field: 'arrivalDate',
        cellRenderer: function (data) {
            return data.value.split('T')[0];
        }
    },
    {
        headerName: 'Departure Date', field: 'departureDate',
        cellRenderer: function (data) {
            return data.value.split('T')[0];
        }
    },
    { headerName: 'Agent', field: 'agent' },
    { headerName: 'Agent Staff', field: 'agentStaff' },
    { headerName: 'Guide', field: 'guideName' },
    {
        headerName: 'Edit', maxWidth: 80, sortable: false, filter: false,
        cellRenderer: function () {
            return '<i class="btn fas fa-edit" id="editButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            Edit(params.data);

        }
    },
    //{
    //    headerName: 'Delete', maxWidth: 80, sortable: false, filter: false,
    //    cellRenderer: function () {
    //        return '<i class="btn fas fa-trash" id="trashButton"></i>';
    //    },
    //    onCellClicked(params) {
    //        console.log(params.data);
    //        Delete(params.data);

    //    }
    //}
];

//Function to set the data for the grid
const setGridData = () => {

    $.ajax({
        url: 'Customer/Get',
        method: 'GET',
        success: (data) => {
            gridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};


//Settings for the Customer grid
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
    $('#tourName').val('');
    $('#country').val('');
    $('#arrivalDate').val('');
    $('#departureDate').val('');
    $('#agent').val('');
    $('#agentStaff').val('');
    $('#guideName').val('');
};

//Function specifying rules for validating the form
const customerValidation = () => {

    $('#customerForm').validate({
        rules: {
            tourName: {
                required: true,
                maxlength: 100,
                isOnlyWhiteSpace: true
            },
            country: {
                required: true,
                isOnlyWhiteSpace: true
            },
            agent: {
                isOnlyWhiteSpace: true
            },
            agentStaff: {
                isOnlyWhiteSpace: true
            },
            guideName: {
                isOnlyWhiteSpace: true
            },
            arrivalDate: {
                required: true,
                lesserDate: '#departureDate'
            },
            departureDate: {
                required: true,
                greaterDate: '#arrivalDate'               
            }
        }
    });
};

const call = () => {
    let filter = {
        fileCodeNo: { type: 'contains', filter: $('#searchFieldFileCode').val() },   
        tourName: { type: 'contains', filter: $('#searchFieldTourName').val() },        
        agent: { type: 'contains', filter: $('#searchFieldAgent').val() }
    };
    gridOptions.api.setFilterModel(filter);
    gridOptions.api.onFilterChanged();
};   

const Edit = (data) => {

    Clear();
    $('#customerTitle').html("Edit Customer");
    $('#id').val(data.id);
    $('#tourName').val(data.tourName);
    $('#country').val(data.country);
    $('#arrivalDate').val(data.arrivalDate.split('T')[0]);
    $('#departureDate').val(data.departureDate.split('T')[0]);
    $('#agent').val(data.agent);
    $('#agentStaff').val(data.agentStaff);
    $('#guideName').val(data.guideName);
    $('#customerForm').validate().destroy();
    customerValidation();
    $('#customerForm').validate().resetForm();
    $('#createCustomer').modal('toggle');
};

const Delete = (data) => {

    var confirm = window.confirm("Are you sure you want to delete?");

    if (confirm) {
        $.ajax({
            url: 'Customer/Delete',
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

    $('#customerForm').off('submit').on('submit', function (e) {

        e.preventDefault();

        var record = {
            Id: $('#id').val(),
            TourName: $('#tourName').val(),
            Country: $('#country').val(),
            ArrivalDate: $('#arrivalDate').val(),
            DepartureDate: $('#departureDate').val(),
            Agent: $('#agent').val(),
            AgentStaff: $('#agentStaff').val(),
            GuideName: $('#guideName').val()
        };

        $.ajax({
            url: 'Customer/Save',
            method: 'POST',
            data: { customer: record },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });
                $('#createCustomer').modal('toggle');
                setGridData();
            }
        });

    });
};

$(document).ready(function () {
    var customerGrid = document.querySelector('#customerGrid');

    new agGrid.Grid(customerGrid, gridOptions);

    setGridData();

    $('#addCustomerBtn').click(function () {
        console.log('Button Pressed');
        $('#customerTitle').html("Add Customer");
        Clear();
        $('#customerForm').validate().destroy();
        customerValidation();
        $('#customerForm').validate().resetForm();
    });

    $('#btnSave').off('click').on('click', function () {
        if ($('#customerForm').valid()) {
            Save();
        }
    });


    $('#searchFieldFileCode').on('keyup', function () {
        call();
    });

    $('#searchFieldTourName').on('keyup', function () {
        call();
    });

    $('#searchFieldAgent').on('keyup', function () {
        call();
    });
});