<%@ Page Title="" Language="C#" MasterPageFile="~//Template/CMS.master" AutoEventWireup="true" CodeFile="Product_List.aspx.cs" Inherits="Product_List" %>

<%@ Register Assembly="ASPnetPagerV2_8" Namespace="ASPnetControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/ctlDatePicker.ascx" TagPrefix="uc1" TagName="ctlDatePicker" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHead" runat="Server">
    <!-- DataTables -->
    <link href="/theme/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    <link href="/theme/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    <!-- Responsive datatable examples -->
    <link href="/theme/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" type="text/css" />
    <link href="/theme/plugins/select2/select2.min.css" rel="stylesheet" type="text/css" />
    <link href="/theme/plugins/RWD-Table-Patterns/dist/css/rwd-table.min.css" rel="stylesheet" type="text/css" media="screen">
    <style>
        .btn-group.focus-btn-group {
            display: none;
        }

        .class-label > label {
            margin-left: 5px;
        }

        .alert-warning1 {
            color: red;
            /* background-color: #ff9f43;*/
            border-color: #ffe4ca;
            border: 1px solid !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderBody" runat="Server">
    <form runat="server" id="frm1">
        <div class="container-fluid">
            <!-- Page-Title -->
            <div class="row">
                <div class="col-sm-12">
                    <div class="page-title-box">
                        <div class="float-right">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item active"><a>Quản lý sản phẩm</a></li>
                                <li class="breadcrumb-item"><a href="/">Trang chủ</a></li>
                            </ol>
                        </div>
                        <h4 class="page-title">Quản lý sản phẩm</h4>
                    </div>
                    <!--end page-title-box-->
                </div>
                <!--end col-->
            </div>
            <!-- end page title end breadcrumb -->
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body">
                            <uc1:ctlDatePicker ID="ctlDatePicker1" runat="server" OnDateChange="ctlDatePicker1_DateChange" />
                            <br />
                            <div class="row">
                                <div class="col-md-3 mb-2">

                                    <%--              <telerik:RadComboBox RenderMode="Lightweight" MaxHeight="300px" ID="ddlDMSP" Skin="MetroTouch" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%"  AutoPostBack="true" EmptyMessage="Chọn danh mục sản phẩm" Localization-ItemsCheckedString="danh mục sản phẩm được chọn">
                            <Localization CheckAllString="Chọn tất cả"
                                AllItemsCheckedString="Tất cả đều được chọn" />
                            <Items>
                                <telerik:RadComboBoxItem Text="ok" Value="1"/>
                                <telerik:RadComboBoxItem Text="ok1" Value="2"/>
                                <telerik:RadComboBoxItem Text="ok2" Value="3"/>
                                <telerik:RadComboBoxItem Text="ok3" Value="4"/>
                                <telerik:RadComboBoxItem Text="ok4" Value="5"/>
                            </Items>
                        </telerik:RadComboBox>--%>
                                    <asp:DropDownList runat="server" ID="ddlCha" CssClass="select2 form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCha_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <asp:DropDownList runat="server" ID="ddlProductSet" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlProductSet_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <asp:DropDownList runat="server" ID="ddlStatusType" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusType_SelectedIndexChanged">
                                        <asp:ListItem Text="-- Trạng Thái --" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Sản phẩm New" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Sản phẩm Hot" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <!-- end row -->
                                <div class="col-md-3 mb-3 none">
                                    <asp:DropDownList runat="server" ID="ddlTieuChuan" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlTieuChuan_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <asp:TextBox runat="server" ID="txtName" placeholder="Tên sản phẩm" CssClass="form-control"></asp:TextBox>
                                    <br />
                                    <div class='checkbox checkbox-success font-13 ml-2 none'>
                                        <asp:CheckBox runat="server" ID="ckChung" Text="Hiển thị sản phẩm dùng chung" AutoPostBack="true" />
                                    </div>
                                </div>
                                <div class="col-md-12 mb-2 right">
                                    <asp:Button CssClass="btn btn-gradient-primary mr-3" runat="server" ID="btnSearch" Text="Tìm kiếm" OnClick="btnSearch_Click" />
                                </div>
                                <div class="col-12 none">
                                    <asp:Panel Visible="false" runat="server" ID="pnOpen" CssClass="row">
                                        <div class="col-md-4 mb-2">
                                            <asp:DropDownList runat="server" ID="ddlColor" CssClass="select2 form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlColor_SelectedIndexChanged"></asp:DropDownList>

                                        </div>
                                        <div class="col-md-4 mb-2">
                                            <asp:TextBox runat="server" ID="txtSize" placeholder="Kích thước" CssClass="form-control"></asp:TextBox>

                                        </div>
                                        <div class="col-md-4 mb-2">
                                            <asp:DropDownList runat="server" ID="ddlMaterial" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged"></asp:DropDownList>

                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="col-md-2 mb-3 mt-3 class-label none">
                                    <asp:CheckBox runat="server" ID="ckShow" Text="Tìm kiếm mở rộng" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="ckShow_Checked" />
                                </div>
                                <!-- end row -->
                                <div class="col-md-3 mb-3 none">
                                    <asp:DropDownList runat="server" ID="ddlProductBrand" CssClass="select2 form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProductBrand_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-12 right">
                                    <asp:Button CssClass="btn btn-gradient-primary mr-3" Visible="false" runat="server" ID="btnCopy" Text="Copy nhanh đầu mục" OnClick="btnCopy_Click" />
                                    <asp:Button CssClass="btn btn-gradient-primary mr-3" runat="server" ID="btnAdd" Text="Thêm mới" OnClick="btnAdd_Click" />
                                    <a href='Product_DownloadExcel.aspx?Action=data' class="btn btn-gradient-primary mr-3" target="_blank"><i class="mdi mdi-download font-16"></i>Xuất File</a>

                                    <asp:Button CssClass="btn btn-gradient-primary mr-3" Visible="false" runat="server" ID="btnExport" Text="Xuất file" OnClick="btnExport_Click" />
                                    <asp:Button CssClass="btn btn-gradient-primary mr-3" runat="server" ID="btnUploadFile" Text="Upload-Update File " OnClick="btnUpload_Click" />

                                    <asp:Button CssClass="btn btn-gradient-primary mr-3" runat="server" ID="btnIcon" Text="Cài đặt Icon " OnClick="btnIcon_Click" />



                                </div>
                                <div class="col-md-12 right mt-2">
                                    <asp:Button CssClass="btn btn-warning mr-3" runat="server" ID="btnColor" Text="Màu sản phẩm" OnClick="btnColor_Click" />
                                    <asp:Button CssClass="btn btn-success mr-3" runat="server" ID="btnSpace" Text="Không gian sản phẩm" OnClick="btnSpace_Click" />
                                    <asp:Button CssClass="btn btn-success mr-3" Visible="false" runat="server" ID="btnUpdateURL" Text="UPdate URL" OnClick="btnURL_Click" />
                                    <asp:Button CssClass="btn btn-info mr-3" runat="server" ID="btnClassify" Text="Phân loại sản phẩm" OnClick="btnClassify_Click" />
                                    <asp:Button CssClass="btn btn-danger mr-3" runat="server" ID="btnMaterial" Text="Chất liệu - vật liệu" OnClick="btnMaterial_Click" />
                                    <asp:Button CssClass="btn btn-danger mr-3" runat="server" ID="btnProductType" Text="Loại Hàng" OnClick="btnProductType_Click" />
                                    <%-- <a href="../Admin/Material/MaterialCategory_List.aspx"  class="btn btn-danger mr-3">Chất liệu vật tư</a>--%>
                                </div>
                                <div class="col-md-12 right mt-3 mb-1">
                                    <asp:Button CssClass="btn btn-success mr-2" runat="server" ID="btnIsNew" Text="Kích hoạt SP New" OnClick="btnIsNew_Click" />
                                    <asp:Button CssClass="btn btn-success mr-2" runat="server" ID="btnIsHome" Text="Kích hoạt gộp bộ-hiển thị Web" OnClick="btnIsHome_Click" />
                                    <asp:Button CssClass="btn btn-success mr-2" runat="server" ID="btnIsHot" Text="Kích hoạt SP Hot" OnClick="btnIsHot_Click" />
                                    <asp:Button CssClass="btn btn-success mr-2" runat="server" ID="btnActice" Text="Kích hoạt SP" OnClick="btnActice_Click" />
                                    <asp:Button CssClass="btn btn-success mr-2" runat="server" ID="btnIsQRCode" Text="Kích hoạt QR Code" OnClick="btnIsQRCode_Click" />

                                    <!-- end row -->
                                </div>
                                <div class="col-md-12 right mt-1 mb-3">
                                    <asp:Button CssClass="btn btn-info mr-2" runat="server" ID="btnCloseIsNew" Text="Hủy kích hoạt SP New" OnClick="btnCloseIsNew_Click" />
                                    <asp:Button CssClass="btn btn-info mr-2" runat="server" ID="btnCloseIsHome" Text="Hủy kích hoạt gộp bộ-hiển thị Web" OnClick="btnCloseIsHome_Click" />
                                    <asp:Button CssClass="btn btn-info mr-2" runat="server" ID="btnCloseIsHot" Text="Ngừng kích hoạt SP Hot" OnClick="btnCloseIsHot_Click" />
                                    <asp:Button CssClass="btn btn-info mr-2" runat="server" ID="btnCloseActive" Text="Ngừng kích hoạt SP" OnClick="btnCloseActive_Click" />
                                    <asp:Button CssClass="btn btn-info mr-2" runat="server" ID="btnUnIsQRCode" Text="Ngừng kích hoạt QR Code" OnClick="btnUnIsQRCode_Click" />
                                </div>
                            </div>
                            <div class="form-group">
                            </div>
                            <%-- <h4 class="mt-0 header-title">Buttons example</h4>--%>
                            <label>Hiển thị </label>
                            <asp:DropDownList runat="server" ID="ddlItem" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" CssClass="custom-select custom-select-sm form-control form-control-sm">
                                <asp:ListItem Value="20"></asp:ListItem>
                                <asp:ListItem Value="50"></asp:ListItem>
                                <asp:ListItem Value="100"></asp:ListItem>
                                <%--<asp:ListItem Value="30"></asp:ListItem>--%>
                                <%--<asp:ListItem Value="50"></asp:ListItem>
                                <asp:ListItem Value="100"></asp:ListItem>--%>
                            </asp:DropDownList>
                            <asp:UpdatePanel runat="server" ID="up">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblMessage" CssClass="msg-sc-edit" ClientIDMode="Static" Visible="false"></asp:Label>

                                    <div class="table-rep-plugin">
                                        <div class="table-responsive mb-0 table-bordered" data-pattern="priority-columns">
                                            <table id="tech-companies-1" class="table table-striped mb-0">
                                                <thead>
                                                    <tr>
                                                        <th width="10%">
                                                            <input type="checkbox" onclick="toggle(this)" /></th>
                                                        <th width="10%">Mã SP</th>
                                                        <th width="10%">Ảnh</th>
                                                        <th width="35%">Tên sản phẩm/chi tiết</th>
                                                        <th width="20%">SKU web</th>
                                                        <th width="18%">SKU hệ thống</th>
                                                        <%-- <th width="18%">Màu</th>
                                                        <th width="18%">Kích thước</th>
                                                        <th width="20%">Chất liệu</th>--%>
                                                        <th width="10%">Thông tin</th>
                                                        <%-- <th width="4%" class="text-center">Gộp Bộ - hiển thị web</th>
                                                        <th width="4%" class="text-center">SP Hot</th>
                                                        <th width="4%" class="text-center">Trạng thái</th>
                                                        <th width="4%" class="text-center">QRCode</th>--%>
                                                        <th width="30%">Cấu hình hiển thị</th>
                                                        <th></th>
                                                        <th width="10%" class="text-center">Chức năng</th>

                                                    </tr>
                                                </thead>
                                                <tbody>

                                                    <asp:Repeater runat="server" ID="grdProduct" OnItemDataBound="grdProduct_ItemDataBound" OnItemCommand="grdProduct_ItemCommand">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Literal ID="lblProductID1" runat="server" Text='<%#Eval("Product_ID")%>' Visible="false"></asp:Literal>
                                                                    <asp:CheckBox ID="ckproductID1" runat="server" onclick='GetCheck(this.id);' />
                                                                </td>
                                                                <td><%#Eval("Product_ID")%></td>
                                                                <td>
                                                                    <a href='Product_Edit?Product_ID=<%#Eval("Product_ID") %>' class="breadcrumb-item active">
                                                                        <img src='<%# Common.GetImg(Eval("Image"))%>' width="70" />
                                                                    </a>
                                                                </td>

                                                                <td><a href='Product_Edit?Product_ID=<%#Eval("Product_ID") %>' class="breadcrumb-item active"><b><%#Eval("Name").ToString().ToUpper()%></b></a>  || <span style="color: red"><%#string.IsNullOrEmpty(Eval("ProductInWarehouse").ToString())?"Sản phẩm Web":"Sản phẩm Kho" %></span><br />
                                                                    Danh mục:
                                                            <a href='?ProductCategory_ID=<%#Eval("ProductCategory_ID") %>' class="breadcrumb-item active" title="<%#Eval("ProductCategoryName") %>"><%# Common.CatChuoiHTML(Eval("ProductCategoryName").ToString(),42) %></a>
                                                                    <br />
                                                                    <%--Đăng bởi: <%# (new MyUser().FullNameFromUser_ID(Eval("CreateBy").ToString()))%> vào lúc  <%# DateTime.Parse( Eval("CreateDate").ToString()).ToString("dd/MM/yyyy hh:mm:ss") %>
                                                                    <br />
                                                                    Sửa bởi: <%#(new MyUser().FullNameFromUser_ID(Eval("LastEditBy").ToString())) %> vào lúc  <%# DateTime.Parse( Eval("LastEditDate").ToString()).ToString("dd/MM/yyyy hh:mm:ss") %>--%>
                                                                      Đăng bởi: <%# string.IsNullOrEmpty(Eval("UserName").ToString())?"":Eval("UserName")%> vào lúc  <%# DateTime.Parse( Eval("CreateDate").ToString()).ToString("dd/MM/yyyy hh:mm:ss") %>
                                                                    <br />
                                                                    Sửa bởi:  <%# string.IsNullOrEmpty(Eval("NguoiSua").ToString())?"":Eval("NguoiSua")%> vào lúc  <%# string.IsNullOrEmpty(Eval("LastEditDate").ToString())?"": DateTime.Parse( Eval("LastEditDate").ToString()).ToString("dd/MM/yyyy hh:mm:ss") %>
                                                                    <br />
                                                                    <asp:Literal ID="lblView" runat="server"></asp:Literal>
                                                                    <asp:Literal ID="lblViewURL" runat="server" Text='<%#Eval("URL")%>' Visible="false"></asp:Literal>

                                                                </td>
                                                                <td><%#Eval("SKU_Web_ID") %></td>
                                                                <td><%#Eval("SKU_ID") %></td>
                                                                <%--                                                                <td><%#Eval("Color")%></td>
                                                                <td><%#Eval("Size")%></td>
                                                                <td><%#Eval("NameMaterial") %></td>--%>
                                                                <td><a href='ProductInfo?Product_ID=<%#Eval("Product_ID") %>' class="mr-2" title="Thêm ảnh sản phẩm"><i class="fas fa-edit text-warning font-16"></i>Thư viện ảnh SP</a></td>
                                                                <td>Gộp Bộ - hiển thị Web
                                                                    <br />
                                                                    SP Hot
                                                                    <br />
                                                                    SP New
                                                                    <br />
                                                                    Trạng thái
                                                                    <br />
                                                                    QR Code
                                                                </td>
                                                                <td align="center">
                                                                    <asp:LinkButton runat="server" ID="btnDeactiveHome" CommandName="DeactiveHome" CssClass="mr-2" ToolTip="Kích hoạt" CommandArgument='<%#Eval("Product_ID") %>'><i class="fas fa-check text-success font-16 ml-2"></i></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="btnActiveHome" CommandName="ActiveHome" CssClass="mr-2" ToolTip="Ngừng kích hoạt" CommandArgument='<%#Eval("Product_ID") %>'><i class="fas fa-stop text-danger font-16 ml-2"></i></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton runat="server" ID="btnDeactiveHot" CommandName="DeactiveHot" CssClass="mr-2" ToolTip="Kích hoạt" CommandArgument='<%#Eval("Product_ID") %>'><i class="fas fa-check text-success font-16 ml-2"></i></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="btnActiveHot" CommandName="ActiveHot" CssClass="mr-2" ToolTip="Ngừng kích hoạt" CommandArgument='<%#Eval("Product_ID") %>'><i class="fas fa-stop text-danger font-16 ml-2"></i></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton runat="server" ID="btnDeactiveNew" CommandName="DeactiveNew" CssClass="mr-2" ToolTip="Kích hoạt" CommandArgument='<%#Eval("Product_ID") %>'><i class="fas fa-check text-success font-16 ml-2"></i></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="btnActiveNew" CommandName="ActiveNew" CssClass="mr-2" ToolTip="Ngừng kích hoạt" CommandArgument='<%#Eval("Product_ID") %>'><i class="fas fa-stop text-danger font-16 ml-2"></i></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton runat="server" ID="btnActive" CommandName="Deactive" CssClass="mr-2" ToolTip="Kích hoạt" CommandArgument='<%#Eval("Product_ID") %>'><i class="fas fa-check text-success font-16 ml-2"></i></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="btnDeactive" CommandName="Active" CssClass="mr-2" ToolTip="Ngừng kích hoạt" CommandArgument='<%#Eval("Product_ID") %>'><i class="fas fa-stop text-danger font-16 ml-2"></i></asp:LinkButton>
                                                                    <br />
                                                                    <asp:LinkButton runat="server" ID="btnActiveQR" CommandName="DeactiveQR" CssClass="mr-2" ToolTip="Kích hoạt" CommandArgument='<%#Eval("Product_ID") %>'><i class="fas fa-check text-success font-16 ml-2"></i></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="btnDeactiveQR" CommandName="ActiveQR" CssClass="mr-2" ToolTip="Ngừng kích hoạt" CommandArgument='<%#Eval("Product_ID") %>'><i class="fas fa-stop text-danger font-16 ml-2"></i></asp:LinkButton>

                                                                    <asp:Literal runat="server" ID="lblText" Visible="false"></asp:Literal>
                                                                    <asp:Literal runat="server" ID="lblID" Text='<%#Eval("Product_ID") %>' Visible="false"></asp:Literal>
                                                                    <asp:Literal runat="server" ID="lblApproved" Text='<%#Eval("Active") %>' Visible="false"></asp:Literal>
                                                                    <asp:Literal runat="server" ID="lblHome" Text='<%#Eval("IsHome") %>' Visible="false"></asp:Literal>
                                                                    <asp:Literal runat="server" ID="lblHot" Text='<%#Eval("IsHot") %>' Visible="false"></asp:Literal>
                                                                    <asp:Literal runat="server" ID="lblNew" Text='<%#Eval("IsNew") %>' Visible="false"></asp:Literal>
                                                                    <asp:Literal runat="server" ID="lblIsQrCode" Text='<%#Eval("IsQRCode") %>' Visible="false"></asp:Literal>

                                                                </td>
                                                                <td align="center">
                                                                    <span class='<%=style %>'>
                                                                        <a href='Product_Edit?Product_ID=<%#Eval("Product_ID") %>' class="mr-2" title="Sửa thông tin"><i class="fas fa-edit text-success font-16"></i></a>
                                                                        <a href='Product_Add?Product_ID=<%#Eval("Product_ID") %>' class="mr-2" title="Sao chép thông tin"><i class="fas fa-copy text-success font-16"></i></a>
                                                                    </span>
                                                                    <asp:LinkButton runat="server" Visible="false" ID="btnCopy" CommandName="Copy" CssClass="mr-2" ToolTip="Nhân bản sản phẩm" CommandArgument='<%#Eval("Product_ID") %>' OnClientClick="return confirm('Bạn có chắc chắn muốn nhân bản sản phẩm này không?')"><i class="fas fa-copy text-success font-16"></i></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="btnDelete" CommandName="Delete" CssClass="mr-2" ToolTip="Xóa" CommandArgument='<%#Eval("Product_ID") %>' OnClientClick="return confirm('Bạn có chắc chắn muốn xóa không?')"><i class="fas fa-trash-alt text-danger font-16"></i></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="x_box_pager" style="float: right; text-align: right; margin-top: 10px" runat="Server" class="box_pager">
                                        <label>Trang <%=Pager1.CurrentIndex %>/<%=TotalPage %></label>
                                        (<label> <%=TotalItem %> sản phẩm</label>)
                                        <cc1:PagerV2_8 ID="Pager1" runat="server" OnCommand="Pager1_Command"
                                            GenerateFirstLastSection="True" GenerateGoToSection="False" GenerateHiddenHyperlinks="False"
                                            GeneratePagerInfoSection="False" NextToPageClause="" OfClause="/" PageClause=""
                                            ToClause="" CompactModePageCount="1" MaxSmartShortCutCount="5" NormalModePageCount="5"
                                            GenerateToolTips="False" BackToFirstClause="" BackToPageClause="" FromClause=""
                                            GenerateSmartShortCuts="False" GoClause="" GoToLastClause="" />
                                        <div class="clear">
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>
                <!-- end col -->
            </div>
        </div>
        <!-- container -->

        <!--  Modal content for the above example -->
        <asp:HiddenField ID="hdnCId" runat="server" />
        <asp:HiddenField runat="server" ID="ProductBrandList" />
        <asp:HiddenField runat="server" ID="ProductList" Value="0" />
    </form>
    <!-- /.modal -->
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
            $(".select2").select2({
                width: '100%'
            });
            setTimeout(function () { $('#lblMessage').fadeOut(); }, 15000);
            if (typeof (Sys) !== 'undefined') {
                var parameter = Sys.WebForms.PageRequestManager.getInstance();
                parameter.add_endRequest(function () {
                    setTimeout(function () { $('#lblMessage').fadeOut(); }, 15000);
                    $('.table-responsive').responsiveTable({
                        addDisplayAllBtn: 'btn btn-secondary',
                    });
                });
            }
        });
    </script>
    <script>
        function GetCheck(element) {
            var checkBox = document.getElementById(element);
            /* alert(element);*/
            /* var ID = "#" + ($(element).parent().parent().attr("id")) + " input[type='checkbox']";*/
            if (checkBox.checked) {
                $(checkBox).attr('checked', true);
                /*alert("1");*/
            }
            else {
                $(checkBox).attr('checked', false);
                /*alert("2");*/
            }
        }
        function toggle(source) {
            var checkboxes = document.querySelectorAll('input[type="checkbox"]');
            for (var i = 1; i < checkboxes.length; i++) {
                if (checkboxes[i] != source)
                    checkboxes[i].checked = source.checked;
            }
        }
    </script>
</asp:Content>
