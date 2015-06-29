<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <title>Comino Document Browser | B&NES</title>
    <link href="//maxcdn.bootstrapcdn.com/bootswatch/3.3.5/paper/bootstrap.min.css" rel="stylesheet">
    <link href="//cdn.datatables.net/plug-ins/1.10.7/integration/bootstrap/3/dataTables.bootstrap.css" rel="stylesheet">
</head>
<body>
    <div class="jumbotron">
        <div class="container">
            <h1>Comino migration documents</h1>
            <p>A viewer for Revs and Benefits documents migrated from Comino</p>
        </div>
    </div>
    <div class="container">
        <table id="tblData" class="table">
            <thead>
                <tr class="gridStyle">
                    <th>Created Date</th>
                    <th>Document Label</th>
                    <!--<th>Document Label Code</th>
                    <th>Document Label Guid</th>-->
                    <th>File Location</th>
                    <!--<th>File Reference</th>-->
                    <th>Reference</th>
                    <!--<th>File Link</th>-->
                </tr>
            </thead>
            <tbody></tbody>
        </table>
    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="//cdn.datatables.net/1.10.7/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.7/integration/bootstrap/3/dataTables.bootstrap.js" type="text/javascript"></script>
    <script>

        $.ajaxSetup({
            cache: false
        });

        var table = $('#tblData').DataTable({
            "filter": true,
            "pagingType": "simple_numbers",
            "orderClasses": false,
            "order": [[0, "asc"]],
            "info": false,
            "bProcessing": true,
            "bServerSide": true,
            "sAjaxSource": "Default.aspx/GetTableData",
            "fnServerData": function (sSource, aoData, fnCallback) {
                $.ajax({
                    "dataType": 'json',
                    "contentType": "application/json; charset=utf-8",
                    "type": "GET",
                    "url": sSource,
                    "data": aoData,
                    "success": function (msg) {
                        var json = jQuery.parseJSON(msg.d);
                        fnCallback(json);
                        $("#tblData").show();
                    },
                    error: function (xhr, textStatus, error) {
                        if (typeof console == "object") {
                            console.log(xhr.status + "," + xhr.responseText + "," + textStatus + "," + error);
                        }
                    }
                });
            },
            fnDrawCallback: function () {
                $('.image-details').bind("click", showDetails);
            }
        });

        function showDetails() {
            //so something funky with the data
        }
    </script>
</body>
</html>
