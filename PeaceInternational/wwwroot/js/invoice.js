"use strict";

//Column defination for InvoiceInfo Grid
const invoiceColumnDefs = [

    { headername: 'InvoiceNo', field: 'invoiceNo' },
    { headername: 'Dr', field: 'dr' },
    { headername: 'Agent Name', field: 'agentName' },
    { headername: 'Client Name', field: 'clientName' },
    { headername: 'Currency', field: 'currency' },
    { headername: 'PAX', field: 'pax' },
    {
        headername: 'Details',
        cellRenderer: function () {
            return '<i class="btn fas fa-clipboard" id="detailsButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            InvoiceDetails(params.data.Id);
        }
    }
];


//Column defination for InvoiceDetails Grid
const invoiceDetailColumnDefs = [

    { headername: 'Particulars', field: 'particulars' },
    { headername: 'Amount', field: 'amount' }

];

//Function to set the data for the Invoice grid
const setInvoiceGridData = () => {

    $.ajax({
        url: 'Invoice/GetInvoice',
        method: 'GET',
        success: (data) => {
            gridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};


//Function to set the data for the Invoice Detail grid
const setInvoiceDetailGridData = (invoiceId) => {

    $.ajax({
        url: 'Invoice/GetInvoiceDetail',
        method: 'GET',
        data: { invoiceId: invoiceId },
        success: (data) => {
            invoiceDetailGridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};

//Settings for the Invoice grid
const gridOptions = {
    columnDefs: invoiceColumnDefs,
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

//Settings for the Invoice Detail grid
const invoiceDetailGridOptions = {
    columnDefs: invoiceDetailColumnDefs,
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


//Invoice Details
const InvoiceDetails = (invoiceId) => {

    setInvoiceDetailGridData(invoiceId);

};

