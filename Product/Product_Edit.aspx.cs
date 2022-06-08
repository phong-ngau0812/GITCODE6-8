using DbObj;
using evointernal;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SystemFrameWork;

public partial class Product_Edit : System.Web.UI.Page
{
    int Product_ID = 0;
    public string title = "Thông tin sản phẩm";
    public string avatar = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "/ckfinder";
        _FileBrowser.SetupCKEditor(txtContent);
        if (!string.IsNullOrEmpty(Request["Product_ID"]))
            int.TryParse(Request["Product_ID"].ToString(), out Product_ID);
        if (!Page.IsPostBack)
        {
            LoadProductBH();
            LoadColor();
            LoadProductSet();
            LoadProductType();
            LoadProductClassify();
            LoadProductMaterial();
            LoadProductCategory();
            LoadProductParent();
            LoadProductSpace();
            LoadProductAttach();
            FillInfoProduct();
            //QRCode();
        }

        ResetMsg();
    }
    protected void ResetMsg()
    {
        lblMessage.Text = "";
        lblMessage.Visible = false;
    }
    private void LoadProductBH()
    {
        string where = string.Empty;
        //if (ddlProductSet.SelectedValue != "")
        //{
        //    where += "And ProductSet_ID=" + ddlProductSet.SelectedValue;
        //}
        if (Product_ID != 0)
        {
            where += "And Product_ID !=" + Product_ID;
        }
        ddlProductBH.DataSource = BusinessRulesLocator.GetProductBO().GetAsDataTable("IsQRCode = 1" + where, "CreateDate DESC");
        ddlProductBH.DataValueField = "Product_ID";
        ddlProductBH.DataTextField = "Name";
        ddlProductBH.DataBind();
    }
    private void LoadProductSpace()
    {
        ddlProductSpace.DataSource = BusinessRulesLocator.GetWEB_SpaceBO().GetAsDataTable("Active = 1", "CreateDate DESC");
        ddlProductSpace.DataValueField = "Space_ID";
        ddlProductSpace.DataTextField = "Name";
        ddlProductSpace.DataBind();
        ddlProductSpace.Items.Insert(0, new ListItem("-- Chọn không gian sản phẩm --", "0"));

    }

    private void LoadProductAttach()
    {
        ddlProductAttach.DataSource = BusinessRulesLocator.Conllection().GetAllList(@"Select Product_ID, Name from Product where Active = 1 Order By CreateDate DESC");
        ddlProductAttach.DataValueField = "Product_ID";
        ddlProductAttach.DataTextField = "Name";
        ddlProductAttach.DataBind();
        ddlProductAttach.Items.Insert(0, new ListItem("-- Chọn sản phẩm đính kèm --", "0"));
    }
    protected void LoadProductMaterial()
    {
        DataTable dt = BusinessRulesLocator.GetMaterialCategoryBO().GetAsDataTable("Active=1", "");
        ddlMaterial.DataSource = dt;
        ddlMaterial.DataTextField = "Name";
        ddlMaterial.DataValueField = "MaterialCategory_ID";
        ddlMaterial.DataBind();
        ddlMaterial.Items.Insert(0, new ListItem("-- Chọn vật liệu --", "0"));

    }
    protected void LoadProductClassify()
    {
        try
        {
            DataTable dt = BusinessRulesLocator.GetProductClassifyBO().GetAsDataTable("", "");
            ddlClassify.DataSource = dt;
            ddlClassify.DataTextField = "Name";
            ddlClassify.DataValueField = "ProductClassify_ID";
            ddlClassify.DataBind();
            ddlClassify.Items.Insert(0, new ListItem("-- Chọn phân loại --", "0"));
        }
        catch (Exception ex)
        {
            Log.writeLog("LoadProductMaterial", ex.ToString());
        }


    }
    protected void LoadProductType()
    {
        DataTable dt = BusinessRulesLocator.GetProductTypeBO().GetAsDataTable("", "");
        ddlProductType.DataSource = dt;
        ddlProductType.DataTextField = "Name";
        ddlProductType.DataValueField = "ProductType_ID";
        ddlProductType.DataBind();
        ddlProductType.Items.Insert(0, new ListItem("-- Chọn loại hàng --", "0"));

    }
    protected void LoadProductParent()
    {
        try
        {
            DataTable dtProduct = new DataTable();
            dtProduct.Clear();
            dtProduct.Columns.Add("Product_ID");
            dtProduct.Columns.Add("Parent_ID");
            dtProduct.Columns.Add("Name");
            dtProduct.Columns.Add("Active");
            DataTable dt = BusinessRulesLocator.GetProductBO().GetAsDataTable("Active = 1 and Parent_ID is null Or Parent_ID =0", "CreateDate DESC");
            foreach (DataRow item in dt.Rows)
            {
                DataRow itemProduct = dtProduct.NewRow();
                itemProduct["Product_ID"] = item["Product_ID"];
                itemProduct["Parent_ID"] = item["Parent_ID"];
                itemProduct["Name"] = item["Name"];
                itemProduct["Active"] = item["Active"];
                dtProduct.Rows.Add(itemProduct);
                if (item["Product_ID"] != null)
                {
                    DataTable dtChild = new DataTable();
                    dtChild = BusinessRulesLocator.GetProductBO().GetAsDataTable(" Parent_ID =" + item["Product_ID"], "");
                    if (dtChild.Rows.Count > 0)
                    {
                        foreach (DataRow itemChild in dtChild.Rows)
                        {
                            itemProduct = dtProduct.NewRow();
                            itemProduct["Product_ID"] = itemChild["Product_ID"];
                            itemProduct["Parent_ID"] = item["Product_ID"];
                            itemProduct["Name"] = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;") + "--" + itemChild["Name"];
                            itemProduct["Active"] = itemChild["Active"];
                            dtProduct.Rows.Add(itemProduct);
                        }

                    }

                }

            }
            ddlProduct.DataSource = dtProduct;
            ddlProduct.DataValueField = "Product_ID";
            ddlProduct.DataTextField = "Name";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("-- Sản phẩm --", "0"));

        }
        catch (Exception ex)
        {

            Log.writeLog("LoaddllProduct", ex.ToString());
        }

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
            //dt = BusinessRulesLocator.GetProductCategoryBO().GetAsDataTable(" Active<>-1 ", " Sort ASC");

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
    protected void LoadProductSet()
    {
        DataTable dt = BusinessRulesLocator.GetProductSetBO().GetAsDataTable("", "");
        ddlProductSet.DataSource = dt;
        ddlProductSet.DataTextField = "Name";
        ddlProductSet.DataValueField = "ProductSet_ID";
        ddlProductSet.DataBind();
        ddlProductSet.Items.Insert(0, new ListItem("-- Chọn Bộ sản phẩm --", "0"));
    }
    public string ChangeName(string FullNameColor)
    {


        string Result = string.Empty;

        FullNameColor = FullNameColor.Trim();


        while (FullNameColor.IndexOf("  ") != -1)
        {
            FullNameColor = FullNameColor.Replace("  ", " ");
        }

        string[] SubName = FullNameColor.Split(' ');

        for (int i = 0; i < SubName.Length; i++)
        {
            string FirstChar = SubName[i].Substring(0, 1);
            string OtherChar = SubName[i].Substring(1);
            SubName[i] = FirstChar.ToUpper() + OtherChar.ToLower();
            Result += SubName[i] + " ";
        }
        return Result;
    }

    protected void FillInfoProduct()
    {
        try
        {
            if (Product_ID != 0)
            {
                ProductRow _ProductRow = new ProductRow();
                _ProductRow = BusinessRulesLocator.GetProductBO().GetByPrimaryKey(Product_ID);
                if (_ProductRow != null)
                {
                    //Kiểm tra xem đối tượng vào sửa có cùng doanh nghiệp hay không
                    //MyActionPermission.CheckPermission(_ProductRow.ProductBrand_ID.ToString(), _ProductRow.CreateBy.ToString(), "/Admin/Product/Product_List");
                    ddlProductSet.SelectedValue = _ProductRow.IsProductSet_IDNull ? "0" : _ProductRow.ProductSet_ID.ToString();
                    LoadProductBH();
                    txtName.Text = _ProductRow.IsNameNull ? string.Empty : _ProductRow.Name;
                    ddlCha.SelectedValue = _ProductRow.IsProductCategory_IDNull ? "0" : _ProductRow.ProductCategory_ID.ToString();
                    ddlColor.SelectedValue = _ProductRow.IsColor_IDNull ? "0" : _ProductRow.Color_ID.ToString();
                    txtTag.Text = _ProductRow.IsTagsNull ? string.Empty : _ProductRow.Tags;
                    txtVideo.Text = _ProductRow.IsVideoNull ? string.Empty : _ProductRow.Video;
                    txtAgencyNorthPrice.Text = _ProductRow.IsAgencyNorthPriceNull ? string.Empty : _ProductRow.AgencyNorthPrice.ToString("N0");
                    txtAgencySouthPrice.Text = _ProductRow.IsAgencySouthPriceNull ? string.Empty : _ProductRow.AgencySouthPrice.ToString("N0");
                    //ddlMaterial.SelectedValue = _ProductRow.IsMaterialCategory_IDNull ? "0" : _ProductRow.MaterialCategory_ID.ToString();
                    txtPrice.Text = _ProductRow.IsPriceNull ? string.Empty : _ProductRow.Price.ToString("N0");
                    txtNote.Text = _ProductRow.IsDescriptionNull ? string.Empty : _ProductRow.Description;
                    txtContent.Text = _ProductRow.IsContentNull ? string.Empty : _ProductRow.Content;
                    txtSize.Text = _ProductRow.IsSizeNull ? string.Empty : _ProductRow.Size;
                    txtSKU.Text = _ProductRow.IsSKU_IDNull ? string.Empty : _ProductRow.SKU_ID;
                    txtSkuWeb.Text = _ProductRow.IsSKU_Web_IDNull ? string.Empty : _ProductRow.SKU_Web_ID;
                    ddlProductType.SelectedValue = _ProductRow.IsProductType_IDNull ? "" : _ProductRow.ProductType_ID.ToString();
                    //ddlProduct.SelectedValue = _ProductRow.IsParent_IDNull ? "" : _ProductRow.Parent_ID.ToString();
                    //ddlLevel.SelectedValue = _ProductRow.IsLevelNull ? "0" : _ProductRow.Level.ToString();
                    ddlClassify.SelectedValue = _ProductRow.IsClassifyNull ? "" : _ProductRow.Classify.ToString();
                    txtWarranty.Text = _ProductRow.IsWarrantyDateNull ? "0" : _ProductRow.WarrantyDate.ToString();
                    txtUnit.Text = _ProductRow.IsUnitNull ? string.Empty : _ProductRow.Unit.ToString();
                    txtWeight.Text = _ProductRow.IsTrongluongNull ? "0" : _ProductRow.Trongluong.ToString();
                    ckisHot.Checked = _ProductRow.IsHot.ToString() == "0" ? false : true;
                    ckisNew.Checked = _ProductRow.IsNew.ToString() == "0" ? false : true;
                    ckisHome.Checked = _ProductRow.IsHome.ToString() == "0" ? false : true;
                    txtURL.Text = _ProductRow.IsURLNull ? string.Empty : _ProductRow.URL.ToString();
                    txtKeyword.Text = _ProductRow.IsKeywordNull ? string.Empty : _ProductRow.Keyword.ToString();
                    txtSeodescription.Text = _ProductRow.IsSeoDescriptionNull ? string.Empty : _ProductRow.SeoDescription.ToString();
                    txtSeokeyword.Text = _ProductRow.IsSeoKeywordNull ? string.Empty : _ProductRow.SeoKeyword.ToString();
                    // ckisPromotion.Checked = _ProductRow.IsPromotion.ToString() == "0" ? false : true;
                    ckOpenBH.Checked = _ProductRow.IsProductTwoStamp.ToString() == "False" ? false : true;
                    if (!_ProductRow.IsIsProductTwoStampNull)
                    {
                        if (_ProductRow.IsProductTwoStamp == true)
                        {
                            spbh.Visible = true;
                        }
                    }
                    if (!_ProductRow.IsListProduct_ID_TwoStampNull)
                    {
                        string[] array = _ProductRow.ListProduct_ID_TwoStamp.Split(',');
                        foreach (string value in array)
                        {
                            foreach (ListItem item in ddlProductBH.Items)
                            {
                                if (value == item.Value)
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                    if (!_ProductRow.IsProductAttach_IDNull)
                    {
                        string[] array = _ProductRow.ProductAttach_ID.Split(',');
                        foreach (string value in array)
                        {
                            foreach (ListItem item in ddlProductAttach.Items)
                            {
                                if (value == item.Value)
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                    if (!_ProductRow.IsSpace_IDNull)
                    {
                        string[] array = _ProductRow.Space_ID.Split(',');
                        foreach (string value in array)
                        {
                            foreach (ListItem item in ddlProductSpace.Items)
                            {
                                if (value == item.Value)
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                    if (!_ProductRow.IsMaterialCategory_IDNull)
                    {
                        string[] array = _ProductRow.MaterialCategory_ID.Split(',');
                        foreach (string value in array)
                        {
                            foreach (ListItem item in ddlMaterial.Items)
                            {
                                if (value == item.Value)
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                    txtNoteAgencyNorthPrice.Text = _ProductRow.IsNoteAgencyNorthPriceNull ? "" : _ProductRow.NoteAgencyNorthPrice.ToString();
                    txtNoteAgencySouthPrice.Text = _ProductRow.IsNoteAgencySouthPriceNull ? "" : _ProductRow.NoteAgencySouthPrice.ToString();
                    if (!_ProductRow.IsImageNull)
                    {
                        imganh.ImageUrl = "../../data/product/mainimages/original/" + _ProductRow.Image;
                    }
                    txtCreateDate.Text = _ProductRow.IsCreateDateNull ? string.Empty : _ProductRow.CreateDate.ToString("dd/MM/yyyy");

                }
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("FillInfoUser", ex.ToString());
        }
    }
    protected void LoadColor()
    {
        DataTable dt = BusinessRulesLocator.GetProductColorBO().GetAsDataTable("", "");
        ddlColor.DataSource = dt;
        ddlColor.DataValueField = "Color_ID";
        ddlColor.DataTextField = "Name";
        ddlColor.DataBind();
        ddlColor.Items.Insert(0, new ListItem("-- Chọn Màu --", "0"));
    }
    protected void UpdateProduct()
    {
        try
        {
            string msg = string.Empty;
            ProductRow _ProductRow = new ProductRow();
            if (Product_ID != 0)
            {
                _ProductRow = BusinessRulesLocator.GetProductBO().GetByPrimaryKey(Product_ID);
                if (_ProductRow != null)
                {
                    _ProductRow.Name = string.IsNullOrEmpty(txtName.Text) ? string.Empty : txtName.Text;
                    if (ddlCha.SelectedValue != "")
                    {
                        _ProductRow.ProductCategory_ID = Convert.ToInt32(ddlCha.SelectedValue);
                    }

                    //if (ddlProductSet.SelectedValue != "")
                    //{
                    //    _ProductRow.ProductSet_ID = Convert.ToInt32(ddlProductSet.SelectedValue);
                    //}
                    if (ddlProductType.SelectedValue != "")
                    {
                        _ProductRow.ProductType_ID = Convert.ToInt32(ddlProductType.SelectedValue);
                    }
                    //if (ddlProduct.SelectedValue != "")
                    //{
                    //    _ProductRow.Parent_ID = Convert.ToInt32(ddlProduct.SelectedValue);
                    //}
                    if (ddlClassify.SelectedValue != "")
                    {
                        _ProductRow.Classify = Convert.ToInt32(ddlClassify.SelectedValue);
                    }

                    if (ckOpenColor.Checked == true)
                    {
                        if (ddlColor.SelectedValue != "")
                        {
                            _ProductRow.Color_ID = Convert.ToInt32(ddlColor.SelectedValue);
                        }
                        else
                        {
                            _ProductRow.Color_ID = 0;
                        }
                    }
                    else
                    {
                        if (txtColor.Text != "")
                        {
                            ProductColorRow _ProductColorRow = new ProductColorRow();
                            _ProductColorRow.Name = string.IsNullOrEmpty(txtColor.Text) ? string.Empty : ChangeName(txtColor.Text);
                            _ProductColorRow.Active = 1;
                            BusinessRulesLocator.GetProductColorBO().Insert(_ProductColorRow);
                            if (!_ProductColorRow.IsColor_IDNull)
                            {
                                _ProductRow.Color_ID = _ProductColorRow.Color_ID;
                            }
                        }
                    }
                    if (openckProductSet.Checked == true)
                    {
                        if (ddlProductSet.SelectedValue != "")
                        {
                            _ProductRow.ProductSet_ID = Convert.ToInt32(ddlProductSet.SelectedValue);
                        }
                        else
                        {
                            _ProductRow.ProductSet_ID = 0;
                        }
                    }
                    else
                    {
                        if (txtProductSet.Text != "")
                        {
                            ProductSetRow _ProductSetRow = new ProductSetRow();
                            _ProductSetRow.Name = string.IsNullOrEmpty(txtProductSet.Text) ? string.Empty : ChangeName(txtProductSet.Text);
                            _ProductSetRow.Active = 1;
                            _ProductSetRow.WarrantyDate = 365;
                            BusinessRulesLocator.GetProductSetBO().Insert(_ProductSetRow);
                            if (!_ProductSetRow.IsProductSet_IDNull)
                            {
                                _ProductRow.ProductSet_ID = _ProductSetRow.ProductSet_ID;
                            }
                        }
                    }
                    //if (ckOpenMaterial.Checked == true)
                    //{
                    //    if (ddlMaterial.SelectedValue != "")
                    //    {
                    //        _ProductRow.MaterialCategory_ID = Convert.ToInt32(ddlMaterial.SelectedValue);
                    //    }
                    //}
                    //else
                    //{
                    //    if (txtMaterial.Text != "")
                    //    {
                    //        MaterialCategoryRow _MaterialCategoryRow = new MaterialCategoryRow();
                    //        _MaterialCategoryRow.Name = string.IsNullOrEmpty(txtMaterial.Text) ? string.Empty : ChangeName(txtMaterial.Text);
                    //        _MaterialCategoryRow.Active = 1;
                    //        _MaterialCategoryRow.CreateDate = DateTime.Now;
                    //        _MaterialCategoryRow.CreateBy = MyUser.GetUser_ID();
                    //        BusinessRulesLocator.GetMaterialCategoryBO().Insert(_MaterialCategoryRow);
                    //        if (!_MaterialCategoryRow.IsMaterialCategory_IDNull)
                    //        {
                    //            _ProductRow.MaterialCategory_ID = _MaterialCategoryRow.MaterialCategory_ID;
                    //        }
                    //    }
                    //}
                    if (ckOpenBH.Checked == true)
                    {
                        _ProductRow.ListProduct_ID_TwoStamp = ADDListProductStamp_ID();
                        _ProductRow.IsProductTwoStamp = true;
                    }
                    else
                    {
                        _ProductRow.ListProduct_ID_TwoStamp = null;
                        _ProductRow.IsProductTwoStamp = false;
                    }
                    _ProductRow.MaterialCategory_ID = ADDListProductMaterial_ID();
                    _ProductRow.SKU_ID = string.IsNullOrEmpty(txtSKU.Text) ? string.Empty : txtSKU.Text;
                    _ProductRow.SKU_Web_ID = string.IsNullOrEmpty(txtSkuWeb.Text) ? string.Empty : txtSkuWeb.Text;
                    _ProductRow.Video = string.IsNullOrEmpty(txtVideo.Text) ? string.Empty : txtVideo.Text;
                    _ProductRow.AgencyNorthPrice = string.IsNullOrEmpty(txtAgencyNorthPrice.Text) ? 0 : Convert.ToInt32(txtAgencyNorthPrice.Text.Replace(",", ""));
                    _ProductRow.AgencySouthPrice = string.IsNullOrEmpty(txtAgencySouthPrice.Text) ? 0 : Convert.ToInt32(txtAgencySouthPrice.Text.Replace(",", ""));
                    _ProductRow.Tags = string.IsNullOrEmpty(txtTag.Text) ? string.Empty : txtTag.Text;
                    _ProductRow.Unit = string.IsNullOrEmpty(txtUnit.Text) ? string.Empty : txtUnit.Text;
                    _ProductRow.Price = string.IsNullOrEmpty(txtPrice.Text) ? 0 : Convert.ToInt32(txtPrice.Text.Replace(",", ""));
                    _ProductRow.Size = string.IsNullOrEmpty(txtSize.Text) ? string.Empty : txtSize.Text;
                    _ProductRow.Description = string.IsNullOrEmpty(txtNote.Text) ? string.Empty : txtNote.Text;
                    _ProductRow.Content = string.IsNullOrEmpty(txtContent.Text) ? string.Empty : txtContent.Text;
                    //_ProductRow.Level = Convert.ToInt32(ddlLevel.SelectedValue);
                    _ProductRow.WarrantyDate = string.IsNullOrEmpty(txtWarranty.Text) ? 0 : Convert.ToInt32(txtWarranty.Text);
                    //_ProductRow.URL = Common.ConvertTitleDomain(_ProductRow.Name + "-" + Product_ID);
                    _ProductRow.LastEditBy = MyUser.GetUser_ID();
                    _ProductRow.LastEditDate = DateTime.Now;
                    _ProductRow.IsHot = ckisHot.Checked ? 1 : 0;
                    _ProductRow.IsNew = ckisNew.Checked ? 2 : 0;
                    _ProductRow.NoteAgencySouthPrice = string.IsNullOrEmpty(txtNoteAgencySouthPrice.Text) ? "" : txtNoteAgencySouthPrice.Text;
                    _ProductRow.NoteAgencyNorthPrice = string.IsNullOrEmpty(txtNoteAgencyNorthPrice.Text) ? "" : txtNoteAgencyNorthPrice.Text;
                    _ProductRow.Space_ID = ADDListProductSpace_ID();
                    _ProductRow.ProductAttach_ID = ADDListProductAttach_ID();
                    _ProductRow.URL = string.IsNullOrEmpty(txtURL.Text) ? "" : txtURL.Text.ToString();
                    _ProductRow.Keyword = string.IsNullOrEmpty(txtKeyword.Text) ? "" : txtKeyword.Text.ToString();
                    // _ProductRow.IsPromotion = ckisPromotion.Checked ? 3 : 0;
                    _ProductRow.SeoKeyword = string.IsNullOrEmpty(txtSeokeyword.Text) ? "" : txtSeokeyword.Text;
                    _ProductRow.SeoDescription = string.IsNullOrEmpty(txtSeodescription.Text) ? "" : txtSeodescription.Text;
                    _ProductRow.Trongluong = string.IsNullOrEmpty(txtWeight.Text) ? 0 : Convert.ToInt32(txtWeight.Text);
                    string fileimage = "";
                    if (fulAnh.HasFile)
                    {
                        //Xóa file
                        if (!_ProductRow.IsImageNull)
                        {
                            if (_ProductRow.Image != null)
                            {
                                string strFileFullPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + _ProductRow.Image.Replace("../", "");
                                if (File.Exists(strFileFullPath))
                                {
                                    File.Delete(strFileFullPath);
                                }
                            }
                        }
                        fileimage = Product_ID + "_" + (fulAnh.FileName);
                        fulAnh.SaveAs(Server.MapPath("../../data/product/mainimages/original/" + fileimage));
                        if (!string.IsNullOrEmpty(fileimage))
                        {
                            _ProductRow.Image = fileimage;

                        }
                    }
                    if (!string.IsNullOrEmpty(txtCreateDate.Text))
                    {
                        DateTime dateC = DateTime.ParseExact(txtCreateDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        
                        _ProductRow.CreateDate = dateC.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second); ;
                    }
                    if (BusinessRulesLocator.GetProductBO().Update(_ProductRow))
                    {
                        ProductRow _ProductRow1 = new ProductRow();
                        _ProductRow1 = BusinessRulesLocator.GetProductBO().GetByPrimaryKey(Product_ID);
                        if (!_ProductRow1.IsListProduct_ID_TwoStampNull)
                        {
                            string Value = string.Empty;
                            string[] words = _ProductRow1.ListProduct_ID_TwoStamp.Trim().Split(',');
                            foreach (var word in words)
                            {
                                if (word != "")
                                {
                                    ProductRow _ProductRowTwoStamp = new ProductRow();
                                    _ProductRowTwoStamp = BusinessRulesLocator.GetProductBO().GetByPrimaryKey(Convert.ToInt32(word));
                                    if (_ProductRowTwoStamp != null)
                                    {
                                        _ProductRowTwoStamp.ListProduct_ID_TwoStamp = _ProductRow.ListProduct_ID_TwoStamp.Replace("," + word + ",", "," + Product_ID.ToString() + ",");
                                        _ProductRowTwoStamp.IsProductTwoStamp = true;
                                        BusinessRulesLocator.GetProductBO().Update(_ProductRowTwoStamp);
                                    }

                                }
                            }
                        }
                    }
                    lblMessage.Text = "Cập nhật thông tin thành công!";
                }

                lblMessage.Visible = true;
                FillInfoProduct();
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("UpdateProduct", ex.ToString());
        }
    }
    private string ADDListProductStamp_ID()
    {
        string ProductStamp_ID = string.Empty;
        try
        {
            foreach (ListItem item in ddlProductBH.Items)
            {
                if (item.Selected)
                {
                    ProductStamp_ID += item.Value + ",";
                }
            }

            if (!string.IsNullOrEmpty(ProductStamp_ID))
            {

                ProductStamp_ID = "," + ProductStamp_ID;

            }

        }
        catch (Exception ex)
        {
            Log.writeLog("Customer_ID", ex.ToString());
        }
        return ProductStamp_ID;
    }
    private string ADDListProductAttach_ID()
    {
        string ProductA_ID = string.Empty;
        try
        {
            foreach (ListItem item in ddlProductAttach.Items)
            {
                if (item.Selected)
                {
                    ProductA_ID += item.Value + ",";
                }
            }

            if (!string.IsNullOrEmpty(ProductA_ID))
            {

                ProductA_ID = "," + ProductA_ID;

            }

        }
        catch (Exception ex)
        {
            Log.writeLog("Customer_ID", ex.ToString());
        }
        return ProductA_ID;
    }
    private string ADDListProductSpace_ID()
    {
        string Space_ID = string.Empty;
        try
        {
            foreach (ListItem item in ddlProductSpace.Items)
            {
                if (item.Selected)
                {
                    Space_ID += item.Value + ",";
                }
            }

            if (!string.IsNullOrEmpty(Space_ID))
            {

                Space_ID = "," + Space_ID;

            }

        }
        catch (Exception ex)
        {
            Log.writeLog("Customer_ID", ex.ToString());
        }
        return Space_ID;
    }
    private string ADDListProductMaterial_ID()
    {
        string Material_ID = string.Empty;
        try
        {
            foreach (ListItem item in ddlMaterial.Items)
            {
                if (item.Selected)
                {
                    Material_ID += item.Value + ",";
                }
            }

            if (!string.IsNullOrEmpty(Material_ID))
            {

                Material_ID = "," + Material_ID;

            }

        }
        catch (Exception ex)
        {
            Log.writeLog("Customer_ID", ex.ToString());
        }
        return Material_ID;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                UpdateProduct();
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("btnSave_Click", ex.ToString());
        }
    }
    //public void QRCode()
    //{
    //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
    //    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode("https://esupplychain.vn/p/" + Product_ID.ToString(), QRCodeGenerator.ECCLevel.Q);
    //    System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
    //    imgBarCode.Height = 150;
    //    imgBarCode.Width = 150;
    //    using (Bitmap bitMap = qrCode.GetGraphic(20))
    //    {
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
    //            byte[] byteImage = ms.ToArray();
    //            lblQR.Text = "<img width='120px' src='" + "data:image/png;base64," + Convert.ToBase64String(byteImage) + "'/>";
    //        }
    //    }
    //}

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Product_List.aspx", false);
    }
    //protected void ddlProductSet_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    LoadProductBH();
    //}
    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProduct.SelectedValue != "")
        {
            ProductRow _ProductRow = new ProductRow();
            _ProductRow = BusinessRulesLocator.GetProductBO().GetByPrimaryKey(Convert.ToInt32(ddlProduct.SelectedValue));
            if (_ProductRow != null)
            {
                ddlProductSet.SelectedValue = _ProductRow.ProductSet_ID.ToString();
                ddlProductSet.Enabled = false;
            }
            int level = 2;
            ddlLevel.SelectedValue = level.ToString();
            ckActive.Checked = false;
            ckisHome.Checked = false;
            ckisHome.Enabled = false;
        }
        else
        {
            ddlLevel.SelectedValue = "1";
            ddlProductSet.SelectedValue = "";
            ddlProductSet.Enabled = true;
            ckActive.Checked = true;
            ckisHome.Checked = true;
            ckisHome.Enabled = false;
        }

    }
    protected void ckShowColor_Checked(object sender, EventArgs e)
    {
        if (ckOpenColor.Checked)
        {
            pnColorAvailable.Visible = true;
            pnColorUnavailable.Visible = false;
            ckOpenColor.Text = "Nhập màu";
        }
        else
        {
            pnColorAvailable.Visible = false;
            pnColorUnavailable.Visible = true;
            ckOpenColor.Text = "Sử dụng màu có sẵn";
        }
    }
    //protected void ckShowMaterial_Checked(object sender, EventArgs e)
    //{
    //    if (ckOpenMaterial.Checked)
    //    {
    //        pnMaterialAvailable.Visible = true;
    //        pnMaterialUnavailable.Visible = false;
    //        ckOpenMaterial.Text = "Nhập vật liệu";
    //    }
    //    else
    //    {
    //        pnMaterialAvailable.Visible = false;
    //        pnMaterialUnavailable.Visible = true;
    //        ckOpenMaterial.Text = "Sử dụng vật liệu có sẵn";
    //    }
    //}
    protected void ckShowProductSet_Checked(object sender, EventArgs e)
    {
        if (openckProductSet.Checked)
        {
            Panel2.Visible = true;
            Panel1.Visible = false;
            openckProductSet.Text = "Nhập bộ sản phẩm";
        }
        else
        {
            Panel2.Visible = false;
            Panel1.Visible = true;
            openckProductSet.Text = "Sử dụng bộ sản phẩm đã có";
        }
    }
    protected void ckShow_Checked(object sender, EventArgs e)
    {
        if (ckShow.Checked)
        {
            pnOpen.Visible = true;
            ckShow.Text = "Ẩn thông tin chi tiết sản phẩm";
        }
        else
        {
            pnOpen.Visible = false;
            ckShow.Text = "Thông tin chi tiết sản phẩm";
        }
    }
    protected void ckShow1_Checked(object sender, EventArgs e)
    {
        if (ckOpenBH.Checked)
        {
            // LoadProductBH();
            // FillInfoProduct();
            spbh.Visible = true;

        }
        else
        {
            spbh.Visible = false;
        }
    }
}