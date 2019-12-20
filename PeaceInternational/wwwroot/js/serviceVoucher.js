"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Receipt No.', field: 'id', maxWidth: 120 },
    { headerName: 'File Code No.', field: 'fileCodeNo', maxWidth: 120 },
    {
        headerName: 'Hotel', field: 'hotel.name'
    },
    { headerName: 'Client Name', field: 'clientName', sortable: false, filter: false },
    {
        headerName: 'Reciept', maxWidth: 200, sortable: false, filter: false,
        cellRenderer: function () {
            return '<i class="btn fas fa-clipboard" id="receiptButton"></i>';
        },
        onCellClicked(params) {

            GenerateReceipt(params.data);
        }
    },
    {
        headerName: 'Edit', maxWidth: 200, sortable: false, filter: false,
        cellRenderer: function () {
            return '<i class="btn fas fa-edit" id="editButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            Edit(params.data);
        }
    }
];

//Function to set the data for the grid
const setGridData = () => {

    $.ajax({
        url: 'ServiceVoucher/Get',
        method: 'GET',
        success: (data) => {
            console.log(data);
            gridOptions.api.setRowData(data);
        }
    });
};


function call() {
    let filter = {
        id: { type: 'contains', filter: $('#searchFieldReceipt').val() },
        fileCodeNo: { type: 'contains', filter: $('#searchFieldFileCode').val() },   
        'hotel.name': { type: 'contains', filter: $('#searchFieldHotel').val() },
        clientName: { type: 'contains', filter: $('#searchFieldClientname').val() }
    };
    gridOptions.api.setFilterModel(filter);
    gridOptions.api.onFilterChanged();
}


//Settings for the ServiceVoucher grid
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

//Function to generate reeipt
const GenerateReceipt = (receiptData) => {
    console.log(receiptData);
    $.ajax({
        url: 'ServiceVoucher/GetServiceVoucher',
        method: 'GET',
        data: { id: receiptData.id },
        success: (data) => {
            data.serviceVoucher.exchangeOrderNo = data.serviceVoucher.exchangeOrderNo.split('/')[1];
            data.serviceVoucher.fileCodeNo = data.serviceVoucher.fileCodeNo.split('/')[1];
            console.log(data);
            console.log(receiptData);
            var source = document.getElementById("entry-template").innerHTML;
            var template = Handlebars.compile(source);
            var result = template(data);
            console.log(result);
            $('#receiptTemplate1').html(result);
            $('#receiptTemplate2').html(result);
            $('#viewReceipt').modal('toggle');
        }
    });

};



//Function to clear form
const Clear = () => {
    removeBorderClass();
    $("#id").val('');
    $('#date').val(new Date().toISOString().slice(0, 10));
    $('#exchangeOrderNo').val('');
    $('#fileCodeNo').val('');
    $('#hotel').val('');
    $('#clientName').val('');
    $('#pax').val('');
    $('#arrivalOn').val('');
    $('#from').val('');
    $('#arrivalFlight').val('');
    $('#departureOn').val('');
    $('#to').val('');
    $('#departureFlight').val('');
    $('#services').val('');
};

//Function specifying rules for validating the form
const serviceVoucherValidation = () => {

    $('#serviceVoucherForm').validate({
        rules: {
           
            fileCodeNo: {
                required: true,                
                checkFileCodeNo: true
            },
            hotel: {
                required: true
            },
            clientName: {
                required: true
            },
            pax: {
                required: true
            },
            arrivalOn: {
                required: true,
                lesserDate: '#departureOn'
            },
            from: {
                required: true
            },
            arrivalFlight: {
                required: true
            },
            departureOn: {
                required: true,
                greaterDate: '#arrivalOn'
            },
            to: {
                required: true
            },
            departureFlight: {
                required: true
            }
        }
    });
};

