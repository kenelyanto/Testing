using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;


namespace SistemPendukungKeputusan.Helper
{
    public class MetronicDatatableSetting<T> where T : class
    {
        public bool DisableDetailsButton { get; set; }
        public bool DisableEditButton { get; set; }
        public bool DisableDeleteButton { get; set; }
        public bool CustomFilterRow { get; set; }
        public List<ColumnSetting<T>> CustomFilterColumnSettings { get; private set; }
        public List<CustomAction<T>> CustomActions { get; private set; }

        public IQueryable<T> BaseCollection  { get; set; }
        public List<ColumnSetting<T>> ColumnSettings { get; private set; }
        public MetronicDatatableSetting()
        {
            ColumnSettings = new List<ColumnSetting<T>>();
            CustomFilterColumnSettings = new List<ColumnSetting<T>>();
            CustomActions = new List<CustomAction<T>>();
        }
        public async Task<string> GetJsonContent(Controller controller)
        {
            NameValueCollection FormValue = controller.Request.Form;
            IQueryable<T> queryable = BaseCollection.Where(m => 1 == 1);
            if (FormValue.AllKeys.Contains(ColumnSettings[0].Name) || FormValue.AllKeys.Contains(ColumnSettings[0].Name + "_from"))
            {
                List<ColumnSetting<T>> clm = ColumnSettings;
                if (CustomFilterRow) clm = CustomFilterColumnSettings;
                foreach (ColumnSetting<T> columnSetting in clm)
                {
                    switch (columnSetting.Type)
                    {
                        case FieldType.Text:
                            if (FormValue[columnSetting.Name] != "")
                                queryable = queryable.Where(columnSetting.Name + ".Contains(@0)", FormValue[columnSetting.Name]);
                            break;
                        case FieldType.Boolean:
                            if (FormValue[columnSetting.Name] != "")
                                queryable = queryable.Where(columnSetting.Name + " == @0", bool.Parse(FormValue[columnSetting.Name]));
                            break;
                        case FieldType.ForeignObject:
                            if (FormValue[columnSetting.Name] != "")
                                queryable = queryable.Where(columnSetting.Name + " == @0", int.Parse(FormValue[columnSetting.Name]));
                            break;
                        case FieldType.Enum:
                            if (FormValue[columnSetting.Name] != "")
                                queryable = queryable.Where(columnSetting.Name + " == @0", int.Parse(FormValue[columnSetting.Name]));
                            break;
                        case FieldType.Numeric:
                            if (FormValue[columnSetting.Name + "_from"] != "")
                                queryable = queryable.Where(columnSetting.Name + " >= @0", decimal.Parse(FormValue[columnSetting.Name + "_from"]));
                            if (FormValue[columnSetting.Name + "_to"] != "")
                                queryable = queryable.Where(columnSetting.Name + " <= @0", decimal.Parse(FormValue[columnSetting.Name + "_to"]));
                            break;
                        case FieldType.Date:
                            if (FormValue[columnSetting.Name + "_from"] != "")
                                queryable = queryable.Where(columnSetting.Name + " >= @0", DateTime.Parse(FormValue[columnSetting.Name + "_from"]));
                            if (FormValue[columnSetting.Name + "_to"] != "")
                                queryable = queryable.Where(columnSetting.Name + " <= @0", DateTime.Parse(FormValue[columnSetting.Name + "_to"]));
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            dynamic returnData = new ExpandoObject();
            returnData.draw = FormValue["draw"];
            returnData.recordsTotal = BaseCollection.Count();
            returnData.recordsFiltered = queryable.Count();
            if (FormValue["order[0][column]"] != null)
            {
                if (int.Parse(FormValue["order[0][column]"]) == 1)
                {
                    queryable = queryable.OrderBy(ColumnSettings[int.Parse(FormValue["order[0][column]"]) - 1].Name + " " + FormValue["order[0][dir]"]);
                }
                else
                {
                    queryable = queryable.OrderBy(ColumnSettings[int.Parse(FormValue["order[0][column]"]) - 2].Name + " " + FormValue["order[0][dir]"]);
                }
            }
            else
                queryable = queryable.OrderBy("Id");
            int start = int.Parse(FormValue["start"]);
            if (start > 0)
                queryable = queryable.Skip(start);
            int totalItemPerPage = int.Parse( FormValue["length"]);
            if (totalItemPerPage > 0)
                queryable = queryable.Take(totalItemPerPage);
            List<T> list = (from T obj in queryable
                                 select obj).ToList();

            List<ExpandoObject> myList = new List<ExpandoObject>();
            var propInfo = typeof(T).GetProperty("Id");
            foreach (T obj in list)
            {
                dynamic myObj = new ExpandoObject();
                string Id = propInfo.GetValue(obj).ToString();

                if (propInfo.ReflectedType.FullName == "eVoucher.Module.Entities.DiscountGroupMutationLine" || propInfo.ReflectedType.FullName == "eVoucher.Module.Entities.BatamFastCommissionListMutationLine")
                {
                     //checkbox
                    ((IDictionary<string, object>)myObj)["0"] = "<input type=\"checkbox\" name=\"id['" + Id + "']\" value=\"'" + Id + "'\" />";
                    //action
                    ((IDictionary<string, object>)myObj)[(1).ToString()] = "";
                    if (!DisableDetailsButton) ((IDictionary<string, object>)myObj)[(1).ToString()] += "<a href='" + controller.Url.Action("Details", new { Id = Id }) + "' class='btn btn-xs default'><i class='fa fa-eye'></i> View</a>";
                    if (!DisableEditButton) ((IDictionary<string, object>)myObj)[(1).ToString()] += "<a href='" + controller.Url.Action("Edit", new { Id = Id }) + "' class='btn btn-xs default'><i class='fa fa-edit'></i> Edit</a>";
                    if (!DisableDeleteButton) ((IDictionary<string, object>)myObj)[(1).ToString()] += "<a href='" + controller.Url.Action("Delete", new { Id = Id }) + "' class='btn btn-xs default'><i class='fa fa-close'></i> Delete</a>";
                    //column
                    foreach (CustomAction<T> action in CustomActions)
                    {
                        if (action.ShowCriteriaEvaluator.Invoke(obj)) ((IDictionary<string, object>)myObj)[(1).ToString()] += action.HtmlGenerationEvaluator.Invoke(obj);
                    }

                    for (int i = 2; i <= ColumnSettings.Count + 1; i++)
                    {
                        if (((ColumnSettings[0].GetValueMethod.Method.ToString().Contains("GeneratedLineIndexListDatatableSetting") && ColumnSettings[0].GetValueMethod.Method.ToString().Contains("DiscountGroupMutationLine")) && ((i == 7 || i == 8) && ColumnSettings[i - 2].GetValueMethod.Invoke(obj) != "-")))
                        {
                            ((IDictionary<string, object>)myObj)[i.ToString()] = "<span style='color:red'>" + ColumnSettings[i - 2].GetValueMethod.Invoke(obj) + "</span>";
                        }
                        else if ((ColumnSettings[0].GetValueMethod.Method.ToString().Contains("GeneratedLineIndexListDatatableSetting") && ColumnSettings[0].GetValueMethod.Method.ToString().Contains("BatamFastCommissionListMutationLine")) && (((i == 6) && ColumnSettings[i - 2].GetValueMethod.Invoke(obj) != "-") || ((i == 7) && ColumnSettings[i - 2].GetValueMethod.Invoke(obj) != "No")))
                        {
                            ((IDictionary<string, object>)myObj)[i.ToString()] = "<span style='color:red'>" + ColumnSettings[i - 2].GetValueMethod.Invoke(obj) + "</span>";
                        }
                        else
                        {
                            ((IDictionary<string, object>)myObj)[i.ToString()] = ColumnSettings[i - 2].GetValueMethod.Invoke(obj);
                        }
                    }
                }
                else
                {
                    //checkbox
                    ((IDictionary<string, object>)myObj)["0"] = "<input type=\"checkbox\" name=\"id['" + Id + "']\" value=\"'" + Id + "'\" />";
                    //action
                    ((IDictionary<string, object>)myObj)[(1).ToString()] = "";
                    if (!DisableDetailsButton) ((IDictionary<string, object>)myObj)[(1).ToString()] += "<a href='" + controller.Url.Action("Details", new { Id = Id }) + "' class='btn btn-xs default'><i class='fa fa-eye'></i> View</a>";
                    if (!DisableEditButton) ((IDictionary<string, object>)myObj)[(1).ToString()] += "<a href='" + controller.Url.Action("Edit", new { Id = Id }) + "' class='btn btn-xs default'><i class='fa fa-edit'></i> Edit</a>";
                    if (!DisableDeleteButton) ((IDictionary<string, object>)myObj)[(1).ToString()] += "<a href='" + controller.Url.Action("Delete", new { Id = Id }) + "' class='btn btn-xs default'><i class='fa fa-close'></i> Delete</a>";
                    //column
                    foreach (CustomAction<T> action in CustomActions)
                    {
                        if (action.ShowCriteriaEvaluator.Invoke(obj)) ((IDictionary<string, object>)myObj)[(1).ToString()] += action.HtmlGenerationEvaluator.Invoke(obj);
                    }
                    for (int i = 2; i <= ColumnSettings.Count + 1; i++)
                    {
                        ((IDictionary<string, object>)myObj)[i.ToString()] = ColumnSettings[i - 2].GetValueMethod.Invoke(obj);
                    }
                }
                myList.Add(myObj);
            }

            returnData.data = myList;
            return await Task.Run(() => JsonConvert.SerializeObject(returnData));
        }
    }
    public class ColumnSetting<T>
    {
        public FieldType Type { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public Func<T, string> GetValueMethod { get; set; }
        public int ColumnWidth { get; set; }
        public SelectList SelectList { get; set; }
        public Type EnumType { get; set; }
        public Func<IQueryable<T>, IQueryable<T>> CustomFilterMethod {get;set;}
    }
    public class CustomAction<T>
    {
        public Func<T,bool> ShowCriteriaEvaluator { get; set; }
        public Func<T, string> HtmlGenerationEvaluator { get; set; }
    }
    public enum FieldType
    {
        Text = 0,
        Numeric = 1,
        Date = 2,
        Boolean = 3,
        ForeignObject = 4,
        Enum = 5
    }
}
