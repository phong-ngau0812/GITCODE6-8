using DbObj;
using evointernal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SystemFrameWork;
using Telerik.Web.UI;

public partial class User_Edit : System.Web.UI.Page
{
    string UserName, UserId = string.Empty;
    string mode, flag = string.Empty;
    public string title = "Thông tin tài khoản";
    public string avatar = "";
    MembershipUser user;
    UserProfile UserProfile;
    protected void Page_Load(object sender, EventArgs e)
    {


        //CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        //_FileBrowser.BasePath = "/ckfinder";
        //_FileBrowser.SetupCKEditor(txtInclude);
        txtBirth.Attributes.Add("readonly", "readonly");

        if (!string.IsNullOrEmpty(Request["UserName"]))
            UserName = Request["UserName"].ToString();
        else
        {
            UserName = Context.User.Identity.Name;
            pn.Visible = false;
            phanquyen.Visible = false;
        }
        user = Membership.GetUser(UserName, false);
        UserProfile = UserProfile.GetProfile(UserName);
        UserId = MyUser.GetUser_IDByUserName(UserName).ToString();

        // Kiểm tra xem đối tượng vào sửa có cùng loại tài khoản hay không
        MyActionPermission.CheckPermissionUser(UserId, "/Admin/User/User_List");
        if (!IsPostBack)
        {
            if (Common.GetFunctionGroupDN())
            {
                HideGioiTinh.Visible = HideNgaySinh.Visible = HideSdt.Visible = false;
            }
            LoadProductBrand();
            LoadDepartment();
            FillDDLAccountType();
            FillDDLFunctionGroup();
            FillInfoUser();
            FillDDLFunction();
            FillDDLFunctionDetail();
            LoadPermission();
        }
    }



