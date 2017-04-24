using SistemPendukungKeputusan.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SistemPendukungKeputusan.Helper.MetronicHtmlHelperExtensions
{
    public static class MetronicListViewFilterExtensions
    {
        public static MvcHtmlString MetronicListViewFilter<T>(this HtmlHelper htmlHelper, ColumnSetting<T> columnSetting)
        {
            switch(columnSetting.Type)
            {
                case FieldType.Text:
                    return MetronicListViewFilterText(htmlHelper, columnSetting.Name);
                case FieldType.ForeignObject:
                    return MetronicListViewFilterDropDown(htmlHelper, columnSetting.Name, columnSetting.SelectList);
                case FieldType.Boolean:
                    return MetronicListViewFilterBoolean(htmlHelper, columnSetting.Name);
                case FieldType.Enum:
                    return MetronicListViewFilterEnum(htmlHelper, columnSetting.Name, columnSetting.EnumType);
                case FieldType.Date:
                    return MetronicListViewFilterDate(htmlHelper, columnSetting.Name);
                case FieldType.Numeric:
                    return MetronicListViewFilterNumeric(htmlHelper, columnSetting.Name);
                default:
                    throw new NotImplementedException();
            }
        }
        public static MvcHtmlString MetronicListViewFilterText(this HtmlHelper htmlHelper, string name)
        {
            string HtmlString = "<input type = 'text' class='form-control form-filter input-sm' name='" + name + "' placeholder='"+name+"'>";
            return MvcHtmlString.Create(HtmlString);
        }
        public static MvcHtmlString MetronicListViewFilterDate(this HtmlHelper htmlHelper, string name)
        {
            string HtmlString = "<div class='input-group date date-picker margin-bottom-5' data-date-format='mm/dd/yyyy'>" + Environment.NewLine +
                               "    <input type = 'text' class='form-control form-filter input-sm' readonly name='" + name + "_from' placeholder='From' style='width: 100PX';>" + Environment.NewLine +
                               "    <span class='input-group-btn'>" + Environment.NewLine +
                               "        <button class='btn btn-sm default' type='button'>" + Environment.NewLine +
                               "            <i class='fa fa-calendar'></i>" + Environment.NewLine +
                               "        </button>" + Environment.NewLine +
                               "    </span>" + Environment.NewLine +
                               "</div>" + Environment.NewLine +
                               "<div class='input-group date date-picker' data-date-format='mm/dd/yyyy'>" + Environment.NewLine +
                               "    <input type = 'text' class='form-control form-filter input-sm' readonly name='" + name + "_to' placeholder='To'>" + Environment.NewLine +
                               "    <span class='input-group-btn'>" + Environment.NewLine +
                               "        <button class='btn btn-sm default' type='button'>" + Environment.NewLine +
                               "            <i class='fa fa-calendar'></i>" + Environment.NewLine +
                               "        </button>" + Environment.NewLine +
                               "    </span>" + Environment.NewLine +
                               "</div>";
            return MvcHtmlString.Create(HtmlString);
        }
        public static MvcHtmlString MetronicListViewFilterNumeric(this HtmlHelper htmlHelper, string name)
        {
            string HtmlString = "<input type = 'text' class='form-control form-filter input-sm' name='"+ name + "_from' placeholder='From'>" + Environment.NewLine +
                                "<input type = 'text' class='form-control form-filter input-sm' name='" + name + "_to' placeholder='To'>";
            return MvcHtmlString.Create(HtmlString);
        }
        public static MvcHtmlString MetronicListViewFilterDropDown(this HtmlHelper htmlHelper, string name, SelectList selectList)
        {
            return htmlHelper.DropDownList(name, selectList, "--Select--", new { @class = "form-control select2me form-filter input-sm" });
        }
        public static MvcHtmlString MetronicListViewFilterBoolean(this HtmlHelper htmlHelper, string name)
        {
            List<object> list = new List<object>();
            list.Add(new { Value = true, Caption = "Yes" });
            list.Add(new { Value = false, Caption = "No" });
            SelectList selectList = new SelectList(list, "Value", "Caption");
            return MetronicListViewFilterDropDown(htmlHelper, name, selectList);
        }
        public static MvcHtmlString MetronicListViewFilterEnum(this HtmlHelper htmlHelper, string name, Type enumType)
        {
            List<object> list = new List<object>();
            var enums = Enum.GetValues(enumType);
            foreach(var en in enums)
                list.Add(new { Value = Convert.ToInt32(en), Caption = Enum.GetName(enumType, en) });
            SelectList selectList = new SelectList(list, "Value", "Caption");
            return MetronicListViewFilterDropDown(htmlHelper, name, selectList);
        }
    }
}