const Edit = (data) => {

    Clear();
    console.log(data);
    $('#serviceVoucherTitle').html("Edit Service Voucher");
    $('#id').val(data.id);
    $('#exchangeOrderNo').val(data.exchangeOrderNo);
    $('#fileCodeNo').val(data.fileCodeNo);
    $('#hotel').val(data.hotelId);
    $('#clientName').val(data.clientName);
    $('#pax').val(data.pax);
    $('#arrivalOn').val(data.arrivalDate.split('T')[0]);
    $('#from').val(data.from);
    $('#arrivalFlight').val(data.arrivalFlight);
    $('#departureOn').val(data.departureDate.split('T')[0]);
    $('#to').val(data.to);
    $('#departureFlight').val(data.departureFlight);
    $('#services').val(data.services);
    $('#serviceVoucherForm').validate().destroy();
    serviceVoucherValidation();
    $('#serviceVoucherForm').validate().resetForm();
    $('#createServiceVoucher').modal('toggle');
};


const Save = () => {

    $('#serviceVoucherForm').off('submit').on('submit', function (e) {

        e.preventDefault();

        var record = {
            Id: $('#id').val(),
            //ExchangeOrderNo: $('#exchangeOrderNo').val(),
            FileCodeNo: $('#fileCodeNo').val(),
            HotelId: $('#hotel').val(),
            ClientName: $('#clientName').val(),
            PAX: $('#pax').val(),
            ArrivalDate: $('#arrivalOn').val(),
            From: $('#from').val(),
            ArrivalFlight: $('#arrivalFlight').val(),
            DepartureDate: $('#departureOn').val(),
            To: $('#to').val(),
            DepartureFlight: $('#departureFlight').val(),
            Services: $('#services').val()
        };

        $.ajax({
            url: 'ServiceVoucher/Save',
            method: 'POST',
            data: { serviceVoucher: record },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });
                $('#createServiceVoucher').modal('toggle');
                setGridData();
            }
        });

    });
};

const setHotelDropdown = () => {
    $.ajax({
        url: 'Hotel/Get',
        method: 'GET',
        success: function (data) {
            let options = "";
            for (var i = 0; i < data.length; i++) {
                options += "<option value='" + data[i].id + "'>" + data[i].name + "</option>";
            }
            $('#hotel').append(options);
        }
    });
};

$(document).ready(function () {
    var serviceVoucherGrid = document.querySelector('#serviceVoucherGrid');

    new agGrid.Grid(serviceVoucherGrid, gridOptions);

    setGridData();

    setHotelDropdown();

    $('#addServiceVoucherBtn').click(function () {
        console.log('Button Pressed');
        $('#serviceVoucherTitle').html("Add Service Voucher");
        Clear();
        $('#serviceVoucherForm').validate().destroy();
        serviceVoucherValidation();
        $('#serviceVoucherForm').validate().resetForm();
    });
    $('#printInvoice').off('click').on('click', function () {

        html2canvas($("#invoiceBody")[0], {
            scale: 3
        }).then(function (canvas) {
            var myImage = canvas.toDataURL("image/png");
            var tWindow = window.open("");
            $(tWindow.document.body)
                .html("<img id='Image' src=" + myImage + " style='width:100%;'></img>")
                .ready(function () {
                    tWindow.focus();
                    tWindow.print();
                });
        });
    });
    $('#btnSave').off('click').on('click', function () {
        if ($('#serviceVoucherForm').valid()) {
            Save();
        }
    });

    $('#searchFieldFileCode').on('keyup', function () {
        call();
    });

    $('#searchFieldReceipt').on('keyup', function () {
        call();
    });

    $('#searchFieldHotel').on('keyup', function () {
        call();
    });

    $('#searchFieldClientname').on('keyup', function () {
        call();
    });

    $('#fileCodeNo').on('change', function () {
        $.ajax({
            url: 'Customer/Get',
            method: 'GET',
            data: { fileCodeNo: $('#fileCodeNo').val() },
            success: function (data) {
                console.log(data);
                $('#clientName').val(data.tourName);
                $('#arrivalOn').val(data.arrivalDate.split('T')[0]);
                $('#departureOn').val(data.departureDate.split('T')[0]);
            }
        });
    });
});