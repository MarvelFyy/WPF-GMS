using GreeManagmentSystem.User.Method;

namespace GreeManagmentSystem.User.ListWindow
{
    class ListModel
    {

        Multiply multiply = new Multiply();
        public ListModel()
        {
            timer=multiply.getNumber();
            currentDate = multiply.getCurrentDate();
            subNumber = multiply.getSubNumber();
        }
        //获取时间作为总订单编号
        public string timer
        {
            get;
            set;

        }
        //当前日期
        public string currentDate
        {
            get;
            set;
        }

        //分订单号
        public string subNumber
        {
            get;
            set;
        }

    }
}
