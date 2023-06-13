using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Module.Extension;

/// <summary>
/// Điều chỉnh detail view
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CustomDetailViewAttribute : Attribute
{
    /// <summary>
    /// Nếu null sẽ tác động đến detail view mặc định.<br/>
    /// Nếu khác null sẽ tạo detail view mới."
    /// </summary>
    public string ViewId { get; set; } = null;
    public string[] FieldsToRemove { get; set; } = Array.Empty<string>();
    public string[] FieldsReadonly { get; set; } = Array.Empty<string>();
    /// <summary>
    /// <see langword="false"/> sẽ làm toàn bộ view thành readonly
    /// </summary>
    public bool AllowEdit { get; set; } = true;
    public bool AllowDelete { get; set; } = true;
    public bool AllowNew { get; set; } = true;

    public bool Tabbed { get; set; } = false;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CustomListViewAttribute : Attribute
{
    /// <summary>
    /// Chỉ định detail view nào sẽ được hiển thị khi click vào nested list view
    /// </summary>
    public string DetailViewId { get; set; }

    /// <summary>
    /// Những trường cần ẩn sẽ có index = -1.<br/>
    /// Trường ẩn vẫn xuất hiện trên column chooser (và có thể cho hiển thị lại)
    /// </summary>
    public string[] FieldsToHide { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Sẽ loại bỏ các trường này hoàn toàn, ở runtime không cho hiện lại được
    /// </summary>
    public string[] FieldsToRemove { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Mặc định sẽ xếp tăng dần.<br/>
    /// Nếu muốn xếp giảm dần thêm dấu chấm (.) vào sau tên trường
    /// </summary>
    public string[] FieldsToSort { get; set; } = Array.Empty<string>();
    public string[] FieldsToGroup { get; set; } = Array.Empty<string>();
    public string[] FieldsToSum { get; set; } = Array.Empty<string>();

    public string GroupSummary { get; set; }

    /// <summary>
    /// <see langword="true"/> - bật inline edit
    /// </summary>
    public bool AllowEdit { get; set; } = false;
    public bool AllowDelete { get; set; } = true;
    public bool AllowNew { get; set; } = true;
    public bool AllowLink { get; set; } = false;
    public bool AllowUnlink { get; set; } = false;
}

/// <summary>
/// Chỉ định các đặc tính của listview
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CustomRootListViewAttribute : CustomListViewAttribute
{

    /// <summary>
    /// Nếu null sẽ tác động đến list view mặc định.<br/>
    /// Nếu khác null sẽ tạo list view mới."
    /// </summary>
    public string ViewId { get; set; } = null;


}

/// <summary>
/// Điều chỉnh nested listview
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CustomNestedListViewAttribute : CustomListViewAttribute
{
    public CustomNestedListViewAttribute(string collectionProperty)
    {
        _collectionProperty = collectionProperty;
    }

    private readonly string _collectionProperty;
    public string CollectionProperty => _collectionProperty;
}

/// <summary>
/// Chỉ định class/field là readonly
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class ReadonlyAttribute : Attribute
{
    /// <summary>
    /// Nếu true - các trường trong danh sách sẽ editable, các trường khác readonly.<br/>
    /// Nếu false (mặc định) - các trường trong danh sách là readonly.
    /// </summary>
    public bool IsReversed { get; set; } = false;
    public string[] Fields { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Chỉ định độ rộng các cột trong list view, áp dụng đồng thời cho cả nested và root lis view
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CustomListViewColumnWidthAttribute : Attribute
{
    readonly string[] columnWidths;

    public CustomListViewColumnWidthAttribute(string[] columnWidths)
    {
        this.columnWidths = columnWidths;
    }
    public string[] ColumnWidths => columnWidths;
}