    protected void LoadZone()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetZoneBO().GetAsDataTable(" Active=1 and ProductBrand_ID=" + MyUser.GetProductBrand_ID(), " Zone_ID DESC");
            ddlZone.DataSource = dt;
            ddlZone.DataTextField = "Name";
            ddlZone.DataValueField = "Zone_ID";
            ddlZone.DataBind();
            ddlZone.Items.Insert(0, new ListItem("-- Chọn vùng --", ""));
        }
        catch (Exception ex)
        {
            Log.writeLog("LoadZone", ex.ToString());
        }
    }
    protected void LoadArea()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetAreaBO().GetAsDataTable("  ProductBrand_ID=" + MyUser.GetProductBrand_ID(), " Area_ID DESC");
            ddlArea.DataSource = dt;
            ddlArea.DataTextField = "Name";
            ddlArea.DataValueField = "Area_ID";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("-- Chọn khu vực --", ""));
        }
        catch (Exception ex)
        {
            Log.writeLog("LoadArea", ex.ToString());
        }
    }
    protected void LoadFarm()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetFarmBO().GetAsDataTable("  ProductBrand_ID=" + MyUser.GetProductBrand_ID(), " Farm_ID DESC");
            ddlFarm.DataSource = dt;
            ddlFarm.DataTextField = "Name";
            ddlFarm.DataValueField = "Farm_ID";
            ddlFarm.DataBind();
            ddlFarm.Items.Insert(0, new ListItem("-- Chọn thửa --", ""));
        }
        catch (Exception ex)
        {
            Log.writeLog("LoadArea", ex.ToString());
        }
    }
    protected void ddlAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAccountType.SelectedValue == "2")
        {
            phongban.Visible = true;
            LoadDepartment();
            LoadZone();
            LoadArea();
            LoadFarm();

            HideGioiTinh.Visible = false;
            HideHoTen.Visible = false;
            HideNgaySinh.Visible = false;
            ddlWorkshop.Visible = ddlArea.Visible = ddlFarm.Visible = true;
        }
        else if (ddlAccountType.SelectedValue == "7")
        {
            phongban.Visible = true;
            LoadDepartment();
            LoadZone();
            ddlWorkshop.Visible = ddlArea.Visible = ddlFarm.Visible = false;
            //LoadArea();
            //LoadFarm();

            HideGioiTinh.Visible = false;
            HideHoTen.Visible = false;
            HideNgaySinh.Visible = false;
        }
        else
        {
            HideGioiTinh.Visible = true;
            HideHoTen.Visible = true;
            HideNgaySinh.Visible = true;
            phongban.Visible = false;
        }
    }
    protected void LoadDepartment()
    {
        ddlDepartmentUser.DataSource = BusinessRulesLocator.GetDepartmentBO().GetAsDataTable("Active = 1", "Sort DESC");
        ddlDepartmentUser.DataTextField = "Name";
        ddlDepartmentUser.DataValueField = "Department_ID";
        ddlDepartmentUser.DataBind();
        ddlDepartmentUser.Items.Insert(0, new ListItem("-- Chọn phòng ban --", ""));
    }
    protected string GetFunction()
    {

        string FunctionGroup_ID = string.Empty;
        try
        {
            foreach (RadComboBoxItem item in ddlFunction.Items)
            {
                if (item.Checked)
                {
                    FunctionGroup_ID += item.Value + ",";
                }
            }
            if (!string.IsNullOrEmpty(FunctionGroup_ID))
            {
                FunctionGroup_ID = FunctionGroup_ID.Substring(0, FunctionGroup_ID.Length - 1);
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("GetFunctionGroup_ID", ex.ToString());
        }
        return FunctionGroup_ID;
    }
    private void FillDDLFunction()
    {
        try
        {
            string Where = string.Empty;
            if (ddlFunctionGroup.SelectedValue != "0")
            {
                Where += " and FunctionGroup_ID like '%," + ddlFunctionGroup.SelectedValue + ",%'";
            }
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetFunctionBO().GetAsDataTable(" Active=1 " + Where, " Sort ASC");
            ddlFunction.DataSource = dt;
            ddlFunction.DataTextField = "Name";
            ddlFunction.DataValueField = "Function_ID";
            ddlFunction.DataBind();
            //ddlFunctionGroup.Items.Insert(0, new ListItem("-- Chọn nhóm chức năng --", "0"));
        }
        catch (Exception ex)
        {
            Log.writeLog("FillDDLCha", ex.ToString());
        }
    }
    private void FillDDLFunctionGroup()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetFunctionGroupBO().GetAsDataTable(" Active=1", " Sort ASC");
            ddlFunctionGroup.DataSource = dt;
            ddlFunctionGroup.DataTextField = "Name";
            ddlFunctionGroup.DataValueField = "FunctionGroup_ID";
            ddlFunctionGroup.DataBind();
            ddlFunctionGroup.Items.Insert(0, new ListItem("-- Chọn nhóm chức năng --", "0"));
        }
        catch (Exception ex)
        {
            Log.writeLog("FillDDLCha", ex.ToString());
        }
    }

    private void FillDDLFunctionDetail()
    {
        try
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(GetFunction()))
            {
                where += " and Function_ID in (" + GetFunction() + ")";

            }

            if (!string.IsNullOrEmpty(MyUser.GetFunctionGroup_ID()))
            {
                where += " and FunctionGroup_ID like '%," + ddlFunctionGroup.SelectedValue + ",%'";
            }
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetFunctionBO().GetAsDataTable(" Active=1" + where, " Sort ASC");
            rptFunction.DataSource = dt;
            rptFunction.DataBind();
            //btnSaveRole1.Visible = true;
            //btnSaveRole.Visible = true;

            //ddlFunctionGroup.Items.Insert(0, new ListItem("-- Chọn nhóm chức năng --", "0"));
        }
        catch (Exception ex)
        {
            Log.writeLog("FillDDLCha", ex.ToString());
        }
    }
    private void FillDDLAccountType()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = BusinessRulesLocator.GetAccountTypeBO().GetAsDataTable(" Active=1", " Sort ASC");
            ddlAccountType.DataSource = dt;
            ddlAccountType.DataTextField = "Name";
            ddlAccountType.DataValueField = "AccountType_ID";
            ddlAccountType.DataBind();
            ddlAccountType.Items.Insert(0, new ListItem("-- Chọn loại tài khoản --", ""));

        }
        catch (Exception ex)
        {
            Log.writeLog("FillDDLDeparment", ex.ToString());
        }
    }
    protected void FillInfoUser()
    {
        try
        {
            if (!string.IsNullOrEmpty(UserName))
            {

                if (UserProfile != null && user != null)
                {
                    txtUser.Text = UserProfile.UserName;
                    txtEmail.Text = user.Email;
                    txtAddress.Text = UserProfile.Address;
                    txtFullName.Text = UserProfile.FullName;
                    txtPhone.Text = UserProfile.Phone;
                    ddlGioiTinh.SelectedValue = UserProfile.Gender;

                    if (!string.IsNullOrEmpty(UserProfile.ProductBrand_ID))
                    {
                        ddlProductBrand.SelectedValue = UserProfile.ProductBrand_ID;
                    }
                    if (!string.IsNullOrEmpty(UserProfile.Module_ID))
                    {
                        ddlHethong1.SelectedValue = UserProfile.Module_ID;
                        // ddlHethong1.Enabled = false;
                    }
                    if (!string.IsNullOrEmpty(UserProfile.ProductBrand_ID))
                    {
                        ddlProductBrand.SelectedValue = UserProfile.ProductBrand_ID;
                    }
                    else
                    {
                        ddlHethong1.SelectedValue = "1";
                        pnEs.Visible = true;
                    }
                    if (!string.IsNullOrEmpty(UserProfile.ModuleGroup_ID))
                    {
                        ddlModuleGroup.SelectedValue = UserProfile.ModuleGroup_ID;
                        if (ddlModuleGroup.SelectedValue == "1")
                        {
                            pnEs.Visible = true;
                            //ddlModuleGroup.Enabled = false;
                        }
                        else
                        {
                            pnEs.Visible = false;
                        }
                    }
                    if (UserProfile.BirthDate != null)
                        txtBirth.Text = UserProfile.BirthDate.ToString("dd/MM/yyyy");
                    avatar = UserProfile.AvatarUrl;
                    if (avatar != null)
                    {
                        imganh.ImageUrl = avatar;
                    }
                    else
                    {
                        avatar = "../../images/no-image-icon.png";
                    }
                    if (!string.IsNullOrEmpty(UserProfile.FunctionGroup_ID))
                    {
                        ddlFunctionGroup.SelectedValue = UserProfile.FunctionGroup_ID;
                        if (UserProfile.FunctionGroup_ID == "2")
                        {
                            ddlProductBrand.Enabled = false;
                            if (MyUser.GetFunctionGroup_ID() != "16")
                            {
                                ddlFunctionGroup.Enabled = false;
                            }
                        }
                        ddlFunctionGroup_SelectedIndexChanged(null, null);
                    }
                    if (!string.IsNullOrEmpty(UserProfile.AccountType_ID))
                    {
                        ddlAccountType.SelectedValue = UserProfile.AccountType_ID;
                        if (UserProfile.AccountType_ID == "2")
                        {
                            HideGioiTinh.Visible = false;
                            //HideHoTen.Visible = false;
                            HideNgaySinh.Visible = false;
                            phongban.Visible = true;
                            LoadDepartment();
                            if (!string.IsNullOrEmpty(UserProfile.Department_ID))
                            {
                                ddlDepartmentUser.SelectedValue = UserProfile.Department_ID;
                            }
                            LoadWorkShop();
                            if (!string.IsNullOrEmpty(UserProfile.Workshop_ID))
                            {
                                ddlWorkshop.SelectedValue = UserProfile.Workshop_ID;
                            }

                            LoadZone();
                            if (!string.IsNullOrEmpty(UserProfile.Zone_ID))
                            {
                                ddlZone.SelectedValue = UserProfile.Zone_ID;
                            }

                            LoadArea();
                            if (!string.IsNullOrEmpty(UserProfile.Area_ID))
                            {
                                ddlArea.SelectedValue = UserProfile.Area_ID;
                            }

                            LoadFarm();
                            if (!string.IsNullOrEmpty(UserProfile.Farm_ID))
                            {
                                ddlFarm.SelectedValue = UserProfile.Farm_ID;
                            }
                            ddlArea.Visible = ddlWorkshop.Visible = ddlFarm.Visible = true;

                        }
                        else if (UserProfile.AccountType_ID == "7")
                        {
                            HideGioiTinh.Visible = false;
                            //HideHoTen.Visible = false;
                            HideNgaySinh.Visible = false;
                            phongban.Visible = true;
                            LoadDepartment();
                            if (!string.IsNullOrEmpty(UserProfile.Department_ID))
                            {
                                ddlDepartmentUser.SelectedValue = UserProfile.Department_ID;
                            }

                            LoadZone();
                            if (!string.IsNullOrEmpty(UserProfile.Zone_ID))
                            {
                                ddlZone.SelectedValue = UserProfile.Zone_ID;
                            }
                            ddlArea.Visible = ddlWorkshop.Visible = ddlFarm.Visible = false;
                        }
                        else
                        {
                            ddlAccountType.Enabled = false;
                            phongban.Visible = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("FillInfoUser", ex.ToString());
        }
    }

    protected void ddlDepartmentUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadWorkShop();
    }
    private void LoadWorkShop()
    {
        try
        {
            DataTable dt = new DataTable();

            string where = " Active=1 and ProductBrand_ID=" + ddlProductBrand.SelectedValue;
            if (ddlDepartmentUser.SelectedValue != "")
            {
                where += " and Department_ID=" + ddlDepartmentUser.SelectedValue;
            }
            dt = BusinessRulesLocator.GetWorkshopBO().GetAsDataTable(where, "");
            if (dt.Rows.Count > 0)
            {
                ddlWorkshop.DataSource = dt;
                ddlWorkshop.DataValueField = "Workshop_ID";
                ddlWorkshop.DataTextField = "Name";
                ddlWorkshop.DataBind();
                ddlWorkshop.Items.Insert(0, new ListItem("-- Chọn nhân viên/ hộ sx --", ""));
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("LoadWorkShop", ex.ToString());
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Default.aspx", false);
        }
        catch (Exception ex)
        {
            Log.writeLog("btnReset_Click", ex.ToString());
        }
    }

    private void UpdateUser()
    {
        user.Email = txtEmail.Text;
        user.IsApproved = ckActive.Checked;
        Membership.UpdateUser(user);
    }

    private void UpdateProfile()
    {
        UserProfile.FullName = txtFullName.Text;
        UserProfile.Gender = ddlGioiTinh.SelectedValue;
        //UserProfile.Company = txtCompany.Text;
        UserProfile.Address = txtAddress.Text;
        UserProfile.Phone = txtPhone.Text;
        if (ddlHethong1.SelectedValue != "0")
        {
            UserProfile.Module_ID = ddlHethong1.SelectedValue;
        }
        if (ddlModuleGroup.SelectedValue != "0")
        {
            UserProfile.ModuleGroup_ID = ddlModuleGroup.SelectedValue;
        }
        if (ddlAccountType.SelectedValue != "")
        {
            UserProfile.AccountType_ID = (ddlAccountType.SelectedValue);
            if (ddlAccountType.SelectedValue == "2")
            {
                UserProfile.Department_ID = (ddlDepartmentUser.SelectedValue);
                if (ddlWorkshop.SelectedValue != "0")
                {
                    UserProfile.Workshop_ID = (ddlWorkshop.SelectedValue);
                }
                UserProfile.Zone_ID = (ddlZone.SelectedValue);
                UserProfile.Area_ID = (ddlArea.SelectedValue);
                UserProfile.Farm_ID = (ddlFarm.SelectedValue);
            }
            if (ddlAccountType.SelectedValue == "7")
            {
                UserProfile.Department_ID = (ddlDepartmentUser.SelectedValue);
                UserProfile.Zone_ID = (ddlZone.SelectedValue);
            }
        }

        if (!string.IsNullOrEmpty(txtBirth.Text.Trim()))
        {
            DateTime birth = DateTime.ParseExact(txtBirth.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            UserProfile.BirthDate = birth;
        }

        string fileimage = "";
        if (fulAnh.HasFile)
        {
            //Xóa file
            if (UserProfile.AvatarUrl != null)
            {
                string strFileFullPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + UserProfile.AvatarUrl.Replace("../", "");
                if (File.Exists(strFileFullPath))
                {
                    File.Delete(strFileFullPath);
                }
            }
            fileimage = "../../data/user/avatar/original/" + Common.CreateImgName(fulAnh.FileName);
            fulAnh.SaveAs(Server.MapPath(fileimage));
            if (!string.IsNullOrEmpty(fileimage))
            {
                UserProfile.AvatarUrl = fileimage;
            }
        }
        if (ddlProductBrand.SelectedValue != "")
        {
            UserProfile.ProductBrand_ID = (ddlProductBrand.SelectedValue);
        }
        if (ddlFunctionGroup.SelectedValue != "")
        {
            UserProfile.FunctionGroup_ID = (ddlFunctionGroup.SelectedValue);
        }

        UserProfile.Save();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                UpdateUser();
                UpdateProfile();
                SaveRole();
                lblMessage.Text = "Cập nhật thông tin thành công!";
                lblMessage.Visible = true;
                FillInfoUser();
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("btnSave_Click", ex.ToString());
        }
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("User_List.aspx", false);
    }
    protected void rptFunction_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Literal lblFunction_ID = e.Item.FindControl("lblFunction_ID") as Literal;
            Repeater rptPage = e.Item.FindControl("rptPage") as Repeater;

            CheckBox ckParent = e.Item.FindControl("ckParent") as CheckBox;
            if (lblFunction_ID != null)
            {
                DataTable dt = new DataTable();
                dt = BusinessRulesLocator.Conllection().GetAllList("select PageFunction_ID, Name from PageFunction where Function_ID =" + lblFunction_ID.Text + " and Active=1 order by SORT ASC");
                if (dt.Rows.Count > 0)
                {
                    rptPage.DataSource = dt;
                    rptPage.DataBind();
                    int index = 0;
                    foreach (RepeaterItem itemRpt in rptPage.Items)
                    {
                        CheckBox ckRole = itemRpt.FindControl("ckRole") as CheckBox;
                        Literal lblPageFunction_ID = itemRpt.FindControl("lblPageFunction_ID") as Literal;

                        DataTable dtChild = BusinessRulesLocator.Conllection().GetAllList("select * from UserVsPageFunction where UserId='" + UserId + "' and PageFunction_ID=" + lblPageFunction_ID.Text);
                        if (dtChild.Rows.Count > 0)
                        {
                            ckRole.Checked = true;
                            index++;
                        }
                        else
                        {
                            ckRole.Checked = false;
                        }
                    }
                    if (dt.Rows.Count == index)
                    {
                        ckParent.Checked = true;
                    }
                    else
                    {
                        ckParent.Checked = false;
                    }
                }

            }
        }

    }
    protected void LoadPermission()
    {
        if (Common.CheckPackage(ddlFunctionGroup))
        {
            ddlProductBrand.Visible = false;
            ddlAccountType.Visible = false;
        }
        else
        {

            ddlAccountType.Visible = true;
            ddlProductBrand.Visible = true;

        }
    }
    protected void LoadProductBrand()
    {
        DataTable dt = BusinessRulesLocator.GetProductBrandBO().GetAsDataTable("", "");
        ddlProductBrand.DataSource = dt;
        ddlProductBrand.DataValueField = "ProductBrand_ID";
        ddlProductBrand.DataTextField = "Name";
        ddlProductBrand.DataBind();
        ddlProductBrand.SelectedValue = "1473";
        ddlProductBrand.Enabled = false;

    }
    protected void ddlFunction_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        FillDDLFunctionDetail();
    }

    protected void SaveRole()
    {
        //if (ddlFunctionGroup.SelectedValue == "2" && MyUser.GetFunctionGroup_ID() == "16")
        //{
        //    BusinessRulesLocator.Conllection().UpdateUserVsPageFunctionProductBrandV2(UserId, "2");
        //}
        string roleID = string.Empty;
        foreach (RepeaterItem item in rptFunction.Items)
        {
            Repeater rptPage = item.FindControl("rptPage") as Repeater;
            foreach (RepeaterItem itemRole in rptPage.Items)
            {
                Literal lblPageFunction_ID = itemRole.FindControl("lblPageFunction_ID") as Literal;
                CheckBox ckRole = itemRole.FindControl("ckRole") as CheckBox;
                if (ckRole.Checked)
                {
                    roleID += lblPageFunction_ID.Text + ",";
                }
            }
        }

        if (!string.IsNullOrEmpty(roleID))
        {
            roleID = roleID.Remove(roleID.Length - 1);
            BusinessRulesLocator.Conllection().UpdateUserVsPageFunction(UserId, roleID);
        }
    }

    protected void btnSaveRole_Click(object sender, EventArgs e)
    {

        lblMessage.Text = "Cập nhật quyền thành công!";
        lblMessage.Visible = true;
        //lblMsg.Text = roleID;
    }
    protected void ddlFunctionGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        RolePermission.Visible = true;
        FillDDLFunction();
        LoadProductBrand();

        if (ddlFunctionGroup.SelectedValue == "16" || ddlFunctionGroup.SelectedValue == "8" || ddlFunctionGroup.SelectedValue == "9")
        {
            ddlProductBrand.Visible = false;
            ddlAccountType.Visible = false;
            if (ddlFunctionGroup.SelectedValue == "9")
            {
                //HideCapBac.Visible = false;
                //u.Visible = false;
                //ddlHeThong.Enabled = false;
                //DivSale.Visible = true;
            }
            else
            {
                //DivSale.Visible = false;
                //HideCapBac.Visible = true;
                //u.Visible = true;
                //ddlHeThong.Enabled = true;
            }
        }
        else
        {
            if (ddlFunctionGroup.SelectedValue == "3")
            {
                //ddlChainLink.Visible = true;
                //LoadProductBrandList();
                //ddlProductBrandList.Visible = true;
                //ddlDepartment.Visible = false;
                //ddlProductBrand.Visible = false;
                //DivSale.Visible = false;
            }
            else
            {
                //ddlChainLink.Visible = false;
                //ddlProductBrandList.Visible = false;
                if (MyUser.GetFunctionGroup_ID() == "16")
                {
                    RolePermission.Visible = false;
                    ddlAccountType.SelectedValue = "1";
                    ddlAccountType.Enabled = false;
                    // ddlDepartment.SelectedValue = "1";
                    // ddlDepartment.Enabled = false;
                }
                else
                {
                    RolePermission.Visible = true;
                    ddlAccountType.SelectedValue = "0";
                    ddlAccountType.Enabled = true;

                }
                ddlProductBrand.Visible = true;
                ddlAccountType.Visible = true;
            }

        }
        LoadPermission();
        FillDDLFunctionDetail();
    }
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAccountType.SelectedValue == "2")
        {
            phongban.Visible = true;
            LoadDepartment();
            LoadZone();
            LoadArea();
            LoadFarm();
            FillInfoUser();
            HideGioiTinh.Visible = false;
            HideHoTen.Visible = false;
            HideNgaySinh.Visible = false;
        }
        else if (ddlAccountType.SelectedValue == "7")
        {
            phongban.Visible = true;
            LoadDepartment();
            LoadZone();
            ddlWorkshop.Visible = ddlArea.Visible = ddlFarm.Visible = false;
            //LoadArea();
            //LoadFarm();

            HideGioiTinh.Visible = false;
            HideHoTen.Visible = false;
            HideNgaySinh.Visible = false;
        }
        else
        {
            HideGioiTinh.Visible = true;
            HideHoTen.Visible = true;
            HideNgaySinh.Visible = true;
            phongban.Visible = false;
        }
    }
    protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlModuleGroup.SelectedValue == "1")
        {
            pnEs.Visible = true;
        }
        else
        {
            pnEs.Visible = false;
        }
    }
}