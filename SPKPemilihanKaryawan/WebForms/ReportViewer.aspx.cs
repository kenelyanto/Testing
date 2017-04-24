using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SistemPendukungKeputusan.Models;
using SistemPendukungKeputusan.DAL;
using SistemPendukungKeputusan.Helper;

namespace SistemPendukungKeputusan.Web.WebForms
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        #region Methods
        public bool isExportSchema = false;
        protected void btnExportToPdf_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValid = true;
                isExportSchema = false;
                // Setting ReportName
                Report Report = (Report)HttpContext.Current.Session["Report"];
                ShowReportParameters ReportParameter = (ShowReportParameters)HttpContext.Current.Session["ReportParameter"];

                if (Report == null)
                    isValid = false;

                if (isValid) // If Report Name provided then do other operation
                {
                    ReportDocument rd = GetReportDocument(Report, ReportParameter);
                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Report.Name);
                }
                else
                {
                    Response.Write("<H2>Nothing Found; No Report name found</H2>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void btnExportToXls_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValid = true;
                isExportSchema = false;
                // Setting ReportName
                Report Report = (Report)HttpContext.Current.Session["Report"];
                ShowReportParameters ReportParameter = (ShowReportParameters)HttpContext.Current.Session["ReportParameter"];

                if (Report == null)
                    isValid = false;

                if (isValid) // If Report Name provided then do other operation
                {
                    ReportDocument rd = GetReportDocument(Report, ReportParameter);
                    rd.ExportToHttpResponse(ExportFormatType.Excel, Response, true, Report.Name);
                }
                else
                {
                    Response.Write("<H2>Nothing Found; No Report name found</H2>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void btnExportSchema_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValid = true;
                isExportSchema = true;

                // Setting ReportName
                Report Report = (Report)HttpContext.Current.Session["Report"];
                ShowReportParameters ReportParameter = (ShowReportParameters)HttpContext.Current.Session["ReportParameter"];

                if (Report == null)
                    isValid = false;

                if (isValid) // If Report Name provided then do other operation
                {
                    ReportDocument rd = GetReportDocument(Report, ReportParameter);
                }
                else
                {
                    Response.Write("<H2>Nothing Found; No Report name found</H2>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SistemPendukungKeputusan.DAL.SPKContext dbcontext = new SistemPendukungKeputusan.DAL.SPKContext();
            string currentUserId = User.Identity.GetUserId();
            IdentityUser currentUser = dbcontext.Users.FirstOrDefault(x => x.Id == currentUserId);

            if (!IsPostBack)
            {
                try
                {

                    btnExportSchema.Visible = true;
                    bool isValid = true;

                    // Setting ReportName
                    Report Report = (Report)HttpContext.Current.Session["Report"];
                    ShowReportParameters ReportParameter = (ShowReportParameters)HttpContext.Current.Session["ReportParameter"];

                    if (Report == null)
                        isValid = false;

                    if (isValid) // If Report Name provided then do other operation
                    {
                        ReportDocument rd = GetReportDocument(Report, ReportParameter);
                        crViewer.ReportSource = rd;
                        Session["ReportDocument"] = rd;
                    }
                    else
                    {
                        Response.Write("<H2>Nothing Found; No Report name found</H2>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
            }
            else
            {
                ReportDocument rd = (ReportDocument)Session["ReportDocument"];
                crViewer.ReportSource = rd;
            }
        }

        private ReportDocument GetReportDocument(Report report, ShowReportParameters reportParameter)
        {
            SPKContext db = new SPKContext();

            ReportDocument rd = new ReportDocument();
            string strRptPath = Server.MapPath(@"~\ReportFiles\" + report.FileName);

            string Query = report.Query
                                .Replace("[[Vacancy]]", reportParameter.VacancyId == null ? "NULL" : reportParameter.VacancyId.ToString());
            DataSet ds = GlobalFunction.ExecuteQuery(db.Database.Connection, Query);
            rd.Load(strRptPath);

            if (isExportSchema == true)
            {
                MemoryStream xml = new MemoryStream();
                ds.WriteXmlSchema(xml);

                Response.Clear();
                Response.ContentType = "text/xml";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + report.FileName + ".xml");
                Response.BinaryWrite(xml.ToArray());
                Response.Flush();
                Response.Close();
                Response.End();
            }
            else
            {
                rd.SetDataSource(ds);
            }
            return rd;
        }

        #endregion Methods
    }
}