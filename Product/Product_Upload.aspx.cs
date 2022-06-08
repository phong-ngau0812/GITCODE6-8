using DbObj;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SystemFrameWork;

public partial class Admin_Product_Product_Upload : System.Web.UI.Page
{
    public string title = "Thao tác sản phẩm bằng file Excel";
    OleDbConnection Econ;
    SqlConnection con;
    public string constr, Query, sqlconn;
    public bool checkSKU = true;
    public string SKU = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xlsx");
        string filePath = Server.MapPath("~/Temp/" + Guid.NewGuid().ToString("N") + ".xlsx");
        FileUpload1.SaveAs(filePath);
        try
        {
            if (FileUpload1.HasFile)
            {
                if (InsertExcelRecords(filePath))
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
                    lblMessage.Text = "Thêm mới sản phẩm thành công!";
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessageErr.Text = "Thêm mới sản phẩm theo File không thành công!";
                    lblMessageErr.Visible = true;

                }
            }
            else
            {
                lblMessageErr.Text = "Chưa chọn File";
                lblMessageErr.Visible = true;
            }
        }
        finally
        {
            //File.Delete(filePath);
        }

    }
    protected void btnSave1_Click(object sender, EventArgs e)
    {
        spinner.Attributes.Add("class", "displayNotNone");

        //string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xlsx");
        string FilePath = Server.MapPath("~/Temp/" + Guid.NewGuid().ToString("N") + ".xlsx");
        FileUpload2.SaveAs(FilePath);
        try
        {
            if (FileUpload2.HasFile)
            {
                if (UpdateExcelRecords(FilePath))
                {
                    spinner.Attributes.Add("class", "displayNone");
                    lblMessage.Text = "Cập nhật sản phẩm thành công!";
                    lblMessage.Visible = true;
                }
                else
                {
                    spinner.Attributes.Add("class", "displayNone");
                    lblMessageErr.Text = "Cập nhật sản phẩm theo File không thành công!";
                    lblMessageErr.Visible = true;

                }
            }
            else
            {
                lblMessageErr.Text = "Chưa chọn File";
                lblMessageErr.Visible = true;
            }
        }
        finally
        {
            //File.Delete(filePath);
        }

    }
    public string ChangeChar(string Name)
    {
        string Value = string.Empty;
        string[] words = Name.Trim().Split(',');
        foreach (var word in words)
        {
            if (word != "" && word != " ")
            {
                Value += word + ",";
            }
        }
        if (!string.IsNullOrEmpty(Value))
        {
            Value = "," + Value;

        }
        return Value;
    }
    private bool UpdateExcelRecords(string FilePath)
    {
        bool check = false;
        try
        {
            ExcelConn(FilePath);

            Query = string.Format(@"Select [Mã Sản Phẩm],[Ngành,Nhóm sản phẩm (Nhập ID)],[Bộ Sản Phẩm (nhập ID)],[Sản phẩm cha (nhập ID)],
    [SKU hệ thông effect],[SKU hiển thị Web],[Tên Sản Phẩm],[Đường dẫn ảnh],[Đơn vị],[Màu sản phẩm (Nhập ID)],[Giá sản phẩm],
    [Phân loại sản phẩm (Nhập ID)],[Kích Thước],[Giá đại lý miền bắc],[Giá đại lý miền nam],[Loại hàng (Nhập ID)],[Chất liệu sản phẩm (Nhập ID, mỗi ID cách nhau bởi dấu ',')],[Link nhúng Video],[Mô tả sản phẩm],[Số Sao],[Thời gian bảo hành (Ngày)],[Trọng Lượng],[Url Sản phẩm],[Không gian sản phẩm(Nhập ID, mỗi ID cách nhau bởi dấu ',')], [Tags(Mỗi tag cách nhau bởi dấu ',')],[Mô tả Giá đại lý miền bắc],[Mô tả Giá đại lý miền nam],[Sản phẩm đi kèm(Nhập ID, mỗi ID cách nhau bởi dấu ',')],[Từ khóa],[Trạng thái],[IsHot],[IsHome],[IsNew],[IsQRCode],[Kích hoạt 2 tem],[SP cùng kích hoạt 2 tem(Nhập ID, mỗi ID cách nhau bởi dấu ',')],[SeoKeyword],[SeoDescription] FROM [{0}]", "File-nhap-san-pham$");
            using (OleDbCommand Ecom = new OleDbCommand(Query, Econ))
            {
                Econ.Open();
                DataSet ds = new DataSet();
                OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);

                Econ.Close();
                oda.Fill(ds);
                DataTable Exceldt = ds.Tables[0];

                foreach (DataRow row in Exceldt.Rows)
                {
                    if (row["Mã Sản Phẩm"].ToString() != "")
                    {
                        int Product_ID = Convert.ToInt32(row["Mã Sản Phẩm"].ToString());
                        ProductRow _ProductRow = BusinessRulesLocator.GetProductBO().GetByPrimaryKey(Product_ID);
                        if (_ProductRow != null)
                        {
                            _ProductRow.ProductCategory_ID = string.IsNullOrEmpty(row["Ngành,Nhóm sản phẩm (Nhập ID)"].ToString()) ? 0 : Convert.ToInt32(row["Ngành,Nhóm sản phẩm (Nhập ID)"].ToString());
                            _ProductRow.ProductSet_ID = string.IsNullOrEmpty(row["Bộ Sản Phẩm (nhập ID)"].ToString()) ? 0 : Convert.ToInt32(row["Bộ Sản Phẩm (nhập ID)"].ToString());
                            _ProductRow.Parent_ID = string.IsNullOrEmpty(row["Sản phẩm cha (nhập ID)"].ToString()) ? 0 : Convert.ToInt32(row["Sản phẩm cha (nhập ID)"].ToString());
                            _ProductRow.SKU_ID = string.IsNullOrEmpty(row["SKU hệ thông effect"].ToString()) ? string.Empty : row["SKU hệ thông effect"].ToString();
                            _ProductRow.SKU_Web_ID = string.IsNullOrEmpty(row["SKU hiển thị Web"].ToString()) ? string.Empty : row["SKU hiển thị Web"].ToString();
                            _ProductRow.Name = string.IsNullOrEmpty(row["Tên Sản Phẩm"].ToString()) ? string.Empty : row["Tên Sản Phẩm"].ToString();
                            _ProductRow.Image = string.IsNullOrEmpty(row["Đường dẫn ảnh"].ToString()) ? string.Empty : row["Đường dẫn ảnh"].ToString();
                            _ProductRow.Unit = string.IsNullOrEmpty(row["Đơn vị"].ToString()) ? string.Empty : row["Đơn vị"].ToString();
                            _ProductRow.Color_ID = string.IsNullOrEmpty(row["Màu sản phẩm (Nhập ID)"].ToString()) ? 0 : Convert.ToInt32(row["Màu sản phẩm (Nhập ID)"].ToString());
                            _ProductRow.Price = string.IsNullOrEmpty(row["Giá sản phẩm"].ToString()) ? 0 : Convert.ToInt32(row["Giá sản phẩm"].ToString());
                            _ProductRow.Classify = string.IsNullOrEmpty(row["Phân loại sản phẩm (Nhập ID)"].ToString()) ? 0 : Convert.ToInt32(row["Phân loại sản phẩm (Nhập ID)"].ToString());
                            _ProductRow.Size = string.IsNullOrEmpty(row["Kích Thước"].ToString()) ? string.Empty : row["Kích Thước"].ToString();
                            _ProductRow.AgencyNorthPrice = string.IsNullOrEmpty(row["Giá đại lý miền bắc"].ToString()) ? 0 : Convert.ToInt32(row["Giá đại lý miền bắc"].ToString());
                            _ProductRow.AgencySouthPrice = string.IsNullOrEmpty(row["Giá đại lý miền nam"].ToString()) ? 0 : Convert.ToInt32(row["Giá đại lý miền nam"].ToString());
                            _ProductRow.ProductType_ID = string.IsNullOrEmpty(row["Loại hàng (Nhập ID)"].ToString()) ? 0 : Convert.ToInt32(row["Loại hàng (Nhập ID)"].ToString());
                            _ProductRow.MaterialCategory_ID = string.IsNullOrEmpty(row["Chất liệu sản phẩm (Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString()) ? string.Empty : ChangeChar(row["Chất liệu sản phẩm (Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString());
                            _ProductRow.ListProduct_ID_TwoStamp = string.IsNullOrEmpty(row["SP cùng kích hoạt 2 tem(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString()) ? string.Empty : ChangeChar(row["SP cùng kích hoạt 2 tem(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString());
                            _ProductRow.Video = string.IsNullOrEmpty(row["Link nhúng Video"].ToString()) ? string.Empty : row["Link nhúng Video"].ToString();
                            _ProductRow.Trongluong = string.IsNullOrEmpty(row["Trọng Lượng"].ToString()) ? 0 : Convert.ToInt32(row["Trọng Lượng"].ToString());
                            _ProductRow.URL = string.IsNullOrEmpty(row["Url Sản phẩm"].ToString()) ? string.Empty : row["Url Sản phẩm"].ToString();
                            _ProductRow.Space_ID = string.IsNullOrEmpty(row["Không gian sản phẩm(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString()) ? string.Empty : ChangeChar(row["Không gian sản phẩm(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString());
                            _ProductRow.Tags = string.IsNullOrEmpty(row["Tags(Mỗi tag cách nhau bởi dấu ',')"].ToString()) ? string.Empty : row["Tags(Mỗi tag cách nhau bởi dấu ',')"].ToString();
                            _ProductRow.LastEditBy = MyUser.GetUser_ID();
                            _ProductRow.LastEditDate = DateTime.Now;
                            _ProductRow.WarrantyDate = string.IsNullOrEmpty(row["Thời gian bảo hành (Ngày)"].ToString()) ? 0 : Convert.ToInt32(row["Thời gian bảo hành (Ngày)"].ToString());
                            _ProductRow.Star = string.IsNullOrEmpty(row["Số Sao"].ToString()) ? 0 : Convert.ToInt32(row["Số Sao"].ToString());
                            _ProductRow.Keyword = string.IsNullOrEmpty(row["Từ khóa"].ToString()) ? string.Empty : row["Từ khóa"].ToString();
                            _ProductRow.Description = string.IsNullOrEmpty(row["Mô tả sản phẩm"].ToString()) ? string.Empty : row["Mô tả sản phẩm"].ToString();
                            _ProductRow.NoteAgencyNorthPrice = string.IsNullOrEmpty(row["Mô tả Giá đại lý miền bắc"].ToString()) ? string.Empty : row["Mô tả Giá đại lý miền bắc"].ToString();
                            _ProductRow.NoteAgencySouthPrice = string.IsNullOrEmpty(row["Mô tả Giá đại lý miền nam"].ToString()) ? string.Empty : row["Mô tả Giá đại lý miền nam"].ToString();
                            // _ProductRow.Note = string.IsNullOrEmpty(row["Ghi Chú"].ToString()) ? string.Empty : row["Ghi Chú"].ToString();
                            _ProductRow.ProductAttach_ID = string.IsNullOrEmpty(row["Sản phẩm đi kèm(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString()) ? string.Empty : ChangeChar(row["Sản phẩm đi kèm(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString());
                            if (!string.IsNullOrEmpty(row["Trạng thái"].ToString()))
                            {
                                if (row["Trạng thái"].ToString() == "1")
                                {
                                    _ProductRow.Active = true;
                                }
                                else
                                {
                                    _ProductRow.Active = false;
                                }
                            }
                            if (!string.IsNullOrEmpty(row["IsHome"].ToString()))
                            {
                                if (row["IsHome"].ToString() == "1")
                                {
                                    _ProductRow.IsHome = 1;
                                }
                                else
                                {
                                    _ProductRow.IsHome = 0;
                                }
                            }
                            if (!string.IsNullOrEmpty(row["IsNew"].ToString()))
                            {
                                if (row["IsNew"].ToString() == "1")
                                {
                                    _ProductRow.IsNew = 1;
                                }
                                else
                                {
                                    _ProductRow.IsNew = 0;
                                }
                            }
                            if (!string.IsNullOrEmpty(row["IsHot"].ToString()))
                            {
                                if (row["IsHot"].ToString() == "1")
                                {
                                    _ProductRow.IsHot = 1;
                                }
                                else
                                {
                                    _ProductRow.IsHot = 0;
                                }
                            }
                            if (!string.IsNullOrEmpty(row["IsQRCode"].ToString()))
                            {
                                if (row["IsQRCode"].ToString() == "1")
                                {
                                    _ProductRow.IsQRCode = true;
                                }
                                else
                                {
                                    _ProductRow.IsQRCode = false;
                                }
                            }
                            if (!string.IsNullOrEmpty(row["Kích hoạt 2 tem"].ToString()))
                            {
                                if (row["Kích hoạt 2 tem"].ToString() == "1")
                                {
                                    _ProductRow.IsProductTwoStamp = true;
                                }
                                else
                                {
                                    _ProductRow.IsProductTwoStamp = false;
                                }
                            }
                            _ProductRow.SeoDescription = string.IsNullOrEmpty(row["SeoDescription"].ToString()) ? string.Empty : row["SeoDescription"].ToString();
                            _ProductRow.SeoKeyword = string.IsNullOrEmpty(row["SeoKeyword"].ToString()) ? string.Empty : row["SeoKeyword"].ToString();
                            BusinessRulesLocator.GetProductBO().Update(_ProductRow);
                        }
                    }
                }
                check = true;
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("UpdateExcelRecords", ex.ToString());
        }
        return check;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Product_List.aspx", false);
    }
    private void ExcelConn(string FilePath)
    {

        constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", FilePath);
        Econ = new OleDbConnection(constr);
    }
    private void connection()
    {
        sqlconn = ConfigurationManager.ConnectionStrings["UserConnectionString"].ConnectionString;
        con = new SqlConnection(sqlconn);
    }
    private bool InsertExcelRecords(string FilePath)
    {
        bool check = false;
        try
        {
            ExcelConn(FilePath);

            Query = string.Format(@"Select [Ngành,Nhóm sản phẩm (Nhập ID)],[Bộ Sản Phẩm (nhập ID)],[Sản phẩm cha (nhập ID)],
    [SKU hệ thông effect],[SKU hiển thị Web],[Tên Sản Phẩm],[Đường dẫn ảnh],[Đơn vị],[Màu sản phẩm (Nhập ID)],[Giá sản phẩm],
    [Phân loại sản phẩm (Nhập ID)],[Kích Thước],[Giá đại lý miền bắc],[Giá đại lý miền nam],[Loại hàng (Nhập ID)],[Chất liệu sản phẩm (Nhập ID, mỗi ID cách nhau bởi dấu ',')],[Link nhúng Video],[Mô tả sản phẩm],[Số Sao],[Trọng Lượng],[Url Sản phẩm],[Không gian sản phẩm(Nhập ID, mỗi ID cách nhau bởi dấu ',')], [Tags(Mỗi tag cách nhau bởi dấu ',')],[Mô tả Giá đại lý miền bắc],[Mô tả Giá đại lý miền nam],[Sản phẩm đi kèm(Nhập ID, mỗi ID cách nhau bởi dấu ',')],[Từ khóa],[Trạng thái],[IsHot],[IsHome],[IsNew],[IsQRCode],[Thời gian bảo hành (Ngày)],[Kích hoạt 2 tem],[SP cùng kích hoạt 2 tem(Nhập ID, mỗi ID cách nhau bởi dấu ',')],[SeoKeyword],[SeoDescription] FROM [{0}]", "File-nhap-san-pham$");
            using (OleDbCommand Ecom = new OleDbCommand(Query, Econ))
            {
                Econ.Open();
                DataSet ds = new DataSet();
                OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);

                Econ.Close();
                oda.Fill(ds);

                //SET Typeof
                DataTable Exceldt = ds.Tables[0];
                //Exceldt.Columns.Add("Trạng thái");
                //Exceldt.Columns.Add("IsHot");
                //Exceldt.Columns.Add("IsNew");
                //Exceldt.Columns.Add("IsHome");
                // Exceldt.Columns.Add("Thời gian bảo hành (Ngày)");
                Exceldt.Columns.Add("Ngày tạo", typeof(DateTime));
                Exceldt.Columns.Add("Mã Người tạo", typeof(Guid));
                Exceldt.Columns.Add("Deleted");
                DataTable dtCloned = Exceldt.Clone();
                //dtCloned.Columns.Add("Ngày tạo", typeof(DateTime));
                //dtCloned.Columns.Add("Mã Người tạo", typeof(Guid));

                //dtCloned.Columns["Mã Người tạo"].DataType = typeof(Guid);
                int Count = 0;
                foreach (DataRow row in Exceldt.Rows)
                {
                    if (row["Bộ Sản Phẩm (nhập ID)"].ToString() != "" && row["SKU hiển thị Web"].ToString() != "" && row["SKU hệ thông effect"].ToString() != "" && row["Tên Sản Phẩm"].ToString() != "")
                    {
                        Count++;
                        if (row["Không gian sản phẩm(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString() != "")
                        {
                            row["Không gian sản phẩm(Nhập ID, mỗi ID cách nhau bởi dấu ',')"] = ChangeChar(row["Không gian sản phẩm(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString());
                        }

                        if (row["Chất liệu sản phẩm (Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString() != "")
                        {
                            row["Chất liệu sản phẩm (Nhập ID, mỗi ID cách nhau bởi dấu ',')"] = ChangeChar(row["Chất liệu sản phẩm (Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString());
                        }

                        if (row["Sản phẩm đi kèm(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString() != "")
                        {
                            row["Sản phẩm đi kèm(Nhập ID, mỗi ID cách nhau bởi dấu ',')"] = ChangeChar(row["Sản phẩm đi kèm(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString());
                        }
                        if (row["SP cùng kích hoạt 2 tem(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString() != "")
                        {
                            row["SP cùng kích hoạt 2 tem(Nhập ID, mỗi ID cách nhau bởi dấu ',')"] = ChangeChar(row["SP cùng kích hoạt 2 tem(Nhập ID, mỗi ID cách nhau bởi dấu ',')"].ToString());
                        }
                        if (row["Ngành,Nhóm sản phẩm (Nhập ID)"].ToString() != "")
                        {
                            row["Ngành,Nhóm sản phẩm (Nhập ID)"] = Convert.ToInt32(row["Ngành,Nhóm sản phẩm (Nhập ID)"].ToString());
                        }
                        else
                        {
                            row["Ngành,Nhóm sản phẩm (Nhập ID)"] = 0;
                        }
                        if (row["Bộ Sản Phẩm (nhập ID)"].ToString() != "")
                        {
                            row["Bộ Sản Phẩm (nhập ID)"] = Convert.ToInt32(row["Bộ Sản Phẩm (nhập ID)"].ToString());
                        }
                        else
                        {
                            row["Bộ Sản Phẩm (nhập ID)"] = 0;
                        }
                        if (row["Sản phẩm cha (nhập ID)"].ToString() != "")
                        {
                            row["Sản phẩm cha (nhập ID)"] = Convert.ToInt32(row["Sản phẩm cha (nhập ID)"].ToString());
                        }
                        else
                        {
                            row["Sản phẩm cha (nhập ID)"] = null;
                        }
                        if (row["Phân loại sản phẩm (Nhập ID)"].ToString() != "")
                        {
                            row["Phân loại sản phẩm (Nhập ID)"] = Convert.ToInt32(row["Phân loại sản phẩm (Nhập ID)"].ToString());
                        }
                        else
                        {
                            row["Phân loại sản phẩm (Nhập ID)"] = 0;

                        }
                        if (row["Màu sản phẩm (Nhập ID)"].ToString() != "")
                        {
                            row["Màu sản phẩm (Nhập ID)"] = Convert.ToInt32(row["Màu sản phẩm (Nhập ID)"].ToString());
                        }
                        else
                        {
                            row["Màu sản phẩm (Nhập ID)"] = 0;
                        }
                        if (row["Loại hàng (Nhập ID)"].ToString() != "")
                        {
                            row["Loại hàng (Nhập ID)"] = Convert.ToInt32(row["Loại hàng (Nhập ID)"].ToString());
                        }
                        else
                        {
                            row["Loại hàng (Nhập ID)"] = 0;
                        }
                        //if (row["Chất liệu sản phẩm (Nhập ID)"].ToString() != "")
                        //{
                        //    row["Chất liệu sản phẩm (Nhập ID)"] = Convert.ToInt32(row["Chất liệu sản phẩm (Nhập ID)"].ToString());
                        //}
                        //else
                        //{
                        //    row["Chất liệu sản phẩm (Nhập ID)"] = 0;
                        //}
                        if (row["Giá sản phẩm"].ToString() != "")
                        {
                            row["Giá sản phẩm"] = decimal.Parse(row["Giá sản phẩm"].ToString());
                        }
                        else
                        {
                            row["Giá sản phẩm"] = 0;
                        }
                        if (row["Giá đại lý miền bắc"].ToString() != "")
                        {
                            row["Giá đại lý miền bắc"] = decimal.Parse(row["Giá đại lý miền bắc"].ToString());
                        }
                        else
                        {
                            row["Giá đại lý miền bắc"] = 0;
                        }
                        if (row["Giá đại lý miền nam"].ToString() != "")
                        {
                            row["Giá đại lý miền nam"] = decimal.Parse(row["Giá đại lý miền nam"].ToString());
                        }
                        else
                        {
                            row["Giá đại lý miền nam"] = 0;
                        }
                        if (row["Url Sản phẩm"].ToString() != "")
                        {
                            row["Url Sản phẩm"] = row["Url Sản phẩm"].ToString();
                        }
                        else
                        {

                        }
                        if (row["Trạng thái"].ToString() != "")
                        {
                            if (row["Trạng thái"].ToString() == "1")
                            {
                                row["Trạng thái"] = true;
                            }
                            else
                            {
                                row["Trạng thái"] = false;
                            }
                        }
                        if (row["IsHome"].ToString() != "")
                        {
                            if (row["IsHome"].ToString() == "1")
                            {
                                row["IsHome"] = 1;
                            }
                            else
                            {
                                row["IsHome"] = 0;
                            }
                        }
                        if (row["IsNew"].ToString() != "")
                        {
                            if (row["IsNew"].ToString() == "1")
                            {
                                row["IsNew"] = 1;
                            }
                            else
                            {
                                row["IsNew"] = 0;
                            }
                        }
                        if (row["IsHot"].ToString() != "")
                        {
                            if (row["IsHot"].ToString() == "1")
                            {
                                row["IsHot"] = 1;
                            }
                            else
                            {
                                row["IsHot"] = 0;
                            }
                        }
                        if (row["IsQRCode"].ToString() != "")
                        {
                            if (row["IsQRCode"].ToString() == "1")
                            {
                                row["IsQRCode"] = true;
                            }
                            else
                            {
                                row["IsQRCode"] = false;
                            }
                        }
                        if (row["Kích hoạt 2 tem"].ToString() != "")
                        {
                            if (row["Kích hoạt 2 tem"].ToString() == "1")
                            {
                                row["Kích hoạt 2 tem"] = true;
                            }
                            else
                            {
                                row["Kích hoạt 2 tem"] = false;
                            }
                        }
                        //row["Trạng thái"] = true;
                        //row["IsHot"] = 0;
                        //row["IsNew"] = 0;
                        //row["IsHome"] = 0;
                        // row["Thời gian bảo hành (Ngày)"] = 365;
                        row["Ngày tạo"] = DateTime.Now;
                        row["Mã Người tạo"] = MyUser.GetUser_ID();
                        row["Deleted"] = false;
                        dtCloned.ImportRow(row);
                    }
                }
                connection();
                //creating object of SqlBulkCopy    
                SqlBulkCopy objbulk = new SqlBulkCopy(con);
                //assigning Destination table name    
                objbulk.DestinationTableName = "Product";
                //Mapping Table column  
                objbulk.ColumnMappings.Add("Ngành,Nhóm sản phẩm (Nhập ID)", "ProductCategory_ID");
                objbulk.ColumnMappings.Add("Bộ Sản Phẩm (nhập ID)", "ProductSet_ID");
                objbulk.ColumnMappings.Add("Sản phẩm cha (nhập ID)", "Parent_ID");
                objbulk.ColumnMappings.Add("SKU hệ thông effect", "SKU_ID");
                objbulk.ColumnMappings.Add("SKU hiển thị Web", "SKU_Web_ID");
                objbulk.ColumnMappings.Add("Tên Sản Phẩm", "Name");
                objbulk.ColumnMappings.Add("Thời gian bảo hành (Ngày)", "WarrantyDate");
                objbulk.ColumnMappings.Add("Đường dẫn ảnh", "Image");
                objbulk.ColumnMappings.Add("Đơn vị", "Unit");
                objbulk.ColumnMappings.Add("Màu sản phẩm (Nhập ID)", "Color_ID");
                objbulk.ColumnMappings.Add("Giá sản phẩm", "Price");
                objbulk.ColumnMappings.Add("Phân loại sản phẩm (Nhập ID)", "Classify");
                objbulk.ColumnMappings.Add("Kích Thước", "Size");
                objbulk.ColumnMappings.Add("Giá đại lý miền bắc", "AgencyNorthPrice");
                objbulk.ColumnMappings.Add("Giá đại lý miền nam", "AgencySouthPrice");
                objbulk.ColumnMappings.Add("Loại hàng (Nhập ID)", "ProductType_ID");
                objbulk.ColumnMappings.Add("Chất liệu sản phẩm (Nhập ID, mỗi ID cách nhau bởi dấu ',')", "MaterialCategory_ID");
                objbulk.ColumnMappings.Add("Link nhúng Video", "Video");
                objbulk.ColumnMappings.Add("Trọng Lượng", "Trongluong");
                objbulk.ColumnMappings.Add("Url Sản phẩm", "URL");
                objbulk.ColumnMappings.Add("Không gian sản phẩm(Nhập ID, mỗi ID cách nhau bởi dấu ',')", "Space_ID");
                objbulk.ColumnMappings.Add("Tags(Mỗi tag cách nhau bởi dấu ',')", "Tags");
                objbulk.ColumnMappings.Add("Trạng thái", "Active");
                objbulk.ColumnMappings.Add("Ngày tạo", "CreateDate");
                objbulk.ColumnMappings.Add("IsHot", "IsHot");
                objbulk.ColumnMappings.Add("IsHome", "IsHome");
                objbulk.ColumnMappings.Add("IsNew", "IsNew");
                objbulk.ColumnMappings.Add("Mã Người tạo", "CreateBy");
                objbulk.ColumnMappings.Add("Số Sao", "Star");
                objbulk.ColumnMappings.Add("Từ khóa", "Keyword");
                objbulk.ColumnMappings.Add("SeoKeyword", "SeoKeyword");
                objbulk.ColumnMappings.Add("SeoDescription", "SeoDescription");
                objbulk.ColumnMappings.Add("Mô tả sản phẩm", "Description");
                objbulk.ColumnMappings.Add("IsQRCode", "IsQRCode");
                objbulk.ColumnMappings.Add("Mô tả Giá đại lý miền bắc", "NoteAgencyNorthPrice");
                objbulk.ColumnMappings.Add("Mô tả Giá đại lý miền nam", "NoteAgencySouthPrice");
                // objbulk.ColumnMappings.Add("Ghi Chú", "Note");
                objbulk.ColumnMappings.Add("Deleted", "Deleted");
                objbulk.ColumnMappings.Add("Sản phẩm đi kèm(Nhập ID, mỗi ID cách nhau bởi dấu ',')", "ProductAttach_ID");
                objbulk.ColumnMappings.Add("Kích hoạt 2 tem", "IsProductTwoStamp");
                objbulk.ColumnMappings.Add("SP cùng kích hoạt 2 tem(Nhập ID, mỗi ID cách nhau bởi dấu ',')", "ListProduct_ID_TwoStamp");



                //inserting Datatable Records to DataBase    
                con.Open();
                objbulk.WriteToServer(dtCloned);
                check = true;
                con.Close();
            }
        }
        catch (Exception ex)
        {
            Log.writeLog("InsertExcelRecords", ex.ToString());
        }
        return check;
    }
}