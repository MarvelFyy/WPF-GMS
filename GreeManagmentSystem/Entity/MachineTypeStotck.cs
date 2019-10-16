using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeManagementSystem.Entity
{
    class MachineTypeStock
    {
        private string machineTypeName;     //机型名称
        private string machineTypeNumber;   //机型编号
        private string machineTypeClass;    //机型分类
        private string machineTypeRemarks;  //机型备注
        private int quantity;  //数量

        public string MTNameVal
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

        public int MTQuantityVal
        {
            get { return quantity; }
            set { this.quantity = value; }
        }

    }
}
