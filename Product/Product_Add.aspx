<%@ Page Title="" Language="C#" MasterPageFile="~/Template/CMS.master" AutoEventWireup="true" CodeFile="Product_Add.aspx.cs" Inherits="Product_Add" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="Server">
    <link href="/theme/plugins/daterangepicker/daterangepicker.css" rel="stylesheet" />
    <link href="/theme/plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.css" rel="stylesheet" type="text/css" />
    <link href="/theme/plugins/timepicker/bootstrap-material-datetimepicker.css" rel="stylesheet">
    <link href="/theme/plugins/select2/select2.min.css" rel="stylesheet" type="text/css" />
    <style>
        .class-label > label {
            margin-left: 5px;
            font-weight: bold;
            font-size: 15px;
        }

        #txtNoteAgencyNorthPrice::placeholder {
            opacity: 0.5;
            font: italic;
        }

        #txtNoteAgencySouthPrice::placeholder {
            opacity: 0.5;
            font: italic;
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
            <asp:ScriptManager runat="server" ID="up"></asp:ScriptManager>
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-lg-4">

                                        <div class="form-group">
                                            <label>Ngành, Nhóm sản phẩm <span class="red">*</span></label>
                                            <asp:DropDownList runat="server" ID="ddlCha" CssClass="form-control select2" data-parsley-required="true" data-parsley-allselected="true" data-parsley-required-message="Chưa chọn danh mục sản phẩm"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <asp:UpdatePanel runat="server" ID="up3">
                                            <ContentTemplate>
                                                <asp:Panel runat="server" ID="Panel1" Visible="false" class="form-group">
                                                    <label>Bộ sản phẩm </label>
                                                    <asp:TextBox runat="server" ID="txtProductSet" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="Panel2" class="form-group">
                                                    <label>Bộ sản phẩm </label>
                                                    <asp:DropDownList runat="server" ID="ddlProductSet" CssClass="form-control select2"></asp:DropDownList>
                                                </asp:Panel>
                                                <div class="form-group class-label m-0">
                                                    <asp:CheckBox runat="server" ID="openckProductSet" Text="Nhập bộ sản phẩm" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="ckShowProductSet_Checked" Checked="true" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%--<div class="form-group">
                                            <label>Bộ sản phẩm <span class="red">*</span></label>
                                            <asp:DropDownList runat="server" ID="ddlProductSet" CssClass="form-control select2"  data-parsley-required="true" data-parsley-allselected="true" data-parsley-required-message="chưa chọn bộ sản phẩm"></asp:DropDownList>
                                        </div>--%>
                                    </div>
                                </div>
                                <div class="row none">
                                    <div class="col-lg-4">
                                        <div class="form-group">
                                            <label>Sản phẩm Cha</label>
                                            <asp:DropDownList runat="server" ID="ddlProduct" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="form-group">
                                            <label>Cấp</label>
                                            <asp:DropDownList runat="server" ID="ddlLevel" Enabled="false" required data-parsley-required-message="Chưa chọn cấp danh mục" CssClass="form-control select2">
                                                <asp:ListItem Text="-- Chọn cấp --" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Cấp 1" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Cấp 2" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Cấp 3" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Cấp 4" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Cấp 5" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Cấp 6" Value="6"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-8">
                                        <div class="row">
                                            <div class="col-lg-6 ">
                                                <div class="form-group">
                                                    <label>SKU hệ thống effect <span class="red">*</span></label>
                                                    <asp:TextBox runat="server" ID="txtSKU" ClientIDMode="Static" CssClass="form-control" required data-parsley-required-message="Chưa nhập mã SKU sản phẩm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-6 ">
                                                <div class="form-group">
                                                    <label>SKU hiển thị Web <span class="red">*</span></label>
                                                    <asp:TextBox runat="server" ID="txtSkuWeb" ClientIDMode="Static" CssClass="form-control" required data-parsley-required-message="Chưa nhập mã SKU sản phẩm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="form-group">
                                                    <label>Tên sản phẩm <span class="red">*</span></label>
                                                    <asp:TextBox runat="server" ID="txtName" ClientIDMode="Static" CssClass="form-control" required data-parsley-required-message="Chưa nhập tên sản phẩm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-6 ">
                                                <div class="form-group">
                                                    <label>Thời gian bảo hành (ngày)</label>
                                                    <asp:TextBox runat="server" ID="txtWarranty" type="number" value="365" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <label>Ảnh (600px - 600px)</label>
                                        <br />

                                        <div style="margin: 5px 0px;">
                                            <a href="<%=avatar %>" target="_blank">
                                                <asp:Image ID="imganh" runat="server" ImageUrl='../../images/no-image-icon.png' Width="100px" />
                                            </a>
                                        </div>

                                        <asp:FileUpload ID="fulAnh" runat="server" ClientIDMode="Static" onchange="img();" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <div class="form-group">
                                            <div class="form-group">
                                                <%-- <label>Kích hoạt</label>--%>
                                                <div class="custom-control custom-checkbox">
                                                    <asp:CheckBox runat="server" ID="ckOpenBH" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="ckShow1_Checked" />
                                                    <label for="ckOpenBH" class="custom-control-label">
                                                        Kích hoạt sản phẩm 2 tem
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-4" runat="server" id="spbh" visible="false">
                                        <div class="form-group">
                                            <label>Sản phẩm đi kèm sản phẩm kích hoạt 2 tem</label>
                                            <asp:ListBox runat="server" ClientIDMode="Static" ID="ddlProductBH" SelectionMode="Multiple" CssClass="select2 form-control" Width="100%" data-placeholder="&nbsp;&nbsp;-- Chọn sản phẩm đi kèm  --"></asp:ListBox>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <div class="form-group">
                                            <label>Sản phẩm đi kèm</label>
                                            <asp:ListBox runat="server" ClientIDMode="Static" ID="ddlProductAttach" SelectionMode="Multiple" CssClass="select2 form-control" Width="100%" data-placeholder="&nbsp;&nbsp;-- Chọn sản phẩm đi kèm  --"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="form-group">
                                            <label>Không gian sản phẩm</label>
                                            <asp:ListBox runat="server" ClientIDMode="Static" ID="ddlProductSpace" SelectionMode="Multiple" CssClass="select2 form-control" Width="100%" data-placeholder="&nbsp;&nbsp;-- Chọn không gian sản phẩm  --"></asp:ListBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4" style="margin-top: 30px;">
                                        <div class="row">
                                            <div class="col-lg-4">
                                                <div class="form-group">
                                                    <%-- <label>Kích hoạt</label>--%>
                                                    <div class="custom-control custom-checkbox">
                                                        <asp:CheckBox runat="server" ID="ckisHot" ClientIDMode="Static" />
                                                        <label for="ckisHot" class="custom-control-label">
                                                            Is Hot
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-4">
                                                <div class="form-group">
                                                    <%-- <label>Kích hoạt</label>--%>
                                                    <div class="custom-control custom-checkbox">
                                                        <asp:CheckBox runat="server" ID="ckisNew" ClientIDMode="Static" />
                                                        <label for="ckisNew" class="custom-control-label">
                                                            Is New
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-4">
                                                <div class="form-group">
                                                    <%-- <label>Kích hoạt</label>--%>
                                                    <div class="custom-control custom-checkbox">
                                                        <asp:CheckBox runat="server" ID="ckisHome" ClientIDMode="Static" Checked="true" />
                                                        <label for="ckisHome" class="custom-control-label">
                                                            Hiển thị Web
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group class-label">
                                    <asp:CheckBox runat="server" ID="ckShow" Text="Thông tin chi tiết sản phẩm" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="ckShow_Checked" Checked="true" />

                                </div>
                                <asp:Panel runat="server" ID="pnOpen" CssClass="form-group p-2" Visible="true" BorderStyle="Solid" BorderWidth="1px">
                                    <div class="row">
                                        <div class=" col-lg-8">
                                            <div class="row">
                                                <div class="col-lg-6 ">
                                                    <div class="form-group">
                                                        <label>Đơn vị </label>
                                                        <asp:TextBox runat="server" ID="txtUnit" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">

                                                    <asp:UpdatePanel runat="server" ID="up1">
                                                        <ContentTemplate>

                                                            <asp:Panel runat="server" ID="pnColorUnavailable" class="form-group" Visible="false">
                                                                <label>Mã màu/Màu sản phẩm </label>
                                                                <asp:TextBox runat="server" ID="txtColor" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="pnColorAvailable" class="form-group">
                                                                <label>Mã màu/Màu sản phẩm </label>
                                                                <asp:DropDownList runat="server" ID="ddlColor" CssClass="form-control select2"></asp:DropDownList>
                                                            </asp:Panel>
                                                            <div class="form-group class-label m-0">
                                                                <asp:CheckBox runat="server" ID="ckOpenColor" Text="Nhập màu" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="ckShowColor_Checked" Checked="true" />
                                                            </div>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>

                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label>Phân Loại </label>
                                                        <asp:DropDownList runat="server" ID="ddlClassify" CssClass="form-control select2">
                                                            <%--<asp:ListItem Text="-- Phân loại --" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="1 Tầng" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="2 Tầng" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="3 Tầng" Value="3"></asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6 ">
                                                    <div class="form-group">
                                                        <label>Kích thước </label>
                                                        <asp:TextBox runat="server" ID="txtSize" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6 ">
                                                    <div class="form-group">
                                                        <label>Loại Hàng </label>
                                                        <asp:DropDownList runat="server" ID="ddlProductType" CssClass="form-control select2"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6 ">
                                                    <%--<asp:UpdatePanel runat="server" ID="Up2">
                                                        <ContentTemplate>
                                                            <asp:Panel runat="server" ID="pnMaterialUnavailable" Visible="false" class="form-group">
                                                                <label>Chất liệu sản phẩm </label>
                                                                <asp:TextBox runat="server" ID="txtMaterial" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="pnMaterialAvailable" class="form-group">
                                                                <label>Chất liệu sản phẩm </label>
                                                                <asp:DropDownList runat="server" ID="ddlMaterial" CssClass="form-control select2"></asp:DropDownList>
                                                            </asp:Panel>
                                                            <div class="form-group class-label m-0">
                                                                <asp:CheckBox runat="server" ID="ckOpenMaterial" Text="Nhập vật liệu" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="ckShowMaterial_Checked" Checked="true" />
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                                    <div class="form-group">
                                                        <label>Chất liệu sản phẩm</label>
                                                        <asp:ListBox runat="server" ClientIDMode="Static" ID="ddlMaterial" SelectionMode="Multiple" CssClass="select2 form-control" Width="100%" data-placeholder="&nbsp;&nbsp;-- Chọn chất liệu sản phẩm  --"></asp:ListBox>
                                                    </div>

                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label>Link nhúng Video </label>
                                                        <asp:TextBox runat="server" ID="txtVideo" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label>Trọng Lượng </label>
                                                        <asp:TextBox runat="server" ID="txtWeight" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label>Tags ( Mỗi tag cách nhau bởi dấu "," )</label>
                                                        <asp:TextBox runat="server" ID="txtTag" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <%--<div class="col-lg-6 none">
                                                    <div class="form-group">
                                                        <label>URL</label>
                                                        <asp:TextBox runat="server" ID="txtURL" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                        <small>Mặc định lấy theo tiêu đề khi để trống, lưu ý url không được trùng nhau, không chứa ký tự đặc biệt</small><br />
                                                        <small>Ví dụ đường dẫn đúng: "ban-ghe-truong-hoc-alpha-series-14580"</small>
                                                    </div>
                                                </div>--%>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label>Từ Khóa</label>
                                                        <asp:TextBox runat="server" ID="txtKeyword" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                        <small>Mỗi từ khóa cách nhau bởi dấu ','</small>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label>Seo Keyword</label>
                                                        <asp:TextBox runat="server" ID="txtSeokeyword" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label>Seo Description</label>
                                                        <asp:TextBox runat="server" ID="txtSeodescription" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="form-group">
                                                        <label>Giá sản phẩm </label>
                                                        <asp:TextBox runat="server" ID="txtPrice" ClientIDMode="Static" CssClass="form-control formatMoney"></asp:TextBox>
                                                    </div>

                                                </div>
                                                <div class="col-lg-12" style="margin-top: 27px;">
                                                    <div class="form-group">
                                                        <label>Giá đại lý miền bắc</label>
                                                        <asp:TextBox runat="server" ID="txtAgencyNorthPrice" ClientIDMode="Static" CssClass="form-control formatMoney"></asp:TextBox>
                                                        <asp:TextBox runat="server" ID="txtNoteAgencyNorthPrice" ClientIDMode="Static" CssClass="form-control mt-2" placeholder="Mô tả giá đại lý miền Bắc "></asp:TextBox>
                                                    </div>

                                                </div>
                                                <div class="col-lg-12  ">
                                                    <div class="form-group">
                                                        <label>Giá đại lý miền Trung, miền Nam</label>
                                                        <asp:TextBox runat="server" ID="txtAgencySouthPrice" ClientIDMode="Static" CssClass="form-control formatMoney"></asp:TextBox>
                                                        <asp:TextBox runat="server" ID="txtNoteAgencySouthPrice" ClientIDMode="Static" CssClass="form-control mt-2" placeholder="Mô tả giá đại lý miền Trung, miền Nam "></asp:TextBox>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="form-group">
                                    <label>Mô tả sản phẩm</label>
                                    <asp:TextBox runat="server" ID="txtNote" ClientIDMode="Static" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>CẬP NHẬT THÔNG TIN CHI TIẾT SẢN PHẨM</label>
                                    <CKEditor:CKEditorControl ID="txtContent" BasePath="/ckeditor/" runat="server">
                                    </CKEditor:CKEditorControl>
                                </div>
                                <div class="form-group none">
                                    <%-- <label>Kích hoạt</label>--%>
                                    <div class="custom-control custom-checkbox">
                                        <asp:CheckBox runat="server" ID="ckActive" ClientIDMode="Static" Checked="true" />
                                        <label for="ckActive" class="custom-control-label">
                                            KÍCH HOẠT
                                        </label>
                                    </div>
                                </div>
                                <div class="form-group mb-0">
                                    <asp:Button runat="server" ID="btnSave" OnClientClick="return validate();" OnClick="btnSave_Click" CssClass="btn btn-gradient-primary waves-effect waves-light" Text="Lưu" />
                                    <asp:Button ID="btnBack" OnClick="btnBack_Click" UseSubmitBehavior="false" runat="server" ClientIDMode="Static" Text="Quay lại" CssClass="btn btn-gradient-danger waves-effect m-l-5"></asp:Button>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" ID="lblMessage" CssClass="msg-sc-edit" ClientIDMode="Static" Visible="false"></asp:Label>
                                    <asp:Label runat="server" ID="lblMessageErr" CssClass="msg-err-edit" ClientIDMode="Static" Visible="false"></asp:Label>
                                </div>
                                <!--end form-group-->

                                <!--end form-->
                            </div>
                            <!--end card-body-->
                        </div>
                        <!--end card-->
                    </div>

                </div>
                <%--  </ContentTemplate>
            </asp:UpdatePanel>--%>
            </div>
            <asp:HiddenField runat="server" ID="hdImage" />
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceBottom" runat="Server">
    <script>
        function img() {
            var url = inputToURL(document.getElementById("<%=fulAnh.ClientID %>"));
            document.getElementById("<%=imganh.ClientID %>").src = url;
        }
        function inputToURL(inputElement) {
            var file = inputElement.files[0];
            return window.URL.createObjectURL(file);
        }
    </script>
    <!-- Parsley js -->
    <script src="/theme/plugins/parsleyjs/parsley.min.js"></script>
    <script src="/theme/assets/pages/jquery.validation.init.js"></script>

    <!----date---->
    <%--<script src="/theme/plugins/select2/select2.min.js"></script>--%>
    <script src="/theme/plugins/moment/moment.js"></script>
    <script src="/theme/plugins/bootstrap-maxlength/bootstrap-maxlength.min.js"></script>
    <script src="/theme/plugins/daterangepicker/daterangepicker.js"></script>
    <script src="/theme/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
    <script src="/theme/plugins/timepicker/bootstrap-material-datetimepicker.js"></script>
    <script src="/theme/plugins/bootstrap-touchspin/js/jquery.bootstrap-touchspin.min.js"></script>
    <%--<script src="/theme/assets/pages/jquery.forms-advanced.js"></script>--%>
    <script src="/theme/plugins/select2/select2.min.js"></script>
    <!--Wysiwig js-->
    <script src="/theme/plugins/tinymce/tinymce.min.js"></script>
    <script src="/theme/assets/pages/jquery.form-editor.init.js"></script>
    <!-- App js -->
    <script src="/theme/assets/js/jquery.core.js"></script>
    <script src="../../js/Function.js"></script>
    <script>

        $(window).on('load', function () {
            setTimeout(function () { $('#spinner').fadeOut(); }, 100);
        })
        function Init() {
            $("#ckActive").addClass("custom-control-input");
            $("#ckisHot").addClass("custom-control-input");
            $("#ckisHome").addClass("custom-control-input");
            $("#ckisNew").addClass("custom-control-input");
            $("#ckOpenBH").addClass("custom-control-input");
            setTimeout(function () { $('#lblMessage').fadeOut(); }, 3000);
            $(function () {
                $(".formatMoney").keyup(function (e) {
                    $(this).val(format($(this).val()));
                });
            });
        }
        $(document).ready(function () {
            Init();
            $(".select2").select2({
                width: '100%'
            });

        });
    </script>
    <script type="text/javascript">
        function validate() {
            var getMaterial = $('#txtMaterial').val();
            var getColor = $('#txtColor').val();
            if (getColor != "") {
                var bool = true;
                $.ajax({

                    type: "GET",/*method type*/
                    contentType: "application/json; charset=utf-8",
                    url: "/Admin/Product/CheckExist?action=CheckColor&name=" + getColor + "",
                    dataType: "text",
                    async: false,
                    success: function (data) {
                        if (data == "") {
                            window.showToast("msg", "Màu nhập đã tồn tại, vui lòng chọn màu ở phần có sẵn", 3000);
                            $('#txtColor').focus();
                            bool = false;
                        }
                    }
                });
                if (bool == true) {
                    if (getMaterial != "") {
                        var bool1 = true;
                        $.ajax({
                            type: "GET",/*method type*/
                            contentType: "application/json; charset=utf-8",
                            url: "/Admin/Product/CheckExist?action=CheckMaterial&name=" + getMaterial + "",
                            dataType: "text",
                            async: false,
                            success: function (data) {
                                if (data == "") {
                                    window.showToast("msg", "Vật liệu nhập đã tồn tại, vui lòng chọn vật liệu ở phần có sẵn", 3000);
                                    $('#txtMaterial').focus();

                                    bool1 = false;
                                }
                            }
                        });
                        return bool1;
                    }
                }
                else {
                    return bool;
                }
            }


        }

     


    </script>
</asp:Content>

