using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeManagementSystem.Entity
{
    class SalesOrder
    {
        //客户信息
        private string totalNumber;         //总订单
        private long subunitNumber;          //分单号（主键）
        private string customerName;        //客户姓名
        private string customerTel;         //客户电话
        private string customerAddress;     //客户地址

        //机型信息
        private string machineTypeName;     //机型名称
        private string machineTypeNumber;   //机型编号
        private string machineTypeClass;    //机型分类
        private string machineTypeRemarks;  //机型备注
        private int quantity;               //数量
        

        //订单信息
        private string salesman;            //销售人
        private string orderDate;           //下单日期(日期格式为 YYYY-MM-DD)
        private double sellingPrice;        //销售价格
        private double earnestMoney;        //定价
        private double installCosts;        //安装费
        private string paymentMethod;       //付款方式
        private string installDate;         //安装日期(日期格式为 YYYY-MM-DD)
        private string installman;          //安装人员（注意可能涉及到多个人）
        private double pipeLength;          //铜管长度（理论上是以 米 为单位）
        private string remarks;             //备注
        private string state;                  //状态标识 1预订 2需要安装 3表示完成安装进入售后
        //NEW 10-04
        private double totalPrice;          //总价


        public string TNVal
        {
            get { return totalNumber; }
            set { this.totalNumber = value; }
        }

        public long SNVal //object
        {
            get { return subunitNumber; }
            set { this.subunitNumber = value; }
        }

        public string CNameVal
        {
            get { return customerName; }
            set { this.customerName = value; }
        }

        public string CTelVal
        {
            get { return customerTel; }
            set { this.customerTel = value; }
        }

        public string CAddressVal
        {
            get { return customerAddress; }
            set { this.customerAddress = value; }
        }

        public string MTNameVal //object
        {
            get { return machineTypeName; }
            set { this.machineTypeName = value; }
        }

        public string MTNumberVal
        {
            get { return machineTypeNumber; }
            set { this.machineTypeNumber = value; }
        }

        public string MTClassVal
        {
            get { return machineTypeClass; }
            set { this.machineTypeClass = value; }
        }

        public string MTRemarksVal
        {
            get { return machineTypeRemarks; }
            set { this.machineTypeRemarks = value; }
        }

        public int QuantityVal //object
        {
            get { return quantity; }
            set { this.quantity = value; }
        }


        public string SalesmanVal
        {
            get { return salesman; }
            set { this.salesman = value; }
        }

        public string OrderDateVal
        {
            get { return orderDate; }
            set { this.orderDate = value; }
        }

        public double SellingPriceVal
        {
            get { return sellingPrice; }
            set { this.sellingPrice = value; }
        }

        public double EarnestMoneyVal
        {
            get { return earnestMoney; }
            set { this.earnestMoney = value; }
        }

        public double InstallCostsVal
        {
            get { return installCosts; }
            set { this.installCosts = value; }
        }

        public string PaymentMethodVal
        {
            get { return paymentMethod; }
            set { this.paymentMethod = value; }
        }

        public string InstallDateVal
        {
            get { return installDate; }
            set { this.installDate = value; }
        }

        public string InstallmanVal
        {
            get { return installman; }
            set { this.installman = value; }
        }

        public double PipeLengthVal
        {
            get { return pipeLength; }
            set { this.pipeLength = value; }
        }

        public string RemarksVal
        {
            get { return remarks; }
            set { this.remarks = value; }
        }

        public string StateVal
        {
            get { return state; }
            set { this.state = value; }
        }

        public double TotalPriceVal
        {
            get { return totalPrice; }
            set { this.totalPrice = value; }
        }
    }

}
