<%@ Page Title="" Language="C#" MasterPageFile="~/Template/CMS.master" AutoEventWireup="true" CodeFile="Product_Upload.aspx.cs" Inherits="Admin_Product_Product_Upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="Server">
    <link href="/theme/plugins/daterangepicker/daterangepicker.css" rel="stylesheet" />
    <link href="/theme/plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.css" rel="stylesheet" type="text/css" />
    <link href="/theme/plugins/timepicker/bootstrap-material-datetimepicker.css" rel="stylesheet">
    <link href="/theme/plugins/select2/select2.min.css" rel="stylesheet" type="text/css" />

    <!-- DataTables -->
    <link href="/theme/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    <link href="/theme/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    <!-- Responsive datatable examples -->
    <link href="/theme/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    <style>
        .displayNone {
            display: none;
        }

        .displayNotNone {
            display: normal;
        }
    </style>
    <style>
        .accordion {
            background-color: #eee;
            color: #444;
            cursor: pointer;
            padding: 18px;
            width: 100%;
            border: none;
            text-align: left;
            outline: none;
            font-size: 15px;
            transition: 0.4s;
        }

            .active, .accordion:hover {
                background-color: #ccc;
            }

        .panel {
            padding: 0 18px;
            display: none;
            background-color: white;
            overflow: hidden;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderBody" runat="Server">

    <form runat="server" id="frm1" class="form-parsley">
        <div class="container-fluid">
            <!-- Page-Title -->
            <div class="row">
                <div class="col-sm-12">
                    <div class="page-title-box">
                        <div class="float-right">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item active"><%=title %></li>
                                <li class="breadcrumb-item"><a href="Product_List">Quản lý sản phẩm</a></li>
                                <li class="breadcrumb-item"><a href="/">Trang chủ</a></li>
                            </ol>
                        </div>
                        <h4 class="page-title"><%=title %></h4>
                        <asp:Label runat="server" ID="lblMessage1" CssClass="msg-sc-edit" ClientIDMode="Static" Visible="false"></asp:Label>
                    </div>
                    <!--end page-title-box-->
                </div>
                <!--end col-->
            </div>

            <div class="row">
                <div class="col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="form-group">
                                <a href='Product_DownloadExcel.aspx' target="_blank"><i class="mdi mdi-download font-16"></i>Tải file nhập mẫu</a>
                            </div>
                            <h4>Thêm mới sản phẩm bằng file Excel</h4>
                            <div class="form-group">
                                <label>Chọn file upload</label>
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </div>
                            <div class="form-group mb-0">
                                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-gradient-primary waves-effect waves-light" Text="Lưu" />
                                <asp:Button ID="btnBack" OnClick="btnBack_Click" UseSubmitBehavior="false" runat="server" ClientIDMode="Static" Text="Quay lại" CssClass="btn btn-gradient-danger waves-effect m-l-5"></asp:Button>
                            </div>

                            <!--end form-group-->

                            <!--end form-->
                        </div>
                        <!--end card-body-->
                    </div>
                    <!--end card-->
                </div>
                <div class="col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="form-group">
                                <a href='Product_DownloadExcel.aspx?Action=data' target="_blank"><i class="mdi mdi-download font-16"></i>Tải file dữ liệu</a>
                            </div>
                            <h4>Cập nhật sản phẩm bằng file Excel</h4>
                            <div class="form-group">
                                <label>Chọn file update</label>
                                <asp:FileUpload ID="FileUpload2" runat="server" />
                            </div>
                            <div class="form-group mb-0">
                                <asp:Button runat="server" ID="Button1" OnClick="btnSave1_Click" CssClass="btn btn-gradient-primary waves-effect waves-light" Text="Cập nhật" />
                                <asp:Button ID="Button2" OnClick="btnBack_Click" UseSubmitBehavior="false" runat="server" ClientIDMode="Static" Text="Quay lại" CssClass="btn btn-gradient-danger waves-effect m-l-5"></asp:Button>
                            </div>
                            <!--end form-group-->
                            <!--end form-->
                        </div>
                        <!--end card-body-->
                    </div>

                    <!--end card-->
                </div>
                <div class="col-lg-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="accordion">Hướng dẫn thao tác với file</div>
                            <div class="panel" style="display:block;">
                                <p class="mt-3">
                                    1. KHI UPDATE FILE EXCEL CHỈ SỬA NHỮNG TRƯỜNG THÔNG TIN THAY ĐỔI. KHÔNG ĐƯỢC XÓA CÁC CỘT FILE CÓ SẴN <br />
                                    2. NHẬP SẢN PHẨM DÁN 2 TEM --> KHÔNG ĐƯỢC TRÙNG MÃ SẢN PHẨM VỚI SẢN PHẨM MUỐN CẬP NHẬT
                                    3. Khi sản phẩm không thuộc Bộ sản phẩm nào ---> Nhập = 0 (Kiểu số)
                                    4. Đối với những cột nhập nhiều ID, cách nhau bởi dấu ","  --> Nếu nhập 1 ID duy nhất trong cột --> Chuyển sang định dạng TEXT
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblMessage" CssClass="msg-sc-edit" ClientIDMode="Static" Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lblMessageErr" CssClass="msg-err-edit" ClientIDMode="Static" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>

            <%--  </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>

    </form>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceBottom" runat="Server">
    <!-- Required datatable js -->
    <script src="/theme/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/theme/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <!-- Buttons examples -->
    <script src="/theme/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/theme/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="/theme/plugins/datatables/jszip.min.js"></script>
    <script src="/theme/plugins/datatables/pdfmake.min.js"></script>
    <script src="/theme/plugins/datatables/vfs_fonts.js"></script>
    <script src="/theme/plugins/datatables/buttons.html5.min.js"></script>
    <script src="/theme/plugins/datatables/buttons.print.min.js"></script>
    <script src="/theme/plugins/datatables/buttons.colVis.min.js"></script>
    <!-- Responsive examples -->
    <script src="/theme/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/theme/plugins/datatables/responsive.bootstrap4.min.js"></script>
    <script src="/theme/assets/pages/jquery.datatable.init.js"></script>
    <script src="/theme/plugins/select2/select2.min.js"></script>
    <%--<script src="/theme/assets/pages/jquery.forms-advanced.js"></script>--%>
    <!-- Responsive-table-->
    <script src="/theme/plugins/RWD-Table-Patterns/dist/js/rwd-table.min.js"></script>
    <script src="/theme/assets/pages/jquery.responsive-table.init.js"></script>
    <script>
        $(window).on('load', function () {
            setTimeout(function () { $('#spinner').fadeOut(); }, 200);
        })
        $(document).ready(function () {
            setTimeout(function () { $('#lblMessage').fadeOut(); }, 5000);
            setTimeout(function () { $('#lblMessageErr').fadeOut(); }, 5000);
            if (typeof (Sys) !== 'undefined') {
                var parameter = Sys.WebForms.PageRequestManager.getInstance();
                parameter.add_endRequest(function () {
                    setTimeout(function () { $('#lblMessage').fadeOut(); }, 2000);
                    setTimeout(function () { $('#lblMessageErr').fadeOut(); }, 2000);
                });
            }
        });
    </script>
    <script>
        var acc = document.getElementsByClassName("accordion");
        var i;
        for (i = 0; i < acc.length; i++) {
            acc[i].addEventListener("click", function () {
                this.classList.toggle("active");
                var panel = this.nextElementSibling;
                if (panel.style.display === "block") {
                    panel.style.display = "none";
                } else {
                    panel.style.display = "block";
                }
            });
        }
    </script>
    <div id="spinner" runat="server" class="spinner" style="display: none;">
        <p class="textload">Hệ thống đang xử lý, vui lòng chờ</p>
        <img src="/images/logo-xuanhoa.jpg?v=2" class="img-load" />
    </div>
</asp:Content>

