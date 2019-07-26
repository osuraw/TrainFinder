using System;
using System.Windows;
using System.Windows.Controls;
using Desktop.Model;
using Desktop.Reports;
using Desktop.ViewModels;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for ReportUc.xaml
    /// </summary>
    public partial class ReportUc : UserControl
    {
        private readonly ReportVm _reportVm;
        public ReportUc()
        {
            InitializeComponent();
            _reportVm = ReportVm;
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) ReportVm.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) ReportVm.Errors -= 1;
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            string p1 ="", p2="";
            if(Date1.SelectedDate.HasValue)
                p1=_reportVm.Parameter1 = DateTime.Parse(Date1.SelectedDate.ToString()).Date.ToString("yyyy-MM-dd");
            if (Date2.SelectedDate.HasValue)
                p2=_reportVm.Parameter2 = DateTime.Parse(Date2.SelectedDate.ToString()).Date.ToString("yyyy-MM-dd");
            var table = await _reportVm.GetTableTemp();
            Report1 report1 = new Report1();
            if (_reportVm.Parameter1 == null)
                p1= "--------";
            if (_reportVm.Parameter2 == null)
                p2= "--------";
            string trainNo = _reportVm.TrainId.ToString(), type = "By Train",name= ((Train)cmbTrain.SelectedItem).Name;
            if (_reportVm.TrainId==1)
            {
                name=trainNo = "*****";
                type = "All Trains";
            }
            report1.Database.Tables["Report1"].SetDataSource(table);
            report1.SetParameterValue("fromDate",p1);
            report1.SetParameterValue("toDate",p2);
            report1.SetParameterValue("trainNo",trainNo);
            report1.SetParameterValue("type",type);
            report1.SetParameterValue("trainName", name);
            report1.SetParameterValue("noOfRecordes",table.Rows.Count);
            ReportsViewer.ViewerCore.ReportSource = report1;
        }
    }
}
