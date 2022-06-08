using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Product_Product_DownloadExcel : System.Web.UI.Page
{
    string FlagExelData = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        FlagExelData = Request.QueryString["Action"];

        if (FlagExelData != null)
        {
            dt = BusinessRulesLocator.Conllection().GetAllList(@"Select Product_ID,ProductCategory_ID as N'Ngành Hàng',ProductCategory_ID as N'Nhóm Lớn',ProductCategory_ID,ProductSet_ID,SKU_Web_ID,SKU_ID,Name,Classify,Size,Color_ID,MaterialCategory_ID,Unit,Tags,Description,ProductType_ID,AgencyNorthPrice,AgencySouthPrice,Price,Parent_ID,WarrantyDate,Image,NoteAgencyNorthPrice,NoteAgencySouthPrice,Video,Trongluong,URL,Space_ID,ProductAttach_ID,Star,Keyword,CONVERT(INT, Active) AS IntValue,IsHot,IsHome,IsNew,CONVERT(INT, IsQRCode) AS IntValue1,CONVERT(INT, IsProductTwoStamp) AS IntValue2,ListProduct_ID_TwoStamp,SeoKeyword,SeoDescription from Product where Active <>-1 and Deleted = 0 and ProductInWarehouse is null");
            dt.Columns["ProductCategory_ID"].ColumnName = "Ngành,Nhóm sản phẩm (Nhập ID)";
            dt.Columns["ProductSet_ID"].ColumnName = "Bộ Sản Phẩm (nhập ID)";
            dt.Columns["Parent_ID"].ColumnName = "Sản phẩm cha (nhập ID)";
            dt.Columns["SKU_ID"].ColumnName = "SKU hệ thông effect";
            dt.Columns["SKU_Web_ID"].ColumnName = "SKU hiển thị Web";
            dt.Columns["Name"].ColumnName = "Tên Sản Phẩm";
            dt.Columns["WarrantyDate"].ColumnName = "Thời gian bảo hành (Ngày)";
            dt.Columns["Image"].ColumnName = "Đường dẫn ảnh";
            dt.Columns["Unit"].ColumnName = "Đơn vị";
            dt.Columns["Color_ID"].ColumnName = "Màu sản phẩm (Nhập ID)";
            dt.Columns["Price"].ColumnName = "Giá sản phẩm";
            dt.Columns["Classify"].ColumnName = "Phân loại sản phẩm (Nhập ID)";
            dt.Columns["Size"].ColumnName = "Kích Thước";
            dt.Columns["AgencyNorthPrice"].ColumnName = "Giá đại lý miền bắc";
            dt.Columns["AgencySouthPrice"].ColumnName = "Giá đại lý miền nam";
            dt.Columns["ProductType_ID"].ColumnName = "Loại hàng (Nhập ID)";
            dt.Columns["MaterialCategory_ID"].ColumnName = "Chất liệu sản phẩm (Nhập ID, mỗi ID cách nhau bởi dấu ',')";
            dt.Columns["Video"].ColumnName = "Link nhúng Video";
            dt.Columns["Trongluong"].ColumnName = "Trọng Lượng";
            dt.Columns["URL"].ColumnName = "Url Sản phẩm";
            dt.Columns["Space_ID"].ColumnName = "Không gian sản phẩm(Nhập ID, mỗi ID cách nhau bởi dấu ',')";
            dt.Columns["Tags"].ColumnName = "Tags(Mỗi tag cách nhau bởi dấu ',')";
            dt.Columns["IntValue"].ColumnName = "Trạng thái";
            //dt.Columns["CreateDate"].ColumnName = "Ngày tạo";
            dt.Columns["IsHot"].ColumnName = "IsHot";
            dt.Columns["IsHome"].ColumnName = "IsHome";
            dt.Columns["IsNew"].ColumnName = "IsNew";
            dt.Columns["IntValue1"].ColumnName = "IsQRCode";
            dt.Columns["SeoKeyword"].ColumnName = "SeoKeyword";
            dt.Columns["SeoDescription"].ColumnName = "SeoDescription";
            //dt.Columns["CreateBy"].ColumnName = "Mã Người tạo";
            dt.Columns["Product_ID"].ColumnName = "Mã Sản Phẩm";
            dt.Columns["Description"].ColumnName = "Mô tả sản phẩm";
            dt.Columns["NoteAgencyNorthPrice"].ColumnName = "Mô tả Giá đại lý miền bắc";
            dt.Columns["NoteAgencySouthPrice"].ColumnName = "Mô tả Giá đại lý miền nam";
            //dt.Columns["Note"].ColumnName = "Ghi Chú";
            dt.Columns["Star"].ColumnName = "Số Sao";
            dt.Columns["Keyword"].ColumnName = "Từ khóa";
            dt.Columns["ListProduct_ID_TwoStamp"].ColumnName = "SP cùng kích hoạt 2 tem(Nhập ID, mỗi ID cách nhau bởi dấu ',')";
            dt.Columns["IntValue2"].ColumnName = "Kích hoạt 2 tem";
            dt.Columns["ProductAttach_ID"].ColumnName = "Sản phẩm đi kèm(Nhập ID, mỗi ID cách nhau bởi dấu ',')";


        }
        else
        {
            dt.Clear();
            dt.Columns.Add("Ngành Hàng");
            dt.Columns.Add("Nhóm Lớn");
            dt.Columns.Add("Ngành,Nhóm sản phẩm (Nhập ID)");
            dt.Columns.Add("Bộ Sản Phẩm (nhập ID)");
            dt.Columns.Add("SKU hiển thị Web");
            dt.Columns.Add("SKU hệ thông effect");
            dt.Columns.Add("Tên Sản Phẩm");
            dt.Columns.Add("Phân loại sản phẩm (Nhập ID)");
            dt.Columns.Add("Kích Thước");
            dt.Columns.Add("Màu sản phẩm (Nhập ID)");
            dt.Columns.Add("Chất liệu sản phẩm (Nhập ID, mỗi ID cách nhau bởi dấu ',')");
            dt.Columns.Add("Đơn vị");
            dt.Columns.Add("Tags(Mỗi tag cách nhau bởi dấu ',')");
            dt.Columns.Add("Mô tả sản phẩm");
            //dt.Columns.Add("Ghi Chú");
            dt.Columns.Add("Loại hàng (Nhập ID)");
            dt.Columns.Add("Giá đại lý miền bắc");
            dt.Columns.Add("Giá đại lý miền nam");
            dt.Columns.Add("Giá sản phẩm");
            dt.Columns.Add("Sản phẩm cha (nhập ID)");
            dt.Columns.Add("Thời gian bảo hành (Ngày)");
            dt.Columns.Add("Đường dẫn ảnh");
            dt.Columns.Add("Mô tả Giá đại lý miền bắc");
            dt.Columns.Add("Mô tả Giá đại lý miền nam");
            dt.Columns.Add("Link nhúng Video");
            dt.Columns.Add("Trọng Lượng");
            dt.Columns.Add("Url Sản phẩm");
            dt.Columns.Add("Không gian sản phẩm(Nhập ID, mỗi ID cách nhau bởi dấu ',')");
            dt.Columns.Add("Sản phẩm đi kèm(Nhập ID, mỗi ID cách nhau bởi dấu ',')");
            dt.Columns.Add("Số Sao");
            dt.Columns.Add("Từ khóa");
            dt.Columns.Add("Trạng thái");
            //dt.Columns.Add("Ngày tạo");
            dt.Columns.Add("IsHot");
            dt.Columns.Add("IsHome");
            dt.Columns.Add("IsNew");
            dt.Columns.Add("IsQRCode");
            dt.Columns.Add("Kích hoạt 2 tem");
            dt.Columns.Add("SP cùng kích hoạt 2 tem(Nhập ID, mỗi ID cách nhau bởi dấu ',')");
            dt.Columns.Add("SeoKeyword");
            dt.Columns.Add("SeoDescription");
            //dt.Columns.Add("Mã Người tạo");



            //SET Value
            dt.Rows.Add();
            //  dt.Rows[0]["Thời gian bảo hành (Ngày)"] = 365;
            //dt.Rows[0]["Trạng thái"] = true;
            //dt.Rows[0]["IsHot"] = 0;
            //dt.Rows[0]["IsNew"] = 0;
            //dt.Rows[0]["IsHome"] = 1;
            //dt.Rows[0]["Ngày tạo"] = DateTime.Now;
            //dt.Rows[0]["Mã Người tạo"] = MyUser.GetUser_ID();
        }







        //for (int i = 0; i < 200; i++)
        //{
        //    dt.Rows.Add();
        //    dt.Rows[0]["Thời gian bảo hành (Ngày)"] = 365;
        //    dt.Rows[0]["Trạng thái"] = true;
        //    dt.Rows[0]["IsHot"] = 0;
        //    dt.Rows[0]["IsHome"] = 1;
        //    dt.Rows[0]["Ngày tạo"] = Common.ConvertToDate(DateTime.Now.ToString());
        //    dt.Rows[0]["Mã Người tạo"] = MyUser.GetUser_ID();
        //}

        DataTable dtProductSet = BusinessRulesLocator.Conllection().GetAllList(@"Select ProductSet_ID as N'Mã Bộ Sản Phẩm',Name as N'Tên Bộ Sản Phẩm' from ProductSet where Active = 1");

        DataTable dtProductCategory = BusinessRulesLocator.Conllection().GetAllList(@"Select ProductCategory_ID as N'Mã Nhóm Ngành Sản Phẩm',Name as N'Tên Nhóm Ngành Sản Phẩm' from ProductCategory where Active = 1 ");

        DataTable dtProductColor = BusinessRulesLocator.Conllection().GetAllList(@"Select Color_ID as N'Mã Màu', Name as N'Tên Màu' from ProductColor where Active = 1 ");

        DataTable dtMaterialCategory = BusinessRulesLocator.Conllection().GetAllList(@"Select MaterialCategory_ID as N'Mã Chất Liệu Sản Phẩm', Name as N'Tên Chất Liệu' from MaterialCategory where Active = 1");

        DataTable dtProductType = BusinessRulesLocator.Conllection().GetAllList(@"Select ProductType_ID as N'Mã Loại Hàng', Name as N'Tên Loại Hàng' from ProductType");

        DataTable dtClassify = BusinessRulesLocator.Conllection().GetAllList(@"Select ProductClassify_ID as N'Mã Phân Loại',Name as N'Tên Phân Loại' from ProductClassify");
        DataTable dtSpace = BusinessRulesLocator.Conllection().GetAllList(@"Select Space_ID as N'Mã Không gian sử dụng',Name as N'Tên Không gian sử dụng' from WEB_Space");

        DataTable dtNote = new DataTable();
        DataColumn ColNote = dtNote.Columns.Add("Ghi Chú", typeof(string));
        DataRow rowNote = dtNote.NewRow();
        rowNote["Ghi Chú"] = "1. KHI UPDATE FILE EXCEL CHỈ SỬA NHỮNG TRƯỜNG THÔNG TIN THAY ĐỔI. KHÔNG ĐƯỢC XÓA CÁC CỘT FILE CÓ SẴN";
        dtNote.Rows.Add(rowNote);
        DataRow rowNote1 = dtNote.NewRow();
        rowNote1["Ghi Chú"] = "2. NHẬP SẢN PHẨM DÁN 2 TEM-- > KHÔNG ĐƯỢC TRÙNG MÃ SẢN PHẨM VỚI SẢN PHẨM MUỐN CẬP NHẬT";
        dtNote.Rows.Add(rowNote1);
        DataRow rowNote2 = dtNote.NewRow();
        rowNote2["Ghi Chú"] = "3. KHI SẢN PHẨM KHÔNG THUỘC BỘ SẢN PHẨM NÀO ---> NHẬP = 0(KIỂU SỐ)";
        dtNote.Rows.Add(rowNote2);
        DataRow rowNote3 = dtNote.NewRow();
        rowNote3["Ghi Chú"] = "4. ĐỐI VỚI NHỮNG CỘT NHẬP NHIỀU ID, CÁCH NHAU BỞI DẤU ','---> NẾU NHẬP 1 ID DUY NHẤT TRONG CỘT ---> CHUYỂN SANG ĐỊNH DẠNG TEXT";
        dtNote.Rows.Add(rowNote3);



        //dtClassify.Clear();
        //dtClassify.Columns.Add("Mã Phân Loại");
        //dtClassify.Columns.Add("Tên Phân Loại");
        //DataRow Row;
        //for (int i = 1; i <= 3; i++)
        //{
        //    Row = dtClassify.NewRow();
        //    Row["Mã Phân Loại"] = i;
        //    Row["Tên Phân Loại"] = i +" Tầng";
        //    dtClassify.Rows.Add(Row);

        //}

        //SET Name
        dt.TableName = "File-nhap-san-pham";
        dtNote.TableName = "File-Note";
        dtProductSet.TableName = "data-bo-san-pham";
        dtProductCategory.TableName = "data-nhom-nganh-san-pham";
        dtProductColor.TableName = "data-mau-san-pham";
        dtMaterialCategory.TableName = "data-chat-lieu-san-pham";
        dtProductType.TableName = "data-loai-hang";
        dtClassify.TableName = "data-phan-loai";
        dtSpace.TableName = "data-khong-gian-su-dung";

        //SET Copy
        DataTable dtCopy1 = dt.Copy();
        DataTable dtCopyNote = dtNote.Copy();
        DataTable dtCopy = dtProductSet.Copy();
        DataTable dtCopyCate = dtProductCategory.Copy();
        DataTable dtCopyColor = dtProductColor.Copy();
        DataTable dtCopyMaterial = dtMaterialCategory.Copy();
        DataTable dtCopyType = dtProductType.Copy();
        DataTable dtCopyClassify = dtClassify.Copy();
        DataTable dtCopySpace = dtSpace.Copy();


        //SET DataSet
        ds.Tables.Add(dtCopy1);
        ds.Tables.Add(dtCopyNote);
        ds.Tables.Add(dtCopy);
        ds.Tables.Add(dtCopyCate);
        ds.Tables.Add(dtCopyColor);
        ds.Tables.Add(dtCopyMaterial);
        ds.Tables.Add(dtCopyType);
        ds.Tables.Add(dtCopyClassify);
        ds.Tables.Add(dtCopySpace);

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(ds);
            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            wb.Style.Font.Bold = true;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            if (FlagExelData == null)
            {
                Response.AddHeader("content-disposition", "attachment;filename= File_nhap_lieu_san_pham_" + date + ".xlsx");
            }
            else
            {
                Response.AddHeader("content-disposition", "attachment;filename= Bao_cao_San_Pham_" + date + ".xlsx");
            }
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
}