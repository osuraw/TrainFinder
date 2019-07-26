using Desktop.ViewModels;

namespace Desktop.Helpers
{
    class ViewFactory
    {
        public static BaseViewModelMain GetView(string s)
        {
            BaseViewModelMain viewContent=null;
            switch (s )
            {
                case "Control": viewContent = new ControlVm(); break;
                case "Report": viewContent = new ReportVm(); break;
                case "Route": viewContent = new RouteVm(); break;
                case "Train": viewContent = new TrainVm(); break;
                case "Station": viewContent = new StationVm(); break;
                case "PinLocation": viewContent = new PinLocationVm(); break;
                case "TimeTable": viewContent = new TimeTableVm(); break;
                case "User": viewContent = new UserVm(); break;
            }
            return viewContent;
        }
    }
}
