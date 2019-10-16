using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeManagementSystem.Entity
{
    class PurchaseOrder
    {
        private string totalNumber;         //总订单
        private long subunitNumber;          //分单号（主键）

        private string machineTypeName;     //机型名称
        private string machineTypeNumber;   //机型编号
        private string machineTypeClass;    //机型分类
        private string machineTypeRemarks;  //机型备注
        private int quantity;               //数量

        private int quantityReceived;       //已到货数量
        private double purchasePrice;       //进价
        private double totalPrice = 0;      //总价
        private string orderDate;           //下单日期(日期格式为 YYYY-MM-DD)
        private string remarks;             //备注
        private string state;                  //订单状态 1表示向厂家下达订单，2表示订单完成。如该分订单到货为完全则还是为状态1 

        public string TNVal //总订单
        {
            get { return totalNumber; }
            set { this.totalNumber = value; }
        }

        public long SNVal //分单号（主键）
        {
            get { return subunitNumber; }
            set { this.subunitNumber = value; }
        }

        public string MTNameVal //机型名称
        {
            get { return machineTypeName; }
            set { this.machineTypeName = value; }
        }

        public string MTNumberVal //机型编号
        {
            get { return machineTypeNumber; }
            set { this.machineTypeNumber = value; }
        }

        public string MTClassVal //机型分类
        {
            get { return machineTypeClass; }
            set { this.machineTypeClass = value; }
        }

        public string MTRemarksVal //机型备注
        {
            get { return machineTypeRemarks; }
            set { this.machineTypeRemarks = value; }
        }

        public int QuantityVal //数量
        {
            get { return quantity; }
            set { this.quantity = value; }
        }

        public int QuantityReceivedVal //已到货数量
        {
            get { return quantityReceived; }
            set { this.quantityReceived = value; }
        }

        public double PurchasePriceVal //进价
        {
            get { return purchasePrice; }
            set { this.purchasePrice = value; }
        }

        public double TotalPriceVal //总价
        {
            get { return totalPrice; }
            set { this.totalPrice = value; }
        }

        public string OrderDateVal //下单日期
        {
            get { return orderDate; }
            set { this.orderDate = value; }
        }

        public string RemarksVal //备注
        {
            get { return remarks; }
            set { this.remarks = value; }
        }

        public string StateVal //订单状态
        {
            get { return state; }
            set { this.state = value; }
        }

    }
}
