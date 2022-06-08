<%@ Page Title="" Language="C#" MasterPageFile="~/Template/CMS.master" AutoEventWireup="true" CodeFile="User_Edit.aspx.cs" Inherits="User_Edit" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="Server">
    <link href="/theme/plugins/daterangepicker/daterangepicker.css" rel="stylesheet" />
    <link href="/theme/plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.css" rel="stylesheet" type="text/css" />
    <link href="/theme/plugins/timepicker/bootstrap-material-datetimepicker.css" rel="stylesheet">
    <link href="/theme/plugins/select2/select2.min.css" rel="stylesheet" type="text/css" />
    <%--<link href="/theme/plugins/bootstrap-touchspin/css/jquery.bootstrap-touchspin.min.css" rel="stylesheet" />--%>
    <link href="../../css/telerik.css?v=<%=Systemconstants.Version(5) %>" rel="stylesheet" type="text/css" />
    <style>
        .font-13 label {
            color: #656d9a;
            font-size: 14px;
            font-weight: bold;
        }

        .font-11 input {
            margin-left: 25px;
        }

        .role {
            overflow-y: scroll;
            overflow-x: hidden;
            max-height: 600px;
            padding-left: 10px;
            border: 1px solid #edf0f5;
            border-radius: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderBody" runat="Server">
    <form runat="server" id="frm1" class="form-parsley">
        <telerik:RadScriptManager runat="server" ID="sc"></telerik:RadScriptManager>
        <div class="container-fluid">
            <!-- Page-Title -->
            <div class="row">
                <div class="col-sm-12">
                    <div class="page-title-box">
                        <div class="float-right">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item active"><%=title %></li>
                                <li class="breadcrumb-item" runat="server" id="pn"><a href="User_List">Quản lý tài khoản</a></li>
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
            <!-- end page title end breadcrumb -->
            <%-- <asp:ScriptManager runat="server" ID="src"></asp:ScriptManager>
            <asp:UpdatePanel runat="server" ID="up">
                <ContentTemplate>--%>
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">

                                <!--end card-body-->

                                <div class="col-lg-6 " runat="server" id="phanquyen">
                                    <h3 class="mt-0 header-title">Hệ Thống <span class="red">*</span></h3>
                                    <div class="form-group">
                                        <%--<asp:DropDownList runat="server" ID="ddlHeThong" CssClass="select2 form-control" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlHeThong_SelectedIndexChanged" data-parsley-required="true" data-parsley-allselected="true" data-parsley-required-message="Chưa chọn hệ thống"></asp:DropDownList>--%>
                                        <asp:DropDownList runat="server" ID="ddlHethong1" CssClass="select2 form-control" data-parsley-required="true" data-parsley-allselected="true" data-parsley-required-message="Chưa chọn hệ thống">
                                            <asp:ListItem Value="0" Text="-- Chọn Hệ Thống --"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Web"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Kho"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <h3 class="mt-2 header-title">Cấp quản lý <span class="red">*</span></h3>
                                    <asp:DropDownList runat="server" ID="ddlModuleGroup" CssClass="select2 form-control" AutoPostBack="true" data-parsley-required="true" data-parsley-allselected="true" data-parsley-required-message="Chưa chọn cấp quản lý" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="-- Chọn cấp quản lý --"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Công ty"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Đại lý"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Panel runat="server" ID="pnEs" Visible="false">
                                        <h3 class="mt-0 header-title">Phân quyền</h3>
                                        <div class="form-group">
                                            <label>Nhóm quyền <span class="red">*</span></label>
                                            <asp:DropDownList runat="server" ID="ddlFunctionGroup" CssClass="select2 form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFunctionGroup_SelectedIndexChanged" data-parsley-required="true" data-parsley-allselected="true" data-parsley-required-message="Chưa chọn nhóm chức năng"></asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <asp:DropDownList runat="server" ID="ddlProductBrand" CssClass="select2 form-control" Visible="false" data-parsley-required="true" data-parsley-allselected="true" data-parsley-required-message="Chưa chọn doanh nghiệp"></asp:DropDownList>
                                        </div>
                                        <asp:UpdatePanel runat="server" ID="up1">
                                            <ContentTemplate>
                                                <div class="form-group">
                                                    <asp:DropDownList runat="server" ID="ddlAccountType" CssClass="select2 form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAccountType_SelectedIndexChanged" Visible="false" data-parsley-required="true" data-parsley-allselected="true" data-parsley-required-message="Chưa chọn loại tài khoản"></asp:DropDownList>
                                                </div>
                                                <div runat="server" id="phongban" visible="false">
                                                    <div class="form-group">
                                                        <asp:DropDownList runat="server" ID="ddlDepartmentUser" CssClass="select2 form-control" data-parsley-required="true" data-parsley-allselected="true" data-parsley-required-message="Chưa chọn phòng ban" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartmentUser_SelectedIndexChanged"></asp:DropDownList>

                                                    </div>
                                                    <div class="form-group">
                                                        <asp:DropDownList runat="server" ID="ddlWorkshop" CssClass="select2 form-control"></asp:DropDownList>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:DropDownList runat="server" ID="ddlZone" CssClass="select2 form-control"></asp:DropDownList>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:DropDownList runat="server" ID="ddlArea" CssClass="select2 form-control"></asp:DropDownList>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:DropDownList runat="server" ID="ddlFarm" CssClass="select2 form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="RolePermission">
                                                    <label>Chọn module quản lý</label>
                                                    <telerik:RadComboBox RenderMode="Lightweight" MaxHeight="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlFunction_SelectedIndexChanged" ID="ddlFunction" Skin="MetroTouch" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%" EmptyMessage="Chọn module " Localization-ItemsCheckedString="module được chọn">
                                                        <Localization CheckAllString="Chọn tất cả"
                                                            AllItemsCheckedString="Tất cả đều được chọn" />
                                                    </telerik:RadComboBox>

                                                    <br />
                                                    <br />
                                                    <div class="role">
                                                        <asp:Repeater runat="server" ID="rptFunction" OnItemDataBound="rptFunction_ItemDataBound">
                                                            <ItemTemplate>
                                                                <hr />
                                                                <div class="container-checked" id='test<%#Eval("Function_ID") %>'>
                                                                    <div class='checkbox checkbox-success font-13'>
                                                                        <asp:CheckBox runat="server" ID="ckParent" Text='<%#Eval("Name") %>' onclick='GetCheck(this);' Checked="true" />
                                                                    </div>

                                                                    <%-- <b><%#Eval("Name") %>--%>
                                                                    <asp:Literal runat="server" ID="lblFunction_ID" Text='<%#Eval("Function_ID") %>' Visible="false"></asp:Literal>
                                                                    <%--</b>--%>
                                                                    <div class="row">
                                                                        <asp:Repeater runat="server" ID="rptPage">
                                                                            <ItemTemplate>
                                                                                <div class="col-lg-12 col-md-12">
                                                                                    <div class='checkbox checkbox-primary font-11'>
                                                                                        <asp:Literal runat="server" ID="lblPageFunction_ID" Text='<%#Eval("PageFunction_ID") %>' Visible="false"></asp:Literal>
                                                                                        <asp:CheckBox runat="server" ID="ckRole" Text='<%#Eval("Name") %>' onclick='GetCheckChild(this);' Checked="true" />
                                                                                    </div>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <br />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                    <asp:Literal ID="lblMsg" Text="" runat="server"></asp:Literal>
                                </div>
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <label>Tên tài khoản <span class="red">*</span></label>
                                        <asp:TextBox runat="server" ID="txtUser" Enabled="false" ClientIDMode="Static" CssClass="form-control" required data-parsley-required-message="Chưa nhập tên tài khoản"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label>Avatar</label>
                                        <br />

                                        <div style="margin: 5px 0px;">
                                            <a href="<%=avatar %>" target="_blank">
                                                <asp:Image ID="imganh" runat="server" ImageUrl='../../images/no-image-icon.png' Width="100px" />
                                            </a>
                                        </div>

                                        <asp:FileUpload ID="fulAnh" runat="server" ClientIDMode="Static" onchange="img();" />

                                    </div>
                                    <div class="form-group" runat="server" id="HideHoTen">
                                        <label>Họ tên <span class="red">*</span></label>
                                        <asp:TextBox runat="server" ID="txtFullName" ClientIDMode="Static" CssClass="form-control" required data-parsley-required-message="Chưa nhập Họ tên"></asp:TextBox>
                                    </div>
                                    <!--end form-group-->

                                    <div class="form-group">
                                        <label>E-Mail <span class="red">*</span></label>
                                        <div>
                                            <asp:TextBox runat="server" ID="txtEmail" ClientIDMode="Static" TextMode="Email" parsley-type="email" CssClass="form-control" required data-parsley-required-message="Chưa nhập tên Email"></asp:TextBox>

                                        </div>
                                    </div>

                                    <!--end form-group-->

                                    <div class="form-group">
                                        <label>Địa chỉ</label>
                                        <asp:TextBox runat="server" ID="txtAddress" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group" runat="server" id="HideSdt">
                                        <label>Số điện thoại</label>
                                        <asp:TextBox runat="server" ID="txtPhone" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <!--end form-group-->

                                    <div class="form-group none" runat="server" id="HideGioiTinh">
                                        <label>Giới tính</label>
                                        <asp:DropDownList runat="server" ID="ddlGioiTinh" CssClass="form-control">
                                            <asp:ListItem Text="-Chọn giới tính-" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Nữ" Value="Nữ"></asp:ListItem>
                                            <asp:ListItem Text="Nam" Value="Nam"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <%--   <div class="form-group">
                                <label>Ngày sinh</label>
                                <asp:TextBox runat="server" ID="txtBirth" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            </div>--%>
                                    <div class="form-group none" runat="server" id="HideNgaySinh">
                                        <label>Ngày sinh</label>
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="txtBirth" Text="01/01/1980" ClientIDMode="Static" CssClass="form-control" name="birthday" />
                                            <%--<input name="birthday" type="text" value="01/01/1980" id="txtBirth" class="form-control">--%>
                                            <div class="input-group-append">
                                                <span class="input-group-text"><i class="dripicons-calendar"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <!--end form-group-->
                                    <div class="form-group">
                                        <%-- <label>Kích hoạt</label>--%>
                                        <div class="custom-control custom-checkbox">
                                            <asp:CheckBox runat="server" ID="ckActive" ClientIDMode="Static" Checked="true" />
                                            <label for="ckActive" class="custom-control-label">
                                                KÍCH HOẠT
                                            </label>
                                        </div>
                                    </div>
                                    <!--end form-group-->

                                    <%--    <div class="form-group">
                                        <label>Giới thiệu</label>
                                        <asp:TextBox runat="server" ID="txtNote" ClientIDMode="Static" TextMode="MultiLine"></asp:TextBox>
                                    </div>--%>
                                    <!--end form-group-->

                                    <div class="form-group mb-0">
                                        <asp:Button runat="server" ID="btnSave" OnClientClick="SaveLife()" ClientIDMode="Static" OnClick="btnSave_Click" CssClass="btn btn-gradient-primary waves-effect waves-light" Text="Lưu" />
                                        <asp:Button ID="btnBack" OnClick="btnBack_Click" UseSubmitBehavior="false" runat="server" ClientIDMode="Static" Text="Quay lại" CssClass="btn btn-gradient-danger waves-effect m-l-5"></asp:Button>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblMessage" CssClass="msg-sc-edit" ClientIDMode="Static" Visible="false"></asp:Label>
                                    </div>
                                    <!--end form-group-->

                                    <!--end form-->
                                </div>
                            </div>
                        </div>
                        <!--end card-body-->
                    </div>
                </div>
                <!--end card-->
            </div>
            <!-- end col -->
            <!-- end col -->
        </div>
        <%--  </ContentTemplate>
            </asp:UpdatePanel>--%>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceBottom" runat="Server">
    <%--<script type="text/javascript" src="/ckeditor/ckeditor.js"></script>--%>
    <%--<script src="/ckfinder/ckfinder.js"></script>--%>
    <script>
        //function UploadImage() {
        //    var finder = new CKFinder();
        //    //finder.selectActionFunction = function (fileUrl) {
        //    //    //$("#lblDocument").html("<img src='" + fileUrl + "' class='img-responsive'/>");
        //    //    //$('#hdImg').val(fileUrl);
        //    //    //$("#btn_upload").click();
        //    //};
        //    finder.ResourceType = 'Images';
        //    finder.popup();
        //}
        $(window).on('load', function () {
            setTimeout(function () { $('#spinner').fadeOut(); }, 100);
        })
        function SaveLife() {
            tinymce.triggerSave();
        }
        function Init() {
            $("#ckActive").addClass("custom-control-input");
            LoadMCE();
            setTimeout(function () { $('#lblMessage').fadeOut(); }, 3000);

        }
        function GetCheck(element) {
            var ID = "#" + ($(element).parent().parent().attr("id")) + " input[type='checkbox']";
            if (element.checked) {
                $(ID).prop('checked', true);
            }
            else {
                $(ID).prop('checked', false);
            }
        }
        function GetCheckChild(element) {
            var ID = "#" + ($(element).parent().parent().parent().parent().attr("id")) + " .font-13 input[type='checkbox']";
            var ID_Child = "#" + ($(element).parent().parent().parent().parent().attr("id")) + " .font-11 input[type='checkbox']";
            if (!element.checked) {
                $(ID).prop('checked', false);
            }
            var countCheckedCheckboxes = $(ID_Child).filter(':checked').length;
            //alert(countCheckedCheckboxes);
            var dem = 0;
            $(ID_Child).each(function () {
                dem++;

            });

            if (dem == countCheckedCheckboxes) {
                $(ID).prop('checked', true);
            }
        }
        $(document).ready(function () {
            $(".select2").select2({
                width: '100%'
            });
            Init();
            $('#txtBirth').daterangepicker({
                singleDatePicker: true,
                showDropdowns: true,
                //minYear: 1901,
                //maxYear: parseInt(moment().format('YYYY'), 10),
                locale: {
                    format: 'DD/MM/YYYY',
                },
            }, function (start, end, label) {
                //console.log("A new date selection was made: " + start.format('DD/MM/YYYY') + ' to ' + end.format('YYYY-MM-DD'));
            });
        });

    </script>
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
    <script src="/theme/plugins/select2/select2.min.js"></script>
    <script src="/theme/plugins/moment/moment.js"></script>
    <script src="/theme/plugins/bootstrap-maxlength/bootstrap-maxlength.min.js"></script>
    <script src="/theme/plugins/daterangepicker/daterangepicker.js"></script>
    <script src="/theme/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
    <script src="/theme/plugins/timepicker/bootstrap-material-datetimepicker.js"></script>
    <script src="/theme/plugins/bootstrap-touchspin/js/jquery.bootstrap-touchspin.min.js"></script>
    <%--<script src="/theme/assets/pages/jquery.forms-advanced.js"></script>--%>

    <!--Wysiwig js-->
    <script src="/theme/plugins/tinymce/tinymce.min.js"></script>
    <script src="/theme/assets/pages/jquery.form-editor.init.js"></script>
    <!-- App js -->
    <script src="/theme/assets/js/jquery.core.js"></script>

</asp:Content>

