using DbObj;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SystemFrameWork;

public partial class Product_List : System.Web.UI.Page
{
    int ProductCategory_ID = 0;
    public int TotalPage = 1;
    public int TotalItem = 0;
    int pageSize = 5;//Số bản ghi 1 trang
    private int productCategory_ID;
    public string Message = "";
    public string style = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MyUser.GetFunctionGroup_ID() != "1")
        {
            btnCopy.Visible = false;
        }
        btnAdd.Visible = MyActionPermission.CanAdd();
        if (!string.IsNullOrEmpty(Request["ProductCategory_ID"]))
            int.TryParse(Request["ProductCategory_ID"].ToString(), out ProductCategory_ID);
        if (!IsPostBack)
        {
            if (MyUser.GetFunctionGroup_ID() == "3")
            {
                DataTable dt = BusinessRulesLocator.GetUserVsProductBrandBO().GetAsDataTable(" UserID='" + MyUser.GetUser_ID() + "'", "");
                if (dt.Rows.Count == 1)
                {
                    ProductBrandList.Value = dt.Rows[0]["ProductBrand_ID_List"].ToString();
                }
            }
            LoadProductCategory();
            if (ProductCategory_ID != 0)
            {
                ddlCha.SelectedValue = ProductCategory_ID.ToString();
            }
            FillDllProductSet();
            FillDDLMaterial();
            FillDDLColor();
            FillDDLQuality();
            FillProductBrand();

            LoadProduct();
        }
        //if (ckChung.Checked)
        //{
        //    ddlProductBrand.SelectedValue = "0";
        //    style = "none";
        //}
        //else
        //{
        //    style = "";
        //    if (Common.GetFunctionGroupDN())
        //    {
        //        ddlProductBrand.SelectedValue = MyUser.GetProductBrand_ID();
        //        ddlProductBrand.Enabled = false;
        //    }

        //}

        ResetMsg();
    }

    private void LoadProduct()
    {
        try
        {

            //pageSize = Convert.ToInt32(ddlItem.SelectedValue);
            string whereStatus = string.Empty;
            Pager1.PageSize = pageSize = Convert.ToInt32(ddlItem.SelectedValue);
            DataSet dtSet = new DataSet();
            DataTable dt = new DataTable();
            if (hdnCId.Value != null)
            {
                listCategory_ID1 = hdnCId.Value;
            }
            if (ddlStatusType.SelectedValue == "1")
            {
                whereStatus += " and P.IsNew = 1";
            }
            else if (ddlStatusType.SelectedValue == "2")
            {
                whereStatus += " and P.IsHot = 1";
            }
            if (!string.IsNullOrEmpty(ProductBrandList.Value))
            {
                string where = " and P.ProductBrand_ID in (" + ProductBrandList.Value + ")";
                dtSet = BusinessRulesLocator.Conllection().GetProductV3(Pager1.CurrentIndex, pageSize, 7, 0, Convert.ToInt32(ddlProductSet.SelectedValue), Convert.ToInt32(ddlColor.SelectedValue), txtSize.Text, Convert.ToInt32(ddlMaterial.SelectedValue), Convert.ToInt32(ddlTieuChuan.SelectedValue), Convert.ToInt32(ddlProductBrand.SelectedValue), 0, 0, "", ctlDatePicker1.FromDate, ctlDatePicker1.ToDate, listCategory_ID1, txtName.Text, where, " CreateDate DESC");
            }
            else
            {
                dtSet = BusinessRulesLocator.Conllection().GetProductV3(Pager1.CurrentIndex, pageSize, 7, 0, Convert.ToInt32(ddlProductSet.SelectedValue), Convert.ToInt32(ddlColor.SelectedValue), txtSize.Text, Convert.ToInt32(ddlMaterial.SelectedValue), Convert.ToInt32(ddlTieuChuan.SelectedValue), Convert.ToInt32(ddlProductBrand.SelectedValue), 0, 0, "", ctlDatePicker1.FromDate, ctlDatePicker1.ToDate, listCategory_ID1, txtName.Text, whereStatus, "CreateDate DESC");
            }

            grdProduct.DataSource = dtSet.Tables[0];
            grdProduct.DataBind();
            if (dtSet.Tables[0].Rows.Count > 0)
            {
                TotalItem = Convert.ToInt32(dtSet.Tables[2].Rows[0]["TotalRecord"]);
                if (Convert.ToInt32(dtSet.Tables[2].Rows[0]["TotalRecord"]) % pageSize != 0)
                {
                    TotalPage = (Convert.ToInt32(dtSet.Tables[2].Rows[0]["TotalRecord"]) / pageSize) + 1;
                }
                else
                {
                    TotalPage = Convert.ToInt32(dtSet.Tables[2].Rows[0]["TotalRecord"]) / pageSize;
                }
                Pager1.ItemCount = Convert.ToInt32(dtSet.Tables[2].Rows[0]["TotalRecord"]);
                x_box_pager.Visible = Pager1.ItemCount > pageSize ? true : false;
            }
            else
            {
                x_box_pager.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("LoadProduct", ex.ToString());
        }
    }
    protected void Pager1_Command(object sender, CommandEventArgs e)
    {

        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        Pager1.CurrentIndex = currnetPageIndx;
        LoadProduct();

    }
    private void FillProductBrand()
    {
        try
        {
            //DataTable dt = new DataTable();
            //dt = BusinessRulesLocator.GetProductBrandBO().GetAsDataTable(" Active=1", " Sort, Name ASC");
            //ddlProductBrand.DataSource = dt;
            //ddlProductBrand.DataTextField = "Name";
            //ddlProductBrand.DataValueField = "ProductBrand_ID";
            //ddlProductBrand.DataBind();
            //ddlProductBrand.Items.Insert(0, new ListItem("-- Chọn doanh nghiệp --", "0"));
            if (!string.IsNullOrEmpty(ProductBrandList.Value))
            {
                Common.FillProductBrand(ddlProductBrand, " and ProductBrand_ID in (" + ProductBrandList.Value + ")");
            }
            else
            {
                Common.FillProductBrand(ddlProductBrand, " ");
            }
            if (Common.GetFunctionGroupDN())
            {
                ddlProductBrand.SelectedValue = MyUser.GetProductBrand_ID();
                ddlProductBrand.Enabled = false;
            }
            else
            {
                //ckChung.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("FillDDLCha", ex.ToString());
        }
    }
    private void FillDDLQuality()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetQualityBO().GetAsDataTable(" ", " Name ASC");
            ddlTieuChuan.DataSource = dt;
            ddlTieuChuan.DataTextField = "Name";
            ddlTieuChuan.DataValueField = "Quality_ID";
            ddlTieuChuan.DataBind();
            ddlTieuChuan.Items.Insert(0, new ListItem("-- Chọn tiêu chuẩn --", "0"));
        }
        catch (Exception ex)
        {
            Log.writeLog("FillDDLCha", ex.ToString());
        }
    }
    private void FillDllProductSet()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetProductSetBO().GetAsDataTable("", "Name DESC");
            ddlProductSet.DataSource = dt;
            ddlProductSet.DataTextField = "Name";
            ddlProductSet.DataValueField = "ProductSet_ID";
            ddlProductSet.DataBind();
            ddlProductSet.Items.Insert(0, new ListItem("--- Chọn bộ sản phẩm ---", "0"));


        }
        catch (Exception ex)
        {
            Log.writeLog("FillDllProductSet", ex.ToString());
        }
    }
    private void FillDDLColor()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetProductColorBO().GetAsDataTable("", "");
            ddlColor.DataSource = dt;
            ddlColor.DataTextField = "Name";
            ddlColor.DataValueField = "Color_ID";
            ddlColor.DataBind();
            ddlColor.Items.Insert(0, new ListItem("-- Chọn Màu --", "0"));
        }
        catch (Exception ex)
        {
            Log.writeLog("FillDDLCha", ex.ToString());
        }
    }
    private void FillDDLMaterial()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetMaterialCategoryBO().GetAsDataTable("Active=1", "");
            ddlMaterial.DataSource = dt;
            ddlMaterial.DataTextField = "Name";
            ddlMaterial.DataValueField = "MaterialCategory_ID";
            ddlMaterial.DataBind();
            ddlMaterial.Items.Insert(0, new ListItem("-- Chọn vật liệu --", "0"));
        }
        catch (Exception ex)
        {
            Log.writeLog("FillDDLCha", ex.ToString());
        }
    }
    protected void ResetMsg()
    {
        lblMessage.Text = "";
        lblMessage.Visible = false;
    }
    protected void LoadProductCategory()
    {
        try
        {
            //DataTable dtProductCategoryParent = new DataTable();
            //dtProductCategoryParent.Clear();
            //dtProductCategoryParent.Columns.Add("ProductCategory_ID");
            //dtProductCategoryParent.Columns.Add("Parent_ID");
            //dtProductCategoryParent.Columns.Add("Name");
            //dtProductCategoryParent.Columns.Add("Image");
            //dtProductCategoryParent.Columns.Add("Active");

            //DataTable dt = new DataTable();
            //dt = BusinessRulesLocator.GetProductCategoryBO().GetAsDataTable(" Active <>-1 ", " Sort ASC");

            //foreach (DataRow item in dt.Rows)
            //{
            //    DataRow itemProductCategoryParent = dtProductCategoryParent.NewRow();
            //    itemProductCategoryParent["ProductCategory_ID"] = item["ProductCategory_ID"];
            //    itemProductCategoryParent["Parent_ID"] = item["Parent_ID"];
            //    itemProductCategoryParent["Name"] = item["Name"];
            //    itemProductCategoryParent["Image"] = item["Image"];
            //    itemProductCategoryParent["Active"] = item["Active"];
            //    dtProductCategoryParent.Rows.Add(itemProductCategoryParent);
            //    if (item["ProductCategory_ID"] != null)
            //    {
            //        DataTable dtChild = new DataTable();
            //        dtChild = BusinessRulesLocator.GetProductCategoryBO().GetAsDataTable(" Parent_ID =" + item["ProductCategory_ID"], " Sort ASC");
            //        if (dtChild.Rows.Count > 0)
            //        {
            //            foreach (DataRow itemChild in dtChild.Rows)
            //            {
            //                itemProductCategoryParent = dtProductCategoryParent.NewRow();
            //                itemProductCategoryParent["ProductCategory_ID"] = itemChild["ProductCategory_ID"];
            //                itemProductCategoryParent["Parent_ID"] = item["ProductCategory_ID"];
            //                itemProductCategoryParent["Name"] = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;") + " -" + itemChild["Name"];
            //                itemProductCategoryParent["Image"] = itemChild["Image"];
            //                itemProductCategoryParent["Active"] = itemChild["Active"];
            //                dtProductCategoryParent.Rows.Add(itemProductCategoryParent);
            //            }

            //        }
            //    }
            //}

            ddlCha.DataSource = BusinessRulesLocator.Conllection().GetProductCategory();
            //ddlCha.DataSource = dtProductCategoryParent;
            ddlCha.DataTextField = "Name";
            ddlCha.DataValueField = "ProductCategory_ID";
            ddlCha.DataBind();
            ddlCha.Items.Insert(0, new ListItem("-- Chọn danh mục --", "0"));
        }
        catch (Exception ex)
        {

            Log.writeLog("LoadUser", ex.ToString());
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("Product_Add.aspx", false);
    }



    protected void rptProductCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }


    protected void ctlDatePicker1_DateChange(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            Pager1.CurrentIndex = 1;
            LoadProduct();
        }
    }

    protected void ddlCha_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlCha.SelectedValue != "0")
        {
            GetListID(Convert.ToInt32(ddlCha.SelectedValue));
        }

        Pager1.CurrentIndex = 1;
        LoadProduct();
    }

    protected void ddlTieuChuan_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pager1.CurrentIndex = 1;
        LoadProduct();
    }
    protected void ddlProductSet_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pager1.CurrentIndex = 1;
        LoadProduct();
    }

    protected void grdProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Literal lblApproved = e.Item.FindControl("lblApproved") as Literal;
            LinkButton btnActive = e.Item.FindControl("btnActive") as LinkButton;
            LinkButton btnDeactive = e.Item.FindControl("btnDeactive") as LinkButton;
            Literal lblText = e.Item.FindControl("lblText") as Literal;
            Literal lblID = e.Item.FindControl("lblID") as Literal;

            // Hot - Home - New
            LinkButton btnDeactiveHome = e.Item.FindControl("btnDeactiveHome") as LinkButton;
            LinkButton btnActiveHome = e.Item.FindControl("btnActiveHome") as LinkButton;
            LinkButton btnDeactiveHot = e.Item.FindControl("btnDeactiveHot") as LinkButton;
            LinkButton btnActiveHot = e.Item.FindControl("btnActiveHot") as LinkButton;
            LinkButton btnDeactiveNew = e.Item.FindControl("btnDeactiveNew") as LinkButton;
            LinkButton btnActiveNew = e.Item.FindControl("btnActiveNew") as LinkButton;
            //QR
            LinkButton btnActiveQR = e.Item.FindControl("btnActiveQR") as LinkButton;
            LinkButton btnDeactiveQR = e.Item.FindControl("btnDeactiveQR") as LinkButton;

            Literal lblHome = e.Item.FindControl("lblHome") as Literal;
            Literal lblHot = e.Item.FindControl("lblHot") as Literal;
            Literal lblNew = e.Item.FindControl("lblNew") as Literal;
            Literal lblIsQrCode = e.Item.FindControl("lblIsQrCode") as Literal;

            Literal lblView = e.Item.FindControl("lblView") as Literal;
            Literal lblViewURL = e.Item.FindControl("lblViewURL") as Literal;

            // CheckBox ckActive = e.Item.FindControl("ckActive") as CheckBox;
            if (lblApproved != null)
            {
                if (lblApproved.Text == "False")
                {
                    btnDeactive.Visible = true;
                    btnActive.Visible = false;
                    lblText.Text = "Hủy đăng";
                    lblView.Text = "";
                }
                else
                {
                    lblText.Text = "Cho đăng";
                    btnDeactive.Visible = false;
                    btnActive.Visible = true;
                    lblView.Text = "<a target='blank' href='https://www.xuanhoa.vn/sp/" + lblViewURL.Text + "' style='color: #303e67;font-size:15px;float:right;border: none;padding: 0;background: none;' class=''> >> View trên Web</a >";
                }
            }
            if (lblIsQrCode != null)
            {
                if (lblIsQrCode.Text == "True")
                {
                    btnDeactiveQR.Visible = false;
                    btnActiveQR.Visible = true;
                }
                else
                {
                    btnDeactiveQR.Visible = true;
                    btnActiveQR.Visible = false;

                }
            }
            if (lblHome != null)
            {
                if (lblHome.Text == "0")
                {
                    btnDeactiveHome.Visible = false;
                    btnActiveHome.Visible = true;
                }
                else
                {
                    btnDeactiveHome.Visible = true;
                    btnActiveHome.Visible = false;
                }
            }

            if (lblHot != null)
            {
                if (lblHot.Text == "0")
                {
                    btnDeactiveHot.Visible = false;
                    btnActiveHot.Visible = true;
                }
                else
                {
                    btnDeactiveHot.Visible = true;
                    btnActiveHot.Visible = false;
                }
            }
            if (lblNew != null)
            {
                if (lblNew.Text == "0")
                {
                    btnDeactiveNew.Visible = false;
                    btnActiveNew.Visible = true;
                }
                else
                {
                    btnDeactiveNew.Visible = true;
                    btnActiveNew.Visible = false;
                }
            }

            //if (!MyActionPermission.CanDeleteProduct(Convert.ToInt32(lblID.Text), ref Message))
            //{

            //    btnDeactive.Visible = MyActionPermission.CanDeleteProduct(Convert.ToInt32(lblID.Text), ref Message);
            //}
            //else
            //{
            //    btnActive.Visible = MyActionPermission.CanDeleteProduct(Convert.ToInt32(lblID.Text), ref Message);
            //}
        }
    }

    protected void grdProduct_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int Product_ID = Convert.ToInt32(e.CommandArgument);
        ProductRow _ProductRow = new ProductRow();
        _ProductRow = BusinessRulesLocator.GetProductBO().GetByPrimaryKey(Product_ID);
        switch (e.CommandName)
        {
            case "Copy":

                _ProductRow = BusinessRulesLocator.GetProductBO().GetByPrimaryKey(Product_ID);
                if (_ProductRow != null)
                {
                    _ProductRow.Name = _ProductRow.Name + " Copy";
                    _ProductRow.CreateBy = MyUser.GetUser_ID();
                    _ProductRow.CreateDate = DateTime.Now;
                    _ProductRow.LastEditBy = MyUser.GetUser_ID();
                    _ProductRow.LastEditDate = DateTime.Now;
                    _ProductRow.Product_ID_Parent = Product_ID;
                    if (Common.GetFunctionGroupDN())
                        _ProductRow.ProductBrand_ID = Convert.ToInt32(MyUser.GetProductBrand_ID());
                    BusinessRulesLocator.GetProductBO().Insert(_ProductRow);
                    if (_ProductRow != null)
                    {
                        DataTable dt = BusinessRulesLocator.GetTaskStepBO().GetAsDataTable(" Product_ID=" + Product_ID, " Sort ASC");
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                TaskStepRow _TaskStepRow = new TaskStepRow();
                                _TaskStepRow.Product_ID = _ProductRow.Product_ID;
                                _TaskStepRow.Name = item["Name"].ToString();
                                _TaskStepRow.Sort = Convert.ToInt32(item["Sort"].ToString());
                                BusinessRulesLocator.GetTaskStepBO().Insert(_TaskStepRow);
                            }
                        }
                    }
                    lblMessage.Text = "Nhân bản sản phẩm thành công!";
                }
                else
                {
                    lblMessage.Text = "Nhân bản sản phẩm thất bại!";
                    lblMessage.Style.Add("background", "wheat");
                    lblMessage.ForeColor = Color.Red;
                }
                break;
            case "Delete":
                if (MyActionPermission.CanDeleteProduct(Product_ID, ref Message))
                {
                    _ProductRow.Deleted = true;
                    MyActionPermission.WriteLogSystem(Product_ID, "Xóa - " + _ProductRow.Name);
                    BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                    lblMessage.Text = ("Xóa bản ghi thành công !");
                }
                else
                {
                    lblMessage.Text = Message;
                    lblMessage.Style.Add("background", "wheat");
                    lblMessage.ForeColor = Color.Red;
                }
                break;
            case "Active":
                //if (MyActionPermission.CanDeleteProduct(Product_ID, ref Message))
                //{
                _ProductRow.Active = true;
                BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                lblMessage.Text = ("Kích hoạt thành công !");
                //}
                //else
                //{
                //    lblMessage.Text = Message;
                //    lblMessage.Style.Add("background", "wheat");
                //    lblMessage.ForeColor = Color.Red;
                //}
                break;
            case "Deactive":
                //if (MyActionPermission.CanDeleteProduct(Product_ID, ref Message))
                //{
                _ProductRow.Active = false;
                BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                lblMessage.Text = ("Ngừng kích hoạt thành công !");
                //}
                //else
                //{
                //    lblMessage.Text = Message;
                //    lblMessage.Style.Add("background", "wheat");
                //    lblMessage.ForeColor = Color.Red;
                //}
                break;
            case "DeactiveHome":
                _ProductRow.IsHome = 0;
                BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                lblMessage.Text = ("Ngừng kích hoạt thành công !");
                break;
            case "ActiveHome":
                _ProductRow.IsHome = 1;
                BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                lblMessage.Text = ("Kích hoạt thành công !");
                break;
            case "DeactiveHot":
                _ProductRow.IsHot = 0;
                BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                lblMessage.Text = ("Ngừng kích hoạt thành công !");
                break;
            case "ActiveHot":
                _ProductRow.IsHot = 1;
                BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                lblMessage.Text = ("Kích hoạt thành công !");
                break;
            case "DeactiveNew":
                _ProductRow.IsNew = 0;
                BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                lblMessage.Text = ("Ngừng kích hoạt thành công !");
                break;
            case "ActiveNew":
                _ProductRow.IsNew = 1;
                BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                lblMessage.Text = ("Kích hoạt thành công !");
                break;
            case "DeactiveQR":
                if (MyActionPermission.CanDeleteProduct(Product_ID, ref Message))
                {
                    _ProductRow.IsQRCode = false;
                    BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                    lblMessage.Text = ("Ngừng kích hoạt thành công !");
                }
                else
                {
                    lblMessage.Text = "Sản phẩm đã kích hoạt lô mã định danh. Không thể hủy";
                    lblMessage.Style.Add("background", "wheat");
                    lblMessage.ForeColor = Color.Red;
                }
                break;
            case "ActiveQR":
                _ProductRow.IsQRCode = true;
                BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                lblMessage.Text = ("Kích hoạt thành công !");
                break;
        }
        lblMessage.Visible = true;
        LoadProduct();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Pager1.CurrentIndex = 1;
        LoadProduct();
    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pager1.CurrentIndex = 1;
        LoadProduct();
    }

    protected void ddlProductBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pager1.CurrentIndex = 1;
        LoadProduct();
    }
    protected void ddlProductInWarehouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pager1.CurrentIndex = 1;
        LoadProduct();
    }
    protected void ddlColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pager1.CurrentIndex = 1;
        LoadProduct();
    }
    protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pager1.CurrentIndex = 1;
        LoadProduct();
    }
    protected void ddlStatusType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pager1.CurrentIndex = 1;
        LoadProduct();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataSet dtSet = new DataSet();
        if (!string.IsNullOrEmpty(ProductBrandList.Value))
        {
            string where = " and P.ProductBrand_ID in (" + ProductBrandList.Value + ")";
            //dtSet = BusinessRulesLocator.Conllection().GetProductV3(Pager1.CurrentIndex, 5000, 7, 0, Convert.ToInt32(ddlProductSet.SelectedValue), Convert.ToInt32(ddlColor.SelectedValue), txtSize.Text, Convert.ToInt32(ddlMaterial.SelectedValue), Convert.ToInt32(ddlTieuChuan.SelectedValue), Convert.ToInt32(ddlProductBrand.SelectedValue), 0, "", ctlDatePicker1.FromDate, ctlDatePicker1.ToDate, listCategory_ID, txtName.Text, ProductBrandList.Value, " CreateDate DESC");
        }
        else
        {
            //dtSet = BusinessRulesLocator.Conllection().GetProductV3(Pager1.CurrentIndex, 5000, 7, 0, Convert.ToInt32(ddlProductSet.SelectedValue), Convert.ToInt32(ddlColor.SelectedValue), txtSize.Text, Convert.ToInt32(ddlMaterial.SelectedValue), Convert.ToInt32(ddlTieuChuan.SelectedValue), Convert.ToInt32(ddlProductBrand.SelectedValue), 0, "", ctlDatePicker1.FromDate, ctlDatePicker1.ToDate, listCategory_ID, txtName.Text, "", " CreateDate DESC");
        }
        dt = dtSet.Tables[0];
        dt.Columns.Remove("Product_ID");
        //dt.Columns.Remove("Image");
        dt.Columns.Remove("ProductCategory_ID");
        dt.Columns.Remove("UserName");
        dt.Columns.Remove("NguoiSua");
        dt.Columns.Remove("Active");
        dt.Columns.Remove("CreateDate");
        dt.Columns.Remove("LastEditBy");
        dt.Columns.Remove("LastEditDate");
        dt.Columns.Remove("CreateBy");
        dt.Columns.Remove("IsHome");
        dt.Columns.Remove("IsHot");
        dt.Columns["RowNum"].ColumnName = "STT";
        dt.Columns["Name"].ColumnName = "Tên sản phẩm";
        dt.Columns["ProductCategoryName"].ColumnName = "Danh mục sản phẩm";
        dt.Columns["SKU_ID"].ColumnName = "Mã SKU Hệ Thống";
        dt.Columns["SKU_Web_ID"].ColumnName = "Mã SKU WEB";
        dt.Columns["NameMaterial"].ColumnName = "Vật liệu";
        dt.Columns["Size"].ColumnName = "Kích thước";
        dt.Columns["Color"].ColumnName = "Màu sản phẩm";
        dt.Columns["URL"].ColumnName = "URL sản phẩm";
        foreach (DataRow row in dt.Rows)
        {
            row["Image"] = "http://xuanhoa.check.net.vn/data/product/mainimages/original/" + row["Image"].ToString();

        }
        dt.AcceptChanges();
        string tab = "DANH SÁCH SẢN PHẨM (Tổng: " + dt.Rows.Count + ")\n\n";

        GridView gvDetails = new GridView();

        gvDetails.DataSource = dt;

        gvDetails.AllowPaging = false;

        gvDetails.DataBind();

        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "danh-sach-san-pham.xls"));
        Response.ContentEncoding = System.Text.Encoding.Unicode;
        Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);



        //Change the Header Row back to white color
        // gvDetails.HeaderRow.Style.Add("background-color", "#7e94eb");
        gvDetails.HeaderRow.Style.Add("color", "#fff");
        //Applying stlye to gridview header cells
        for (int i = 0; i < gvDetails.HeaderRow.Cells.Count; i++)
        {
            gvDetails.HeaderRow.Cells[i].Style.Add("background-color", "#7e94eb");
        }
        gvDetails.RenderControl(htw);

        Response.Write(tab + sw.ToString());
        Response.End();
    }
    public string QRCode(string Product_ID)
    {

        string img = string.Empty;
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode("https://esupplychain.vn/p/" + Product_ID.ToString(), QRCodeGenerator.ECCLevel.Q);
        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
        imgBarCode.Height = 150;
        imgBarCode.Width = 150;
        using (Bitmap bitMap = qrCode.GetGraphic(20))
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                img = "<img width='70px' src='" + "data:image/png;base64," + Convert.ToBase64String(byteImage) + "'/>";
            }
        }
        return img;
    }
    protected DataTable DeQuyDanhMucListID(DataTable dt, int ParentID)
    {

        try
        {

            if (ParentID > 0)
            {

                DataTable dtChild = new DataTable();
                dtChild = BusinessRulesLocator.GetProductCategoryBO().GetAsDataTable(" Parent_ID =" + ParentID + "  and Active <> - 1", " Sort ASC");
                if (dtChild.Rows.Count > 0)
                {
                    foreach (DataRow itemChild in dtChild.Rows)
                    {

                        DataRow itemProductCategoryParent = dt.NewRow();
                        itemProductCategoryParent["ProductCategory_ID"] = itemChild["ProductCategory_ID"];
                        dt.Rows.Add(itemProductCategoryParent);
                        DeQuyDanhMucListID(dt, Convert.ToInt32(itemProductCategoryParent["ProductCategory_ID"]));
                    }
                }

            }
        }
        catch (Exception ex)
        {
            Log.writeLog("DeQuyDanhMuc", ex.ToString());
        }
        return dt;
    }
    protected void GetListID(int NewsCategory_ID)
    {
        try
        {
            DataTable dtProductCategoryParent = new DataTable();
            dtProductCategoryParent.Clear();
            dtProductCategoryParent.Columns.Add("ProductCategory_ID");
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetProductCategoryBO().GetAsDataTable("Active <>-1 and ProductCategory_ID=" + NewsCategory_ID, " ProductCategory_ID ASC");
            foreach (DataRow item in dt.Rows)
            {
                DataRow itemProductCategoryParent = dtProductCategoryParent.NewRow();
                itemProductCategoryParent["ProductCategory_ID"] = item["ProductCategory_ID"];
                dtProductCategoryParent.Rows.Add(itemProductCategoryParent);
                DeQuyDanhMucListID(dtProductCategoryParent, Convert.ToInt32(itemProductCategoryParent["ProductCategory_ID"]));
            }
            //DataTable dt = BusinessRulesLocator.GetNewsCategoryBO().GetAsDataTable("Status=1 and Type=1", " Title ASC");
            if (dtProductCategoryParent.Rows.Count > 0)
            {
                foreach (DataRow item in dtProductCategoryParent.Rows)
                {
                    listCategory_ID += item["ProductCategory_ID"] + ",";
                }
                listCategory_ID = listCategory_ID.Remove(listCategory_ID.Length - 1);
                hdnCId.Value = listCategory_ID;
                //   Response.Write(listCategoryNews_ID);
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("LoadCateNews", ex.ToString());
        }
    }
    string listCategory_ID = string.Empty;
    string listCategory_ID1 = string.Empty;

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        Response.Redirect("TaskStepProduct_Copy", false);
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        Response.Redirect("Product_Upload", false);
    }
    protected void btnIcon_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductIcon_List", false);
    }
    protected void btnColor_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductColor_List", false);
    }
    protected void btnSpace_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductSpace_List", false);
    }
    protected void btnClassify_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductClassify_List", false);
    }
    protected void btnMaterial_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/Material/MaterialCategory_List.aspx");
    }
    protected void btnProductType_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductType_List", false);
    }
    protected void btnURL_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = BusinessRulesLocator.Conllection().GetAllList(@"Select Product_ID,Name from Product where URL is null");
            foreach (DataRow dtRow in dt.Rows)
            {
                ProductRow product = BusinessRulesLocator.GetProductBO().GetByPrimaryKey(Convert.ToInt32(dtRow["Product_ID"].ToString()));
                if (product != null)
                {
                    product.URL = Common.ConvertTitleDomain(dtRow["Name"].ToString() + "-" + dtRow["Product_ID"].ToString());
                    BusinessRulesLocator.GetProductBO().Update(product);
                }
            }
            lblMessage.Text = "Cập nhật URL thành công!";
            lblMessage.Visible = true;
        }
        catch (Exception ex)
        {
            Log.writeLog("btnURL_Click", ex.ToString());

        }

    }
    protected void ckShow_Checked(object sender, EventArgs e)
    {
        if (ckShow.Checked)
        {
            pnOpen.Visible = true;
            ckShow.Text = "Ẩn tìm kiếm mở rộng";
        }
        else
        {
            pnOpen.Visible = false;
            ckShow.Text = "Tìm kiếm mở rộng";
        }
    }

    protected void LoadCheck()
    {
        try
        {
            string[] value = ProductList.Value.Trim().Split(',');
            foreach (var itemCheck in value)
            {
                if (itemCheck != "")
                {
                    foreach (RepeaterItem item in grdProduct.Items)
                    {
                        Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                        CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                        if (itemCheck == lblProductID1.Text)
                        {
                            ckproductID1.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            Log.writeLog("LoadCheck", ex.ToString());
        }
    }
    #region Button
    protected void btnIsHome_Click(object sender, EventArgs e)
    {
        try
        {
            string ListProductID = string.Empty;
            foreach (RepeaterItem item in grdProduct.Items)
            {
                Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                if (ckproductID1.Checked)
                {
                    ListProductID += lblProductID1.Text + ",";
                }
            }
            if (!string.IsNullOrEmpty(ListProductID))
            {
                ListProductID = ListProductID.Remove(ListProductID.Length - 1);
                ProductList.Value = ListProductID;
                BusinessRulesLocator.Conllection().UpdateStatusProductIDHome(1, ListProductID);
                lblMessage.Text = "Cập nhật thành công";
                lblMessage.Visible = true;

            }

            LoadProduct();
            LoadCheck();
        }
        catch (Exception ex)
        {

            Log.writeLog("btnIsHome_Click", ex.ToString());
        }
    }
    protected void btnCloseIsHome_Click(object sender, EventArgs e)
    {
        try
        {
            string ListProductID = string.Empty;
            foreach (RepeaterItem item in grdProduct.Items)
            {
                Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                if (ckproductID1.Checked)
                {
                    ListProductID += lblProductID1.Text + ",";
                }
            }

            if (!string.IsNullOrEmpty(ListProductID))
            {
                ListProductID = ListProductID.Remove(ListProductID.Length - 1);
                ProductList.Value = ListProductID;
                BusinessRulesLocator.Conllection().UpdateStatusProductIDHome(0, ListProductID);
                lblMessage.Text = "Cập nhật thành công";
                lblMessage.Visible = true;

            }

            LoadProduct();
            LoadCheck();
        }
        catch (Exception ex)
        {

            Log.writeLog("btnIsHome_Click", ex.ToString());
        }
    }
    protected void btnIsHot_Click(object sender, EventArgs e)
    {
        try
        {
            string ListProductID = string.Empty;
            foreach (RepeaterItem item in grdProduct.Items)
            {
                Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                if (ckproductID1.Checked)
                {
                    ListProductID += lblProductID1.Text + ",";
                }
            }
            if (!string.IsNullOrEmpty(ListProductID))
            {
                ListProductID = ListProductID.Remove(ListProductID.Length - 1);
                ProductList.Value = ListProductID;
                BusinessRulesLocator.Conllection().UpdateStatusProductIDHot(1, ListProductID);
                lblMessage.Text = "Cập nhật thành công";
                lblMessage.Visible = true;

            }
            LoadProduct();
            LoadCheck();
        }
        catch (Exception ex)
        {

            Log.writeLog("btnIsHome_Click", ex.ToString());
        }
    }
    protected void btnCloseIsHot_Click(object sender, EventArgs e)
    {
        try
        {
            string ListProductID = string.Empty;
            foreach (RepeaterItem item in grdProduct.Items)
            {
                Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                if (ckproductID1.Checked)
                {
                    ListProductID += lblProductID1.Text + ",";
                }
            }
            if (!string.IsNullOrEmpty(ListProductID))
            {
                ListProductID = ListProductID.Remove(ListProductID.Length - 1);
                ProductList.Value = ListProductID;
                BusinessRulesLocator.Conllection().UpdateStatusProductIDHot(0, ListProductID);
                lblMessage.Text = "Cập nhật thành công";
                lblMessage.Visible = true;

            }
            LoadProduct();
            LoadCheck();
        }
        catch (Exception ex)
        {

            Log.writeLog("btnIsHome_Click", ex.ToString());
        }
    }
    protected void btnActice_Click(object sender, EventArgs e)
    {
        try
        {
            string ListProductID = string.Empty;
            foreach (RepeaterItem item in grdProduct.Items)
            {
                Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                if (ckproductID1.Checked)
                {
                    ListProductID += lblProductID1.Text + ",";
                }
            }
            if (!string.IsNullOrEmpty(ListProductID))
            {
                ListProductID = ListProductID.Remove(ListProductID.Length - 1);
                ProductList.Value = ListProductID;
                BusinessRulesLocator.Conllection().UpdateStatusProductIDActive(1, ListProductID);
                lblMessage.Text = "Cập nhật thành công";
                lblMessage.Visible = true;

            }
            LoadProduct();
            LoadCheck();
        }
        catch (Exception ex)
        {

            Log.writeLog("btnIsHome_Click", ex.ToString());
        }
    }
    protected void btnCloseActive_Click(object sender, EventArgs e)
    {
        try
        {
            string ListProductID = string.Empty;
            foreach (RepeaterItem item in grdProduct.Items)
            {
                Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                if (ckproductID1.Checked)
                {
                    ListProductID += lblProductID1.Text + ",";
                }
            }
            if (!string.IsNullOrEmpty(ListProductID))
            {
                ListProductID = ListProductID.Remove(ListProductID.Length - 1);
                ProductList.Value = ListProductID;
                BusinessRulesLocator.Conllection().UpdateStatusProductIDActive(0, ListProductID);
                lblMessage.Text = "Cập nhật thành công";
                lblMessage.Visible = true;

            }
            LoadProduct();
            LoadCheck();
        }
        catch (Exception ex)
        {

            Log.writeLog("btnIsHome_Click", ex.ToString());
        }
    }
    protected void btnIsQRCode_Click(object sender, EventArgs e)
    {
        try
        {
            string ListProductID = string.Empty;
            foreach (RepeaterItem item in grdProduct.Items)
            {
                Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                if (ckproductID1.Checked)
                {
                    ListProductID += lblProductID1.Text + ",";
                }
            }
            if (!string.IsNullOrEmpty(ListProductID))
            {
                ListProductID = ListProductID.Remove(ListProductID.Length - 1);
                ProductList.Value = ListProductID;
                BusinessRulesLocator.Conllection().UpdateStatusProductIDQRCode(1, ListProductID);
                lblMessage.Text = "Cập nhật thành công";
                lblMessage.Visible = true;

            }
            LoadProduct();
            LoadCheck();
        }
        catch (Exception ex)
        {

            Log.writeLog("btnIsHome_Click", ex.ToString());
        }
    }
    protected void btnUnIsQRCode_Click(object sender, EventArgs e)
    {
        try
        {
            string ListProductID = string.Empty;
            foreach (RepeaterItem item in grdProduct.Items)
            {
                Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                if (ckproductID1.Checked)
                {
                    ListProductID += lblProductID1.Text + ",";
                }
            }
            if (!string.IsNullOrEmpty(ListProductID))
            {
                ListProductID = ListProductID.Remove(ListProductID.Length - 1);
                ProductList.Value = ListProductID;
                BusinessRulesLocator.Conllection().UpdateStatusProductIDQRCode(0, ListProductID);
                lblMessage.Text = "Cập nhật thành công";
                lblMessage.Visible = true;

            }
            LoadProduct();
            LoadCheck();
        }
        catch (Exception ex)
        {

            Log.writeLog("btnIsHome_Click", ex.ToString());
        }
    }
    protected void btnCloseIsNew_Click(object sender, EventArgs e)
    {
        try
        {
            string ListProductID = string.Empty;
            foreach (RepeaterItem item in grdProduct.Items)
            {
                Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                if (ckproductID1.Checked)
                {
                    ListProductID += lblProductID1.Text + ",";
                }
            }

            if (!string.IsNullOrEmpty(ListProductID))
            {
                ListProductID = ListProductID.Remove(ListProductID.Length - 1);
                ProductList.Value = ListProductID;
                BusinessRulesLocator.Conllection().UpdateStatusProductIDNew(0, ListProductID);
                lblMessage.Text = "Cập nhật thành công";
                lblMessage.Visible = true;

            }

            LoadProduct();
            LoadCheck();
        }
        catch (Exception ex)
        {

            Log.writeLog("btnIsHome_Click", ex.ToString());
        }
    }
    protected void btnIsNew_Click(object sender, EventArgs e)
    {
        try
        {
            string ListProductID = string.Empty;
            foreach (RepeaterItem item in grdProduct.Items)
            {
                Literal lblProductID1 = (Literal)item.FindControl("lblProductID1");
                CheckBox ckproductID1 = (CheckBox)item.FindControl("ckproductID1");

                if (ckproductID1.Checked)
                {
                    ListProductID += lblProductID1.Text + ",";
                }
            }
            if (!string.IsNullOrEmpty(ListProductID))
            {
                ListProductID = ListProductID.Remove(ListProductID.Length - 1);
                ProductList.Value = ListProductID;
                BusinessRulesLocator.Conllection().UpdateStatusProductIDNew(1, ListProductID);
                lblMessage.Text = "Cập nhật thành công";
                lblMessage.Visible = true;

            }
            LoadProduct();
            LoadCheck();
        }
        catch (Exception ex)
        {

            Log.writeLog("btnIsHome_Click", ex.ToString());
        }
    }
    #endregion


}